# 🚀 Quick Implementation Guide for Other Controllers

This guide shows how to refactor the remaining controllers using the same patterns.

---

## 📝 Step-by-Step Process

### **1. Create Service Interface**

```csharp
// Services/IVehiculoService.cs
using BackendMecanicaElEnano.Common;
using BackendMecanicaElEnano.Dto;

namespace BackendMecanicaElEnano.Services
{
	public interface IVehiculoService
	{
		Task<Result<VehiculoDto>> GetByIdAsync(Guid id);
		Task<Result<IList<VehiculoDto>>> GetAllAsync();
		Task<Result<VehiculoDto>> CreateAsync(CreateVehiculoDto dto);
		Task<Result<VehiculoDto>> UpdateAsync(Guid id, CreateVehiculoDto dto);
		Task<Result> DeleteAsync(Guid id);
	}
}
```

### **2. Implement Service with Business Logic**

```csharp
// Services/VehiculoService.cs
public class VehiculoService : IVehiculoService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	private readonly ILogger<VehiculoService> _logger;

	public VehiculoService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<VehiculoService> logger)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_logger = logger;
	}

	public async Task<Result<VehiculoDto>> CreateAsync(CreateVehiculoDto dto)
	{
		try
		{
			_logger.LogInformation("Creating vehicle with plate: {Patente}", dto.Patente);

			// Validate business rules
			if (string.IsNullOrWhiteSpace(dto.Patente))
			{
				return Result.Failure<VehiculoDto>("La patente es requerida");
			}

			// Check if patente already exists
			var exists = await _unitOfWork.Vehiculos
				.FindByCondition(v => v.Patente == dto.Patente)
				.AnyAsync();

			if (exists)
			{
				return Result.Failure<VehiculoDto>("Ya existe un vehículo con esa patente");
			}

			var vehiculo = _mapper.Map<Vehiculo>(dto);
			vehiculo.VehiculoId = Guid.NewGuid();

			_unitOfWork.Vehiculos.Create(vehiculo);
			await _unitOfWork.CommitAsync();

			var vehiculoDto = _mapper.Map<VehiculoDto>(vehiculo);
			_logger.LogInformation("Successfully created vehicle with ID: {Id}", vehiculo.VehiculoId);

			return Result.Success(vehiculoDto);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error creating vehicle");
			await _unitOfWork.RollbackAsync();
			return Result.Failure<VehiculoDto>("Error al crear el vehículo");
		}
	}

	// Implement other methods similarly...
}
```

### **3. Simplify Repository**

```csharp
// Repositories/VehiculoRepository.cs
public class VehiculoRepository : RepositoryBase<Vehiculo>, IVehiculoRepository
{
	public VehiculoRepository(MecanicaContext context) : base(context)
	{
	}

	// That's it! All CRUD inherited from RepositoryBase
}

// Repositories/IVehiculoRepository.cs
public interface IVehiculoRepository : IRepositoryBase<Vehiculo>
{
	// Add custom data access methods only if needed
}
```

### **4. Update Controller**

```csharp
// Controllers/VehiculoController.cs
[ApiController]
[Route("api/vehiculos")]
public class VehiculoController : ControllerBase
{
	private readonly IVehiculoService _vehiculoService;
	private readonly ILogger<VehiculoController> _logger;

	public VehiculoController(IVehiculoService vehiculoService, ILogger<VehiculoController> logger)
	{
		_vehiculoService = vehiculoService;
		_logger = logger;
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<VehiculoDto>> Create([FromBody] CreateVehiculoDto dto)
	{
		_logger.LogInformation("POST api/vehiculos - Creating vehicle");

		var result = await _vehiculoService.CreateAsync(dto);

		if (result.IsFailure)
		{
			_logger.LogWarning("Failed to create vehicle: {Error}", result.Error);
			return BadRequest(new { error = result.Error });
		}

		return CreatedAtAction(nameof(GetById), new { id = result.Value.VehiculoId }, result.Value);
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<VehiculoDto>> GetById(Guid id)
	{
		var result = await _vehiculoService.GetByIdAsync(id);

		if (result.IsFailure)
		{
			return NotFound(new { error = result.Error });
		}

		return Ok(result.Value);
	}

	// Implement other endpoints...
}
```

### **5. Register in Program.cs**

```csharp
// Program.cs
// Add to Service Layer section:
builder.Services.AddScoped<IVehiculoService, VehiculoService>();
builder.Services.AddScoped<ITrabajoService, TrabajoService>();
builder.Services.AddScoped<ITurnoService, TurnoService>();
builder.Services.AddScoped<IOrdenTrabajoService, OrdenTrabajoService>();
```

---

## ✅ Checklist for Each Entity

For each entity (Vehiculo, Trabajo, Turno, OrdenTrabajo):

- [ ] Create `I[Entity]Service` interface
- [ ] Implement `[Entity]Service` with business logic
- [ ] Add proper validation in service methods
- [ ] Add comprehensive logging
- [ ] Add try-catch with Result Pattern
- [ ] Simplify repository to thin data access layer
- [ ] Update interface to remove DTOs
- [ ] Update controller to use service
- [ ] Add proper HTTP status codes
- [ ] Add XML documentation comments
- [ ] Register service in Program.cs
- [ ] Test the endpoints in Swagger

---

## 🎯 Common Validation Patterns

### **GUID Validation**
```csharp
if (id == Guid.Empty)
{
	return Result.Failure<TDto>("El ID no es válido");
}
```

### **Entity Existence Check**
```csharp
var entity = await _unitOfWork.Entities
	.FindByCondition(e => e.Id == id)
	.FirstOrDefaultAsync();

if (entity == null)
{
	return Result.Failure<TDto>($"No se encontró el recurso con ID {id}");
}
```

### **Foreign Key Validation**
```csharp
var relatedExists = await _unitOfWork.RelatedEntities
	.FindByCondition(r => r.Id == foreignKeyId)
	.AnyAsync();

if (!relatedExists)
{
	return Result.Failure<TDto>("El recurso relacionado no existe");
}
```

### **Date Range Validation**
```csharp
if (dto.FechaInicio > dto.FechaFin)
{
	return Result.Failure<TDto>("La fecha de inicio no puede ser posterior a la fecha de fin");
}
```

### **Duplicate Check**
```csharp
var duplicate = await _unitOfWork.Entities
	.FindByCondition(e => e.UniqueField == dto.UniqueField && e.Id != id)
	.AnyAsync();

if (duplicate)
{
	return Result.Failure<TDto>("Ya existe un registro con ese valor");
}
```

---

## 📊 Logging Patterns

### **Operation Start**
```csharp
_logger.LogInformation("Creating {Entity} with parameters: {Params}", 
	nameof(Entity), JsonSerializer.Serialize(dto));
```

### **Validation Failure**
```csharp
_logger.LogWarning("Validation failed for {Entity}: {Reason}", 
	nameof(Entity), validationMessage);
```

### **Success**
```csharp
_logger.LogInformation("Successfully created {Entity} with ID: {Id}", 
	nameof(Entity), entity.Id);
```

### **Error**
```csharp
_logger.LogError(ex, "Error occurred while processing {Entity} with ID: {Id}", 
	nameof(Entity), id);
```

---

## 🔄 Transaction Management Pattern

```csharp
try
{
	// Start work
	_unitOfWork.Entities.Create(entity);

	// Related operations
	_unitOfWork.RelatedEntities.Update(relatedEntity);

	// Commit all changes together
	await _unitOfWork.CommitAsync();

	return Result.Success(dto);
}
catch (Exception ex)
{
	_logger.LogError(ex, "Transaction failed");
	await _unitOfWork.RollbackAsync();
	return Result.Failure<TDto>("Error en la operación");
}
```

---

## 🎨 Controller Response Patterns

### **Create (POST)**
```csharp
return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
```

### **Update (PUT)**
```csharp
if (result.IsFailure)
{
	if (result.Error.Contains("no se encontró"))
		return NotFound(new { error = result.Error });

	return BadRequest(new { error = result.Error });
}

return Ok(result.Value);
```

### **Delete (DELETE)**
```csharp
if (result.IsFailure)
{
	if (result.Error.Contains("no se encontró"))
		return NotFound(new { error = result.Error });

	return BadRequest(new { error = result.Error });
}

return NoContent();
```

### **Get (GET)**
```csharp
if (result.IsFailure)
{
	return NotFound(new { error = result.Error });
}

return Ok(result.Value);
```

---

## 💡 Pro Tips

1. **Keep repositories thin** - only data access, no business logic
2. **Services should not reference DbContext** - only through UnitOfWork
3. **Always log before and after operations**
4. **Use structured logging** with named parameters
5. **Validate early** - check inputs before hitting database
6. **Return proper HTTP codes** - 201 for create, 204 for delete, etc.
7. **Use async/await** consistently
8. **Don't catch Exception unless you're rethrowing or logging**
9. **Keep controllers thin** - they should just orchestrate service calls
10. **Write XML comments** - they appear in Swagger

---

## 🧪 Testing Strategy

### **Unit Testing Services**
```csharp
[Fact]
public async Task CreateAsync_WithValidData_ReturnsSuccess()
{
	// Arrange
	var mockUnitOfWork = new Mock<IUnitOfWork>();
	var mockMapper = new Mock<IMapper>();
	var mockLogger = new Mock<ILogger<VehiculoService>>();

	var service = new VehiculoService(
		mockUnitOfWork.Object, 
		mockMapper.Object, 
		mockLogger.Object);

	// Act
	var result = await service.CreateAsync(validDto);

	// Assert
	Assert.True(result.IsSuccess);
	mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
}
```

### **Integration Testing Controllers**
```csharp
[Fact]
public async Task Post_ValidVehiculo_Returns201Created()
{
	// Arrange
	var client = _factory.CreateClient();
	var dto = new CreateVehiculoDto { Patente = "ABC123" };

	// Act
	var response = await client.PostAsJsonAsync("/api/vehiculos", dto);

	// Assert
	Assert.Equal(HttpStatusCode.Created, response.StatusCode);
}
```

---

## 📚 Additional Resources

- **Result Pattern**: [Vladimir Khorikov - Railway Oriented Programming](https://enterprisecraftsmanship.com/posts/functional-c-handling-failures-input-errors/)
- **Unit of Work**: [Martin Fowler - Patterns of Enterprise Application Architecture](https://martinfowler.com/eaaCatalog/unitOfWork.html)
- **Clean Architecture**: [Robert C. Martin - Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- **ASP.NET Core Best Practices**: [Microsoft Docs](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/best-practices)

---

**Happy Refactoring! 🎉**
