using DataAccess;

namespace Services.Services
{
    public class PostsServices
    {
        private readonly ApplicationDbContext _db;
        public PostsServices(ApplicationDbContext db)
        {
            _db = db;
        }

        public string VerificarTamanhoDaPostagem(string title, string text)
        {
            string resposta = String.Empty;

            if (title.Length > 50)
            {
                resposta = "O título deve ter no máximo 50 caracteres.";
            }

            if (text.Length > 120)
            {
                resposta = "O conteúdo da postagem deve ter no máximo 120 caracteres.";
            }

            return resposta;
        }
    }
}
