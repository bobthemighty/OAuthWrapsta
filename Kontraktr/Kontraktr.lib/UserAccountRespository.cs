using System;
using System.Collections.Generic;
using System.Linq;

namespace Kontraktr.lib
{
    public class UserAccountRespository : IUserAccountRepository
    {
        private static Dictionary<string, string> Accounts 
            = new Dictionary<string, string>{
                  { "bob", "password" }
            };

        public bool Verify(string username, string password)
        {
            return Accounts.Any(entry => entry.Key == username && entry.Value == password);
        }
    }

    public class VerificationCodeRepository : IVerificationRepository
    {
        private static int _NextInt = 0;
        private Dictionary<string, WrapUserResponse> Codes = new Dictionary<string, WrapUserResponse>();
        public string CreateCode(WrapUserResponse response)
        {
            var code = "foo" + NextInt();
            Codes.Add(code, response);
            return code;
        }

        private int NextInt()
        {
            return _NextInt++;
        }
    }
}
