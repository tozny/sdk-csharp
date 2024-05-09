using Tozny.Auth;

namespace TozSdkTest
{
    public class RecordApiTests
    {
        [Fact]
        public void TestRecordApiDecryptsRecord()
        {
            var recordApi = new RecordApi();

            byte[] accessKey = new byte[]
            {
                // <Your access key here as byte array>
            };

            string recordAsJson = "<Your record here formatted as JSON";

            // Example:
            // "{"
            //     + "\"First Name\": \"wflFJcZ5iTVpV5VdtjvcQCEvgIUvwBMWFHhLTLAmHuBOslmB9CuyzOCacXfQ_2Xz._OF1MV9zOcPUvfgF7twr1aN5U8P8_2Ke.2922mOVXlu-H6N6fZsf3QekyRDON.ujXUtGrKcMkk0jxCbeRkbIC3MmWddms3\","
            //     + "\"Last Name\": \"2uwUnbzN-1rlzso-bSwdJRtaj4uBhbQTiy4a0Eec0AEbkXYfi1LfGU6yp9e3CfSc.GaxBvlOlc4qJUksWkx882C0-ald1-FRT.xwSbF_W8PF0iigXQ7gyllDe7n4ocTBcj.yXOqxpWrioaldmnv4LdPgb5RjtX2MP5i\","
            //     + "\"Phone Number\": \"1xbnH670yt61UF45jz3BHKKnAjFGF3V5JL9Kpp9VIncPHFA84E35y_GFvZb_p4br.R84G_Q9mkT58LHwm7uFrfcyBkUntZ33d.m5bI9tO_k2kcN3xI4WEny8_5dDvoe358i341xg==.8PqhuJgS1YpmHnVuEZo_wXk3Ad4Llla2\","
            //     + "\"Hourly Pay\": \"nS3sApRGL9s-P9DndBxKl0Xt0qxhP71VdgUli6SYhdYM_eyn0zmdgpLfTc4OIGIT.J-8w4eAxVtJVAqEzRDV3j_cs3PmxvBks.YjckQVY6kjCbWOpGsxXzTP4i.c1_gIA1RGaCMZRAHHvF8WSpQ2vkHZVBM\","
            //     + "\"Max Hours Allowed\": \"BBXXSaoaO8SJvt3Vhw5HWCLJ3Cp5wZNHDQxUnIRz4WRD47NCHznFOMP_xFe4K1zC.G1SMjkW_XqJedvAlebu0tJyK7arctGE9.sJ2i873J_45-tygkFIJtczLf.Sj_YUXB3WldbS5ZOGE2IV5gMcBm0RP2e\","
            //     + "\"Plain Meta\": {"
            //     + "\"Type\": \"Employee\","
            //     + "\"Company\": \"Tozny\","
            //     + "\"Team\": \"Software\""
            //     + "}"
            //     + "}";
            var decryptedRecord = recordApi.DecryptRecordFromJson(accessKey, recordAsJson);
            Console.WriteLine(decryptedRecord);
        }
    }
}
