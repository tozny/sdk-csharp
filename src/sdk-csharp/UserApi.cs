using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Tozny.Auth
{
	public class UserRequest : ApiRequest
	{
		public UserRequest(String method): base("user." + method) { }
	}

	public class UserResponse : ApiResponse
	{

	}

	public class UserApi : BaseApi
    {
		public UserApi(String realmKeyId) : base(realmKeyId) { }
		public UserApi(String apiUrl, String realmKeyId): base(apiUrl, realmKeyId) { }

		public async Task check_session_status(String sessionId)
		{
			var request = new UserRequest("check_session_status");

			var response = await this.rawCall(request);
		}

		protected override async Task<ApiResponse> rawCall(ApiRequest request)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(this.apiUrl);
				var data = await client.PostAsJsonAsync("", request);

				Console.WriteLine(data);

				return default(ApiResponse);
			}
		}
	}
}
