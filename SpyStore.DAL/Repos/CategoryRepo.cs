using Microsoft.EntityFrameworkCore;
using SpyStore.DAL.EF;
using SpyStore.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using SpyStore.DAL.Repos.Interfaces;

namespace SpyStore.DAL.Repos.Base
{
    public class CategoryRepo : RepoBase<Category>, ICategoryRepo
    {
        public CategoryRepo(DbContextOptions<StoreContext> options) : base(options)
        {

        }

        public CategoryRepo()
        {

        }

        public override IEnumerable<Category> GetAll() => Table.OrderBy(x => x.CategoryName);
        public override IEnumerable<Category> GetRange(int skip, int take) => GetRange(Table.OrderBy(x => x.CategoryName), skip, take);
        public Category GetOneWithProducts(int? id) => Table.Include(x => x.Products).SingleOrDefault(x => x.Id == id);
        public IEnumerable<Category> GetAllWithProducts() => Table.Include(x => x.Products).ToList();
    }
}
