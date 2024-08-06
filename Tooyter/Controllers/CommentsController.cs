using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Models.ModelsDto;
using Services.Services;

namespace Tooyter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly CommentsServices _commentService;

        public CommentsController(ApplicationDbContext db, CommentsServices commentService)
        {
            _db = db;
            _commentService = commentService;
        }

        [HttpGet]
        [Route("LerComentarios/{idDoPost}")]
        public IActionResult LerComentarios(int idDoPost)
        {
            var post = _db.Posts
                .Include(p => p.Comments)
                .Where(p => p.PostId == idDoPost)
                .Select(p => new
                {
                    p.PostId,
                    p.PostTitle,
                    p.PostText,
                    p.LikesQuantity,
                    Comments = p.Comments.Select(c => new
                    {
                        c.CommentId,
                        c.CommentText,
                        c.LikeQuantity
                    }).ToList()
                })
                .FirstOrDefault();

            if (post == null)
            {
                return NotFound("Nenhuma postagem com esse ID foi encontrada.");
            }

            return Ok(post);
        }


        [HttpPost]
        [Route("AdicionarComentario")]
        public IActionResult AdicionarComentario([FromBody] CommentDto newComment)
        {
            var tamanhoIncorreto = _commentService.VerificarTamanhoDoComentario(newComment);
            if (tamanhoIncorreto != String.Empty)
            {
                return BadRequest(tamanhoIncorreto);
            }

            var comment = new Comment()
            {
                CommentText = newComment.CommentText,
                PostId = newComment.PostId,
            };

            var post = _db.Posts.FirstOrDefault(p => p.PostId == newComment.PostId);
            if (post == null)
            {
                return BadRequest("Nenhuma postagem com esse ID foi encontrada.");
            }

            _db.Comments.Add(comment);
            _db.SaveChanges();

            _commentService.AtualizaQntdComentarios(post);

            return Ok("Comentário adicionado.");
        }

        [HttpPut]
        [Route("DarLikeEmComentario")]
        public IActionResult DarLikeEmComentario(int idDoComentario)
        {
            var comentario = _db.Comments.FirstOrDefault(p => p.CommentId == idDoComentario);

            if (comentario == null)
            {
                return NotFound();
            }

            comentario.LikeQuantity += +1;
            _db.SaveChanges();

            return Ok(comentario);
        }

        [HttpDelete]
        [Route("DeletarComentario")]
        public IActionResult DeletarComentario(int idDoComentario)
        {
            var comentario = _db.Comments.FirstOrDefault(p => p.CommentId == idDoComentario);

            if (comentario == null)
            {
                return NotFound("Comentario não encontrado.");
            }

            _db.Comments.Remove(comentario);
            _db.SaveChanges();

            return Ok("Comentario deletado.");
        }
    }
}
