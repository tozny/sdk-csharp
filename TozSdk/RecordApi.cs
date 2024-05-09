using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using Sodium;

namespace Tozny.Auth
{
    public class RecordApi
    {
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
