using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tozny.Auth;
using Xunit;

namespace Tozny.Auth.Tests
{
    public class UserApiTests
    {
		[Fact]
		public async Task testSessionStatus()
		{
			var realm_key_id = "sid_52f12506e4f08";
			var session_id = "52835d3ca3b9e23bd182d540826b2c9d9688b7b5061a40c9a2b3a753f61c3d43";

			var api = new UserApi(realm_key_id);

			await api.check_session_status(session_id);
		}
    }
}
