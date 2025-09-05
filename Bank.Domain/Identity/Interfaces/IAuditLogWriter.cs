using Bank.Domain.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain.Identity.Interfaces;
public interface IAuditLogWriter
{
    Task WriteAsync(AuditLog entry, CancellationToken ct = default);
}