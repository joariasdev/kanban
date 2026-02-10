using Kanban.API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

// Connection string is stored on secret file instead of appsetting.json
string? kanbanDbConnection = builder.Configuration.GetConnectionString("KanbanDbConnection");

builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(kanbanDbConnection));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();