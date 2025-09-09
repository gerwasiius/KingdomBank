using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.API.Models
{
    public sealed class TokenOptions
    {
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public int AccessMinutes { get; set; } = 5;    // 5–10 min
        public int RefreshDays { get; set; } = 30;     // rotirajući family
        public string Algorithm { get; set; } = "RS256";
    }
}
