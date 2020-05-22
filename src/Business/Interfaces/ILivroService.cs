using Business.Models;
using System;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ILivroService
    {
        Task Adicionar(Livro livro);
        Task Atualizar(Livro livro);
        Task Remover(Guid id);
    }
}
