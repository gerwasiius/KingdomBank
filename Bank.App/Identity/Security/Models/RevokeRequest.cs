using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.App.Identity.Security.Models
{
    public sealed class RevokeRequest
    {
        public Guid UserId { get; init; }
        public Guid? DeviceId { get; init; } // null => global sign-out
        public Guid? FamilyId { get; init; } // null => sve familije
    }
}
