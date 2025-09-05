using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.App.Identity.Security.Models
{
    public sealed class IssuedTokenPair
    {
        public string AccessToken { get; init; } = default!;
        public DateTime AccessExpiresAt { get; init; }
        public string RefreshToken { get; init; } = default!; // raw (za cookie/response)
        public DateTime RefreshExpiresAt { get; init; }
        public Guid FamilyId { get; init; }
    }
}
