namespace Bank.Domain.Identity.Entities;

public sealed class Role
{
    public Guid Id { get; init; }
    public string Name { get; private set; } = default!;
}
