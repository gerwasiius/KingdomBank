namespace Bank.Domain.Identity.ValueObjects;

public readonly record struct Username(string Value)
{
    public override string ToString() => Value;
}
