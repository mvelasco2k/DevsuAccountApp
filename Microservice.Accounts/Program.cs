using Microsoft.EntityFrameworkCore;
using Microservice.Accounts.Data;
using Microservice.Accounts.Repositories;
using Microservice.Clients.Repositories;
using ClientsAppDbContext = Microservice.Clients.Data.AppDbContext;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register clients DbContext and repository so reports can access client info
builder.Services.AddDbContext<ClientsAppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();
app.Run();
