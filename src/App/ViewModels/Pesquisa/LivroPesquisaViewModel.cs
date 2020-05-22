using System;
using System.Collections.Generic;
using X.PagedList;

namespace App.ViewModels.Pesquisa
{
    public class LivroPesquisaViewModel
    {
        public LivroPesquisaViewModel()
        {
            Pagina = 1;
            Ordenacao = 1; //A-Z
            Livros = new PagedList<LivroViewModel>(new List<LivroViewModel>(), 1, 1);
        }

        public int Pagina { get; set; }

        public string PalavraChave { get; set; }

        public DateTime? DtPublicacaoInicial { get; set; }

        public DateTime? DtPublicacaoFinal { get; set; }

        public int Ordenacao { get; set; }

        public IPagedList<LivroViewModel> Livros { get; set; }
    }
}
