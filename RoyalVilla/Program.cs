using Microsoft.EntityFrameworkCore;
using RoyalVilla.Data;
using RoyalVilla.Dto;
using Scalar.AspNetCore;
using RoyalVilla.Models;
using RoyalVilla.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;


var builder = WebApplication.CreateBuilder(args);
var key = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JwtSettings:Secret") ?? throw new InvalidOperationException("JWT Secret not configured"));


//Add authentication
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };

});

// Add services to the container
builder.Services.AddControllers();

//OpenApi
// builder.Services.AddOpenApi(options =>
// {
//     options.AddDocumentTransformer((document, context, cancellationToken) =>
//     {
//         document.Components ??= new();
//         document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>
//         {
//             ["Bearer"]= new OpenApiSecurityScheme
//             {
//                 Type = SecuritySchemeType.Http,
//                 Scheme = "bearer",
//                 BearerFormat ="JWT",
//                 Description = "Enter JWT Bearer Token"
//             }
//         };
//         document.Security = [
//             new OpenApiSecurityRequirement{
//                { new OpenApiSecuritySchemeReference("Bearer"), new List<string>()}
//             }
//         ];
//         return Task.CompletedTask;
//     });
// });

builder.Services.AddOpenApi("v1");
builder.Services.AddOpenApi("v2");

// Add OpenAPI with Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Version = "v1",
        Title = "Royal Villa API",
        Description = "API for managing Royal Villa properties"
    });
});

//Add CORS
builder.Services.AddCors();

// Add DbContext with MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));
//Add Automapper VillaDTO to Villa
builder.Services.AddAutoMapper(o =>
{
    o.CreateMap<Villa, VillaCreateDTO>().ReverseMap();
    o.CreateMap<Villa, VillaUpdateDTO>().ReverseMap();
    o.CreateMap<Villa, VillaDTO>().ReverseMap();
    o.CreateMap<VillaUpdateDTO, VillaDTO>().ReverseMap();
    o.CreateMap<User, UserDTO>().ReverseMap();
    o.CreateMap<VillaAmenities, VillaAmenitiesCreateDTO>().ReverseMap();
    o.CreateMap<VillaAmenities, VillaAmenitiesUpdateDTO>().ReverseMap();
    o.CreateMap<VillaAmenities, VillaAmenitiesDTO>()
    .ForMember(dest => dest.VillaName, opt => opt.MapFrom(src => src.Villa != null ? src.Villa.Name : null));

    o.CreateMap<VillaAmenitiesDTO, VillaAmenities>();
}
);

builder.Services.AddScoped<IAuthService, AuthService>();
var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
   app.MapOpenApi();
  
    app.MapScalarApiReference(options =>
 {
     options.Title = "Demo - Royal Villa API";
     options.AddDocument("v1", "Demo API v1", "/openapi/v1.json", isDefault: true)
            .AddDocument("v2", "Demo API v2", "/openapi/v2.json");
 });
}

app.UseCors(o => o.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().WithExposedHeaders("*"));

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

await SeedDataAsync(app);

app.Run();

static async Task SeedDataAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    await context.Database.MigrateAsync();
}