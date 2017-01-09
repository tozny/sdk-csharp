using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tozny.Auth
{
    public class RealmApi : BaseApi
    {
		private String realmSecret;

		public RealmApi(String apiUrl, String realmKeyId, String realmSecret) : base(apiUrl, realmKeyId)
		{
			this.realmSecret = realmSecret;
		}

		protected override async Task<JObject> rawCall(ApiRequest request)
		{
			throw new NotImplementedException();
		}
	}
}
