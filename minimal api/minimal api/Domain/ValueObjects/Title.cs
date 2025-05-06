namespace dotnet_starter.Domain.ValueObjects;

public record Title
{
    public string? Value { get; private set; }

    private Title(string value)
    {
        Value = value;
    }

    // For EF Core
    private Title() { }

    public static Title Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Title cannot be empty", nameof(value));

        if (value.Length > 200)
            throw new ArgumentException("Title cannot exceed 200 characters", nameof(value));

        return new Title(value.Trim());
    }

    public override string ToString() => Value ?? string.Empty;
}