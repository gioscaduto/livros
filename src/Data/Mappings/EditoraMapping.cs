using Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mappings
{
    public class EditoraMapping : IEntityTypeConfiguration<Editora>
    {
        public void Configure(EntityTypeBuilder<Editora> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Nome)
                   .IsRequired()
                   .HasColumnType("varchar(150)");

            // 1 : N => Editora : Livros
            builder.HasMany(e => e.Livros)
                   .WithOne(e => e.Editora)
                   .HasForeignKey(e => e.EditoraId);

            builder.ToTable("Editoras");
        }
    }
}
