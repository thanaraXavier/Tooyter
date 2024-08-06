using DataAccess;
using Models.ModelsDto;

namespace Services.Services
{
    public class PostsServices
    {
        private readonly ApplicationDbContext _db;
        public PostsServices(ApplicationDbContext db)
        {
            _db = db;
        }

        public string VerificarTamanhoDaPostagem(PostDto postagem)
        {
            string resposta = String.Empty;

            if (postagem.PostTitle.Length > 50)
            {
                resposta = "O título deve ter no máximo 50 caracteres.";
            }

            if (postagem.PostText.Length > 120)
            {
                resposta = "O conteúdo da postagem deve ter no máximo 120 caracteres.";
            }

            return resposta;
        }
    }
}
