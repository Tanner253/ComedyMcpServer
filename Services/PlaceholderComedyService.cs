using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json; // Requires System.Net.Http.Json NuGet package
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;

namespace ComedyMcpServer.Services;

// Represents the structure of the JokeAPI response
internal class JokeApiResponse
{
    public bool Error { get; set; }
    public string? Joke { get; set; }
    public string? Setup { get; set; }
    public string? Delivery { get; set; }
    public string? Category { get; set; }
    public string? Type { get; set; }
    public string? ErrorMessage { get; set; } // Add property to capture potential error messages from API
}

public class PlaceholderComedyService : IComedyService
{
    private readonly ILogger<PlaceholderComedyService> _logger;
    private readonly HttpClient _httpClient;
    private readonly string _jokeApiUrl;

    // Inject IHttpClientFactory to create HttpClient instances
    public PlaceholderComedyService(ILogger<PlaceholderComedyService> logger, IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient(); // Create a client

        // Use the specific URL for programming jokes, allow override from config
        _jokeApiUrl = configuration["ComedyApi:ProgrammingJokeUrl"] ?? "https://v2.jokeapi.dev/joke/Programming?blacklistFlags=nsfw,religious,political,racist,sexist,explicit&type=single";

        _logger.LogInformation($"PlaceholderComedyService initialized. Using Joke API URL: {_jokeApiUrl}");
    }

    public async Task<string> GetFunnyCommentAsync(string originalComment, string topic = "programming")
    {
        _logger.LogInformation($"Attempting to fetch a funny programming joke for comment: '{originalComment}'");

        // Use the specific programming joke URL
        string requestUrl = _jokeApiUrl;

        try
        {
            // Make the API call
            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode(); // Throw if HTTP error

            // Deserialize the response
            var jokeResponse = await response.Content.ReadFromJsonAsync<JokeApiResponse>();

            if (jokeResponse == null || jokeResponse.Error || string.IsNullOrEmpty(jokeResponse.Joke))
            {
                string errorMessage = jokeResponse?.ErrorMessage ?? "Unknown error from JokeAPI";
                _logger.LogWarning($"JokeAPI did not return a valid joke. Response error: {jokeResponse?.Error}, API Error Message: {errorMessage}");
                return $"// I tried to think of a joke, but my funny bone returned null. (Original: {originalComment})";
            }

            _logger.LogInformation($"Successfully fetched joke: '{jokeResponse.Joke}'");
            
            // Format the joke based on the context
            string formattedJoke = FormatJokeForContext(jokeResponse.Joke, originalComment, topic);
            return formattedJoke;
        }
        catch (HttpRequestException httpEx)
        {
            _logger.LogError(httpEx, "HTTP Error fetching joke from {Url}", requestUrl);
            return $"// Couldn't connect to the joke server. Maybe it's taking a coffee break? (Original: {originalComment})";
        }
        catch (JsonException jsonEx)
        {
            _logger.LogError(jsonEx, "Error deserializing joke response from {Url}", requestUrl);
            return $"// The joke server's response was funny, but not in a format I understand! (Original: {originalComment})";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error fetching joke for comment: {Comment}", originalComment);
            return $"// An unexpected error occurred while trying to be funny. Ironic, isn't it? (Original: {originalComment})";
        }
    }

    private string FormatJokeForContext(string joke, string originalComment, string topic)
    {
        // Extract code context for better joke formatting
        var codeContext = ParseCodeContext(originalComment);
        
        switch (codeContext.Type)
        {
            case "variable":
                return FormatVariableJoke(joke, codeContext.Name, codeContext.Value);
            case "function":
                return FormatFunctionJoke(joke, codeContext.Name);
            case "class":
                return FormatClassJoke(joke, codeContext.Name);
            default:
                return $"// {joke}\n// (Relates to: {originalComment})";
        }
    }

    private (string Type, string Name, string Value) ParseCodeContext(string code)
    {
        // Simple parsing of code context
        code = code.Trim();
        
        // Variable declaration pattern (e.g., "int a = 5")
        if (code.Contains("="))
        {
            var parts = code.Split('=').Select(p => p.Trim()).ToArray();
            var varParts = parts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return ("variable", varParts.Last(), parts[1].TrimEnd(';'));
        }
        
        // Function pattern
        if (code.Contains("("))
        {
            var name = code.Split('(')[0].Trim();
            return ("function", name, "");
        }
        
        // Class pattern
        if (code.Contains("class"))
        {
            var name = code.Split("class")[1].Trim();
            return ("class", name, "");
        }

        return ("unknown", "", "");
    }

    private string FormatVariableJoke(string joke, string varName, string value)
    {
        // Create variable-specific joke format
        var prefix = $"// Variable {varName} walks into a bar...";
        return $"{prefix}\n// {joke}\n// (Fun fact: Even {value} would laugh at that one!)";
    }

    private string FormatFunctionJoke(string joke, string funcName)
    {
        // Create function-specific joke format
        return $"// Function {funcName} says:\n// {joke}\n// (Warning: This joke has O(n) complexity!)";
    }

    private string FormatClassJoke(string joke, string className)
    {
        // Create class-specific joke format
        return $"// Class {className} inherits from Humor:\n// {joke}\n// (Implements ILaughable)";
    }
} 