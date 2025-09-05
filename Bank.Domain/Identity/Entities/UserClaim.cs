namespace Bank.Domain.Identity.Entities;

public sealed class UserClaim
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string Type { get; init; } = default!;
    public string Value { get; init; } = default!;
}
