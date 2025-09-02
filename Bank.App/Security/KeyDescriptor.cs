using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.App.Security
{
    public sealed record KeyDescriptor(
     string Kid,
     KeyAlgorithm Algorithm,
     DateTime CreatedAtUtc
 );
}
