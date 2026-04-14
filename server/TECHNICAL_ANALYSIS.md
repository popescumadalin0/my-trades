# MyTrades Server - Document Tehnic

## 1. Rezumat

Solutia este o aplicatie .NET 10 compusa din mai multe proiecte separate pe responsabilitati:

- `MyTrades.Engine` este host-ul principal si compune serviciile.
- `MyTrades.Api` este un API HTTP separat, foarte minimal, folosit in prezent doar pentru un endpoint demo.
- `MyTrades.External.Api` este un API HTTP mock care simuleaza candles de piata.
- `MyTrades.Processor`, `MyTrades.Indicator`, `MyTrades.Strategy` implementeaza pipeline-ul intern bazat pe evenimente.
- `MyTrades.Gateway` face integrarea HTTP catre API-ul extern mock.
- `MyTrades.Persistence` gestioneaza PostgreSQL, Redis si migratiile.
- `MyTrades.EventSource` ofera bus-ul intern de evenimente si dispatcher-ul de handlers.
- `MyTrades.Domain` contine modelele de baza.

Arhitectural, intentia este:

1. `Processor` ia lista de simboluri.
2. Apeleaza gateway-ul extern pentru candle-uri.
3. Persista datele in PostgreSQL si Redis.
4. Publica evenimente catre `Indicator`.
5. `Indicator` ar trebui sa calculeze indicatori si sa emita alte evenimente.
6. `Strategy` ar trebui sa reactioneze la indicatori.

In starea actuala, designul exista, dar mai multe legaturi importante sunt incomplete sau defecte, deci doar o parte mica din flux este efectiv functionala.

## 2. Structura completa a proiectului

Mai jos este structura relevanta a repository-ului. Folderele `bin/` si `obj/` exista in aproape toate proiectele si contin artefacte generate la build; nu contin logica sursa, dar sunt prezente in workspace.

```text
server/
|-- .idea/
|   `-- .idea.MyTrades.Server/
|       `-- .idea/
|           |-- .name
|           |-- git_toolbox_prj.xml
|           |-- indexLayout.xml
|           |-- projectSettingsUpdater.xml
|           |-- vcs.xml
|           `-- workspace.xml
|-- MyTrades.Server.sln
|-- MyTrades.Server.sln.DotSettings.user
|-- TECHNICAL_ANALYSIS.md
`-- src/
    |-- MyTrades.Api/
    |   |-- appsettings.Development.json
    |   |-- appsettings.json
    |   |-- MyTrades.Api.csproj
    |   |-- Program.cs
    |   |-- Properties/
    |   |   `-- launchSettings.json
    |   |-- TestEndpoint/
    |   |   |-- MyEndpoint.cs
    |   |   |-- MyRequest.cs
    |   |   `-- MyResponse.cs
    |   |-- bin/
    |   `-- obj/
    |-- MyTrades.Domain/
    |   |-- IEntity.cs
    |   |-- MyTrades.Domain.csproj
    |   |-- _.cs
    |   |-- Market/
    |   |   |-- Candle.cs
    |   |   `-- Symbol.cs
    |   |-- bin/
    |   `-- obj/
    |-- MyTrades.Engine/
    |   |-- appsettings.Development.json
    |   |-- appsettings.json
    |   |-- MyTrades.Engine.csproj
    |   |-- Program.cs
    |   |-- Logs/
    |   |   |-- applog-20260407.txt
    |   |   `-- applog-20260414.txt
    |   |-- Properties/
    |   |   `-- launchSettings.json
    |   |-- bin/
    |   `-- obj/
    |-- MyTrades.EventSource/
    |   |-- DependencyInjection.cs
    |   |-- IEventHandler.cs
    |   |-- InMemoryEventBus.cs
    |   |-- MyTrades.EventSource.csproj
    |   |-- BackgroundService/
    |   |   `-- EventDispatcher.cs
    |   |-- Events/
    |   |   |-- CandleCreated.cs
    |   |   |-- IndicatorUpdated.cs
    |   |   `-- SymbolUpdated.cs
    |   |-- bin/
    |   `-- obj/
    |-- MyTrades.External.Api/
    |   |-- appsettings.Development.json
    |   |-- appsettings.json
    |   |-- MyTrades.External.Api.csproj
    |   |-- Program.cs
    |   |-- Properties/
    |   |   `-- launchSettings.json
    |   |-- bin/
    |   `-- obj/
    |-- MyTrades.Gateway/
    |   |-- DependencyInjection.cs
    |   |-- MockApiService.cs
    |   |-- MyTrades.Gateway.csproj
    |   |-- Exceptions/
    |   |   `-- ApiException.cs
    |   |-- Refit/
    |   |   |-- ApiResponse.cs
    |   |   |-- RefitApiClient.cs
    |   |   |-- Clients/
    |   |   |   `-- MockApi.cs
    |   |   `-- Responses/
    |   |       `-- CandleResponse.cs
    |   |-- bin/
    |   `-- obj/
    |-- MyTrades.Indicator/
    |   |-- DependencyInjection.cs
    |   |-- MyTrades.Indicator.csproj
    |   |-- Events/
    |   |   `-- CandleCreatedHandler.cs
    |   |-- Profiles/
    |   |   `-- MapsterConfig.cs
    |   |-- bin/
    |   `-- obj/
    |-- MyTrades.Persistence/
    |   |-- CacheDriver.cs
    |   |-- DapperDbContext.cs
    |   |-- DependencyInjection.cs
    |   |-- MigrationRunner.cs
    |   |-- MyTrades.Persistence.csproj
    |   |-- PostgresRepositoryDriver.cs
    |   |-- PostgresStore.cs
    |   |-- RedisStore.cs
    |   |-- StoreFactory.cs
    |   |-- Contracts/
    |   |   |-- ICacheDriver.cs
    |   |   |-- IRepositoryDriver.cs
    |   |   `-- IStore.cs
    |   |-- Enums/
    |   |   `-- StorageType.cs
    |   |-- Migrations/
    |   |   |-- 000_Init.sql
    |   |   `-- 001_AddIndexes.sql
    |   |-- bin/
    |   `-- obj/
    |-- MyTrades.Processor/
    |   |-- DependencyInjection.cs
    |   |-- MyTrades.Processor.csproj
    |   |-- SymbolLookup.cs
    |   |-- WebSocketListener.cs
    |   |-- BackgroundServices/
    |   |   |-- MarketPollingService.cs
    |   |   `-- SymbolRefresher.cs
    |   |-- Contracts/
    |   |   `-- ISymbolLookup.cs
    |   |-- EventHandlers/
    |   |   `-- SymbolUpdatedHandler.cs
    |   |-- Profiles/
    |   |   `-- MapsterConfig.cs
    |   |-- bin/
    |   `-- obj/
    `-- MyTrades.Strategy/
        |-- DependencyInjection.cs
        |-- MyTrades.Strategy.csproj
        |-- EventHandlers/
        |   `-- IndicatorUpdatedHandler.cs
        |-- Profiles/
        |   `-- MapsterConfig.cs
        |-- bin/
        `-- obj/
```

## 3. Module principale

### 3.1 `MyTrades.Engine`

Rol:
- host-ul principal al aplicatiei
- inregistreaza logging, Mapster, event source, persistence, processor, indicatori si strategii
- ruleaza migratiile la startup

Cum functioneaza:
- `Program.cs` construieste `WebApplication`
- configureaza Serilog din `appsettings`
- cheama:
  - `RegisterProcessor(...)`
  - `RegisterIndicators(...)`
  - `RegisterStrategies(...)`
  - `AddEventSourceServices(...)`
  - `AddPersistenceServices(...)`
- inainte de `app.Run()`, ruleaza `MigrationRunner.RunAsync()`

Observatii:
- nu expune endpoints HTTP proprii
- este practic orchestratorul serviciilor de fundal

### 3.2 `MyTrades.Api`

Rol:
- API web separat de host-ul principal
- in starea actuala este doar un exemplu de FastEndpoints

Cum functioneaza:
- `Program.cs` activeaza `FastEndpoints`, logging si `Mapster`
- endpoint-ul `TestEndpoint/MyEndpoint.cs` defineste un POST demo care concateneaza numele si calculeaza daca varsta este peste 18

Observatii:
- nu este conectat la engine, persistence sau event bus
- nu exista endpoint-uri reale de business

### 3.3 `MyTrades.External.Api`

Rol:
- sursa externa mock pentru candle-uri

Cum functioneaza:
- `Program.cs` expune `GET /api/candle`
- `MarketSimulator` genereaza pseudo-random un candle pe minut per simbol
- foloseste doua `ConcurrentDictionary`:
  - una pentru candle-ul curent pe minut
  - una pentru ultimul `Close` pe simbol

Observatii:
- este explicit un mock/simulator, nu o integrare reala de market data
- este singura integrare HTTP externa efectiv implementata

### 3.4 `MyTrades.Domain`

Rol:
- defineste modelele de baza

Continut:
- `IEntity.cs`: contract comun cu `Id`
- `Market/Candle.cs`: model de candle intern
- `Market/Symbol.cs`: model de simbol
- `_.cs`: marker class (`DomainMarker`)

Observatii:
- modelele sunt foarte simple, fara validare sau comportament de domeniu

### 3.5 `MyTrades.EventSource`

Rol:
- infrastructura interna de event bus
- dispatch catre handlers descoperiti din DI

Cum functioneaza:
- `IEventHandler.cs` defineste `IEvent` si `IEventHandler<T>`
- `InMemoryEventBus.cs` foloseste `Channel<IEvent>` pentru publish/consume
- `BackgroundService/EventDispatcher.cs` citeste evenimentele si cauta handlers in scope nou
- `DependencyInjection.cs`:
  - configureaza Marten
  - porneste `EventDispatcher`
  - inregistreaza `IEventBus`
  - face scan pentru toate implementarile `IEventHandler<>`

Observatii importante:
- Marten este configurat, dar nu este folosit mai departe in cod
- event bus-ul este in memorie, deci evenimentele nu sunt persistate
- `IEventBus` este inregistrat `Transient`, ceea ce rupe fluxul: publisherii si dispatcher-ul pot primi instante diferite de bus

### 3.6 `MyTrades.Gateway`

Rol:
- adaptor pentru apeluri HTTP catre sursa externa mock

Cum functioneaza:
- `DependencyInjection.cs` configureaza un client Refit cu `Gateway:MockApi:BaseUrl`
- `Refit/Clients/MockApi.cs` defineste apelul `GET api/candle`
- `MockApiService.cs` expune `ICandleGatewayService`
- `RefitApiClient.cs` normalizeaza raspunsurile si transforma exceptiile Refit in `ApiResponse`

Observatii:
- numele claselor arata clar ca integrarea curenta este mock (`MockApi`, `MockApiClient`)
- in fisierele de configurare existente nu apare cheia `Gateway:MockApi:BaseUrl`, deci serviciul nu poate porni corect fara configurare suplimentara

### 3.7 `MyTrades.Persistence`

Rol:
- infrastructura de stocare pentru PostgreSQL si Redis
- runner de migratii

Cum functioneaza:
- `DapperDbContext.cs` deschide conexiuni PostgreSQL cu `Npgsql`
- `PostgresRepositoryDriver<TEntity>` implementeaza CRUD generic prin Dapper
- `PostgresStore<TEntity>` face upsert logic: daca entitatea exista -> update, altfel insert
- `CacheDriver<TEntity>` serializeaza cu MessagePack in `IDistributedCache`
- `RedisStore<TEntity>` persista entitati in cache
- `DependencyInjection.cs` inregistreaza:
  - Redis cache
  - repository/store pentru `Candle`
  - repository/store pentru `Symbol`
  - cache repository si `RedisStore<Candle>`
  - `MigrationRunner`

Observatii importante:
- exista doua implementari `IStore<Candle>`: PostgreSQL si Redis; ambele sunt folosite in `Processor`
- `StoreFactory` exista, dar nu este folosit nicaieri
- `StorageType.Memory` exista in enum, dar nu exista implementare
- `CacheDriver.SetItem()` scrie cheia bruta, iar `GetItem()` cauta cheia prefixata cu `Item_`; citirea din Redis nu poate functiona corect in forma actuala
- `MigrationRunner` citeste resursele embedded din assembly-ul `MyTrades.Domain`, desi scripturile SQL sunt embedded in `MyTrades.Persistence`; in forma actuala nu va gasi migratiile corecte
- SQL-urile de migrare par inconsistente sau rupte:
  - `000_Init.sql` este corupt sintactic si include un fragment fara `CREATE TABLE IF NOT EXISTS symbols`
  - `001_AddIndexes.sql` indexeaza coloana `candles.symbol`, dar tabela definita foloseste `symbol_id`

### 3.8 `MyTrades.Processor`

Rol:
- orchestrarea datelor de piata
- polling periodic
- publicarea evenimentelor de tip `CandleCreated`
- sincronizarea lookup-ului de simboluri

Cum functioneaza:
- `BackgroundServices/MarketPollingService.cs`
  - asteapta alinierea la minutul urmator
  - ia simbolurile din `ISymbolLookup`
  - pentru fiecare simbol:
    - cere candle prin `ICandleGatewayService`
    - mapeaza la eveniment `CandleCreated`
    - publica in `IEventBus`
    - mapeaza la entitate `Domain.Market.Candle`
    - persista in toate `IStore<Candle>` disponibile
- `BackgroundServices/SymbolRefresher.cs`
  - citeste toate simbolurile din PostgreSQL
  - le incarca in `ISymbolLookup`
- `SymbolUpdatedHandler.cs`
  - ar trebui sa actualizeze lookup-ul la evenimente `SymbolUpdated`

Observatii importante:
- `SymbolLookup` este inregistrat `Scoped`, dar tine stare in-memory intr-un `ConcurrentDictionary`; cum se creeaza scope nou des, starea nu se pastreaza global
- `SymbolLookup.GetAllAsync()` intoarce `null` cand lookup-ul nu este initializat; `MarketPollingService` face `symbols.Select(...)`, ceea ce poate produce exceptie
- `SymbolUpdatedHandler` nu face `await` la `_lookUp.StoreSymbolNameAsync(...)`; metoda returneaza imediat `Task.CompletedTask`
- mapping-urile Mapster sunt configurate invers fata de modul in care sunt folosite in polling pentru unele tipuri
- `WebSocketListener.cs` este doar placeholder

### 3.9 `MyTrades.Indicator`

Rol:
- ar trebui sa proceseze `CandleCreated` si sa emita indicatori

Cum functioneaza:
- `DependencyInjection.cs` inregistreaza `CandleCreatedHandler`
- `CandleCreatedHandler.cs` implementeaza handler-ul, dar `Handle(...)` nu face nimic

Observatii:
- modulul este conectat in DI si in event system
- logica de indicatori nu exista inca

### 3.10 `MyTrades.Strategy`

Rol:
- ar trebui sa reactioneze la evenimente de indicatori

Cum functioneaza:
- `DependencyInjection.cs` inregistreaza `IndicatorUpdatedHandler`
- `IndicatorUpdatedHandler.cs` returneaza imediat `Task.CompletedTask`

Observatii:
- modulul este conectat structural
- logica de strategie nu exista inca

## 4. Endpoints si API-uri existente

### 4.1 API-uri HTTP

#### `MyTrades.Api`

- `POST /api/user/create`
  - intrare: `MyRequest`
    - `FirstName`
    - `LastName`
    - `Age`
  - iesire: `MyResponse`
    - `FullName`
    - `IsOver18`
  - scop: endpoint demo/test

#### `MyTrades.External.Api`

- `GET /api/candle?symbol=BTCUSDT`
  - parametru query: `symbol` cu default `BTCUSDT`
  - raspuns: candle generat de `MarketSimulator`
  - scop: simulare/mock de market data

### 4.2 API-uri interne / contracte de integrare

- `ICandleGatewayService.GetCandlesAsync(symbol, cancellationToken)`
- `IMockApi.GetCandleAsync(symbol, cancellationToken)` prin Refit
- `IEventBus.PublishAsync(IEvent evt)`
- `IEventBus.ReadAllAsync(CancellationToken ct)`
- `IStore<TEntity>.GetAsync(...)`
- `IStore<TEntity>.StoreAsync(...)`
- `IRepositoryDriver<TEntity>` pentru CRUD generic
- `ISymbolLookup`

## 5. Ce este conectat si functional vs stub/mock/neconectat

### 5.1 Conectat si aparent functional

- `MyTrades.External.Api` ca server HTTP mock pentru `/api/candle`
- `MyTrades.Api` ca API demo FastEndpoints
- `Gateway` -> apel Refit catre endpoint-ul mock, daca este furnizat `Gateway:MockApi:BaseUrl`
- inregistrarea serviciilor in `Engine`
- Dapper repository generic si Redis cache wiring la nivel DI

### 5.2 Conectat structural, dar incomplet sau nefunctional in practica

- pipeline-ul de evenimente `Processor -> EventBus -> Indicator/Strategy`
  - motiv: `IEventBus` este `Transient`, nu partajat corect intre publisheri si dispatcher
- `SymbolRefresher -> SymbolLookup -> MarketPollingService`
  - motiv: `SymbolLookup` este `Scoped` si starea in-memory nu este comuna intre scope-uri
- `MigrationRunner`
  - motiv: cauta scripturile in assembly gresit
- migratiile SQL
  - motiv: scripturile sunt inconsistente sau invalide
- Redis read/write pentru `CacheDriver`
  - motiv: foloseste chei diferite la scriere fata de citire

### 5.3 Stub / mock / placeholder

- `MyTrades.External.Api` este mock market data
- `MyTrades.Gateway` foloseste explicit `MockApi`
- `MyTrades.Processor/WebSocketListener.cs` este placeholder marcat `//todo`
- `MyTrades.Indicator/Events/CandleCreatedHandler.cs` este stub
- `MyTrades.Strategy/EventHandlers/IndicatorUpdatedHandler.cs` este stub
- `MyTrades.Api/TestEndpoint/*` este endpoint demonstrativ, nu business endpoint real

### 5.4 Neconectat sau nefolosit efectiv

- Marten este configurat in `EventSource`, dar nu este folosit explicit
- `StoreFactory` exista, dar nu este consumat
- `StorageType.Memory` nu are implementare concreta

## 6. Dependente principale

### Biblioteci externe

- `FastEndpoints` - framework pentru endpoint-uri HTTP in `MyTrades.Api`
- `Mapster`, `Mapster.DependencyInjection` - mapare intre DTO-uri, entitati si evenimente
- `Serilog`, `Serilog.Extensions.Logging`, `Serilog.Settings.Configuration`, `Serilog.Sinks.Console`, `Serilog.Sinks.File` - logging
- `Dapper` - acces SQL simplificat
- `Npgsql` - driver PostgreSQL
- `Marten` - event/document store peste PostgreSQL, momentan doar configurat
- `Scrutor` - scan/inject pentru handlers
- `Refit`, `Refit.HttpClientFactory` - client HTTP typed pentru API-ul mock
- `MessagePack` - serializare pentru cache
- `Microsoft.Extensions.Caching.StackExchangeRedis` - cache Redis distribuit

### Configuratii importante observate

- `ConnectionStrings:DefaultConnection`
  - definit in `MyTrades.Engine/appsettings.Development.json`
  - definit in `MyTrades.External.Api/appsettings.Development.json`
- `ConnectionStrings:Redis`
  - definit in `MyTrades.Api/appsettings.Development.json`
  - definit in `MyTrades.Engine/appsettings.Development.json`
  - definit in `MyTrades.External.Api/appsettings.Development.json`
- `Gateway:MockApi:BaseUrl`
  - referit in cod, dar absent din fisierele `appsettings` analizate

## 7. TODO-uri si comentarii lasate in cod

### TODO explicit

- `src/MyTrades.Processor/WebSocketListener.cs`
  - comentariu: `//todo`
  - clasa este goala

### Comentarii relevante

- `src/MyTrades.EventSource/DependencyInjection.cs`
  - comentarii despre configurarea Marten si schema DB
- `src/MyTrades.Engine/Program.cs`
  - comentarii care arata ca unele optiuni Serilog au fost dezactivate
- `src/MyTrades.Api/Program.cs`
  - comentarii similare despre Serilog
- `src/MyTrades.Persistence/DependencyInjection.cs`
  - comentariu pentru `InstanceName` Redis
- `src/MyTrades.External.Api/Program.cs`
  - comentarii despre volatilitate si trend in simulator
- `src/MyTrades.Persistence/Migrations/000_Init.sql`
  - comentarii despre timeframe-uri (`1m`, `5m`, `1h`)

## 8. Probleme si riscuri tehnice identificate

- Event bus-ul in memorie este inregistrat cu lifetime gresit (`Transient`), deci evenimentele publicate probabil nu ajung la dispatcher-ul activ.
- `SymbolLookup` nu poate functiona ca memorie comuna deoarece este `Scoped`.
- `SymbolLookup.GetAllAsync()` poate intoarce `null`, iar consumatorul presupune colectie valida.
- Configuratia `Gateway:MockApi:BaseUrl` lipseste.
- Handler-ele din `Indicator` si `Strategy` sunt goale, deci pipeline-ul de business se opreste acolo.
- `MigrationRunner` foloseste assembly gresit pentru embedded resources.
- Scripturile SQL de migrare sunt defecte si par nealiniate la modelele C# actuale.
- `CacheDriver` citeste si scrie sub chei diferite.
- `SymbolUpdatedHandler` nu asteapta operatia async.
- `WebSocketListener` nu este implementat deloc.

## 9. Concluzie

Proiectul are o structura modulara clara si o directie arhitecturala buna pentru un sistem de procesare market data bazat pe evenimente. Totusi, implementarea este intr-un stadiu intermediar: host-urile web pornesc, mock-ul extern exista, iar wiring-ul dintre module este in mare parte prezent, dar fluxul end-to-end real nu poate fi considerat stabil sau complet functional fara corectarea problemelor de DI, config, migratii si a modulelor inca neimplementate.
