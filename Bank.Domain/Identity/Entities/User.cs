namespace Bank.Domain.Identity.Entities;

public sealed class User
{
    public Guid Id { get; init; }
    public string Username { get; private set; } = default!;
    public string DisplayName { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public bool EmailConfirmed { get; private set; }
    public bool IsActive { get; private set; }

    public byte[] PasswordHash { get; private set; } = default!;
    public byte[] PasswordSalt { get; private set; } = default!;
}
