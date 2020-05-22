using Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mappings
{
    public class LivroMapping : IEntityTypeConfiguration<Livro>
    {
        public void Configure(EntityTypeBuilder<Livro> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Titulo)
                   .IsRequired()
                   .HasColumnType("varchar(200)");

            builder.Property(e => e.ImagemCapa)
                   .IsRequired()
                   .HasColumnType("varchar(150)");

            builder.Property(e => e.ISBN)
                  .IsRequired()
                  .HasColumnType("varchar(13)");

            builder.Property(e => e.Autor)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(e => e.Sinopse)
                .IsRequired()
                .HasColumnType("varchar(500)");

            builder.Property(e => e.DtPublicacao)
               .IsRequired()
               .HasColumnType("datetime");

            builder.ToTable("Livros");
        }
    }
}
