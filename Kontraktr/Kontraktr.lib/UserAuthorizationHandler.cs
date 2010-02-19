using System;
using System.Collections.Generic;
using System.Text;
using OpenRasta.Web;
using OpenWrapsta;

namespace Kontraktr.lib
{
    public class UserAuthorizationHandler
    {
        private readonly IUserAccountRepository _accounts;
        private IVerificationRepository _verificationCodes;

        public UserAuthorizationHandler(IUserAccountRepository accounts, IVerificationRepository verificationCodes)
        {
            _accounts = accounts;
            _verificationCodes = verificationCodes;
        }

        public OperationResult Get(string client, string callbackUri)
        {
            return new OperationResult.OK(new WrapClientRequest( client, "foo", callbackUri ));
        }

        public OperationResult Post(WrapUserResponse userResponse, Authority authorization)
        {
            if (authorization == Authority.Deny)
                return UserDenied(userResponse);

            if(_accounts.Verify(userResponse.Username, userResponse.Password))
            {
                return UserAccepted(userResponse);
            }

            return UserBadCredentials();
        }

        private static OperationResult UserBadCredentials()
        {
           throw new NotImplementedException();
        }

        private OperationResult UserAccepted(WrapUserResponse response)
        {
            var code = _verificationCodes.CreateCode(response);
            return RedirectToCallBack(response, new Dictionary<string, string>
                                           {
                                               {"wrap_verification_code", code},
                                               {"wrap_client_state", response.State}
                                           });
        }

        private static OperationResult UserDenied(WrapUserResponse response)
        {
            return RedirectToCallBack(response, new Dictionary<string, string>
                                                  {
                                                      {
                                                          "wrap_error_reason", "user_denied"
                                                          }
                                                  });
        }

        private static OperationResult RedirectToCallBack(WrapUserResponse response, Dictionary<string, string> uriParams)
        {
            var sb = new StringBuilder();
            sb.Append(response.CallbackUri);
            sb.Append("?");
            foreach(var k in uriParams.Keys)
            {
                sb.AppendFormat("{0}={1}", k, uriParams[k]);
                sb.Append("&");
            }

            return new OperationResult.SeeOther
                       {
                           RedirectLocation = new Uri(sb.ToString())
                       };
        }
    }
}