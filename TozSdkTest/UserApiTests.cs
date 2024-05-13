namespace Tozny.Auth.Tests
{
	public class UserApiTests
	{
		[Fact]
		public async Task testSessionStatus()
		{
			var realm_key_id = "sid_52f12506e4f08";
			var session_id = "69b8cfb12e1051dc71a48598ed648b72088ad0b10f696b40947ecd95b4827a80";

			var api = new UserApi(realm_key_id);

			var status = await api.check_session_status(session_id);

			Assert.Equal(status.Status, "Invalid Session");
		}
	}
}
