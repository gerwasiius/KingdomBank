using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain.Identity.Entities;
public sealed class LoginAttempt
{
    public Guid Id { get; init; }
    public string UsernameOrEmail { get; init; } = default!;
    public bool Success { get; init; }
    public string? Ip { get; init; }
    public DateTime CreatedAt { get; init; }
}
