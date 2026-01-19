using System.Text.Json.Serialization;

namespace Termo.Api.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum LetterState
{
    Correct,
    Present,
    Absent,
}
