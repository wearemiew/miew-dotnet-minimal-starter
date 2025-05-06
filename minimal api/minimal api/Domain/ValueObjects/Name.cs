namespace dotnet_starter.Domain.ValueObjects;

public record Name
{
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }

    private Name(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    // For EF Core
    private Name() { }

    public static Name Create(string? firstName, string? lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty", nameof(lastName));

        return new Name(firstName.Trim(), lastName.Trim());
    }

    public string FullName => $"{FirstName ?? string.Empty} {LastName ?? string.Empty}".Trim();

    public override string ToString() => FullName;
}