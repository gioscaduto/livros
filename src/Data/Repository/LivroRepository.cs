using Business.Interfaces;
using Business.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class LivroRepository : Repository<Livro>, ILivroRepository
    {
        public LivroRepository(LivrariaDbContext db) : base(db)
        {

        }

        public async Task<Livro> ObterCompleto(Guid id)
        {
            return
            await Db.Livros.AsNoTracking()
                            .Include(l => l.Editora)
                            .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<IEnumerable<Livro>> ObterTodos(string palavraChave, 
                                                         DateTime? dtPublicacaoInicial,
                                                         DateTime? dtPublicacaoFinal,
                                                         int ordenacao)
        {
            if (!string.IsNullOrEmpty(palavraChave)) palavraChave = palavraChave.Trim();

            /*PODERIAMOS CRIAR UMA QUERY SQL CONFORME O NUMERO DE FILTROS AUMENTAM 
              PARA TERMOS UM BOM NIVEL DE PERFORMANCE
             */
            var livros =
             Db.Livros.AsNoTracking()
                           .Include(l => l.Editora)
                           .Where(l => (string.IsNullOrEmpty(palavraChave) ||
                                        l.Autor.Contains(palavraChave) ||
                                        l.Editora.Nome.Contains(palavraChave) ||
                                        l.ISBN.Contains(palavraChave) ||
                                        l.Sinopse.Contains(palavraChave) ||
                                        l.Titulo.Contains(palavraChave))
                                        &&
                                        (!dtPublicacaoInicial.HasValue ||
                                         l.DtPublicacao.Date >= dtPublicacaoInicial.Value.Date)
                                        &&
                                        (!dtPublicacaoFinal.HasValue ||
                                         l.DtPublicacao.Date <= dtPublicacaoFinal.Value.Date));

            //A-Z
            if (ordenacao == 1) 
                return await livros.OrderBy(l => l.Titulo).ToListAsync();

            //Z-A
            return await livros.OrderByDescending(l => l.Titulo).ToListAsync();
        }

    }
}
