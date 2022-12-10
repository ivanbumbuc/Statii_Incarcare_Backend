using Microsoft.EntityFrameworkCore;
using Statii_Incarcare_Proiect_Tehnologii_Web.Context;

var builder = WebApplication.CreateBuilder(args);

// conection to database
string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<StatiiIncarcareContext>(options =>
{
    options.UseSqlServer(connection);
});

builder.Services.AddCors(o => o.AddPolicy("CORSPolicy", builder =>
{
    builder.WithOrigins("http://localhost:3000")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials().Build();
}));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CORSPolicy");
app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();
