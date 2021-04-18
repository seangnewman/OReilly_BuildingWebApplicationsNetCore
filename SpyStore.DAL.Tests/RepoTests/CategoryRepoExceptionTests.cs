using System;
using System.Collections.ObjectModel;
using SpyStore.Models.Entities;
using SpyStore.DAL.Repos;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SpyStore.DAL.EF;
//using SpyStore.DAL.EF.Migrations;
using SpyStore.DAL.Repos.Base;

namespace SpyStore.DAL.Tests.RepoTests
{
    [Collection("SpyStore.DAL")]
    public class CategoryRepoExceptionTests : IDisposable
    {
        private readonly CategoryRepo _repo;

        public CategoryRepoExceptionTests()
        {
            _repo = new CategoryRepo();
            CleanDatabase();
        }

        public void Dispose()
        {
            CleanDatabase();
            _repo.Dispose();
        }

        private void CleanDatabase()
        {
            _repo.Context.Database.ExecuteSqlRaw("Delete from Store.Categories");
            _repo.Context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT (\"Store.Categories\", RESEED, -1);");
        }

        [Fact]
        public void ShouldNotDeleteACategoryFromSameContextWithConcurrencyIssue()
        {
            var category = new Category { CategoryName = "Foo" };
            _repo.Add(category);
            Assert.Equal(1, _repo.Count);
            var ex = Assert.Throws<Exception>(() => _repo.Delete(category.Id, null, false));
        }

        [Fact]
        public void ShouldNotDeleteOnConcurrencyIssue()
        {
            var category = new Category { CategoryName = "Foo" };
            _repo.Add(category);
            _repo.Context.Database.ExecuteSqlRaw("Update Store.Categories set CategoryName = 'Bar'");
            var ex = Assert.Throws<DbUpdateConcurrencyException>(() => _repo.Delete(category.Id, category.TimeStamp));
        }

        [Fact]
        public void ShouldThrowRetryExeptionWhenCantConnect()
        {
            var contextOptionsBuilder = new DbContextOptionsBuilder<StoreContext>();
            var connectionString =
                @"Server=(localdb)\mssqllocaldb;Database=SpyStore;user id=foo;password=bar;MultipleActiveResultSets=true;";
            //contextOptionsBuilder.UseSqlServer(connectionString, 
            //    o => o.EnableRetryOnFailure(2,new TimeSpan(0,0,0,0,100),new Collection<int>{ -2146232060 }));
            contextOptionsBuilder.UseSqlServer(connectionString,
                o => o.ExecutionStrategy(c => new MyConnectionStrategy(c, 5, new TimeSpan(0, 0, 0, 0, 30))));
            var repo = new CategoryRepo(contextOptionsBuilder.Options);
            var category = new Category { CategoryName = "Foo" };
            var ex = Assert.Throws<RetryLimitExceededException>(() => repo.Add(category));
        }

    }
}