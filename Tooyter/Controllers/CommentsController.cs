using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
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
        [Route("LerComentarios")]
        public IActionResult LerComentarios(int idDoPost)
        {
            var post = _db.Posts.Where(p => p.PostId == idDoPost).Select(p => new
            {
                p.PostId,
                p.PostTitle,
                p.PostText,
                p.LikesQuantity,
                Comentarios = p.Comments.Select(c => new
                {
                    c.CommentId,
                    c.CommentText,
                    c.LikeQuantity
                }).ToList()
            }).FirstOrDefault();

            if (post == null)
            {
                return NotFound("Nenhuma postagem com esse ID foi encontrada.");
            }

            return Ok(post);
        }

        [HttpPost]
        [Route("AdicionarComentario")]
        public IActionResult AdicionarComentario(int idDaPostagem, string textoDoComentario)
        {
            string tamanhoCorreto = _commentService.VerificarTamanhoDoComentario(textoDoComentario);
            if (tamanhoCorreto != String.Empty)
            {
                return BadRequest(tamanhoCorreto);
            }

            var postagem = _db.Posts.FirstOrDefault(p => p.PostId == idDaPostagem);

            if (postagem == null)
            {
                return NotFound("Essa postagem não existe.");
            }

            var comment = new Comment();

            comment.CommentText = textoDoComentario;
            comment.PostId = idDaPostagem;

            _db.Comments.Add(comment);
            _db.SaveChanges();
            _commentService.AtualizaQntdComentarios(postagem);

            return Ok(comment);
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
