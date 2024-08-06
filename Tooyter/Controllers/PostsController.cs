using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Models.ModelsDto;
using Services.Services;

namespace Tooyter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly PostsServices _postService;

        public PostsController(ApplicationDbContext db, PostsServices postService)
        {
            _db = db;
            _postService = postService;
        }

        [HttpGet]
        [Route("Timeline")]
        public ActionResult RetornarPostagens()
        {
            var posts = _db.Posts.ToList();

            //var postsDto = posts.Select(p => new
            //{
            //    p.PostId,
            //    p.PostTitle,
            //    p.PostText,
            //    p.LikesQuantity,
            //    p.CommentsQuantity
            //}).ToList();

            return Ok(posts);
        }

        [HttpPost]
        [Route("AdicionarPostagem")]
        public IActionResult AdicionarPostagem([FromBody] PostDto newPost)
        {
            var tamanhoIncorreto = _postService.VerificarTamanhoDaPostagem(newPost);
            if (tamanhoIncorreto != String.Empty)
            {
                return BadRequest(tamanhoIncorreto);
            }

            var post = new Post()
            {
                PostTitle = newPost.PostTitle,
                PostText = newPost.PostText
            };

            _db.Posts.Add(post);
            _db.SaveChanges();

            return Ok(post);
        }

        [HttpPut]
        [Route("DarLikeEmPost")]
        public IActionResult DarLikeEmPost(int idDaPostagem)
        {
            var postagem = _db.Posts.FirstOrDefault(p => p.PostId == idDaPostagem);

            if (postagem == null)
            {
                return NotFound();
            }

            postagem.LikesQuantity += +1;
            _db.SaveChanges();

            return Ok(postagem);
        }

        [HttpDelete]
        [Route("DeletarPostagem")]
        public IActionResult DeletarPostagem(int idDaPostagem)
        {
            var postagem = _db.Posts.FirstOrDefault(p => p.PostId == idDaPostagem);

            if (postagem == null)
            {
                return NotFound("Postagem não encontrada.");
            }

            _db.Posts.Remove(postagem);
            _db.SaveChanges();

            return Ok("Postagem deletada.");
        }
    }
}