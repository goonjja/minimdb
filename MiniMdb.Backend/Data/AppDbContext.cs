using Microsoft.EntityFrameworkCore;
using MiniMdb.Backend.Models;
using MiniMdb.Backend.Shared;

namespace MiniMdb.Backend.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<MediaTitle> Titles { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Series> Series { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.UseIdentityColumns();

            builder.Entity<MediaTitle>(b =>
            {
                b.HasKey(e => e.Id);
                b.Property(e => e.Name).IsRequired();
                b.Property(e => e.Type).IsRequired();
                b.Property(e => e.Plot).IsRequired();
            });

            builder.Entity<MediaTitle>()
                .HasDiscriminator<MediaTitleType>(nameof(MediaTitle.Type))
                .HasValue<Movie>(MediaTitleType.Movie)
                .HasValue<Series>(MediaTitleType.Series);

            builder.Entity<MediaTitle>().HasIndex(p => p.Name);
            builder.Entity<MediaTitle>().HasIndex(p => p.Type);
        }
    }
}
