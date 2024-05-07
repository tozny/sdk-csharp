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
            string encryptedRecord =
                "fK4sK2PLeCq_KdwwdmkCFp736mFl9upxIFDAELM_eIFuQ3emjhFWZozDZJRt9I74.Aq4kJOuqfw-SaEVATrK75YerPDan8Rfq.1eOsaPvJ_QoWpaHzMpodOdokt6M5.xDYR8MnCWlRmCUtgcYc8LvXjsfiPIkQh";

            recordApi.DecryptRecord(accessKey, encryptedRecord);
        }
    }
}
