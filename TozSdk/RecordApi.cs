using System.Text.Json;
using Sodium;

namespace Tozny.Auth
{
    public class RecordApi
    {
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

        public string DecryptRecordFromJson(byte[] accessKey, string recordJson)
        {
            var jsonDocument = JsonDocument.Parse(recordJson);
            var decryptedRecord = new Dictionary<string, JsonElement>();
            foreach (var element in jsonDocument.RootElement.EnumerateObject())
            {
                if (element.Name == "data")
                {
                    var newData = new Dictionary<string, string>();
                    foreach (var key in element.Value.EnumerateObject())
                    {
                        var plainText = DecryptRecord(accessKey, key.Value.ToString());
                        newData.Add(key.Name, plainText);
                    }
                    decryptedRecord.Add(
                        element.Name.ToString(),
                        JsonDocument.Parse(JsonSerializer.Serialize(newData)).RootElement
                    );
                }
                else
                {
                    decryptedRecord.Add(element.Name.ToString(), element.Value);
                }
            }

            string decryptedRecordJson = JsonSerializer.Serialize(decryptedRecord);

            return decryptedRecordJson;
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
    }
}
