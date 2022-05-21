using Boards.Dto;
using Boards.Entities;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

services.AddDbContext<MyBoardContext>(
    option => option
        //.UseLazyLoadingProxies()
        .UseSqlServer(configuration.GetConnectionString("MyBoards"))
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetService<MyBoardContext>();

var pendingMigrations = dbContext.Database.GetPendingMigrations();
if (pendingMigrations.Any())
{
    dbContext.Database.Migrate();
}

var users = dbContext.Users;
if (!users.Any())
{
    var user1 = new User()
    {
        Email = "user1@test.com",
        FullName = "User One",
        Address = new Address()
        {
            City = "Warszawa",
            Street = "Szeroka"
        }
    };

    var user2 = new User()
    {
        Email = "user2@test.com",
        FullName = "User Two",
        Address = new Address()
        {
            City = "Krakow",
            Street = "Długa"
        }
    };

    dbContext.Users.AddRange(user1, user2);
    dbContext.SaveChanges();
}

app.MapGet("data", async (MyBoardContext db) =>
{
    var withAdress = true;

    var user = db.Users.First(u => u.Id == Guid.Parse("EBFBD70D-AC83-4D08-CBC6-08DA10AB0E61"));

    if (withAdress)
    {
        var result = new { FullName = user.FullName, Address = $"{user.Address.Street} {user.Address.City}" };
        return result;
    }
    return new { FullName = user.FullName, Address = "-" };
});

app.MapGet("pagination", async (MyBoardContext db) =>
{
    //user input
    var filter = "a";
    var sortBy = "FullName";
    bool sortByDescending = false;
    int pageNumber = 1;
    int pageSize = 10;

    var query = db.Users
        .Where(u => filter == null
            || u.Email.ToLower()
                .Contains(filter.ToLower())
            || u.FullName
                .ToLower()
                .Contains(filter.ToLower()));

    var totalCount = query.Count();

    if (sortBy != null)
    {
        //Expression<Func<User, object>> sortByExpression = user => user.Email;
        var columnsSelector = new Dictionary<string, Expression<Func<User, object>>>
        {
            {nameof(User.Email), user => user.Email },
            {nameof(User.FullName), user => user.FullName },
        };

        var sortByExpression = columnsSelector[sortBy];

        query = sortByDescending
            ? query.OrderByDescending(sortByExpression)
            : query.OrderBy(sortByExpression);
    }

    var result = query.Skip(pageSize * (pageNumber - 1))
        .Take(pageSize)
        .ToList();

    var pageResult = new PageResult<User>(result, totalCount, pageSize, pageNumber);

    return pageResult;
});

app.MapPost("update", async (MyBoardContext db) =>
{
    var epic = await db.Epics.FirstAsync(epic => epic.Id == 1);

    var rejectedState = await db.WorkItemStates.FirstAsync(a => a.Value == "Rejected");

    epic.State = rejectedState;

    await db.SaveChangesAsync();

    return epic;
});

app.MapPost("create", async (MyBoardContext db) =>
{
    var address = new Address()
    {
        Id = Guid.NewGuid(),
        City = "Kraków",
        Country = "Poland",
        Street = "Długa"
    };

    var user = new User()
    {
        Email = "user@test.com",
        FullName = "Test User",
        Address = address,
    };

    await db.Users.AddAsync(user);
    await db.SaveChangesAsync();
});

app.MapDelete("delete", async (MyBoardContext db) =>
{
    var workItem = new Epic()
    {
        Id = 2
    };

    var entry = db.Attach(workItem);
    entry.State = EntityState.Deleted;

    db.WorkItems.Remove(workItem);

    await db.SaveChangesAsync();
});

app.Run();
