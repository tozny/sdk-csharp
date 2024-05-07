using Sodium;

namespace Tozny.Auth
{
    public class RecordApi
    {
        public void DecryptRecord(byte[] accessKey, string encryptedRecord)
        {
            string[] dataFields = encryptedRecord.Split('.');
            string edk = dataFields[0];
            string edkN = dataFields[1];
            string ef = dataFields[2];
            string efN = dataFields[3];

            // Decode data key
            byte[] edkBytes = Base64UrlDecode(edk);
            byte[] edkNBytes = Base64UrlDecode(edkN);
            var dataKey = SecretBox.Open(edkBytes, edkNBytes, accessKey);
            // Decode data
            byte[] efBytes = Base64UrlDecode(ef);
            byte[] efNBytes = Base64UrlDecode(efN);
            var data = SecretBox.Open(efBytes, efNBytes, dataKey);
            var plainText = System.Text.Encoding.Default.GetString(data);
            Console.WriteLine(plainText);
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
