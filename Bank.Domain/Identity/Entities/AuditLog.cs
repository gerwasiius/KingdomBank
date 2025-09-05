using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain.Identity.Entities;

public sealed class AuditLog
{
    public Guid Id { get; init; }
    public Guid? UserId { get; init; }
    public string Action { get; init; } = default!;
    public string? MetadataJson { get; init; }
    public DateTime CreatedAt { get; init; }
}