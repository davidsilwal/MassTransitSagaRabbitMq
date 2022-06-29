using Bogus;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AppDbContext : DbContext
{
    public ILogger<AppDbContext> Logger { get; }

    public AppDbContext(ILogger<AppDbContext> logger)
    {
        Logger = logger;
    }

    public DbSet<Customer.API.Data.Customer> Customers { get; set; } = default!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseSqlite("Data Source=database.db")
            .LogTo(m => Logger.LogInformation(m),
                (id, _) => id.Name?.Contains("CommandExecuted") == true);

    public static void Initialize(WebApplication app, int count = 1_000)
    {
        using var scope = app.Services.CreateScope();
        var database = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppDbContext>>();

        // migrate the database if we haven't already
        database.Database.Migrate();

        if (database.Customers.Any())
        {
            logger.LogInformation("Database already initialized");
            return;
        }

        var generator = new Faker<Customer.API.Data.Customer>()
            //.RuleFor(m => m.Id, (f, _) => f.IndexFaker)
            .RuleFor(m => m.FirstName, f => f.Name.FirstName())
            .RuleFor(m => m.LastName, f => f.Name.LastName())
            .RuleFor(m => m.Email, f => f.Person.Email)
            .RuleFor(m => m.City, f => f.Address.Country());

        var chunks = Enumerable.Range(1, count).Chunk(100).Select((v, i) => (Index: i, Value: v.Length)).ToList();
        logger.LogInformation("{ChunkCount} of Chunks To Initialize", chunks.Count);
        foreach (var chunk in chunks)
        {
            logger.LogInformation("#{Index}: Generating {Chunk} rows of People", chunk.Index, chunk.Value);
            var records = generator.Generate(chunk.Value);
            database.Customers.AddRange(records);
            database.SaveChanges();
            database.ChangeTracker.Clear();
        }
    }
}