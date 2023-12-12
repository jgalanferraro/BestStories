using System.Text.Json.Serialization;

namespace BestStories
{
    public record Story(int Id,
                        bool? Deleted = null,
                        string? Type = null,
                        string? By = null,
                        long? Time = null,
                        string? Text = null,
                        bool? Dead = null,
                        int? Parent = null,
                        int? Poll = null,
                        IEnumerable<int>? Kids = null,
                        string? Url = null,
                        int? Score = null,
                        string? Title = null,
                        IEnumerable<int>? Parts = null,
                        int? Descendants = null);
}
