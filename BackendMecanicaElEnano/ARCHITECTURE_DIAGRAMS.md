# 🎨 Architecture Visual Guide

## Request Flow Diagram

### Complete Request/Response Flow

```
┌─────────────────────────────────────────────────────────────────────┐
│                          CLIENT REQUEST                             │
│                    POST /api/presupuestos                           │
│                    { "vehiculoId": "guid" }                         │
└────────────────────────────┬────────────────────────────────────────┘
							 │
							 ▼
┌─────────────────────────────────────────────────────────────────────┐
│                   GLOBAL EXCEPTION HANDLER                          │
│                      (Middleware Layer)                             │
│  ✓ Wraps entire request pipeline                                   │
│  ✓ Catches all unhandled exceptions                                │
│  ✓ Returns consistent error responses                              │
│  ✓ Logs errors with TraceId                                        │
└────────────────────────────┬────────────────────────────────────────┘
							 │
							 ▼
┌─────────────────────────────────────────────────────────────────────┐
│                    PRESUPUESTO CONTROLLER                           │
│                   (HTTP/API Layer - Thin)                           │
│  📝 Responsibilities:                                               │
│     • Parse HTTP request                                            │
│     • Call service method                                           │
│     • Log HTTP operation                                            │
│     • Map Result → HTTP status code                                 │
│     • Return HTTP response                                          │
│                                                                     │
│  ❌ Does NOT:                                                       │
│     • Validate business rules                                       │
│     • Access database                                               │
│     • Contain business logic                                        │
│                                                                     │
│  Code:                                                              │
│  var result = await _service.CreateAsync(vehiculoId);              │
│  if (result.IsFailure)                                             │
│      return BadRequest(new { error = result.Error });              │
│  return CreatedAtAction(..., result.Value);                        │
└────────────────────────────┬────────────────────────────────────────┘
							 │
							 ▼
┌─────────────────────────────────────────────────────────────────────┐
│                    PRESUPUESTO SERVICE                              │
│                  (Business Logic Layer)                             │
│  🧠 Responsibilities:                                               │
│     • Validate input (GUID empty?)                                  │
│     • Enforce business rules (FK exists?)                           │
│     • Coordinate multiple repositories                              │
│     • Apply business logic (default values, calculations)           │
│     • Handle transactions via UnitOfWork                            │
│     • Log business operations                                       │
│     • Return Result<T> with success/failure                         │
│                                                                     │
│  ❌ Does NOT:                                                       │
│     • Know about HTTP                                               │
│     • Access DbContext directly                                     │
│     • Map to/from DTOs (uses AutoMapper)                            │
│                                                                     │
│  Code:                                                              │
│  _logger.LogInformation("Creating presupuesto...");                │
│  if (vehiculoId == Guid.Empty)                                     │
│      return Result.Failure("Invalid ID");                          │
│  var exists = await _unitOfWork.Vehiculos                          │
│      .FindByCondition(v => v.Id == vehiculoId).AnyAsync();         │
│  if (!exists)                                                      │
│      return Result.Failure("Vehicle not found");                   │
│  // Create entity...                                               │
│  _unitOfWork.Presupuestos.Create(presupuesto);                     │
│  await _unitOfWork.CommitAsync();                                  │
│  return Result.Success(dto);                                       │
└────────────────────────────┬────────────────────────────────────────┘
							 │
							 ▼
┌─────────────────────────────────────────────────────────────────────┐
│                      UNIT OF WORK                                   │
│                 (Transaction Coordinator)                           │
│  🔄 Responsibilities:                                               │
│     • Provide access to all repositories                            │
│     • Coordinate SaveChanges across repos                           │
│     • Manage transactions                                           │
│     • Rollback on error                                             │
│     • Log transaction operations                                    │
│                                                                     │
│  Properties:                                                        │
│     • IPresupuestoRepository Presupuestos                           │
│     • IVehiculoRepository Vehiculos                                 │
│     • ITrabajoRepository Trabajos                                   │
│     • ITurnoRepository Turnos                                       │
│     • IOrdenTrabajoRepository OrdenesTrabajo                        │
│                                                                     │
│  Methods:                                                           │
│     • Task<int> CommitAsync()    ← Single SaveChanges point        │
│     • Task RollbackAsync()       ← Undo all changes                │
│                                                                     │
│  Code:                                                              │
│  public async Task<int> CommitAsync()                              │
│  {                                                                  │
│      var changes = await _context.SaveChangesAsync();              │
│      _logger.LogInformation("Saved {Count} changes", changes);     │
│      return changes;                                                │
│  }                                                                  │
└────────────────────────────┬────────────────────────────────────────┘
							 │
							 ▼
┌─────────────────────────────────────────────────────────────────────┐
│                  PRESUPUESTO REPOSITORY                             │
│                   (Data Access Layer - Thin)                        │
│  💾 Responsibilities:                                               │
│     • ONLY data access operations                                   │
│     • Inherits CRUD from RepositoryBase<T>                          │
│     • Add custom queries if needed                                  │
│                                                                     │
│  ❌ Does NOT:                                                       │
│     • Validate business rules                                       │
│     • Call SaveChanges (UnitOfWork does this)                       │
│     • Map DTOs                                                      │
│     • Contain business logic                                        │
│                                                                     │
│  From RepositoryBase<T>:                                            │
│     • IQueryable<T> FindAll()                                       │
│     • IQueryable<T> FindByCondition(Expression<Func<T, bool>>)     │
│     • void Create(T entity)                                         │
│     • void Update(T entity)                                         │
│     • void Delete(T entity)                                         │
│                                                                     │
│  Complete Implementation:                                           │
│  public class PresupuestoRepository :                              │
│      RepositoryBase<Presupuesto>, IPresupuestoRepository           │
│  {                                                                  │
│      public PresupuestoRepository(MecanicaContext context)         │
│          : base(context) { }                                        │
│  }                                                                  │
│  ← That's it! Only 15 lines!                                       │
└────────────────────────────┬────────────────────────────────────────┘
							 │
							 ▼
┌─────────────────────────────────────────────────────────────────────┐
│                      ENTITY FRAMEWORK CORE                          │
│                         (MecanicaContext)                           │
│  📊 Responsibilities:                                               │
│     • Track entity changes                                          │
│     • Generate SQL queries                                          │
│     • Execute queries against database                              │
│     • Lazy load navigation properties                               │
│     • Manage connections                                            │
└────────────────────────────┬────────────────────────────────────────┘
							 │
							 ▼
┌─────────────────────────────────────────────────────────────────────┐
│                        SQL SERVER DATABASE                          │
│                                                                     │
│  Tables:                                                            │
│     • presupuesto                                                   │
│     • vehiculo                                                      │
│     • trabajo                                                       │
│     • turno                                                         │
│     • orden_trabajo                                                 │
│     • repuesto                                                      │
└─────────────────────────────────────────────────────────────────────┘
```

---

## Result Pattern Flow

### Success Flow
```
Service Method
	 │
	 ├─► Validation Passes
	 │
	 ├─► Business Logic Executes
	 │
	 ├─► Repository Operations
	 │
	 ├─► UnitOfWork.CommitAsync()
	 │
	 └─► return Result.Success(dto)
			  │
			  ▼
		 Controller
			  │
			  ├─► result.IsSuccess == true
			  │
			  └─► return Ok(result.Value)
					   │
					   ▼
				  HTTP 200/201
				  { data }
```

### Failure Flow (Validation)
```
Service Method
	 │
	 ├─► Validation FAILS
	 │
	 └─► return Result.Failure("Error message")
			  │
			  ▼
		 Controller
			  │
			  ├─► result.IsFailure == true
			  │
			  └─► return BadRequest(new { error = result.Error })
					   │
					   ▼
				  HTTP 400
				  { "error": "Validation message" }
```

### Failure Flow (Exception)
```
Service Method
	 │
	 ├─► try { ... }
	 │
	 ├─► Exception thrown!
	 │
	 └─► catch (Exception ex)
			  │
			  ├─► _logger.LogError(ex, ...)
			  │
			  ├─► await _unitOfWork.RollbackAsync()
			  │
			  └─► return Result.Failure("Error message")
					   │
					   ▼
				  Controller
					   │
					   └─► return BadRequest/InternalServerError
								│
								▼
						   HTTP 400/500
						   { "error": "..." }
```

### Unhandled Exception Flow
```
Any Layer
	 │
	 ├─► Unhandled Exception!
	 │
	 └─► Caught by Global Exception Handler Middleware
			  │
			  ├─► _logger.LogError(ex, "Unhandled exception")
			  │
			  ├─► context.Response.StatusCode = 500
			  │
			  └─► Write JSON error response
					   │
					   ▼
				  HTTP 500
				  {
					"statusCode": 500,
					"message": "Ha ocurrido un error interno",
					"traceId": "00-abc123-def456-00",
					"details": null  // only in Development
				  }
```

---

## Dependency Injection Flow

### Startup (Program.cs)
```
┌─────────────────────────────────────────────────────────────────┐
│                         PROGRAM.CS                              │
│                     (Dependency Registration)                   │
└─────────────────────────────────────────────────────────────────┘

// Database
builder.Services.AddDbContext<MecanicaContext>(...)
	 │
	 │ Lifetime: Scoped (per request)
	 │
	 ▼
┌──────────────────┐
│ MecanicaContext  │ ← Injected into Repositories & UnitOfWork
└──────────────────┘

// Repositories (Data Access)
builder.Services.AddScoped<IPresupuestoRepository, PresupuestoRepository>
builder.Services.AddScoped<IVehiculoRepository, VehiculoRepository>
builder.Services.AddScoped<ITrabajoRepository, TrabajoRepository>
	 │
	 │ Lifetime: Scoped (per request)
	 │ Dependencies: MecanicaContext
	 │
	 ▼
┌──────────────────────┐
│ Repositories         │ ← Injected into UnitOfWork
└──────────────────────┘

// Unit of Work (Transaction Management)
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>
	 │
	 │ Lifetime: Scoped (per request)
	 │ Dependencies: MecanicaContext, All Repositories
	 │
	 ▼
┌──────────────────┐
│   UnitOfWork     │ ← Injected into Services
└──────────────────┘

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program))
	 │
	 │ Lifetime: Singleton (shared)
	 │
	 ▼
┌──────────────────┐
│    IMapper       │ ← Injected into Services
└──────────────────┘

// Logging
builder.Services.AddLogging()
	 │
	 │ Lifetime: Singleton (shared)
	 │
	 ▼
┌──────────────────┐
│  ILogger<T>      │ ← Injected everywhere
└──────────────────┘

// Services (Business Logic)
builder.Services.AddScoped<IPresupuestoService, PresupuestoService>
	 │
	 │ Lifetime: Scoped (per request)
	 │ Dependencies: IUnitOfWork, IMapper, ILogger<PresupuestoService>
	 │
	 ▼
┌──────────────────────┐
│ PresupuestoService   │ ← Injected into Controllers
└──────────────────────┘

// Controllers
builder.Services.AddControllers()
	 │
	 │ Lifetime: Transient (per call)
	 │ Dependencies: IPresupuestoService, ILogger<PresupuestoController>
	 │
	 ▼
┌─────────────────────────┐
│ PresupuestoController   │ ← Handles HTTP requests
└─────────────────────────┘
```

### Runtime (Per Request)
```
HTTP Request arrives
	 │
	 ▼
┌─────────────────────────────────────┐
│  ASP.NET Core DI Container          │
│  (Creates instances for this        │
│   request - Scoped lifetime)        │
└─────────────────────────────────────┘
	 │
	 ├─► Creates MecanicaContext instance
	 │        │
	 │        ▼
	 ├─► Creates Repository instances
	 │   (inject MecanicaContext)
	 │        │
	 │        ▼
	 ├─► Creates UnitOfWork instance
	 │   (inject MecanicaContext + Repositories)
	 │        │
	 │        ▼
	 ├─► Creates Service instance
	 │   (inject UnitOfWork + IMapper + ILogger)
	 │        │
	 │        ▼
	 └─► Creates Controller instance
		 (inject Service + ILogger)
			  │
			  ▼
		 Controller handles request
			  │
			  ▼
		 Response sent
			  │
			  ▼
┌─────────────────────────────────────┐
│  DI Container disposes instances    │
│  • MecanicaContext.Dispose()        │
│  • UnitOfWork.Dispose()             │
└─────────────────────────────────────┘
```

---

## SOLID Principles Applied

### Single Responsibility Principle (SRP)
```
┌─────────────────────┐
│ PresupuestoController│  ← Only handles HTTP requests/responses
└─────────────────────┘

┌─────────────────────┐
│ PresupuestoService  │  ← Only contains business logic
└─────────────────────┘

┌─────────────────────┐
│ UnitOfWork          │  ← Only manages transactions
└─────────────────────┘

┌─────────────────────┐
│PresupuestoRepository│  ← Only handles data access
└─────────────────────┘

Each class has ONE reason to change!
```

### Open/Closed Principle (OCP)
```
┌──────────────────────────────────────┐
│  IPresupuestoService (interface)     │  ← Open for extension
│  - GetByIdAsync()                    │
│  - CreateAsync()                     │
│  - UpdateAsync()                     │
└──────────────────────────────────────┘
		 ▲
		 │ Implements
		 │
┌──────────────────────────────────────┐
│  PresupuestoService                  │  ← Closed for modification
│  + Business logic implementation     │
└──────────────────────────────────────┘

Can create new implementations without modifying existing code:
- PresupuestoServiceWithCaching
- PresupuestoServiceWithAudit
- MockPresupuestoService (for testing)
```

### Liskov Substitution Principle (LSP)
```
┌──────────────────────────────────────┐
│  IRepositoryBase<T>                  │
│  - FindAll()                         │
│  - FindByCondition()                 │
│  - Create()                          │
└──────────────────────────────────────┘
		 ▲
		 │ Implements
		 ├─────────────────┬─────────────────┐
		 │                 │                 │
┌────────────────┐  ┌────────────────┐  ┌────────────────┐
│PresupuestoRepo │  │ VehiculoRepo   │  │ TrabajoRepo    │
└────────────────┘  └────────────────┘  └────────────────┘

Any IRepositoryBase<T> can be substituted without breaking code!
```

### Interface Segregation Principle (ISP)
```
❌ WRONG (Fat Interface):
┌──────────────────────────────────────┐
│  IRepository                         │
│  - GetPresupuestos()                 │
│  - GetVehiculos()                    │
│  - CreatePresupuesto()               │
│  - CreateVehiculo()                  │
│  ... (100 methods!)                  │
└──────────────────────────────────────┘

✅ CORRECT (Small, focused interfaces):
┌──────────────────┐  ┌──────────────────┐  ┌──────────────────┐
│IPresupuestoService│  │IVehiculoService  │  │ITrabajoService   │
│- GetByIdAsync()  │  │- GetByIdAsync()  │  │- GetByIdAsync()  │
│- CreateAsync()   │  │- CreateAsync()   │  │- CreateAsync()   │
└──────────────────┘  └──────────────────┘  └──────────────────┘

Clients only depend on methods they need!
```

### Dependency Inversion Principle (DIP)
```
❌ WRONG (High-level depends on low-level):
┌─────────────────────┐
│ PresupuestoController│
│                     │
│ new PresupuestoRepo() ← Concrete class!
└─────────────────────┘

✅ CORRECT (Both depend on abstractions):
┌─────────────────────┐
│ PresupuestoController│
│                     │
│ IPresupuestoService  ← Interface!
└─────────────────────┘
		 │
		 │ Injected by DI Container
		 │
		 ▼
┌─────────────────────┐
│ PresupuestoService  │
│                     │
│ IUnitOfWork         ← Interface!
└─────────────────────┘

Both high and low-level modules depend on abstractions!
```

---

## Testing Pyramid

```
					▲
				   ╱ ╲
				  ╱   ╲
				 ╱     ╲
				╱  E2E  ╲          ← Few (Slow, Expensive)
			   ╱─────────╲           • Test full system
			  ╱           ╲          • Browser automation
			 ╱─────────────╲         • Real database
			╱               ╲
		   ╱  Integration   ╲      ← Some (Medium Speed)
		  ╱─────────────────╲       • Test API endpoints
		 ╱                   ╲      • Test Controller → Service → Repo
		╱       Unit          ╲    ← Many (Fast, Cheap)
	   ╱─────────────────────  ╲    • Test Service methods
	  ╱                         ╲   • Test business logic
	 ╱___________________________╲  • Mock dependencies
```

### Unit Test Example (Service Layer)
```csharp
[Fact]
public async Task CreateAsync_WithInvalidVehiculoId_ReturnsFailure()
{
	// Arrange
	var mockUnitOfWork = new Mock<IUnitOfWork>();
	var mockMapper = new Mock<IMapper>();
	var mockLogger = new Mock<ILogger<PresupuestoService>>();

	var service = new PresupuestoService(
		mockUnitOfWork.Object,
		mockMapper.Object,
		mockLogger.Object
	);

	// Act
	var result = await service.CreateAsync(Guid.Empty);

	// Assert
	Assert.True(result.IsFailure);
	Assert.Contains("no es válido", result.Error);
	mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);
}
```

### Integration Test Example (Controller)
```csharp
[Fact]
public async Task Post_ValidPresupuesto_Returns201Created()
{
	// Arrange
	var factory = new WebApplicationFactory<Program>();
	var client = factory.CreateClient();
	var vehiculoId = Guid.NewGuid();

	// Act
	var response = await client.PostAsJsonAsync(
		"/api/presupuestos",
		vehiculoId
	);

	// Assert
	Assert.Equal(HttpStatusCode.Created, response.StatusCode);
	Assert.NotNull(response.Headers.Location);
}
```

---

## Comparison: Monolithic vs Layered

### ❌ Monolithic (Before)
```
┌────────────────────────────────────────┐
│         Presupuesto Everything         │
│                                        │
│  • HTTP parsing                        │
│  • Validation                          │
│  • Business logic                      │
│  • Database access                     │
│  • DTO mapping                         │
│  • Error handling                      │
│  • Logging                             │
│  • Transaction management              │
│                                        │
│  ❌ Hard to test                       │
│  ❌ Hard to maintain                   │
│  ❌ Hard to understand                 │
│  ❌ Tight coupling                     │
└────────────────────────────────────────┘
```

### ✅ Layered (After)
```
┌────────────────────────────────────────┐
│     PresupuestoController              │
│  • HTTP parsing                        │
│  • Status code mapping                 │
└───────────────┬────────────────────────┘
				│
┌───────────────▼────────────────────────┐
│     PresupuestoService                 │
│  • Validation                          │
│  • Business logic                      │
│  • Orchestration                       │
└───────────────┬────────────────────────┘
				│
┌───────────────▼────────────────────────┐
│     UnitOfWork                         │
│  • Transaction management              │
└───────────────┬────────────────────────┘
				│
┌───────────────▼────────────────────────┐
│     PresupuestoRepository              │
│  • Database access                     │
└────────────────────────────────────────┘

✅ Easy to test (mock each layer)
✅ Easy to maintain (find code quickly)
✅ Easy to understand (clear responsibilities)
✅ Loose coupling (depend on interfaces)
```

---

**This architecture is production-ready and demonstrates professional software engineering! 🚀**
