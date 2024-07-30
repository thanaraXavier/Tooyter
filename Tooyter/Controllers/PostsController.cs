using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
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
        public ActionResult Timeline()
        {
            var posts = _db.Posts.ToList();

            var postsDto = posts.Select(p => new
            {
                p.PostId,
                p.PostTitle,
                p.PostText,
                p.LikesQuantity,
                p.CommentsQuantity
            }).ToList();

            return Ok(postsDto);
        }

        [HttpPost]
        [Route("AdicionarPostagem")]
        public IActionResult AdicionarPostagem(string titulo, string texto)
        {
            string tamanhoCorreto = _postService.VerificarTamanhoDaPostagem(titulo, texto);
            if(tamanhoCorreto == String.Empty)
            {
                var post = new Post();

                post.PostTitle = titulo;
                post.PostText = texto;

                _db.Posts.Add(post);
                _db.SaveChanges();

                return Ok(post);
            }

            return BadRequest(tamanhoCorreto);
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