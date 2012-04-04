namespace Puddle
{
    public class User
    {
        private readonly string _topLevelDomain;
        private readonly string _authorizationClientId;

        public User(int id)
        {
            _topLevelDomain = "local";
            _authorizationClientId = "integration-test";
            Id = id;
            Token = "{'iss':'login.huddle." + _topLevelDomain + "', 'exp':1986681600, 'urn:huddle.claims.userid':" + Id + ", 'urn:huddle.claims.clientId':'" + _authorizationClientId + "'}";
        }

        protected int Id { get; private set; }
        public string Token { get; private set; }
    }
}
