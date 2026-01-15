using System;
using System.Text.Json.Serialization;

namespace EmployeeTimeTracker;

public class TimeEntry
{
    [JsonPropertyName("Id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("EmployeeName")]
    public string EmployeeName { get; set; } = string.Empty;

    [JsonPropertyName("StarTimeUtc")]
    public DateTime? StarTimeUtc { get; set; }

    [JsonPropertyName("EndTimeUtc")]
    public DateTime? EndTimeUtc { get; set; }

    [JsonPropertyName("EntryNotes")]
    public string? EntryNotes { get; set; }

    [JsonPropertyName("DeletedOn")]
    public DateTime? DeletedOn { get; set; }
}
