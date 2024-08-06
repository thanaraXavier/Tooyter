using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Models.Models;
using Models.ModelsDto;

namespace Services.Services
{
    public class CommentsServices
    {
        private readonly ApplicationDbContext _db;
        public CommentsServices(ApplicationDbContext db)
        {
            _db = db;
        }

        public void AtualizaQntdComentarios(Post postagem)
        {
            int qntdComentarios = 0;

            qntdComentarios = _db.Comments.Count(c => c.PostId == postagem.PostId);

            postagem.CommentsQuantity = qntdComentarios;
            _db.SaveChanges();
        }

        public string VerificarTamanhoDoComentario(CommentDto comment)
        {
            string resposta = String.Empty;

            if (comment.CommentText.Length > 120)
            {
                resposta = "O conteúdo da postagem deve ter no máximo 120 caracteres.";
            }

            return resposta;
        }
    }
}
