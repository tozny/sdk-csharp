using Newtonsoft.Json.Linq;

namespace Tozny.Auth
{
	abstract public class ApiRequest : Dictionary<string, string>
	{
		protected string method;

		public ApiRequest()
		{
			throw new NotImplementedException();
		}

		public ApiRequest(string method)
		{
			this.method = method;
		}

		public string Method
		{
			get { return this.method; }
		}
	}

	abstract public class BaseApi
	{
		protected string apiUrl;
		protected string realmKeyId;

		public BaseApi()
		{
			throw new NotImplementedException();
		}

		public BaseApi(string realmKeyId) : this("https://api.tozny.com/", realmKeyId) { }

		public BaseApi(string apiUrl, string realmKeyId)
		{
			this.apiUrl = apiUrl;
			this.realmKeyId = realmKeyId;
		}

		abstract protected Task<JObject> rawCall(ApiRequest request);
	}
}
