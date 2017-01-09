using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tozny.Auth;
using Xunit;

namespace Tozny.Auth.Tests
{
	public class RealmApiTest
	{
		protected String realm_key_id = "DO_NOT_COMMIT_THIS_VALUE";
		protected String realm_secret = "DO_NOT_COMMIT_THIS_VALUE";

		[Fact]
		public void testHashGeneration()
		{
			var realmSecret = "cb0325c9dce8aad854c3f65d30420d1602ee4538419165f32ead9504702dbd54";
			var realmAPI = new RealmApi("sid_1234567", realmSecret);

			var dataToSign = "eyJkYXRhIjoidG8gYmUgc2lnbmVkIiwiZ29lcyI6ImhlcmUifQ";

			var signature = realmAPI.sign(dataToSign);

			Assert.Equal("Fbliq-QszY33uWiy1z4_HUfqAFm7i3qwqjWaLvtnTw0", signature);
		}

		[Fact]
		public async Task testUserAdd()
		{
			var realmApi = new RealmApi(realm_key_id, realm_secret);

			var userResponse = await realmApi.user_add(true, new Dictionary<string, string>(), null);

			Assert.Equal(realm_key_id, userResponse.RealmKeyId);
		}

		[Fact]
		public async Task testUserUpdate()
		{
			var realmApi = new RealmApi(realm_key_id, realm_secret);

			var user_id = "sid_5873e244deff1";
			var username = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 8);
			var fields = new Dictionary<String, String>();
			fields.Add("username", username);

			var userUpdateResponse = await realmApi.user_update(user_id, fields);

			Assert.Equal(username, userUpdateResponse.Username);
		}

		[Fact]
		public async Task testQuestionChallenge()
		{
			var realmApi = new RealmApi(realm_key_id, realm_secret);

			var user_id = "sid_5873e244deff1";
			var challenge = await realmApi.question_challenge(user_id, "shall we begin?");

			Assert.Equal(realm_key_id, challenge.RealmKeyId);
		}

		[Fact]
		public async Task testUserPush()
		{
			var realmApi = new RealmApi(realm_key_id, realm_secret);

			var user_id = "sid_5873fa1dea085";
			var challenge = await realmApi.question_challenge(user_id, "Shall we begin?");

			var pushed = await realmApi.user_push(user_id, challenge.SessionId);

			Assert.True(pushed);
		}
	}
}
