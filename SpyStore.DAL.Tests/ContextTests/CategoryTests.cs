using Microsoft.EntityFrameworkCore;
using SpyStore.DAL.EF;
using System;
using Xunit;

namespace SpyStore.DAL.Tests.ContextTests
{
    [Collection("SpyStore.DAL")]                    // Runs all test serially to isolate from other tests
    public class CategoryTests : IDisposable
    {
        private readonly StoreContext _db;

        public CategoryTests()
        {
            _db = new StoreContext();
            CleanDatabase();
        }
        public void Dispose()
        {
            _db.Dispose();
            CleanDatabase();
        }

        private void CleanDatabase()
        {
            _db.Database.ExecuteSqlRaw("Delete from Store.Categories");
            _db.Database.ExecuteSqlRaw($"DBCC CHECKIDENT (\"Store.Categories\", RESEED, -1);");
        }

        [Fact]
        public void FirstTest()
        {
            Assert.True(true);
        }

    }
}
