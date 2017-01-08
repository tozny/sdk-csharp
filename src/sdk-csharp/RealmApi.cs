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

		protected override T rawCall<T>(string method, Dictionary<string, string> parameters)
		{
			throw new NotImplementedException();
		}
	}
}
