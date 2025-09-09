using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Worker.Models
{
    public sealed record KeyDescriptor(
     string Kid,
     KeyAlgorithm Algorithm,
     DateTime CreatedAtUtc
 );
}
