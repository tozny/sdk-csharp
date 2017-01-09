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

		public class UserAddResponse
		{
			protected String user_id;
			protected String user_key_id;
			protected String temp_user_secret;
			protected String secret_enrollment_url;
			protected String secret_enrollment_qr_url;
			protected String realm_id;
			protected String realm_key_id;
			protected String logo_url;
			protected String info_url;
			protected String crypto_suite;
			protected String display_name;

			public UserAddResponse(
				String user_id,
				String user_key_id,
				String temp_user_secret,
				String secret_enrollment_url,
				String secret_enrollment_qr_url,
				String realm_id,
				String realm_key_id,
				String logo_url,
				String info_url,
				String crypto_suite,
				String display_name
				)
			{
				this.user_id = user_id;
				this.user_key_id = user_key_id;
				this.temp_user_secret = temp_user_secret;
				this.secret_enrollment_url = secret_enrollment_url;
				this.secret_enrollment_qr_url = secret_enrollment_qr_url;
				this.realm_id = realm_id;
				this.realm_key_id = realm_key_id;
				this.logo_url = logo_url;
				this.info_url = info_url;
				this.crypto_suite = crypto_suite;
				this.display_name = display_name;
			}

			public String UserID { get { return this.user_id; } }
			public String UserKeyId { get { return this.user_key_id; } }
			public String TempUserSecret { get { return this.temp_user_secret; } }
			public String SecretEnrollmentUrl { get { return this.secret_enrollment_url; } }
			public String SecretEnrollmentQrUrl { get { return this.secret_enrollment_qr_url; } }
			public String RealmId { get { return this.realm_id; } }
			public String RealmKeyId { get { return this.realm_key_id; } }
			public String LogoUrl { get { return this.logo_url; } }
			public String InfoUrl { get { return this.info_url; } }
			public String CryptoSuite { get { return this.crypto_suite; } }
			public String DisplayName { get { return this.display_name; } }
		}

		public class UserUpdateResponse
		{
			protected String username;
			protected String user_id;
			protected String user_temp_key;
			protected String secret_enrollment_url;
			protected String secret_enrollment_qr_url;
			protected String logo_url;
			protected String info_url;
			protected String crypto_suite;
			protected String display_name;

			public UserUpdateResponse(
				String username,
				String user_id,
				String user_temp_key,
				String secret_enrollment_url,
				String secret_enrollment_qr_url,
				String logo_url,
				String info_url,
				String crypto_suite,
				String display_name
				)
			{
				this.username = username;
				this.user_id = user_id;
				this.user_temp_key = user_temp_key;
				this.secret_enrollment_url = secret_enrollment_url;
				this.secret_enrollment_qr_url = secret_enrollment_qr_url;
				this.logo_url = logo_url;
				this.info_url = info_url;
				this.crypto_suite = crypto_suite;
				this.display_name = display_name;
			}

			public String Username { get { return this.username; } }
			public String UserID { get { return this.user_id; } }
			public String UserTempKey { get { return this.user_temp_key; } }
			public String SecretEnrollmentUrl { get { return this.secret_enrollment_url; } }
			public String SecretEnrollmentQrUrl { get { return this.secret_enrollment_qr_url; } }
			public String LogoUrl { get { return this.logo_url; } }
			public String InfoUrl { get { return this.info_url; } }
			public String CryptoSuite { get { return this.crypto_suite; } }
			public String DisplayName { get { return this.display_name; } }
		}

		public class QuestionChallenge
		{
			protected String challenge;
			protected String session_id;
			protected String realm_key_id;
			protected String qr_url;
			protected String mobile_url;
			protected Int32 created_at;
			protected String presence;

			public QuestionChallenge(
				String challenge,
				String session_id,
				String realm_key_id,
				String qr_url,
				String mobile_url,
				Int32 created_at,
				String presence
				)
			{
				this.challenge = challenge;
				this.session_id = session_id;
				this.realm_key_id = realm_key_id;
				this.qr_url = qr_url;
				this.mobile_url = mobile_url;
				this.created_at = created_at;
				this.presence = presence;
			}

			public String Challenge { get { return this.challenge; } }
			public String SessionId { get { return this.session_id; } }
			public String RealmKeyId { get { return this.realm_key_id; } }
			public String QrUrl { get { return this.qr_url; } }
			public String MobileUrl { get { return this.mobile_url; } }
			public Int32 CreatedAt { get { return this.created_at; } }
			public String Presence { get { return this.presence; } }
		}
		
		public async Task<UserAddResponse> user_add(Boolean defer, Dictionary<String, String> fields, String public_key)
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

			dynamic response = await this.rawCall(request);

			if (response.GetValue("return") == "ok")
			{
				String user_id = response.GetValue("user_id");
				String user_key_id = response.GetValue("user_key_id");
				String temp_user_secret = response.GetValue("temp_user_secret");
				String secret_enrollment_url = response.GetValue("secret_enrollment_url");
				String secret_enrollment_qr_url = response.GetValue("secret_enrollment_qr_url");
				String realm_id = response.GetValue("realm_id");
				String realm_key_id = response.GetValue("realm_key_id");
				String logo_url = response.GetValue("logo_url");
				String info_url = response.GetValue("info_url");
				String crypto_suite = response.GetValue("crypto_suite");
				String display_name = response.GetValue("display_name");

				return new UserAddResponse(
					user_id,
					user_key_id,
					temp_user_secret,
					secret_enrollment_url,
					secret_enrollment_qr_url,
					realm_id,
					realm_key_id,
					logo_url,
					info_url,
					crypto_suite,
					display_name
					);
			} else
			{
				throw new Exception("API Error");
			}
		}

		public async Task<UserUpdateResponse> user_update(String user_id, Dictionary<String, String> fields)
		{
			var request = new RealmRequest("user_update");
			request.Add("user_id", user_id);
			request.Add("extra_fields", encodedMap(fields));

			dynamic response = await this.rawCall(request);

			if (response.GetValue("return") == "ok")
			{
				String username = response.GetValue("username");
				String server_user_id = response.GetValue("user_id");
				String user_temp_key = response.GetValue("user_temp_key");
				String secret_enrollment_url = response.GetValue("secret_enrollment_url");
				String secret_enrollment_qr_url = response.GetValue("secret_enrollment_qr_url");
				String logo_url = response.GetValue("logo_url");
				String info_url = response.GetValue("info_url");
				String crypto_suite = response.GetValue("crypto_suite");
				String display_name = response.GetValue("display_name");

				return new UserUpdateResponse(
					username,
					server_user_id,
					user_temp_key,
					secret_enrollment_url,
					secret_enrollment_qr_url,
					logo_url,
					info_url,
					crypto_suite,
					display_name
					);
			}
			else
			{
				throw new Exception("API Error");
			}
		}

		public async Task<QuestionChallenge> question_challenge(String user_id, String question)
		{
			var request = new RealmRequest("question_challenge");
			request.Add("user_id", user_id);
			request.Add("question", question);

			dynamic response = await this.rawCall(request);

			if (response.GetValue("return") == "ok")
			{
				String challenge = response.GetValue("challenge");
				String session_id = response.GetValue("session_id");
				String realm_key_id = response.GetValue("realm_key_id");
				String qr_url = response.GetValue("qr_url");
				String mobile_url = response.GetValue("mobile_url");
				Int32 created_at = response.GetValue("created_at");
				String presence = response.GetValue("presence");

				return new QuestionChallenge(challenge, session_id, realm_key_id, qr_url, mobile_url, created_at, presence);
			}else
			{
				throw new Exception("API Error");
			}
		}

		public async Task<Boolean> user_push(String user_id, String session_id)
		{
			var request = new RealmRequest("user_push");
			request.Add("user_id", user_id);
			request.Add("session_id", session_id);

			dynamic response = await this.rawCall(request);

			return response.GetValue("return") == "ok";
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
