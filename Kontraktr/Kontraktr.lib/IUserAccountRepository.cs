namespace Kontraktr.lib
{
    public interface IUserAccountRepository
    {
        bool Verify(string username, string password);
    }
}