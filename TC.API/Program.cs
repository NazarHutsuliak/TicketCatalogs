using TC.Application.Interfaces;
using TC.Application.Services;
using TC.Persistence.Data;
using TC.Persistence.Command;
using TC.Persistence.Query;
using TC.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

var mongoConfig = builder.Configuration.GetSection("MongoDB");
var connectionString = mongoConfig["ConnectionString"];
var databaseName = mongoConfig["DatabaseName"];

// Add services to the container.
builder.Services.AddScoped<ITicketsCatalogService, TicketsCatalogService>();
builder.Services.AddSingleton<MongoDbContext>(sp => new MongoDbContext(connectionString, databaseName));

builder.Services.AddScoped<IAddUserToCatalogCommand, AddUserToCatalogCommand>();
builder.Services.AddScoped<ICreateTicketsCatalogCommand, CreateTicketsCatalogCommand>();
builder.Services.AddScoped<IDeleteTicketsCatalogCommand, DeleteTicketsCatalogCommand>();
builder.Services.AddScoped<IDeleteUserFromCatalogCommand, DeleteUserFromCatalogCommand>();
builder.Services.AddScoped<IUpdateTicketsCatalogCommand, UpdateTicketsCatalogCommand>();
builder.Services.AddScoped<IGetAllTicketsCatalogsQuery, GetAllTicketsCatalogsQuery>();
builder.Services.AddScoped<IGetTicketsCatalogByIdQuery, GetTicketsCatalogByIdQuery>();
builder.Services.AddScoped<IGetUsersByCatalogIdQuery, GetUsersByCatalogIdQuery>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
