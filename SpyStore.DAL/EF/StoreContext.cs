using Microsoft.EntityFrameworkCore;

namespace SpyStore.DAL.EF
{
    class StoreContext : DbContext
    {
        public StoreContext()
        {

        }
        public StoreContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    @"Server=(localdb)\mssqllocaldb;Database=SpyStore;Trusted_Connection=True;MultipleActiveResultSets=true;"
                        , options => options.ExecutionStrategy(c => new MyConnectionStrategy(c))
                );
            }
        }
    }
}
