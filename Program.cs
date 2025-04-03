using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using System.ComponentModel;
using ComedyMcpServer.Services;
using Microsoft.AspNetCore.Builder;

// Command to run with the inspector tool:
// npx @modelcontextprotocol/inspector dotnet run

var builder = Host.CreateApplicationBuilder(args);

// Configure logging
builder.Logging.ClearProviders(); // Optional: Remove default providers
builder.Logging.AddConsole(options => options.LogToStandardErrorThreshold = LogLevel.Trace);
builder.Logging.SetMinimumLevel(LogLevel.Debug); // Set minimum log level

// Register services
builder.Services.AddHttpClient(); // Register IHttpClientFactory
builder.Services.AddSingleton<IComedyService, PlaceholderComedyService>();

// Configure MCP Server
builder.Services
    .AddMcpServer()
    .WithStdioServerTransport() // Use stdio for communication with Cursor
    .WithToolsFromAssembly(); // Automatically discover tools in this assembly

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var host = builder.Build();

await host.RunAsync();

// --- Tool Definitions ---

[McpServerToolType]
public static class ComedyTool
{
    // Tool to get a programming joke
    [McpServerTool, Description("Gets a random programming joke.")]
    public static async Task<string> GetProgrammingJoke(IServiceProvider serviceProvider)
    {
        var comedyService = serviceProvider.GetRequiredService<IComedyService>();
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        try
        {
            // Using "programming" topic, though the API URL is specific
            return await comedyService.GetFunnyCommentAsync("", "programming");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting programming joke.");
            return "Couldn't fetch a joke right now, the programmer humor circuits are down!";
        }
    }

    // Example tool showing how to access services
    [McpServerTool, Description("Echoes the message back, using ILogger.")]
    public static string EchoWithLog(string message, IServiceProvider serviceProvider)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogInformation($"Echoing message: {message}");
        return $"Echo: {message}";
    }
}