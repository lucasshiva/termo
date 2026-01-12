namespace Termo.Api.Models;

public record Word
{
    // Maybe we could support other lengths in the future.
    public const int DefaultLength = 5;

    public Word(string value, string? displayText = null)
    {
        ValidateValue(value);

        Value = value.ToLowerInvariant();
        DisplayText = string.IsNullOrWhiteSpace(displayText)
            ? Value.ToUpperInvariant()
            : displayText.ToUpperInvariant();
    }

    public string Value { get; }
    public string DisplayText { get; }
    public int Length => Value.Length;

    private static void ValidateValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(message: "Value cannot be empty", paramName: nameof(value));

        if (value.Length != DefaultLength)
            throw new ArgumentException(
                message: "Value is of wrong length",
                paramName: nameof(value)
            );
    }
}
