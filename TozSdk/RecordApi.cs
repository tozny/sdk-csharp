using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sodium;

namespace Tozny.Auth
{
    public class ClientConfig
    {
        public string ApiUrl { get; set; }
        public string ApiKeyId { get; set; }
        public string ApiSecret { get; set; }
        public string ClientId { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
    }

    public class RecordApi
    {
        public async Task<string> GetAuthTokenAsync(
            string apiUrl,
            string apiKeyId,
            string apiSecret
        )
        {
            HttpClient httpClient = new HttpClient();
            string authHeader = Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{apiKeyId}:{apiSecret}")
            );

            var requestBody = new StringContent(
                "grant_type=client_credentials",
                Encoding.UTF8,
                "application/x-www-form-urlencoded"
            );
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {authHeader}");

            var response = await httpClient.PostAsync($"{apiUrl}/v1/auth/token", requestBody);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            string authToken = jsonObj.access_token;

            return authToken;
        }

        public async Task<byte[]> GetAccessKey(ClientConfig config, string type)
        {
            var authToken = await GetAuthTokenAsync(
                config.ApiUrl,
                config.ApiKeyId,
                config.ApiSecret
            );
            string urlString =
                $"{config.ApiUrl}/v1/storage/access_keys/{config.ClientId}/{config.ClientId}/{config.ClientId}/{type}";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, urlString);
                request.Headers.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
                );

                HttpResponseMessage response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                dynamic responseObject = JsonConvert.DeserializeObject(responseBody);
                string eak = responseObject.eak;
                string[] eakFields = eak.Split('.');
                byte[] cipherBytes = Base64UrlDecode(eakFields[0]);
                byte[] nonceBytes = Base64UrlDecode(eakFields[1]);
                byte[] pubKeyBytes = Base64UrlDecode(config.PublicKey);
                byte[] privKeyBytes = Base64UrlDecode(config.PrivateKey);
                // Decrypt the encrypted access key
                byte[] akBytes = PublicKeyBox.Open(
                    cipherBytes,
                    nonceBytes,
                    privKeyBytes,
                    pubKeyBytes
                );
                return akBytes;
            }
        }

        public static byte[] Base64UrlDecode(string input)
        {
            string s = input.Replace('-', '+').Replace('_', '/');
            switch (s.Length % 4)
            {
                case 2:
                    s += "==";
                    break;
                case 3:
                    s += "=";
                    break;
            }
            return Convert.FromBase64String(s);
        }

        public string DecryptRecord(byte[] accessKey, string encryptedData)
        {
            string[] dottedQuad = encryptedData.Split('.');
            // Decode data key
            byte[] edkBytes = Base64UrlDecode(dottedQuad[0]);
            byte[] edkNBytes = Base64UrlDecode(dottedQuad[1]);
            var dataKey = SecretBox.Open(edkBytes, edkNBytes, accessKey);
            // Decode data
            byte[] efBytes = Base64UrlDecode(dottedQuad[2]);
            byte[] efNBytes = Base64UrlDecode(dottedQuad[3]);
            var data = SecretBox.Open(efBytes, efNBytes, dataKey);
            var plainText = System.Text.Encoding.Default.GetString(data);
            return plainText;
        }

        public JArray ConvertToJsonArray(object jsonData)
        {
            if (jsonData is JArray array)
            {
                return array;
            }

            if (jsonData is string jsonString)
            {
                JToken jsonToken = JToken.Parse(jsonString);
                if (jsonToken is JArray)
                    return (JArray)jsonToken;
                if (jsonToken is JObject)
                    return new JArray(jsonToken.Children<JProperty>().Select(jp => jp.Value));
            }

            if (jsonData is JObject jsonObject)
            {
                return new JArray(jsonObject.Properties().Select(p => p.Value));
            }

            throw new ArgumentException("Unsupported type of jsonData");
        }

        public string DecryptRecordFromJson(byte[] accessKey, object record)
        {
            // Convert record to JSON array
            var recordArray = ConvertToJsonArray(record);

            // Initialize up decrypted data and metadata
            var decryptedData = new Dictionary<string, string>();
            var metadata = new Dictionary<string, string>();

            if (recordArray.Count > 0)
            {
                // Get the encrypted record data
                JObject data = (JObject)recordArray[0];
                foreach (var property in data.Properties())
                {
                    // Decrypt the data
                    var plainText = DecryptRecord(accessKey, property.Value.ToString());
                    decryptedData.Add(property.Name.ToString(), plainText);
                }

                // Get the plaintext record metadata
                JObject meta = (JObject)recordArray[1];
                foreach (var property in meta.Properties())
                {
                    metadata.Add(property.Name.ToString(), property.Value.ToString());
                }
            }

            // Construct the decrypted record
            JArray resultArray = new JArray
            {
                JObject.FromObject(decryptedData),
                JObject.FromObject(metadata)
            };
            return resultArray.ToString();
        }
    }
}
