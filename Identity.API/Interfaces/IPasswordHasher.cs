namespace Identity.API.Interfaces
{
    public interface IPasswordHasher
    {
        byte[] Sha256(string raw);
    }
}
