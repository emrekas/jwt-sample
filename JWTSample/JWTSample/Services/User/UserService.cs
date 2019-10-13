using System;

namespace JWTSample.Services.User
{
    //DI için oluşturduğumuz arayüzü implemente ediyoruz
    public class UserService : IUserService
    {
        public (string username, string token) Authenticate(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
