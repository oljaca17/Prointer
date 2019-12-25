using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestniZadatak.Models;

namespace TestniZadatak.Services
{
    public interface ICategoryRepository
    {

        ICollection<Category> GetCategories();
        Category GetCategory(int categoryId);

        bool CategoryExists(int categoryId);

        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);
        bool Save();

    }
}





/* ICollection<Article> GetArticlesFromCategory(int categoryId);*/
