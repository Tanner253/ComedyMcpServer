# Comedy MCP Server

This project implements a simple Model Context Protocol (MCP) server using the official C# MCP SDK (`ModelContextProtocol`). The server provides a tool called `EnhanceComment` that fetches a programming/miscellaneous joke from the [JokeAPI](https://v2.jokeapi.dev/) and adds it to a user-provided comment.

The server is designed to provide humorous context for AI agents, potentially:
*   Boosting morale for solo developers.
*   Enabling playful trolling among coworkers.

## Prerequisites

*   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
*   [Node.js and npm](https://nodejs.org/) (for running the MCP Inspector)

## Getting Started

1.  **Clone the repository (or ensure you have the project files).**
2.  **Navigate to the project directory:**
    ```bash
    cd ComedyMcpServer
    ```
3.  **Restore dependencies (if needed, typically handled by build/run):**
    ```bash
    dotnet restore
    ```

## Running the Server

This server uses standard input/output (stdio) for MCP communication, as recommended by the SDK for use with development tools like the MCP Inspector.

1.  **Install the MCP Inspector (if you haven't already):**
    ```bash
    npm install -g @modelcontextprotocol/inspector
    ```
    *(Alternatively, you can use `npx` to run it without global installation, as shown below)*

2.  **Run the server using the MCP Inspector:**
    ```bash
    npx @modelcontextprotocol/inspector dotnet run
    ```
    or if installed globally:
    ```bash
    mcp-inspector dotnet run
    ```

3.  The inspector will start the server and provide a local URL (e.g., `http://localhost:5173`). Open this URL in your web browser.

## Testing with MCP Inspector

1.  Once the MCP Inspector UI is open in your browser, click the **Connect** button.
2.  The Inspector will connect to your running server via stdio.
3.  Click the **List Tools** button.
4.  You should see the `ComedyTool` category and the `EnhanceComment` tool within it.
5.  Click on the `EnhanceComment` tool.
6.  Enter a comment into the `comment` parameter input field (e.g., `I feel lonely`, `let's troll the team lead`).
7.  Click the **Run Tool** button.
8.  The server will process the request, fetch a joke, and return the enhanced comment, which will be displayed in the Inspector's results area.

## Implementation Details

*   **Framework:** .NET 8 Console Application
*   **MCP SDK:** `ModelContextProtocol` (Prerelease)
*   **Dependencies:** `Microsoft.Extensions.Hosting`, `System.Net.Http.Json`
*   **Joke Source:** [JokeAPI](https://v2.jokeapi.dev/)
*   **Communication:** Standard I/O (Stdio)

## Next Steps (GitHub)

To host this on GitHub:

1.  Create a new repository on [GitHub.com](https://github.com/new).
2.  **Do not** initialize the new repository with a README, license, or .gitignore file (as we've already created them locally).
3.  Follow the instructions provided by GitHub under "…or push an existing repository from the command line":
    ```bash
    git remote add origin <Your-GitHub-Repo-URL.git>
    git branch -M main
    git push -u origin main
    ``` 