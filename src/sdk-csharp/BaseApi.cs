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
	}

	abstract public class ApiResponse: Dictionary<String, String>
	{

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

		abstract protected Task<ApiResponse> rawCall(ApiRequest request);
	}
}
