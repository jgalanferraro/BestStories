namespace BestStories
{
    public record ApiStory(string? Title,
                           string? Uri,
                           string? PostedBy,
                           DateTime? Time,
                           int? Score,
                           int? CommentCount)
    {
        public static ApiStory Create(Story story)
        {
            return new ApiStory(story.Title, story.Url, story.By, UnixTimeStampToDateTime(story.Time ?? 0), story.Score, story.Descendants);
        }

        private static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
    }
}
