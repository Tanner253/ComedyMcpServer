using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server; // Assuming this namespace based on SDK patterns
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System;

// Define the Joke API response structure
public class JokeApiResponse
{
    public string? Type { get; set; }
    public string? Joke { get; set; }
    public string? Setup { get; set; }
    public string? Delivery { get; set; }
    public bool Error { get; set; } // JokeAPI indicates errors with this flag
}

public class Program
{
    // Shared HttpClient for joke fetching - configured once
    private static readonly HttpClient Client = new() { Timeout = TimeSpan.FromSeconds(10) };

    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        // Configure console logging to stderr
        builder.Logging.AddConsole(consoleLogOptions =>
        {
            consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
        });

        // Configure MCP Server services
        builder.Services
            .AddMcpServer() // Add MCP server core services
            .WithStdioServerTransport() // Use standard I/O for communication (required for MCP Inspector)
            .WithToolsFromAssembly(); // Automatically discover tools in this assembly

        Console.WriteLine("Starting Comedy MCP Server..."); // Log server start initiation
        var host = builder.Build();

        // Add application lifetime logging
        var logger = host.Services.GetRequiredService<ILogger<Program>>();
        var lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();
        lifetime.ApplicationStarted.Register(() => logger.LogInformation("Comedy MCP Server started successfully via stdio. Connect with an MCP client like the Inspector."));
        lifetime.ApplicationStopping.Register(() => logger.LogInformation("Comedy MCP Server is stopping."));
        lifetime.ApplicationStopped.Register(() => logger.LogInformation("Comedy MCP Server stopped."));


        // Run the host application
        await host.RunAsync();
    }

    // Helper method to fetch jokes asynchronously
    public static async Task<string?> FetchJoke()
    {
        const string fallbackJoke = "Why did the programmer quit his job? He didn't get arrays.";
        try
        {
            // Fetch a programming or miscellaneous joke, preferring safe-for-work content
            var response = await Client.GetFromJsonAsync<JokeApiResponse>("https://v2.jokeapi.dev/joke/Programming,Miscellaneous?safe-mode");

            if (response == null || response.Error)
            {
                Console.Error.WriteLine($"Joke API returned an error or null response. Falling back to default joke.");
                return fallbackJoke;
            }

            // Format the joke based on whether it's single-line or setup/delivery
            return response.Type == "single"
                ? response.Joke
                : $"{response.Setup} ... {response.Delivery}";
        }
        catch (HttpRequestException ex)
        {
            Console.Error.WriteLine($"HTTP request to Joke API failed: {ex.Message}. Falling back to default joke.");
            return fallbackJoke;
        }
        catch (Exception ex) // Catch other potential errors (e.g., JSON parsing, timeouts)
        {
            Console.Error.WriteLine($"Failed to fetch or process joke: {ex.Message}. Falling back to default joke.");
            return fallbackJoke;
        }
    }
}

// Define the MCP Tool Type container
[McpServerToolType]
public static class ComedyTool
{
    // Define the specific tool method callable by an MCP client
    [McpServerTool, Description("Enhances a comment with a programming/misc joke. Tailors the tone based on keywords ('solo', 'lonely', 'troll', 'coworker').")]
    public static async Task<string> EnhanceComment([Description("The comment to enhance.")] string comment)
    {
        string input = comment?.Trim().ToLowerInvariant() ?? "no comment provided"; // Null check, trim, normalize
        string joke = await Program.FetchJoke() ?? "Error fetching joke!"; // Fetch joke, use fallback from FetchJoke if needed

        string enhancedComment;

        if (input.Contains("solo") || input.Contains("lonely"))
        {
            enhancedComment = $"Solo dev boost requested for: '{comment}'. Laugh it off: {joke}";
        }
        else if (input.Contains("troll") || input.Contains("coworker"))
        {
            enhancedComment = $"Troll attempt detected regarding: '{comment}'. Hit 'em with this: {joke}";
        }
        else
        {
            enhancedComment = $"Received comment: '{comment}'. Here's a little spice: {joke}";
        }

        return enhancedComment;
    }
} 