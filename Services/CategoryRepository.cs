using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestniZadatak.Models;
using Microsoft.EntityFrameworkCore;

namespace TestniZadatak.Services
{
    public class CategoryRepository : ICategoryRepository
    {
        private TestDbContext _categoryContext;

        public CategoryRepository(TestDbContext categoryContext)
        {
            _categoryContext = categoryContext;
        }
        public bool CategoryExists(int categoryId)
        {
            return _categoryContext.Categories.Any(c => c.Id == categoryId);
        }

        public bool CreateCategory(Category category)
        {
            
            _categoryContext.AddAsync(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _categoryContext.Remove(category);
            return Save();
        }

        /* public ICollection<Article> GetArticlesFromCategory(int categoryId)
         {
             var listArticles = _categoryContext.Articles.Include(x => x.Category).FirstOrDefault(y => y.CategoryId == categoryId);
             return listArticles.To;
         }*/

        public ICollection<Category> GetCategories()
        {
            return _categoryContext.Categories.OrderBy(c => c.Id).ToList();
        }

        public Category GetCategory(int categoryId)
        {
            return _categoryContext.Categories.Where(c => c.Id == categoryId).FirstOrDefault();
        }

        public bool Save()
        {
            var saved = _categoryContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

        public bool UpdateCategory(Category category)
        {
            _categoryContext.Update(category);
            return Save(); ;
        }
    }
}
