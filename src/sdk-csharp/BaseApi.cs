using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tozny.Auth
{
	abstract public class ApiRequest: Dictionary<String, String>
	{
		protected String method;

		public ApiRequest()
		{
			throw new NotImplementedException();
		}

		public ApiRequest(String method)
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
		protected String apiUrl;
		protected String realmKeyId;

		public BaseApi()
		{
			throw new NotImplementedException();
		}

		public BaseApi(String realmKeyId) : this("https://api.tozny.com/", realmKeyId) { }

		public BaseApi(String apiUrl, String realmKeyId)
		{
			this.apiUrl = apiUrl;
			this.realmKeyId = realmKeyId;
		}

		abstract protected Task<JObject> rawCall(ApiRequest request);
	}
}
