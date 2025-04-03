namespace ComedyMcpServer.Services;

public interface IComedyService
{
    /// <summary>
    /// Gets a humorous comment related to the original comment and topic.
    /// </summary>
    /// <param name="originalComment">The original comment text (may be ignored for joke APIs).</param>
    /// <param name="topic">The topic (e.g., "programming", "software").</param>
    /// <returns>A task representing the asynchronous operation, containing a humorous comment or joke.</returns>
    Task<string> GetFunnyCommentAsync(string originalComment, string topic = "programming");
} 