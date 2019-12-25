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
    public class ArticlesController : Controller
    {
        private IArticleRepository _articleRepository;

        public ArticlesController(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }



        //api/articles 
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ArticleDto>))]
        public IActionResult GetArticles()
        {
            
             var articles = _articleRepository.GetArticles().ToList();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
         

             var articlesDto = new List<ArticleDto>();
             foreach (var article in articles)
             {
                 articlesDto.Add(new ArticleDto
                 {
                     Id = article.Id,
                     ProductName = article.ProductName,
                     Price = article.Price,
                     DatePublished = article.DatePublished,
                     ShortDescription = article.ShortDescription,
                     FullDescription = article.FullDescription
                 });
             }
             return Ok(articles);
         
         
        }




        //api/articles/articleId
        [HttpGet("{articleId}", Name = "GetArticle")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(ArticleDto))]
        public IActionResult GetArticle(int articleId)
        {
            if (!_articleRepository.ArticleExists(articleId))
                return NotFound();

            var article = _articleRepository.GetArticle(articleId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var articleDto = new ArticleDto()
            {
                Id = article.Id,
                ProductName = article.ProductName,
                Price = article.Price,
                DatePublished = article.DatePublished,
                ShortDescription = article.ShortDescription,
                FullDescription = article.FullDescription
            };

           
            return Ok(articleDto);


        }

        //api/article
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Article))]
        [ProducesResponseType(400)]
        public IActionResult CreateArticle([FromBody]Article articleToCreate)
        {
            if (articleToCreate == null)
                return BadRequest(ModelState);

            var article = _articleRepository.GetArticles()
                .Where(a => a.ProductName.Trim().ToUpper() == articleToCreate.ProductName.Trim().ToUpper()).FirstOrDefault(); //validacija za Productname

            if (article != null)
            {
                ModelState.AddModelError("", $"Article with this 'ProductName' {articleToCreate.ProductName} already exists!");
                return StatusCode(400, $"Article with this 'ProductName' {articleToCreate.ProductName} already exists!");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_articleRepository.CreateArticle(articleToCreate))
            {
                ModelState.AddModelError("", $"Something went wrong creating article");
                return StatusCode(400, ModelState);
            }
            return CreatedAtRoute("GetArticle", new { articleId = articleToCreate }, articleToCreate);

        }


        
    }
}
