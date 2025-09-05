using Bank.App.Identity.Security.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.App.Identity.Security.Interfaces
{
    public interface ITokenService
    {
        Task<IssuedTokenPair> IssueAsync(TokenIssueRequest request, CancellationToken ct = default);
        Task<IssuedTokenPair> RefreshAsync(RefreshRequest request, CancellationToken ct = default);
        Task RevokeAsync(RevokeRequest request, CancellationToken ct = default);
    }
}
