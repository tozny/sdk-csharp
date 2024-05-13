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
                136,
                228,
                185,
                215,
                198,
                30,
                68,
                134,
                173,
                206,
                131,
                216,
                157,
                101,
                50,
                31,
                248,
                224,
                123,
                181,
                28,
                163,
                175,
                41,
                19,
                162,
                72,
                11,
                31,
                247,
                177,
                191
            };

            string recordAsJson =
                "{"
                + "\"Hola\": \"qlK8I8BxBFIXaolLTxEdT3pTOwr463Tw-G04Y-RzhweYzoIQghGDGohVI2QC1HHH.odxXRbEhb5aea_2f9KlU065pQKjNCjRB.BsXJPBCTSjI7fijPYBBdY-yVNNdC.Vz9U9K4Z9YNg7q1byBqncLXIJCs3CFXH\","
                + "\"Hello\": \"jIyckoBr2CaYUqhGEQgoD3nKf3L24kdo60yCCg_dH4dOBuSI_ntGlNptynoE_PzG.jHPPcO4tMub85Pb-zfVFhBvKOETU19HN.6L0v4MrTvC1HKRVdJ8E0fy--_66L.92KBaICMD-YzOidw2ZpLpmtrK0DVzCB8\","
                + "\"Plain Meta\": {"
                + "\"Type\": \"Example\""
                + "}"
                + "}";
            var decryptedRecord = recordApi.DecryptRecordFromJson(accessKey, recordAsJson);
            string expectedJson =
                "{\"Hola\":\"Mundo\",\"Hello\":\"World\",\"Plain Meta\":\"{\\\"Type\\\": \\\"Example\\\"}\"}";
            var expectedJObject = JObject.Parse(expectedJson);
            var actualJObject = JObject.Parse(decryptedRecord);
            Assert.True(JToken.DeepEquals(expectedJObject, actualJObject));
        }

        [Fact]
        public async void TestRecordApiShouldFetchAccessKey()
        {
            var recordApi = new RecordApi();

            var clientConfig = new ClientConfig
            {
                ApiUrl = "https://api.e3db.com",
                ApiKeyId = "d99d32fdfcd129bf64e35bc8ee9eca59c7d373c65a94939d47992efe26128e4e",
                ApiSecret = "0e716af806f296165a3cdd5a22ad88b97d1ce05fbee481f74d5741d38ef2d0ab",
                WriterId = "63a7cf10-ca6f-426b-abdf-51db543f72d4",
                ReaderId = "63a7cf10-ca6f-426b-abdf-51db543f72d4",
                PublicKey = "7ipGnoerFVcBfpfw1M4CEWKbZ94zHGPUEHxuMhvq3Tw",
                PrivateKey = "ifhP835PF7pRgCLHxfO6Oth_jegmPIGxrcDBgS1wrDo"
            };

            byte[] accessKey = await recordApi.GetAccessKey(clientConfig, "example");

            byte[] expectedAccessKey = new byte[]
            {
                136,
                228,
                185,
                215,
                198,
                30,
                68,
                134,
                173,
                206,
                131,
                216,
                157,
                101,
                50,
                31,
                248,
                224,
                123,
                181,
                28,
                163,
                175,
                41,
                19,
                162,
                72,
                11,
                31,
                247,
                177,
                191
            };
            Assert.Equal(expectedAccessKey, accessKey);
        }
    }
}
