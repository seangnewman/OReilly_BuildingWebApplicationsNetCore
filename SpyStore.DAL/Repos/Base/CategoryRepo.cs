using Microsoft.EntityFrameworkCore;
using SpyStore.DAL.EF;
using SpyStore.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpyStore.DAL.Repos.Base
{
    class CategoryRepo:RepoBase<Category>
    {
        public CategoryRepo(DbContextOptions<StoreContext> options): base(options)
        {

        }

        public CategoryRepo()
        {

        }

        public override IEnumerable<Category> GetAll() => Table.OrderBy(x => x.CategoryName);
        public override IEnumerable<Category> GetRange(int skip, int take) => GetRange(Table.OrderBy(x => x.CategoryName), skip, take);
    }
}
