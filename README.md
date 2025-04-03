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
- .NET 8.0 SDK or later
- Node.js 14+ (for MCP inspector tool)
- A code editor (preferably Cursor IDE for best integration)
- Git (for version control)

### Installation Steps

1. Clone the repository:
```bash
git clone [your-repository-url]
cd ComedyMcpServer
```

2. Set up environment:
   - Copy the example environment file:
     ```bash
     copy .env.example .env    # On Windows
     # OR
     cp .env.example .env     # On Unix-based systems
     ```
   - Update the `.env` file with your settings if needed

3. Install dependencies:
```bash
dotnet restore
```

4. Build the project:
```bash
dotnet build
```

### Running the Server

You have several options to run the server:

1. **Development Mode**:
```bash
dotnet run --environment Development
```

2. **With MCP Inspector** (recommended for debugging MCP tools):
```bash
npx @modelcontextprotocol/inspector dotnet run
```

3. **Production Mode**:
```bash
dotnet run --environment Production
```

### Verifying Installation

1. Check the server is running:
   - Open your browser to `http://localhost:5000` or the configured port
   - You should see the message "Comedy MCP Server is running!"

2. Access Swagger Documentation:
   - Navigate to `http://localhost:5000/swagger`
   - You should see the API documentation

3. Test MCP Tools:
   - Use the included test script:
     ```bash
     node test-mcp.js
     ```
   - Or use the MCP inspector interface if running with inspector

### Troubleshooting

Common issues and solutions:

1. Port already in use:
   - Change the port in `appsettings.json`
   - Or stop the process using the current port

2. Dependencies missing:
   - Run `dotnet restore` again
   - Check your .NET SDK version matches the project requirements

3. Environment variables not loading:
   - Ensure `.env` file exists and is properly formatted
   - Restart the application after modifying environment files

For more detailed issues, check the application logs in the console output.

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