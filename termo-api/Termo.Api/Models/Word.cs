namespace Termo.Api.Models;

public record Word
{
    public Word(string value, string? displayText = null)
    {
        Value = string.IsNullOrWhiteSpace(value)
            ? throw new ArgumentException("Value cannot be empty", nameof(value))
            : value.ToLowerInvariant();

        DisplayText = string.IsNullOrWhiteSpace(displayText)
            ? Value.ToUpperInvariant()
            : displayText.ToUpperInvariant();
    }

    public string Value { get; }
    public string DisplayText { get; }
    public int Length => Value.Length;
}
