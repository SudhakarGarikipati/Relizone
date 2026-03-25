using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("ocelot-dev.json");
}
else
{
    builder.Configuration.AddJsonFile("ocelot.json");
}
builder.Services.AddOcelot();
var endPointAuthKey = builder.Configuration["Keys:EndPointAuthKey"];
builder.Services.AddAuthentication().AddJwtBearer(endPointAuthKey, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateActor = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecurityKey"]))
    };
});
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.UseOcelot().Wait();
app.Run();
