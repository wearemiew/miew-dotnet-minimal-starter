namespace dotnet_starter.Domain.ValueObjects;

public record Content
{
    public string? Value { get; private set; }

    private Content(string value)
    {
        Value = value;
    }

    // For EF Core
    private Content() { }

    public static Content Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Content cannot be empty", nameof(value));

        return new Content(value.Trim());
    }

    public override string ToString() => Value ?? string.Empty;
}