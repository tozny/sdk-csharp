using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using Sodium;

namespace Tozny.Auth
{
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

        public async Task<byte[]> GetAccessKey(
            string apiUrl,
            string apiKeyId,
            string apiSecret,
            string writerId,
            string readerId,
            string publicKey,
            string privateKey,
            string type
        )
        {
            var authToken = await GetAuthTokenAsync(apiUrl, apiKeyId, apiSecret);
            string urlString =
                $"{apiUrl}/v1/storage/access_keys/{writerId}/{writerId}/{readerId}/{type}";

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
                byte[] pubKeyBytes = Base64UrlDecode(publicKey);
                byte[] privKeyBytes = Base64UrlDecode(privateKey);
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

        public bool IsNotMetaTag(string input)
        {
            string lowercaseInput = input.ToLower(); // Convert input string to lowercase

            return !lowercaseInput.Contains("meta") && !lowercaseInput.Contains("plain");
        }

        public string DecryptRecordFromJson(byte[] accessKey, string recordJson)
        {
            var jsonDocument = JsonDocument.Parse(recordJson);
            var decryptedRecord = new Dictionary<string, string>();
            foreach (var element in jsonDocument.RootElement.EnumerateObject())
            {
                if (IsNotMetaTag(element.Name))
                {
                    var plainText = DecryptRecord(accessKey, element.Value.ToString());

                    decryptedRecord.Add(element.Name.ToString(), plainText);
                }
                else
                {
                    decryptedRecord.Add(element.Name.ToString(), element.Value.ToString());
                }
            }

            string decryptedRecordJson = JsonConvert.SerializeObject(decryptedRecord);

            return decryptedRecordJson;
        }
    }
}
