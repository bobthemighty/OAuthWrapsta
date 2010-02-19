using System;
using Kontraktr.lib;
using Machine.Specifications;
using Moq;
using OpenRasta.Web;
using OpenWrapsta;
using It=Machine.Specifications.It;
using Arg = Moq.It;

#region Windows Form Designer generated code
namespace Kontaktr.Lib.Tests
{
    [Subject("Authorization Request")]
    public class When_the_user_responds_positively : for_a_valid_client_request
    {
        Establish account_is_valid = () => Accounts.Setup(a => a.Verify(Arg.IsAny<string>(), Arg.IsAny<string>())).Returns(true);
        Because the = () => _Result = Handler.Post(UserResponse, Authority.Allow);
        
        It should_be_SeeOther = () => _Result.ShouldBeOfType<OperationResult.SeeOther>();
        It should_have_status_303 = () => _Result.StatusCode.ShouldEqual(303);
        It should_contain_verification_code = () => _Result.RedirectLocation.HasUriParam("wrap_verification_code");

        It should_redirect_to_callback_uri = () => _Result.RedirectLocation.GetLeftPart(UriPartial.Path)
                                                    .TrimEnd('/')
                                                    .ShouldEqual(ClientRequest.CallbackUri);
    }

    [Subject("Authorization Request")]
    public class When_the_user_responds_negatively : for_a_valid_client_request
    {
        Because the = () => _Result = Handler.Post(UserResponse, Authority.Deny);

        It should_be_SeeOther = () => _Result.ShouldBeOfType<OperationResult.SeeOther>();

        It should_not_contain_verification_code = () => _Result.RedirectLocation.DoesNotHaveUriParam("wrap_verification_code");

        It should_contain_client_denial_code = () => _Result.RedirectLocation.HasUriParam("wrap_error_reason", "user_denied");

        It should_redirect_to_callback_uri = () => _Result.RedirectLocation.GetLeftPart(UriPartial.Path)
                                                    .TrimEnd('/')
                                                    .ShouldEqual(ClientRequest.CallbackUri);
    }
    [Ignore]
    [Subject("Authorization Request")]
    public class When_the_user_credentials_are_invalid : for_a_valid_client_request
    {
        Establish account_is_invalid = () => Accounts.Setup(a => a.Verify(Arg.IsAny<string>(), Arg.IsAny<string>())).Returns(false);
        Because the = () => _Result = Handler.Post(UserResponse, Authority.Allow);

        It should_be_SeeOther = () => _Result.ShouldBeOfType<OperationResult.SeeOther>();

        It should_contain_auth_error_code = () => _Result.RedirectLocation.HasUriParam("error", "userCredentialsIncorrect");
        It should_contain_client_id = () => _Result.RedirectLocation.HasUriParam("wrap_client_id", ClientRequest.ClientId);
        It should_redirect_to_callback_uri = () => _Result.RedirectLocation.HasUriParam("wrap_callback", ClientRequest.ClientId);
    }

    public static class UriExtensions
    {
        public static bool HasUriParam(this Uri uri, string paramName)
        {
            return uri.Query.Contains(paramName + "=");
        }

        public static bool DoesNotHaveUriParam(this Uri uri, string paramName)
        {
            return false == HasUriParam(uri, paramName);
        }

        public static bool HasUriParam(this Uri uri, string paramName, string value)
        {
            return uri.Query.Contains(paramName + "="+ value);
        }
    }



    public class for_a_valid_client_request
    {
        protected static WrapClientRequest ClientRequest { get; set; }
        protected static UserAuthorizationHandler Handler { get; set; }
        protected static WrapUserResponse UserResponse { get; set; }
        protected static OperationResult _Result { get; set; }

        protected static Mock<IUserAccountRepository> Accounts = new Mock<IUserAccountRepository>();
        protected static Mock<IVerificationRepository> VerficationCodes = new Mock<IVerificationRepository>();


        private Establish context = () =>
        {
            ClientRequest = new WrapClientRequest("clientId", "Foo The Client", "http://callback.uri");
            UserResponse = new WrapUserResponse { CallbackUri = "http://callback.uri" };
            VerficationCodes.Setup(v => v.CreateCode(Arg.IsAny<WrapUserResponse>())).Returns("foo is a code");

            Handler = new UserAuthorizationHandler(Accounts.Object, VerficationCodes.Object);
            
        };
    }
}
#endregion