using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using university;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient(_ => new Database(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddAuthentication()
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", options => { });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("BasicAuthentication", new AuthorizationPolicyBuilder("BasicAuthentication").RequireAuthenticatedUser().Build());
}); 

builder.Services.AddCors();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
} 

app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//setting the connection string for development environment

if (app.Environment.IsDevelopment()){
app.Use(async (contex, next)=>
{
    MyEnvironment.SetMySQLConnention();
    await next();
}
);
} 


app.Run();