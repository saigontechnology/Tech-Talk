using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Threading;

var builder = WebApplication.CreateBuilder(args);

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
builder.Configuration.AddJsonFile("appsettings.json", false, true);
builder.Configuration.AddJsonFile($"appsettings.{environment}.json", false, true);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ProductCatalogDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationConnection"));
    options.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole())); // console sql script
});

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

#region First Endpoint

app.MapGet("/helloworld", () => "Get Hello World!").WithOpenApi();

app.MapPost("/helloworld", () => "Post Hello World!").WithOpenApi();

app.MapPut("/helloworld", () => "Put Hello World!").WithOpenApi();

app.MapDelete("/helloworld", () => "Delete Hello World!").WithOpenApi();

#endregion

#region Return Types

app.MapGet("/hello/{year}", (int year) =>
{
    if (year <= 0)
        return Results.BadRequest("'Year' is not valid");

    return Results.Ok($"Hello {year}!");
})
.Produces<string>(200)
.Produces(400)
.WithName("SayHello")
.WithTags("Hello")
.WithSummary("Say hello")
.WithDescription("Say hello new year")
.WithOpenApi();

Results<Ok<string>, BadRequest<string>> SayHello(int year)
{
    if (year <= 0)
        return TypedResults.BadRequest("'Year' is not valid");

    return TypedResults.Ok($"Hello {year}!");
}

#endregion

#region CRUD Endpoints

app.MapGet("/api/products", async (ProductCatalogDbContext _dbContext,
    IMapper _mapper,
    CancellationToken cancellationToken) =>
{
    var products = await _dbContext.Products.AsNoTracking()
        .Where(product => product.IsActive)
        .Select(product => _mapper.Map<ProductInformation>(product))
        .ToListAsync(cancellationToken);

    if (products.Count == 0)
        return Results.NotFound("Products are not found");

    return Results.Ok(products);
})
.Produces<List<ProductInformation>>(200)
.Produces(400)
.WithName("GetAllProducts")
.WithTags("Products")
.WithSummary("Get all products")
.WithDescription("Get all active products")
.WithOpenApi();

app.MapGet("/api/products/{id:Guid}", async (ProductCatalogDbContext _dbContext,
    IMapper _mapper,
    CancellationToken cancellationToken,
    [FromRoute] Guid id) =>
{
    var product = await _dbContext.Products.AsNoTracking()
        .Where(product => product.IsActive && product.Id == id)
        .Select(product => _mapper.Map<ProductInformation>(product))
        .SingleOrDefaultAsync(cancellationToken);

    if (product is null)
        return Results.NotFound("Product is not found");

    return Results.Ok(product);
})
.Produces<ProductInformation>(200)
.Produces(400)
.WithName("GetProduct")
.WithTags("Products")
.WithSummary("Get product by id")
.WithDescription("Get product information")
.WithOpenApi();

app.MapPost("/api/products", async (ProductCatalogDbContext _dbContext,
    IMapper _mapper,
    CancellationToken cancellationToken,
    [FromBody] ProductCreate productCreate) =>
{
    var isProductNameExisting = await _dbContext.Products.AnyAsync(p => p.Name.ToLower().Trim().Equals(productCreate.Name.ToLower().Trim()), cancellationToken);

    if (isProductNameExisting)
        return Results.BadRequest($"ProductName: {productCreate.Name} already existed");

    var product = _mapper.Map<Product>(productCreate);

    await _dbContext.Products.AddAsync(product, cancellationToken);
    await _dbContext.SaveChangesAsync(cancellationToken);

    return Results.CreatedAtRoute("GetProduct", new { Id = product.Id }, _mapper.Map<ProductCreateResponse>(product));
})
.Produces<ProductCreateResponse>(200)
.Produces(400)
.WithName("CreateProduct")
.WithTags("Products")
.WithSummary("Create product")
.WithDescription("Create a new product")
.Accepts<ProductCreate>("application/json")
.WithOpenApi();

app.MapPut("/api/products", async (ProductCatalogDbContext _dbContext,
    IMapper _mapper,
    CancellationToken cancellationToken,
    [FromBody] ProductUpdate productUpdate) =>
{
    var isProductExisting = await _dbContext.Products.AnyAsync(p => p.Id == productUpdate.Id, cancellationToken);

    if (!isProductExisting)
        return Results.BadRequest($"Product not exists");

    var product = _mapper.Map<Product>(productUpdate);

    _dbContext.Entry(product).Property(p => p.Name).IsModified = true;
    _dbContext.Entry(product).Property(p => p.Price).IsModified = true;
    _dbContext.Entry(product).Property(p => p.StockQuantity).IsModified = true;
    _dbContext.Entry(product).Property(p => p.Description).IsModified = true;
    _dbContext.Entry(product).Property(p => p.UpdatedBy).IsModified = true;
    _dbContext.Entry(product).Property(p => p.UpdatedDate).IsModified = true;

    await _dbContext.SaveChangesAsync(cancellationToken);

    return Results.Ok(_mapper.Map<ProductUpdateResponse>(product));
})
.Produces<ProductUpdateResponse>(200)
.Produces(400)
.WithName("UpdateProduct")
.WithTags("Products")
.WithSummary("Update product")
.WithDescription("Update a existing product")
.Accepts<ProductUpdate>("application/json")
.WithOpenApi();

app.MapDelete("/api/products/{id:Guid}", async (ProductCatalogDbContext _dbContext,
    IMapper _mapper,
    CancellationToken cancellationToken,
    [FromRoute] Guid id) =>
{
    var isProductExisting = await _dbContext.Products.AnyAsync(p => p.Id == id, cancellationToken);

    if (!isProductExisting)
        return Results.BadRequest($"Product not exists");

    var product = new Product()
    {
        Id = id,
        IsActive = false,
        UpdatedBy = "Admin",
        UpdatedDate = DateTime.UtcNow
    };

    _dbContext.Entry(product).Property(p => p.IsActive).IsModified = true;
    _dbContext.Entry(product).Property(p => p.UpdatedBy).IsModified = true;
    _dbContext.Entry(product).Property(p => p.UpdatedDate).IsModified = true;

    await _dbContext.SaveChangesAsync(cancellationToken);

    return Results.Ok();
})
.Produces(200)
.Produces(400)
.WithName("DeleteProduct")
.WithTags("Products")
.WithSummary("Delete product")
.WithDescription("Delete a existing product")
.WithOpenApi();

app.MapPost("/api/products/upload-image", async ([FromForm] IFormFile file,
    CancellationToken cancellationToken) =>
{
    if (file is null || file.Length == 0)
        return Results.BadRequest("File is not valid.");

    var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images", file.FileName);

    using var stream = System.IO.File.Create(filePath);

    await file.CopyToAsync(stream, cancellationToken);

    return Results.Ok("Image uploaded successfully.");
})
.Produces(200)
.Produces(400)
.WithName("UploadProductImage")
.WithTags("Products")
.WithSummary("Upload product image")
.WithDescription("Support upload product image");

app.MapGet("/api/products/search", async ([FromServices] ProductCatalogDbContext _dbContext,
    [FromServices] IMapper _mapper,
    CancellationToken cancellationToken,
    [AsParameters] ProductSearch request) =>
{
    var products = await _dbContext.Products.AsNoTracking()
        .Where(product => product.IsActive &&
            product.Name.ToLower().Trim().Contains(request.Keyword.ToLower().Trim()))
        .Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize)
        .Select(product => _mapper.Map<ProductInformation>(product))
        .ToListAsync(cancellationToken);

    if (products.Count == 0)
        return Results.NotFound("Products are not found");

    return Results.Ok(products);
})
//app.MapGet("/api/products/search", SearchProductsAsync)
.Produces<List<ProductInformation>>(200)
.Produces(400)
.WithName("SearchProducts")
.WithTags("Products")
.WithSummary("Search all products")
.WithDescription("Get all active products")
.WithOpenApi()
.AddEndpointFilter(async (context, next) =>
{
    if (!(int.TryParse(context.HttpContext.Request.Headers["PageNumber"], out var pageNumber) &&
        int.TryParse(context.HttpContext.Request.Headers["PageSize"], out var pageSize)
        && pageNumber > 0 && pageSize > 0))
        return Results.Problem("'PageNumber' and 'PageSize' are invalid.");

    return await next(context);
});

async Task<Results<Ok<List<ProductInformation>>, NotFound>> SearchProductsAsync([FromServices] ProductCatalogDbContext _dbContext,
    [FromServices] IMapper _mapper,
    CancellationToken cancellationToken,
    [AsParameters] ProductSearch request)
{
    var products = await _dbContext.Products.AsNoTracking()
            .Where(product => product.IsActive &&
                product.Name.ToLower().Trim().Contains(request.Keyword.ToLower().Trim()))
            .Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize)
            .Select(product => _mapper.Map<ProductInformation>(product))
            .ToListAsync(cancellationToken);

    if (products.Count == 0)
        return TypedResults.NotFound();

    return TypedResults.Ok(products);
}

app.MapGet("/", () =>
{
    app.Logger.LogInformation("             Endpoint");
    return "Test of multiple filters";
})
.AddEndpointFilter(async (efiContext, next) =>
{
    app.Logger.LogInformation("Before first filter");
    var result = await next(efiContext);
    app.Logger.LogInformation("After first filter");
    return result;
})
.AddEndpointFilter(async (efiContext, next) =>
{
    app.Logger.LogInformation(" Before 2nd filter");
    var result = await next(efiContext);
    app.Logger.LogInformation(" After 2nd filter");
    return result;
})
.AddEndpointFilter(async (efiContext, next) =>
{
    app.Logger.LogInformation("     Before 3rd filter");
    var result = await next(efiContext);
    app.Logger.LogInformation("     After 3rd filter");
    return result;
});

#endregion

app.Run();

class ProductSearch
{
    [FromQuery(Name = "Keyword")]
    public string Keyword { get; set; }

    [FromHeader(Name = "PageNumber")]
    public int PageNumber { get; set; }

    [FromHeader(Name = "PageSize")]
    public int PageSize { get; set; }
}


#region Models

public class ProductCreate
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string Description { get; set; }
}

public class ProductInformation
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string Description { get; set; }
}

public class ProductCreateResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string Description { get; set; }
}

public class ProductUpdate
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string Description { get; set; }
}

public class ProductUpdateResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string Description { get; set; }
}

#endregion

#region Entities

public class BaseEntity
{
    public Guid Id { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public bool IsActive { get; set; } = true;
}

public class Product : BaseEntity
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string Description { get; set; }
}

#endregion

#region DbContext

public class ProductCatalogDbContext : DbContext
{
    public ProductCatalogDbContext(DbContextOptions<ProductCatalogDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply Configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductCatalogDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseLazyLoadingProxies();
    }

    public DbSet<Product> Products { get; set; }
}

#endregion

#region Mappers

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Product, ProductInformation>()
            .ReverseMap();

        CreateMap<ProductCreate, Product>()
            .ForMember(dest => dest.CreatedBy, option => option.MapFrom(source => "Admin"))
            .ForMember(dest => dest.CreatedDate, option => option.Ignore())
            .ForMember(dest => dest.UpdatedBy, option => option.Ignore())
            .ForMember(dest => dest.UpdatedDate, option => option.Ignore())
            .ForMember(dest => dest.IsActive, option => option.Ignore());

        CreateMap<Product, ProductCreateResponse>()
            .ReverseMap();

        CreateMap<ProductUpdate, Product>()
            .ForMember(dest => dest.UpdatedBy, option => option.MapFrom(source => "Admin"))
            .ForMember(dest => dest.UpdatedDate, option => option.MapFrom(source => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, option => option.Ignore())
            .ForMember(dest => dest.CreatedBy, option => option.Ignore())
            .ForMember(dest => dest.CreatedDate, option => option.Ignore());

        CreateMap<Product, ProductUpdateResponse>()
            .ReverseMap();
    }
}

#endregion