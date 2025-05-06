namespace dotnet_starter.Domain.ValueObjects;

public record Email
{
    public string? Value { get; private set; }

    private Email(string value)
    {
        Value = value;
    }

    // For EF Core
    private Email() { }

    public static Email Create(string? value)
    {
        if (string.IsNullOrEmpty(value?.Trim()))
            throw new ArgumentException("Email cannot be empty", nameof(value));

        if (!value.Contains('@') || value.Contains(' ') ||
            !value.Split('@')[0].Any() ||
            (value.Split('@').Length > 1 && !value.Split('@')[1].Contains('.')))
            throw new ArgumentException("Invalid email format", nameof(value));

        return new Email(value);
    }

    public override string ToString() => Value ?? string.Empty;
}