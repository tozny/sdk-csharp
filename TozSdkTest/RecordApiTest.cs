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
                216,
                36,
                111,
                96,
                219,
                190,
                193,
                124,
                164,
                160,
                68,
                132,
                125,
                32,
                39,
                187,
                19,
                147,
                177,
                135,
                98,
                76,
                207,
                97,
                53,
                126,
                99,
                9,
                90,
                202,
                161,
                75
            };

            string recordAsJson =
                "{"
                + "\"Hello\": \"eXe3NPDQg3Uk-q_oQEDP3XWa1qTvESs21v_436mWbeh1VjdLPLupkkECTr6YF8iX.KupVC7EuKd5BIrlJ83RgJ4shYvVYPxJY.MKChHMri0_mZT3nk4c2U_Yx57RDq.gEkWI_F_0ai0nYmu9gMZnqy5eJubWdP1\","
                + "\"Hello2\": \"ec-r4rXBOWMifdGJE0E3H5SNIzVMlYfpYfOTTOHtUCty1Y_LUJ8Q9MynhDa2MalL.HYRF8Rjn9vh9fOQvT-ZHGSW3J8v-qTfe.k6ij8kAyLndCzBtMV95aJZfv9ItEgw.37s4pAsEyW2_nhx3I9hhGcD_gL6L2KwK\","
                + "\"Plain Meta\": {"
                + "\"Type\": \"Test\""
                + "}"
                + "}";
            var decryptedRecord = recordApi.DecryptRecordFromJson(accessKey, recordAsJson);
            Console.WriteLine(decryptedRecord);
        }

        [Fact]
        public async void TestRecordApiShouldFetchAuthToken()
        {
            var recordApi = new RecordApi();

            var accessKey = await recordApi.GetAccessKey(
                "https://api.e3db.com",
                "7e1310b28746ba91849336028d566d9de8014b86c4bc2bfcd9747b9ad2f9c536",
                "7f1c10f5251a1e2b1fe8259c10edc96933d5912fce2d8debbd486cb98ce6fc31",
                "cdb1ffdb-e483-4c4f-9203-db18c1230c9c",
                "cdb1ffdb-e483-4c4f-9203-db18c1230c9c",
                "C-nEyHg-yElIiyjUN0H6SMQewKu7sxZo3YwfEhwNHVA",
                "hXNziK_9GjcwrXBnUX9vTGT0iVxjXFeWVtxrBGdgWK8",
                "test"
            );
            foreach (byte b in accessKey)
            {
                Console.Write($"{b} ");
            }
        }
    }
}
