namespace Bank.Domain.Identity.ValueObjects;

public readonly record struct Email(string Value)
{
    public override string ToString() => Value;
}
