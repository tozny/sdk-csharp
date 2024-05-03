using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace Tozny.Auth
{
	public class RealmRequest : ApiRequest
	{
		public RealmRequest(string method) : base("realm." + method) { }
	}

	public class RealmApi : BaseApi
	{
		private string realmSecret;

		public RealmApi(string realmKeyId, string realmSecret) : base(realmKeyId)
		{
			this.realmSecret = realmSecret;
		}

		public RealmApi(string apiUrl, string realmKeyId, string realmSecret) : base(apiUrl, realmKeyId)
		{
			this.realmSecret = realmSecret;
		}

		public class UserAddResponse
		{
			protected string user_id;
			protected string user_key_id;
			protected string temp_user_secret;
			protected string secret_enrollment_url;
			protected string secret_enrollment_qr_url;
			protected string realm_id;
			protected string realm_key_id;
			protected string logo_url;
			protected string info_url;
			protected string crypto_suite;
			protected string display_name;

			public UserAddResponse(
				string user_id,
				string user_key_id,
				string temp_user_secret,
				string secret_enrollment_url,
				string secret_enrollment_qr_url,
				string realm_id,
				string realm_key_id,
				string logo_url,
				string info_url,
				string crypto_suite,
				string display_name
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

			public string UserID { get { return this.user_id; } }
			public string UserKeyId { get { return this.user_key_id; } }
			public string TempUserSecret { get { return this.temp_user_secret; } }
			public string SecretEnrollmentUrl { get { return this.secret_enrollment_url; } }
			public string SecretEnrollmentQrUrl { get { return this.secret_enrollment_qr_url; } }
			public string RealmId { get { return this.realm_id; } }
			public string RealmKeyId { get { return this.realm_key_id; } }
			public string LogoUrl { get { return this.logo_url; } }
			public string InfoUrl { get { return this.info_url; } }
			public string CryptoSuite { get { return this.crypto_suite; } }
			public string DisplayName { get { return this.display_name; } }
		}

		public class UserUpdateResponse
		{
			protected string username;
			protected string user_id;
			protected string user_temp_key;
			protected string secret_enrollment_url;
			protected string secret_enrollment_qr_url;
			protected string logo_url;
			protected string info_url;
			protected string crypto_suite;
			protected string display_name;

			public UserUpdateResponse(
				string username,
				string user_id,
				string user_temp_key,
				string secret_enrollment_url,
				string secret_enrollment_qr_url,
				string logo_url,
				string info_url,
				string crypto_suite,
				string display_name
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

			public string Username { get { return this.username; } }
			public string UserID { get { return this.user_id; } }
			public string UserTempKey { get { return this.user_temp_key; } }
			public string SecretEnrollmentUrl { get { return this.secret_enrollment_url; } }
			public string SecretEnrollmentQrUrl { get { return this.secret_enrollment_qr_url; } }
			public string LogoUrl { get { return this.logo_url; } }
			public string InfoUrl { get { return this.info_url; } }
			public string CryptoSuite { get { return this.crypto_suite; } }
			public string DisplayName { get { return this.display_name; } }
		}

		public class QuestionChallenge
		{
			protected string challenge;
			protected string session_id;
			protected string realm_key_id;
			protected string qr_url;
			protected string mobile_url;
			protected int created_at;
			protected string presence;

			public QuestionChallenge(
				string challenge,
				string session_id,
				string realm_key_id,
				string qr_url,
				string mobile_url,
				int created_at,
				string presence
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

			public string Challenge { get { return this.challenge; } }
			public string SessionId { get { return this.session_id; } }
			public string RealmKeyId { get { return this.realm_key_id; } }
			public string QrUrl { get { return this.qr_url; } }
			public string MobileUrl { get { return this.mobile_url; } }
			public int CreatedAt { get { return this.created_at; } }
			public string Presence { get { return this.presence; } }
		}

		public async Task<UserAddResponse> user_add(bool defer, Dictionary<string, string> fields, string public_key)
		{
			var request = new RealmRequest("user_add");
			request.Add("defer", defer ? "true" : "false");
			if (fields.Count > 0)
			{
				request.Add("extra_fields", encodedMap(fields));
			}
			if (!defer)
			{
				request.Add("pub_key", public_key);
			}

			dynamic response = await this.rawCall(request);

			if (response.GetValue("return") == "ok")
			{
				string user_id = response.GetValue("user_id");
				string user_key_id = response.GetValue("user_key_id");
				string temp_user_secret = response.GetValue("temp_user_secret");
				string secret_enrollment_url = response.GetValue("secret_enrollment_url");
				string secret_enrollment_qr_url = response.GetValue("secret_enrollment_qr_url");
				string realm_id = response.GetValue("realm_id");
				string realm_key_id = response.GetValue("realm_key_id");
				string logo_url = response.GetValue("logo_url");
				string info_url = response.GetValue("info_url");
				string crypto_suite = response.GetValue("crypto_suite");
				string display_name = response.GetValue("display_name");

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
			}
			else
			{
				throw new Exception("API Error");
			}
		}

		public async Task<UserUpdateResponse> user_update(string user_id, Dictionary<string, string> fields)
		{
			var request = new RealmRequest("user_update");
			request.Add("user_id", user_id);
			request.Add("extra_fields", encodedMap(fields));

			dynamic response = await this.rawCall(request);

			if (response.GetValue("return") == "ok")
			{
				string username = response.GetValue("username");
				string server_user_id = response.GetValue("user_id");
				string user_temp_key = response.GetValue("user_temp_key");
				string secret_enrollment_url = response.GetValue("secret_enrollment_url");
				string secret_enrollment_qr_url = response.GetValue("secret_enrollment_qr_url");
				string logo_url = response.GetValue("logo_url");
				string info_url = response.GetValue("info_url");
				string crypto_suite = response.GetValue("crypto_suite");
				string display_name = response.GetValue("display_name");

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

		public async Task<QuestionChallenge> question_challenge(string user_id, string question)
		{
			var request = new RealmRequest("question_challenge");
			request.Add("user_id", user_id);
			request.Add("question", question);

			dynamic response = await this.rawCall(request);

			if (response.GetValue("return") == "ok")
			{
				string challenge = response.GetValue("challenge");
				string session_id = response.GetValue("session_id");
				string realm_key_id = response.GetValue("realm_key_id");
				string qr_url = response.GetValue("qr_url");
				string mobile_url = response.GetValue("mobile_url");
				int created_at = response.GetValue("created_at");
				string presence = response.GetValue("presence");

				return new QuestionChallenge(challenge, session_id, realm_key_id, qr_url, mobile_url, created_at, presence);
			}
			else
			{
				throw new Exception("API Error");
			}
		}

		public async Task<bool> user_push(string user_id, string session_id)
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
				var requestArgs = new Dictionary<string, string>();
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

				var encodedArgs = new Dictionary<string, string>();
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

		public string sign(string message)
		{
			var key = System.Text.ASCIIEncoding.ASCII.GetBytes(this.realmSecret);
			using (var hmac = new HMACSHA256(key))
			{
				var messageBytes = System.Text.ASCIIEncoding.UTF8.GetBytes(message);

				var hashValue = hmac.ComputeHash(messageBytes);
				return System.Convert.ToBase64String(hashValue).TrimEnd('=').Replace('+', '-').Replace('/', '_');
			}
		}

		protected static string encodedMap(Dictionary<string, string> fields)
		{
			var json = (JObject)JToken.FromObject(fields);
			var serialized = json.ToString(Formatting.None);
			var bytes = System.Text.ASCIIEncoding.UTF8.GetBytes(serialized);

			return System.Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');
		}

		protected static string getExpires()
		{
			var timestamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

			return (timestamp + (5 * 60 * 1000)).ToString();

		}

		protected static string getNonce()
		{
			var bytes = new byte[32];
			var random = new Random();
			random.NextBytes(bytes);

			return BitConverter.ToString(bytes).Replace("-", "");
		}
	}
}
