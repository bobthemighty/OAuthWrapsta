namespace Kontraktr.lib
{
    public interface IVerificationRepository
    {
        string CreateCode(WrapUserResponse response);
    }
}