using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.App.Identity.Security.Models
{
    public sealed class RefreshRequest
    {
        public Guid UserId { get; init; }
        public Guid DeviceId { get; init; }
        public Guid FamilyId { get; init; }
        public string ProvidedRefreshToken { get; init; } = default!;
    }
}
