# MyTrades

An event-driven **market data processing** application built in **.NET 10**, designed to ingest trading candle data, compute indicators, and react via configurable strategies. The architecture separates concerns into multiple focused projects.

> **Note:** The project is a work in progress. The modular structure and wiring are in place, but several components (indicators, strategies, migrations) are still incomplete or stubbed.
>
> ## Overview
>
> MyTrades polls market candle data from an external API (currently a local mock), persists it to PostgreSQL and Redis, and publishes internal events through an in-memory event bus. Downstream modules (Indicator, Strategy) are wired up but not yet fully implemented.
>
> ## Architecture
>
> ```
> Processor
>   └── polls candles from Gateway (HTTP mock)
>   └── persists to PostgreSQL + Redis
>   └── publishes CandleCreated → EventBus
>         └── Indicator (stub — todo)
>               └── Strategy (stub — todo)
> ```
>
> ## Project Structure
>
> ```
> server/src/
>   MyTrades.Engine          Host — composes and starts all background services
>   MyTrades.Api             HTTP API (FastEndpoints demo endpoint)
>   MyTrades.External.Api    Mock market data server (GET /api/candle)
>   MyTrades.Gateway         Refit HTTP client for the external mock API
>   MyTrades.Processor       Market polling, symbol refresh, event publishing
>   MyTrades.Indicator       Event handler for CandleCreated (stub)
>   MyTrades.Strategy        Event handler for IndicatorUpdated (stub)
>   MyTrades.EventSource     In-memory event bus + dispatcher
>   MyTrades.Persistence     PostgreSQL (Dapper) + Redis (MessagePack) storage
>   MyTrades.Domain          Core domain models (Candle, Symbol)
>
> client/                    Client application
> contexts/                  Context definitions
> sdk/MyTrades.Contracts/    Shared SDK contracts
> ```
>
> ## Technologies
>
> - C# / .NET 10
> - - FastEndpoints
>   - - Dapper + Npgsql (PostgreSQL)
>     - - StackExchange.Redis
>       - - Refit (typed HTTP client)
>         - - Mapster (object mapping)
>           - - Serilog (structured logging)
>             - - MessagePack (Redis serialization)
>               - - Marten (configured, not yet used)
>                
>                 - ## Setup
>                
>                 - ### Prerequisites
>                
>                 - - .NET 10 SDK
> - PostgreSQL instance
> - - Redis instance
>  
>   - ### Configuration
>  
>   - Copy and update the connection strings in `appsettings.Development.json` for the relevant projects:
>  
>   - ```json
>     {
>       "ConnectionStrings": {
>         "DefaultConnection": "Host=localhost;Database=mytrades;Username=...;Password=...",
>         "Redis": "localhost:6379"
>       },
>       "Gateway": {
>         "MockApi": {
>           "BaseUrl": "https://localhost:PORT"
>         }
>       }
>     }
>     ```
>
> ### Run
>
> 1. Start the mock external API:
> 2.    ```bash
>          dotnet run --project server/src/MyTrades.External.Api
>          ```
>
> 2. Start the main engine:
> 3.    ```bash
>          dotnet run --project server/src/MyTrades.Engine
>          ```
>
> 3. Optionally start the HTTP API:
> 4.    ```bash
>          dotnet run --project server/src/MyTrades.Api
>          ```
>
> See `server/TECHNICAL_ANALYSIS.md` for a detailed breakdown of all modules, known issues, and current limitations.
