using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.API.Models
{
    public sealed class TokenIssueRequest
    {
        public Guid UserId { get; init; }
        public Guid DeviceId { get; init; }
        public string Username { get; init; } = default!;
        public string Email { get; init; } = default!;
        public IEnumerable<string> Roles { get; init; } = Array.Empty<string>();
        public IEnumerable<string> Permissions { get; init; } = Array.Empty<string>();
        public bool AmrMfa { get; init; } = false;
        public string? SessionId { get; init; } // sid
    }
}
