using Microsoft.EntityFrameworkCore;
using SpyStore.DAL.EF;
using SpyStore.DAL.Repos.Base;
using SpyStore.DAL.Repos.Interfaces;
using SpyStore.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SpyStore.DAL.Repos
{
    public class CustomerRepo : RepoBase<Customer>, ICustomerRepo
    {
        public CustomerRepo(DbContextOptions<StoreContext> options) : base(options)
        {

        }

        public CustomerRepo() : base()
        {

        }

        public override IEnumerable<Customer> GetAll() => Table.OrderBy(c => c.FullName);
        public override IEnumerable<Customer> GetRange(int skip, int take) => GetRange(Table.OrderBy(c => c.FullName), skip, take);

    }
}
