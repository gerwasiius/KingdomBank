namespace Identity.API.Interfaces
{
    public interface ISecureRandom
    {
        string Base64Url(int bytes);
    }
}
