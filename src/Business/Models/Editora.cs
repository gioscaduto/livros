using System.Collections.Generic;

namespace Business.Models
{
    public class Editora: Entity
    {
        public Editora(string nome, bool ativo)
        {
            Nome = nome;
            Ativo = ativo;
            Livros = new List<Livro>();
        }

        protected Editora() 
        {
            Livros = new List<Livro>();
        }

        public string Nome { get; private set; }
        public bool Ativo { get; private set; }

        /* Entity Framework Relations */
        public IEnumerable<Livro> Livros { get; private set; }
    }
}
