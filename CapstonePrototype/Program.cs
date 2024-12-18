using System.Text;
using CapstonePrototype.Data;
using CapstonePrototype.services.ImageService;
using CapstonePrototype.Services.AuthService;
using CapstonePrototype.Services.CategoryService;
using CapstonePrototype.Services.CompanyService;
using CapstonePrototype.Services.JwtService;
using CapstonePrototype.Services.ProductService;
using CapstonePrototype.Services.PurchasedProductService;
using CapstonePrototype.Services.RfqBidService;
using CapstonePrototype.Services.RfqService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SealBackend.Dto;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
  options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(9, 1, 0)));
});
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
  options.AddPolicy(name: MyAllowSpecificOrigins,
                    policy =>
                    {
                      policy.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                    });
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers()
.ConfigureApiBehaviorOptions(o =>
{
  o.InvalidModelStateResponseFactory = context =>
  {
    var dto = new ServiceResponse<bool>
    {
      Success = false,
      Message = context.ModelState.FirstOrDefault().Value?.Errors.FirstOrDefault()?.ErrorMessage ?? "Ada kesalahan pada input yang anda masukkan",
      Data = false
    };
    return new BadRequestObjectResult(dto);
  };
});
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IPurchasedProductService, PurchasedProductService>();
builder.Services.AddScoped<IRfqService, RfqService>();
builder.Services.AddScoped<ICategoryService,CategoryService>();
builder.Services.AddScoped<IRfqBidService, RfqBidService>();
builder.Services.AddScoped<ICompanyService,CompanyService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(options =>
{
  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
  options.SaveToken = true;
  options.RequireHttpsMetadata = false;
  options.TokenValidationParameters = new TokenValidationParameters
  {
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
    ValidAudience = builder.Configuration["JWT:ValidAudience"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
  };
  options.Events = new JwtBearerEvents
  {
    OnMessageReceived = context =>
    {
      var accessToken = context.Request.Query["access_token"];

      // If the request is for our hub...
      var path = context.HttpContext.Request.Path;
      if (!string.IsNullOrEmpty(accessToken) &&
                  (path.StartsWithSegments("/ws/notification")))
      {
        // Read the token out of the query string
        context.Token = accessToken;
      }
      return Task.CompletedTask;
    }
  };
})
.AddCookie();
var app = builder.Build();
app.UseCors(MyAllowSpecificOrigins);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
