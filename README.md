# Comedy MCP Server

A Model Context Protocol (MCP) server implementation that provides comedy-related tools and services for AI assistants. This server integrates with Cursor IDE and offers various comedy-related functionalities through MCP tools.

## Overview

The Comedy MCP Server is built using ASP.NET Core and implements the Model Context Protocol (MCP) to provide AI assistants with tools for generating programming jokes and funny code comments. It serves as a bridge between AI models and comedy-related functionalities.

![MCP Server Integration Proof](documentation/MCP%20proof.jpg)

## Features

- **Programming Jokes**: Generates random programming-related jokes
- **Code Comments**: Creates humorous comments based on code context and topics
- **Echo Service**: Demonstration service showing logging capabilities
- **Swagger Integration**: API documentation and testing interface
- **Health Check Endpoint**: Basic server status monitoring

## Architecture

The server is built with the following components:

- **MCP Server Integration**: Uses `ModelContextProtocol.Server` for MCP implementation
- **Comedy Services**: Implements `IComedyService` for joke and comment generation
- **HTTP Communication**: Uses stdio for communication with Cursor IDE
- **Logging**: Comprehensive logging with configurable levels
- **CORS Support**: Configured for MCP inspector tool

## Tools

The server provides the following MCP tools:

1. `GetProgrammingJoke`
   - Description: Retrieves a random programming-related joke
   - Parameters: None
   - Returns: A string containing the joke

2. `GetCodeComment`
   - Description: Generates a funny comment related to specific code context
   - Parameters:
     - `codeContext`: The code snippet or context
     - `topic`: The topic for the comment
   - Returns: A humorous comment related to the code

3. `EchoWithLog`
   - Description: Demonstration tool that echoes messages with logging
   - Parameters:
     - `message`: The message to echo
   - Returns: The echoed message

## Setup and Configuration

### Prerequisites
- .NET 6.0 or later
- Node.js (for MCP inspector tool)

### Environment Files
- `.env`: Contains environment-specific variables
- `appsettings.json`: Main application settings
- `appsettings.Development.json`: Development-specific settings

### Running the Server

1. Standard run:
```bash
dotnet run
```

2. With MCP inspector:
```bash
npx @modelcontextprotocol/inspector dotnet run
```

## Development

The project follows standard ASP.NET Core practices with additional MCP-specific components:

- Services are registered in the DI container
- Tools are automatically discovered from the assembly
- Logging is configured for development and debugging
- Swagger is available for API documentation

## Deployment

The project includes:
- `Procfile`: For platform deployment
- `nixpacks.toml`: Build configuration
- Standard ASP.NET Core deployment options

## Testing

A test script (`test-mcp.js`) is included for verifying MCP functionality.

## Health Check

Access the root endpoint (`/`) to verify server status:
```
GET / -> "Comedy MCP Server is running!"
```

## API Documentation

When running, access Swagger UI at:
```
/swagger
```

## Contributing

Feel free to contribute by:
1. Implementing new comedy-related tools
2. Enhancing existing joke generation
3. Improving error handling and logging
4. Adding new comedy services

## License

[Your license information here] 