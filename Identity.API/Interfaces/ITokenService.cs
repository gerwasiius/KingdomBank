using Identity.API.Models;

namespace Identity.API.Interfaces
{
    public interface ITokenService
    {
        Task<IssuedTokenPair> IssueAsync(TokenIssueRequest request, CancellationToken ct = default);
        Task<IssuedTokenPair> RefreshAsync(RefreshRequest request, CancellationToken ct = default);
        Task RevokeAsync(RevokeRequest request, CancellationToken ct = default);
    }
}
