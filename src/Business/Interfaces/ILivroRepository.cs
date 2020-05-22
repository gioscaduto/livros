using Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ILivroRepository : IRepository<Livro>
    {
        Task<Livro> ObterCompleto(Guid id);

        Task<IEnumerable<Livro>> ObterTodos(string palavraChave,
                                            DateTime? dtPublicacaoInicial,
                                            DateTime? dtPublicacaoFinal,
                                            int ordenacao);
    }
}
