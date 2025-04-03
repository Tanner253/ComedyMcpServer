using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using System.ComponentModel;
using ComedyMcpServer.Services;
using Microsoft.AspNetCore.Builder;

// Command to run with the inspector tool:
// npx @modelcontextprotocol/inspector dotnet run

var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Add a health check endpoint
app.MapGet("/", () => "Comedy MCP Server is running!");

// Run both the web app and MCP server
await app.RunAsync();

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

    // Tool to get a funny comment for code
    [McpServerTool, Description("Gets a funny comment related to specific code context.")]
    public static async Task<string> GetCodeComment(string codeContext, string topic, IServiceProvider serviceProvider)
    {
        var comedyService = serviceProvider.GetRequiredService<IComedyService>();
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        try
        {
            return await comedyService.GetFunnyCommentAsync(codeContext, topic);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting funny code comment.");
            return $"// Even my joke generator crashed! Must be some seriously complex code. (Context: {codeContext})";
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