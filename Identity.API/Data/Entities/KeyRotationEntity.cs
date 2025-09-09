using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.API.Data.Entities
{
    public sealed class KeyRotationEntity
    {
        public int Id { get; set; }
        public string Kid { get; set; } = default!;
        public string Alg { get; set; } = default!;
        public string Status { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public DateTime? ActivatedAt { get; set; }
        public DateTime? RetiredAt { get; set; }
    }
}
