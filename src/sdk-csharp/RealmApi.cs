using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace Tozny.Auth
{
	public class RealmRequest : ApiRequest
	{
		public RealmRequest(String method) : base("realm." + method) { }
	}

	public class RealmApi : BaseApi
    {
		private String realmSecret;

		public RealmApi(String realmKeyId, String realmSecret): base(realmKeyId)
		{
			this.realmSecret = realmSecret;
		}

		public RealmApi(String apiUrl, String realmKeyId, String realmSecret) : base(apiUrl, realmKeyId)
		{
			this.realmSecret = realmSecret;
		}

		public async Task user_add(Boolean defer, Dictionary<String, String> fields, String public_key)
		{
			var request = new RealmRequest("user_add");
			request.Add("defer", defer ? "true" : "false");
			if (fields.Count > 0)
			{
				request.Add("extra_fields", encodedMap(fields));
			}
			if (! defer)
			{
				request.Add("pub_key", public_key);
			}

			try
			{
				dynamic response = await this.rawCall(request);

				if (response.GetValue("return") == "ok")
				{

				}
			} catch(Exception e)
			{

			}
		}

		public async Task user_update(String user_id, Dictionary<String, String> fields)
		{

		}

		public async Task question_challenge(String user_id, String question)
		{
			var request = new RealmRequest("question_challenge");
			request.Add("user_id", user_id);
			request.Add("question", question);

			try
			{
				dynamic response = await this.rawCall(request);
			} catch(Exception e)
			{

			}
		}

		public async Task user_push(String user_id, String session_id)
		{

		}

		protected override async Task<JObject> rawCall(ApiRequest request)
		{
			using (var client = new HttpClient())
			{
				var requestArgs = new Dictionary<String, String>();
				requestArgs.Add("method", request.Method);
				requestArgs.Add("realm_key_id", this.realmKeyId);
				requestArgs.Add("nonce", getNonce());
				requestArgs.Add("expires_at", getExpires());

				foreach (KeyValuePair<string, string> parameter in request)
				{
					requestArgs.Add(parameter.Key, parameter.Value);
				}

				// Encode and sign
				var data = encodedMap(requestArgs);
				var signed = this.sign(data);

				var encodedArgs = new Dictionary<String, String>();
				encodedArgs.Add("signed_data", data);
				encodedArgs.Add("signature", signed);

				var content = new FormUrlEncodedContent(encodedArgs);

				client.BaseAddress = new Uri(this.apiUrl);
				var response = await client.PostAsync("", content);

				var responseString = await response.Content.ReadAsStringAsync();

				dynamic responseObject = JObject.Parse(responseString);

				if (responseObject.GetValue("return") == "error")
				{
					throw new Exception("API Error");
				}
				else
				{
					return responseObject;
				}
			}
		}

		public String sign(String message)
		{
			var key = System.Text.ASCIIEncoding.ASCII.GetBytes(this.realmSecret);
			using(var hmac = new HMACSHA256(key))
			{
				var messageBytes = System.Text.ASCIIEncoding.UTF8.GetBytes(message);

				var hashValue = hmac.ComputeHash(messageBytes);
				return System.Convert.ToBase64String(hashValue).TrimEnd('=').Replace('+', '-').Replace('/', '_');
			}
		}

		protected static String encodedMap(Dictionary<String, String> fields)
		{
			var json = (JObject)JToken.FromObject(fields);
			var serialized = json.ToString(Formatting.None);
			var bytes = System.Text.ASCIIEncoding.UTF8.GetBytes(serialized);

			return System.Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');
		}

		protected static String getExpires()
		{
			var timestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

			return (timestamp + (5 * 60 * 1000)).ToString();

		}

		protected static String getNonce()
		{
			var bytes = new byte[32];
			var random = new Random();
			random.NextBytes(bytes);

			return BitConverter.ToString(bytes).Replace("-", "");
		}
	}
}
