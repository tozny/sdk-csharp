# Tozny's C# Software Developers Kit

The Tozny Platform offers powerful tools for developers, enabling them to incorporate strong end-to-end encryption into their code bases. The Software Developer Kits provide the tools necessary for implementing the Tozny Platform without needing high cryptography expertise.

## Testing

### Records

Before you begin testing, you'll need a TOZ Store client config, which you can obtain through several methods. One approach is to visit the [Tozny Dashboard](https://dashboard.tozny.com/) and create an account. Then navigate to TOZ Store > Tokens to create a registration token. After that, proceed to TOZ Store > Clients to set up a new client. Click on the name of the client you've created, then locate and select tozny.key_backup and copy the client config. Another approach is to use one of our Software Developers Kits, such as our [JavaScript SDK](https://github.com/tozny/js-sdk).

The next thing you need to decrypt a record is an encrypted TOZ Store record. You can create an encrypted record via the Dashboard or one of our SDKs. There is no direct way to obtain the encrypted record stored on our server, however there are several workarounds. Here are two suggestions:

- **Using the JavaScript SDK:** Clone a local copy of the js-sdk to your device. Add `console.log(record)` before decrypting the record [here](https://github.com/tozny/js-sdk/blob/c5176da3ae681bcc7077ec7b325c5922d564096d/lib/storage/client.js#L201). Import the local copy of the js-sdk to a node project and follow the js-sdk documentation to create and read a record. When reading the record, the encrypted record will now print to your console.
- **Using the Dashboard:** Select any client in your dashboard and click the + button next to the Records header. Click on the newly created record and locate your network requests in your browser. Select the request for `https://api.e3db.com/v1/storage/records/[record ID]`. The encrypted record will be printed in the response. All encrypted data is listed under `data`.

Now that you have an encrypted record and a client config, you can fetch an access key and decrypt your record.

Update the `TestRecordApiShouldFetchAccessKey` test with your client config details.

Update the `TestRecordApiShouldDecryptRecord` test with your access key and encrypted record. The record is formatted as a JSON string:

```bash
   "{"
    + "\"Data Key 1\": \"[Encrypted Data 1]\","
    + "\"Data Key 2\": \"[Encrypted Data 2]\","
    + "\"Plain Meta\": {"
    + "\"Type\": \"[Record type]\","
    + "\"Meta Key 1\": \"[Plaintext data 1]\""
    + "}"
    + "}";
```

To run the tests in RecordApiTest.cs, run `dotnet test --filter "FullyQualifiedName~TozSdkTest.RecordApiTests"`
