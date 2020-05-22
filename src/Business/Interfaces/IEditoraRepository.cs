using Business.Models;
using System;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IEditoraRepository : IRepository<Editora>
    {
        Task<Editora> ObterCompleto(Guid id);
    }
}
