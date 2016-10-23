namespace DBSoft.EVEAPI.Crest
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public class UserAuth : IUserAuth
    {
        public async Task<AuthenticatedUserDTO> GetAuthenticatedUser(string code, string type, string clientId, string secret)
        {
            var auth = GetAuthResponse(code, type, clientId, secret);
            return await GetUser(auth);
        }

        public async Task<AuthenticatedUserDTO> RefreshAuthenticatedUser(string refresh, string clientId, string secret)
        {
            var auth = GetAuthResponse(refresh, "refresh_token", clientId, secret);
            return await GetUser(auth);
        }

        public static async Task<AuthenticatedUserDTO> GetUser(AuthResponse auth)
        {
            var req = WebRequest.CreateHttp("https://login.eveonline.com/oauth/verify");
            req.Headers.Add("Authorization", "Bearer " + auth.AccessToken);
            req.ContentType = "application/x-www-form-urlencoded";
            using (var response = (await req.GetResponseAsync()).GetResponseStream())
            {
                if (response == null) throw new Exception();
                using (TextReader reader = new StreamReader(response))
                {
                    var readToEnd = reader.ReadToEnd();
                    var result = JsonConvert.DeserializeObject<VerifyResponse>(readToEnd);
                    return new AuthenticatedUserDTO
                    {
                        CharacterName = result.CharacterName,
                        Token = auth.AccessToken,
                        RefreshToken = auth.RefreshToken
                    };
                }
            }
        }


        private static AuthResponse GetAuthResponse(string code, string type, string clientId, string secret)
        {
            var query = type == "refresh_token" ? $"grant_type=refresh_token&refresh_token={code}"
                :
                                                  string.Format("grant_type={1}&code={0}", code, type);

            var req = WebRequest.CreateHttp("https://login.eveonline.com/oauth/token");
            req.Method = "POST";
            var bytes = Encoding.ASCII.GetBytes($"{clientId}:{secret}");
            var encoded = Convert.ToBase64String(bytes);
            req.Headers.Add("Authorization", "Basic " + encoded);
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = query.Length;
            using (var writer = new StreamWriter(req.GetRequestStream()))
            {
                writer.Write(query);
            }
            using (var response = req.GetResponse().GetResponseStream())
            {
                if (response == null) throw new Exception();
                using (TextReader reader = new StreamReader(response))
                {
                    var s = reader.ReadToEnd();
                    var result = JsonConvert.DeserializeObject<AuthResponse>(s);
                    return result;
                }
            }
        }

    }
}
