using InsuranceCorp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<InsCorpDbContext>();
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/", () => "hello");

app.MapGet("/person/{id:int}", (int id, InsCorpDbContext db) =>
{
    db.Persons.Find(id);
});

app.MapGet("/person/list", (int start, int take, InsCorpDbContext db) =>
{
    db.Persons.Skip(start).Take(take);
});

app.Run();
