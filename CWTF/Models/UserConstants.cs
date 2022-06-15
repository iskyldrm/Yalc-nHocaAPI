using System.Collections.Generic;

namespace JwtApp.Models
{
    public class UserConstants
    {
        public static List<UserModel> userModels = new List<UserModel>()
        {
            new UserModel(){ UserName = "iskyldrm",
            EmailAdress = "isak@gmail.com",
            Password = "isakisak1234",
            GivenName = "İsak",
            Surname = "Yıldırım",
            Role = "Administrator"},
            new UserModel()
            {
            UserName = "iskyldrm1",
            EmailAdress = "isak1@gmail.com",
            Password = "isakisak12345",
            GivenName = "İsak1",
            Surname = "Yıldırım1",
            Role = "Seller"
            }
        };
    }
}
