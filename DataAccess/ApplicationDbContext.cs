using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Defina seus DbSets aqui
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Post
            modelBuilder.Entity<Post>().HasData(
                new Post { PostId = 1, PostTitle = "Música nova", PostText = "Adorei a nova música do TWICE chamada Beyond The Horizon." },
                new Post { PostId = 2, PostTitle = "Eu odeio abacaxi na pizza", PostText = "Abacaxi na pizza me deixa triste." },
                new Post { PostId = 3, PostTitle = "Carro amarelo", PostText = "Hoje eu vi um carro amarelo pela janela do prédio." }
                );

            modelBuilder.Entity<Post>().HasMany(p => p.Comments).WithOne(c => c.Post).HasForeignKey(c => c.PostId);

            // Comment
            modelBuilder.Entity<Comment>().HasData(
                new Comment { CommentId = 1, CommentText = "Você deveria experimentar abacaxi na pizza doce", PostId = 2 }
                );
        }
    }
}
