using Tozny.Auth;
using Xunit;

namespace TozSdkTest
{
    public class RecordApiTests
    {
        [Fact]
        public void TestRecordApi_ShouldPrintHelloWorld()
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
            // string encryptedRecord =
            //     "fK4sK2PLeCq_KdwwdmkCFp736mFl9upxIFDAELM_eIFuQ3emjhFWZozDZJRt9I74.Aq4kJOuqfw-SaEVATrK75YerPDan8Rfq.1eOsaPvJ_QoWpaHzMpodOdokt6M5.xDYR8MnCWlRmCUtgcYc8LvXjsfiPIkQh";

            // recordApi.DecryptRecord(accessKey, encryptedRecord);

            string recordAsJson =
                "{ \"meta\": { \"record_id\": \"894a3063-89c5-47ad-a211-28b194ca5235\", \"user_id\": \"cdb1ffdb-e483-4c4f-9203-db18c1230c9c\", \"last_modified\": \"2024-05-07T18:21:55.470283Z\", \"version\": \"c6d77af0-2494-44af-a079-7c8f979b1913\", \"plain\": { \"hola\": \"mundo\" }, \"access_key_id\": \"4b7b5336-e304-4559-bcd0-f33fde9b2f90\", \"type\": \"test\", \"file_meta\": null, \"writer_id\": \"cdb1ffdb-e483-4c4f-9203-db18c1230c9c\", \"created\": \"2024-05-07T18:21:55.470283Z\" }, \"data\": { \"hello\": \"eXe3NPDQg3Uk-q_oQEDP3XWa1qTvESs21v_436mWbeh1VjdLPLupkkECTr6YF8iX.KupVC7EuKd5BIrlJ83RgJ4shYvVYPxJY.MKChHMri0_mZT3nk4c2U_Yx57RDq.gEkWI_F_0ai0nYmu9gMZnqy5eJubWdP1\", \"hello2\": \"ec-r4rXBOWMifdGJE0E3H5SNIzVMlYfpYfOTTOHtUCty1Y_LUJ8Q9MynhDa2MalL.HYRF8Rjn9vh9fOQvT-ZHGSW3J8v-qTfe.k6ij8kAyLndCzBtMV95aJZfv9ItEgw.37s4pAsEyW2_nhx3I9hhGcD_gL6L2KwK\" }, \"rec_sig\": \"TBVb0cjRpzqc7WmLGB04P1HE4pZ3bENvsXPB9_ssorv9Wbzmd0Skp1r9PlfyNio5Dr_y8bypL842BP8Aaxi-Ag\" }";

            var decryptedRecord = recordApi.DecryptRecordFromJson(accessKey, recordAsJson);
            foreach (var kvp in decryptedRecord)
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
            }
        }
    }
}
