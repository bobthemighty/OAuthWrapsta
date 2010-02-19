using Kontraktr.lib;
using Machine.Specifications;
using OpenRasta.Web;

namespace Kontaktr.Lib.Tests
{
    [Subject("Verification Request")]
    public class When_the_verification_request_succeeds : for_a_valid_verification_request
    {
        Because the = () => Result = Handler.Post(ClientId, ClientSecret, VerificationCode);

        It should_be_a_verification_response = () => Result.ResponseResource.ShouldBeOfType<WrapVerificationResponse>();
        It should_contain_a_wrap_token = () => TheResponse.WrapToken.ShouldNotBeEmpty();
        It should_contain_a_wrap_refresh_token = () => TheResponse.RefreshToken.ShouldNotBeEmpty();

        It should_be_200_OK = () => Result.ShouldBeOfType<OperationResult.OK>();
    }

    public class for_a_valid_verification_request
    {
        protected static WrapVerificationResponse TheResponse
        {
            get
            {
                return Result.ResponseResource as WrapVerificationResponse;
            }
        }

        private Establish context = () =>
        {
            ClientId = "abc123";
            ClientSecret = "foo is a secure password";
            VerificationCode = "foo1";
            Handler = new VerificationCodeHandler(new VerificationCodeRepository());
        };

        protected static VerificationCodeHandler Handler { get; set; }
        protected static string VerificationCode { get; set; }
        protected static string ClientSecret { get; set; }
        protected static string ClientId { get; set; }

        protected static OperationResult Result { get; set; }
    }

}