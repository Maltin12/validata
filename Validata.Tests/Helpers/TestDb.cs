using Microsoft.EntityFrameworkCore;
using Validata.Infrastructure.Persistence;

namespace Validata.Tests.Helpers
{
    public static class TestDb
    {
        public static AppDbContext Create()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // unique DB for each test
                .Options;

            var db = new AppDbContext(options);
            db.Database.EnsureCreated();
            return db;
        }
    }
}
