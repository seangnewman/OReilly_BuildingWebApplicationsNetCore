using Microsoft.EntityFrameworkCore;
using SpyStore.DAL.EF;
using System;
using Xunit;
using SpyStore.Models.Entities;
using System.Linq;
using System.Threading.Tasks;

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
          
            CleanDatabase();
            _db.Dispose();
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

        //Test Adding a Category Record
        [Fact]
        public void ShouldAddCategoryWithDbSet()
        {
            var category = new Category { CategoryName = "Foo" };
            _db.Categories.Add(category);
            
            Assert.Equal(EntityState.Added, _db.Entry(category).State);
            Assert.True(category.Id == 0);
            Assert.Null(category.TimeStamp);

            _db.SaveChanges();

            Assert.Equal(EntityState.Unchanged, _db.Entry(category).State);
            Assert.Equal(0, category.Id);
            Assert.NotNull(category.TimeStamp);
            Assert.Equal(1, _db.Categories.Count());

        }

        [Fact]
        public void ShouldAddCategoryWithContext()
        {
            var category = new Category { CategoryName = "Foo" };
            _db.Add(category);

            Assert.Equal(EntityState.Added, _db.Entry(category).State);
            Assert.True(category.Id == 0);
            Assert.Null(category.TimeStamp);

            _db.SaveChanges();

            Assert.Equal(EntityState.Unchanged, _db.Entry(category).State);
            Assert.Equal(-1, category.Id);
            Assert.NotNull(category.TimeStamp);
            Assert.Equal(1, _db.Categories.Count());

        }

        // Test Retrieving All Category Records
        [Fact]
        public void ShouldGetAllCategoriesOrderedByName()
        {
            _db.Categories.Add(new Category { CategoryName = "Foo" });
            _db.Categories.Add(new Category { CategoryName = "Bar"});

            _db.SaveChanges();

            var categories = _db.Categories.OrderBy(c => c.CategoryName).ToList();

            Assert.Equal(2, _db.Categories.Count());
            Assert.Equal("Bar", categories[0].CategoryName);
            Assert.Equal("Foo", categories[1].CategoryName);
        }

        // Test Updating a Category Record
        [Fact]
        public async Task  ShouldUpdtateACategoryAsync()
        {
            var category = new Category { CategoryName = "Foo" };
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();

            category.CategoryName = "Bar";
            _db.Categories.Update(category);
            Assert.Equal(EntityState.Modified, _db.Entry(category).State);
            await _db.SaveChangesAsync();
            Assert.Equal(EntityState.Unchanged, _db.Entry(category).State);

            using (StoreContext context = new StoreContext())
            {
                Assert.Equal("Bar", context.Categories.First().CategoryName);
            }
        }

        [Fact]
        public void ShouldNotUpdateANonAttachedCategory()
        {
            var category = new Category { CategoryName = "Foo" };
            _db.Categories.Add(category);
            category.CategoryName = "Bar";
            Assert.Throws<InvalidOperationException>(() => _db.Categories.Update(category));
        }

        // Test Deleting a Category Record Using Remove
        [Fact]
        public async Task  ShouldDeleteCategoryAsync()
        {
            var category = new Category { CategoryName = "Foo" };
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();

            Assert.Equal(1, _db.Categories.Count());
            _db.Categories.Remove(category);

            Assert.Equal(EntityState.Deleted, _db.Entry(category).State);
            await _db.SaveChangesAsync();

            Assert.Equal(EntityState.Detached, _db.Entry(category).State);
            Assert.Equal(0, _db.Categories.Count());
        }

        // Test Deleting a Record Using EntityState
        [Fact]
        public async Task ShouldDeleteACategoryWthTimestampDataAsync()
        {
            var category = new Category { CategoryName = "Foo"};
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();

            var context = new StoreContext();
            var catToDelete = new Category { Id = category.Id, TimeStamp = category.TimeStamp  };
            context.Entry(catToDelete).State = EntityState.Deleted;

            var affected = await context.SaveChangesAsync();
            Assert.Equal(1, affected);
        }

        // Testing Concurrency Checking
        [Fact]
        public void  ShouldNotDeleteACategoryWithoutTimestampData()
        {
            var category = new Category { CategoryName = "Foo" };
            _db.Categories.Add(category);
            _db.SaveChanges();
            var context = new StoreContext();
            var catToDelete = new Category { Id = category.Id };
            
            var ex2 =  Assert.Throws<InvalidOperationException>(  () => context.Categories.Remove(catToDelete));
            Assert.NotEmpty(ex2.Message);
            
            //var ex = Assert.Throws<DbUpdateConcurrencyException>(() => context.SaveChanges());
            //Assert.Equal(1, ex.Entries.Count);
            //Assert.Equal(category.Id, ((Category)ex.Entries[0].Entity).Id);
        }

    }
}
