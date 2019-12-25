using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestniZadatak.Dtos;
using TestniZadatak.Models;
using TestniZadatak.Services;

namespace TestniZadatak.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : Controller
    {
        private ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }


        //api/categories
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDto>))]
        public IActionResult GetCategories()
        {

            var categories = _categoryRepository.GetCategories().ToList();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var categoriesDto = new List<CategoryDto>();
            foreach (var category in categories)
            {
                categoriesDto.Add(new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name
                });
               

                
            }
            return Ok(categories);
        }



        //api/categories/categoryId
        [HttpGet("{categoryId}", Name = "GetCategory")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CategoryDto))]
        public IActionResult GetCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound();

            var category = _categoryRepository.GetCategory(categoryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var categoryDto = new CategoryDto()
            {
                Id = category.Id,
                Name = category.Name
            };


            return Ok(categoryDto);


        }





        //api/categories
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody]Category categoryToCreate)
        {
            if (categoryToCreate == null)
                return BadRequest(ModelState);

            var category = _categoryRepository.GetCategories()
                .Where(u => u.Name.Trim().ToUpper() == categoryToCreate.Name.Trim().ToUpper()).FirstOrDefault(); //validacija za Name, ako korisnik pokusa da unese ime kategorije koja vec postoji

            if (category != null)
            {
                ModelState.AddModelError("", $"Category with this name {categoryToCreate.Name} already exists!");
                return StatusCode(400, $"Category with this name {categoryToCreate.Name} already exists!");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categoryRepository.CreateCategory(categoryToCreate))
            {
                ModelState.AddModelError("", $"Something went wrong creating category");
                return StatusCode(400, ModelState);
            }
            return CreatedAtRoute("GetCategory", new { categoryId = categoryToCreate }, categoryToCreate);

        }


        //api/categories/categoryId
        [HttpPatch("{categoryId}")]
        [ProducesResponseType(204)] //no content
        [ProducesResponseType(400)]
        public IActionResult UpdateCategory(int categoryId, [FromBody]Category updatedCategoryInfo)
        {
            if (updatedCategoryInfo == null)
                return BadRequest(ModelState);

            if (categoryId != updatedCategoryInfo.Id)
                return BadRequest(ModelState);

            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound();


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categoryRepository.UpdateCategory(updatedCategoryInfo))
            {
                ModelState.AddModelError("", $"Something went wrong updating category");
                return StatusCode(400, ModelState);
            }

            return NoContent();
        }

        //api/categories/categoryId
        [HttpDelete("{categoryId}")]
        [ProducesResponseType(204)] //no content
        [ProducesResponseType(404)]

        public IActionResult DeleteCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound();

            var categoryToDelete = _categoryRepository.GetCategory(categoryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categoryRepository.DeleteCategory(categoryToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting category");
                return StatusCode(400, ModelState);
            }

            return NoContent();

        }



    }
}























//(BONUS) GET /categories/{id}/articles

/*
        //api/categories/{categoryId}/articles
        [HttpGet("{categoryId}/articles")]
        public IActionResult GetArticlesFromCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound();

            var articles = _categoryRepository.GetArticlesFromCategory(categoryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var articlesDto = new List<ArticleDto>(); // articlesDto je samo naziv

            foreach (var article in articles)
            {
                articlesDto.Add(new ArticleDto)
                {
                    //
                }
            });

        }*/

