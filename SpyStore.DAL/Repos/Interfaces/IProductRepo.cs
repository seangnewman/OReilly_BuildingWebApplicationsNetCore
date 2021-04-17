using SpyStore.DAL.Repos.Base;
using SpyStore.Models.Entities;
using SpyStore.Models.ViewModels.Base;
using System.Collections.Generic;


namespace SpyStore.DAL.Repos.Interfaces
{
    public interface IProductRepo : IRepo<Product>
    {
        IEnumerable<ProductAndCategoryBase> Search(string searchString);
        IEnumerable<ProductAndCategoryBase> GetAllWithCategoryName();
        IEnumerable<ProductAndCategoryBase> GetProdctsForCategory(int id);
        IEnumerable<ProductAndCategoryBase> GetFeaturedWithCategoryName();
        ProductAndCategoryBase GetOneWithCategoryName(int id);
    }
}
