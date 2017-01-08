using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Tozny.Auth
{
    public class UserApi : BaseApi
    {
		protected override T rawCall<T>(String method, Dictionary<String, String> parameters)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(this.apiUrl);
			}

			return default(T);
		}
	}
}
