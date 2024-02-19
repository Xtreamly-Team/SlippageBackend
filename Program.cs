using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using SlippageBackend.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Slippage Backend", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
           []
        }
    });
});

builder.Services
    .AddAuthentication()
    .AddBearerToken(); 
builder.Services.AddAuthorization();
builder.Services.AddSingleton<IMongoClient>(_ =>
{
    var settings = MongoClientSettings.FromUrl(new MongoUrl("mongodb://root:xtreamly@db.xtreamly.io:27017/"));
    settings.AllowInsecureTls = true;
    return  new MongoClient(settings);
});
builder.Services.AddHttpClient();
builder.Services.AddSingleton<ModelCommunicationService>();
builder.Services.AddSingleton<ModelInputAggregatorService>();
builder.Services.AddHttpClient();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
