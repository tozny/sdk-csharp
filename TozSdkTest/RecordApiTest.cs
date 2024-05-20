using Newtonsoft.Json.Linq;
using Tozny.Auth;
using Xunit;

namespace TozSdkTest
{
    public class RecordApiTests
    {
        [Fact]
        public void TestRecordApiShouldDecryptRecord()
        {
            var recordApi = new RecordApi();

            byte[] accessKey = new byte[]
            {
                166,
                119,
                10,
                55,
                111,
                166,
                6,
                9,
                168,
                148,
                176,
                139,
                212,
                28,
                53,
                35,
                135,
                85,
                11,
                135,
                117,
                219,
                114,
                49,
                184,
                121,
                242,
                189,
                120,
                53,
                200,
                75
            };

            string recordAsJson =
                @"[
            {
                'batteryRemaining': '82QEc9X0e-h3SFpgBJRYBQ1PLOtlgY1DOAy6aqpPwb-OgM-X6u7VQp4yy9UjUsaN.6FuzjwI2NsGB5lnGx8WF3ejdu0DmeobP.3SwXjJRIPrlfIaa0k4W2OZSq.nH6f5IDP2katGPTe6LhhDStvtjSb3UrP',
                'timestamp': 'dGZDeu1ia3LZx9F1_LgICy4x_dtr4rGVsTfGMsalOEZfGP9I5iE5HsB6ZU87AlHe.GerMBCWMua8xTm5DwJddAVh9f5jwriY4.V1LXySajIXjly4_uWC9tDtzm0xI-8uNuvqfwUHg.wK63ySHfp1h-SvVXV03EYt3irYFFQgAk',
                'grossStatus': 'jwVbHzt3gKi3J9w2_TgllMdvmshYVWenQuBUIE051QaZJtW8U-lxqWdkecqqDHVX.Sq_oOIHTcpIYZL6hyP85t2IW13aus_Eh.JN6O9S7tP7sgkVmExZS397iiVHc6Kw.KWaf0qkKYTLTAbdGiIZPqFEmOP5eB3FO',
                'rssiWifi': 'kHNdRXlURLkrqLm-PHX1uQIl0S3Be03umS_rjJTDUT7HxJiJrX9bbtrl0QXvqkwO.BDKm3xaxIJT6p10BUCqExmBKYii5hPPr.T-QZXU4Zx6Qmo1U2iJSwlm-o.WsrTRvKtiQhgZhOda6BwZpg--kRLpHLz',
                'rssiUwb': 'g6_G8vVn1yWMe5LNwXPEt43eq4nJk9yeMkqY5ftMCbI6BfE4YH-X3AJ45eQGk-k7.N_sAdwKMXu7Mx3m9KbiTrzKMZSzy3-m1.sNoyGH5niw3NUjkMlwHJXzyh.-CgHhqHZtPC4MIpOs5iJLArasC6bD92n',
                'zoneInfo': 'PSZGc3yj360vpE7xR0kaDInvcjmG8QrNirZirZvH0TaoytphLi-v_nyGov9R7MF9.rMr7AdJ5LxwwDtoN2Bl7p-ay_DT8X5cj.0J-MUanmFpyu_XF13xrujCCjCpSoOg._U0k53v2ls5HSBpJSlc_-y3ka5xJYOrm',
                'batteryRaw': 'Aa4t995bYNlFPb7IN45hJ9EwsoZblIQgFr8bOtxHSqwOFgTN4gxvkLbOQK1OHVuM.kFmjEdpbhD86WHvNf8i27JvZ57mXiwsT.6OIpnrdz4TisQprJJP7U99Q1.6je1M7Cf48tOshF5yvkh_JxFRhlGWUk8',
            },
            {
                'eventType': 'telemetry',
                'gatewayId': '9999999',
                'deviceType': 'fab',
                'dataType': 'deviceHealthMetrics',
                'deviceId': '6666666'
            }
            ]";

            var decryptedRecord = recordApi.DecryptRecordFromJson(accessKey, recordAsJson);
            Console.WriteLine(decryptedRecord);

            string expectedRecord =
                "[{\"batteryRemaining\":\"10\",\"timestamp\":\"1707901342000\",\"grossStatus\":\"active\",\"rssiWifi\":\"13\",\"rssiUwb\":\"23\",\"zoneInfo\":\"zone-1\",\"batteryRaw\":\"20\"},{\"eventType\":\"telemetry\",\"gatewayId\":\"9999999\",\"deviceType\":\"fab\",\"dataType\":\"deviceHealthMetrics\",\"deviceId\":\"6666666\"}]";

            JArray expectedJArray = JArray.Parse(expectedRecord);
            JArray actualJArray = JArray.Parse(decryptedRecord);

            Assert.True(
                JToken.DeepEquals(expectedJArray, actualJArray),
                "The expected decrypted record does not match the actual decrypted record."
            );
        }

        [Fact]
        public async void TestRecordApiShouldFetchAccessKey()
        {
            var recordApi = new RecordApi();

            var clientConfig = new ClientConfig
            {
                ApiUrl = "https://api.e3db.com",
                ApiKeyId = "823a50c3552936d2769259f0e1499ec3ddebcaae711269d47b6ae252a6c81450",
                ApiSecret = "7005dbd6c58e10e152ab9287cf8608615d12d0b11c7626a914c1cc035b444675",
                ClientId = "b82f98a3-46ed-495b-98e7-c13ac91be718",
                PublicKey = "aMv9gw1tVPDKog3Nbhbcny5396KlzLElnC8hQmKuRhk",
                PrivateKey = "bm0OtyVQi1F5j6lPaFjUwZKtl9C4oaJVQCJy0S6guxE"
            };

            byte[] accessKey = await recordApi.GetAccessKey(clientConfig, "deviceHealthMetrics");

            Console.Write("[");
            foreach (byte b in accessKey)
            {
                Console.Write($"{b}, ");
            }
            Console.WriteLine("]");

            byte[] expectedAccessKey = new byte[]
            {
                166,
                119,
                10,
                55,
                111,
                166,
                6,
                9,
                168,
                148,
                176,
                139,
                212,
                28,
                53,
                35,
                135,
                85,
                11,
                135,
                117,
                219,
                114,
                49,
                184,
                121,
                242,
                189,
                120,
                53,
                200,
                75
            };
            Assert.Equal(expectedAccessKey, accessKey);
        }
    }
}
