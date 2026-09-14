using Microsoft.EntityFrameworkCore;
using Microservice.Accounts.Data;
using Microservice.Accounts.Repositories;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAccountRepository, AccountRepository>();

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();
app.Run();
