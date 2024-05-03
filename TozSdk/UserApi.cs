using Newtonsoft.Json.Linq;

namespace Tozny.Auth
{
	public class UserRequest : ApiRequest
	{
		public UserRequest(string method) : base("user." + method) { }
	}

	public class UserApi : BaseApi
	{
		public UserApi(string realmKeyId) : base(realmKeyId) { }
		public UserApi(string apiUrl, string realmKeyId) : base(apiUrl, realmKeyId) { }

		public class SessionStatus
		{
			protected string status;
			protected string signed_data;
			protected string signature;

			public SessionStatus(string status, string signed_data, string signature)
			{
				this.status = status;
				this.signed_data = signed_data;
				this.signature = signature;
			}

			public string Status
			{
				get
				{
					return this.status;
				}
			}

			public string Signature
			{
				get
				{
					return this.signature;
				}
			}

			public string SignedData
			{
				get
				{
					return this.signed_data;
				}
			}
		}

		public async Task<SessionStatus> check_session_status(string sessionId)
		{
			var request = new UserRequest("check_session_status");
			request.Add("session_id", sessionId);

			try
			{
				dynamic response = await this.rawCall(request);

				if (response.GetValue("status") == "pending")
				{
					return new SessionStatus("pending", null, null);
				}
				else
				{
					return new SessionStatus(response.GetValue("status"), response.GetValue("signed_data"), response.GetValue("signature"));
				}
			}
			catch (Exception e)
			{
				return new SessionStatus("Invalid Session", null, null);
			}
		}

		protected override async Task<JObject> rawCall(ApiRequest request)
		{
			using (var client = new HttpClient())
			{
				var requestArgs = new Dictionary<string, string>();
				requestArgs.Add("method", request.Method);
				requestArgs.Add("realm_key_id", this.realmKeyId);

				foreach (KeyValuePair<string, string> parameter in request)
				{
					requestArgs.Add(parameter.Key, parameter.Value);
				}

				var content = new FormUrlEncodedContent(requestArgs);

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
	}
}
