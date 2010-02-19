using System;
using System.Collections.Generic;
using OpenRasta.Configuration;
using OpenRasta.DI;
using OpenRasta.Web;
using OpenWrapsta;

namespace Kontraktr.lib
{
    public class Configuration : IConfigurationSource
    {
        public void Configure()
        {
            using (OpenRastaConfiguration.Manual)
            {
                ResourceSpace.Has.ResourcesOfType<WrapClientRequest>().WithoutUri.RenderedByAspx("~/auth/user-request.aspx");

                ResourceSpace.Uses.CustomDependency<IUserAccountRepository, UserAccountRespository>(DependencyLifetime.Singleton);
                ResourceSpace.Uses.CustomDependency<IVerificationRepository, VerificationCodeRepository>(DependencyLifetime.Singleton);

                ResourceSpace.Has.ResourcesOfType<List<Address>>()
                    .AtUri("/addresses")
                    .HandledBy<AddressListHandler>()
                    .AsXmlSerializer();

                ResourceSpace.Has.ResourcesOfType<Address>()
                    .AtUri("/addresses/{id}")
                    .HandledBy<AddressHandler>()
                    .AsXmlSerializer();

                ResourceSpace.Has.ResourcesNamed("UserAuthorization")
                    .AtUri("/auth/user-request?wrap_client_id={client}&wrap_callback={callbackUri}")
                    .And.AtUri("/auth/user-request")
                    .HandledBy<UserAuthorizationHandler>()
                    .RenderedByAspx("~/auth/user-request.aspx")
                    .ForMediaType("text/html;q=1");

                ResourceSpace.Has.ResourcesNamed("CodeVerification")
                    .AtUri("/auth/verification")
                    .HandledBy<VerificationCodeHandler>();
            }


        }

    }

    public class VerificationCodeHandler
    {
        private readonly IVerificationRepository _repository;

        public VerificationCodeHandler(IVerificationRepository repository)
        {
            _repository = repository;
        }

        public OperationResult Post(string clientId, string clientSecret, string verificationCode)
        {
            return new OperationResult.OK
                       {
                           ResponseResource = new WrapVerificationResponse()
                       };
        }
    }

    public class WrapClientVerification
    {
    }

    public class WrapVerificationResponse
    {
        public string WrapToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
