using Business.Interfaces;
using Business.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class EditoraRepository: Repository<Editora>, IEditoraRepository
    {
        public EditoraRepository(LivrariaDbContext db):base(db)
        {
        }

        public async Task<Editora> ObterCompleto(Guid id)
        {
            return
            await Db.Editoras.AsNoTracking()
                             .Include(e => e.Livros)
                             .FirstOrDefaultAsync(l => l.Id == id);
        }
    }
}
