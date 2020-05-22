using System;

namespace Business.Models
{
    public class Livro : Entity
    {
        public Livro(string titulo, 
                     string imagemCapa, 
                     string isbn, 
                     Guid editoraId, 
                     string autor, 
                     string sinopse, 
                     DateTime dtPublicacao,
                     bool ativo)
        {
            Titulo = titulo;
            ImagemCapa = imagemCapa;
            ISBN = isbn;
            EditoraId = editoraId;
            Autor = autor;
            Sinopse = sinopse;
            DtPublicacao = dtPublicacao;
            Ativo = ativo;
        }

        protected Livro() { }

        public string Titulo { get; private set; }

        public string ImagemCapa { get; private set; }

        public string ISBN { get; private set; }

        public Guid EditoraId { get; private set; }

        public string Autor { get; private set; }

        public string Sinopse { get; private set; }

        public DateTime DtPublicacao { get; private set; }

        public bool Ativo { get; private set; }

        /* Entity Framework Relations */
        public Editora Editora { get; set; }
    }
}
