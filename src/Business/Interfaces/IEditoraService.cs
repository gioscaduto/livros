using Business.Models;
using System;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IEditoraService
    {
        Task Adicionar(Editora editora);
        Task Atualizar(Editora editora);
        Task Remover(Guid id);
    }
}
