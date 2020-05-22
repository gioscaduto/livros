using App.Utils;
using App.ViewModels;
using App.ViewModels.Pesquisa;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using X.PagedList;

namespace App.Controllers
{
    [Authorize]
    public class LivrosController : BaseController
    {
        private readonly ILivroRepository _livroRepository;
        private readonly IEditoraRepository _editoraRepository;
        private readonly ILivroService _livroService;
        private readonly IMapper _mapper;

        public LivrosController(ILivroRepository livroRepository,
                                IEditoraRepository editoraRepository,
                                ILivroService livroService,
                                INotificador notificador,
                                IMapper mapper) : base(notificador)
        {
            _livroRepository = livroRepository;
            _editoraRepository = editoraRepository;
            _livroService = livroService;
            _mapper = mapper;
        }

        [Route("lista-de-livros")]
        public async Task<IActionResult> Index(LivroPesquisaViewModel pesquisa, [FromServices]PaginaUtil paginaUtil)
        {
            var livros = await Filtrar(pesquisa);
            pesquisa.Livros = livros.ToPagedList(pesquisa.Pagina, paginaUtil.QtdItensPagina);
            
            return View(pesquisa);
        }

        [Route("dados-do-livro/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var editoraViewModel = await ObterLivro(id);

            if (editoraViewModel == null)
            {
                return NotFound();
            }

            return View(editoraViewModel);
        }

        [Route("novo-livro")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var livroViewModel = await PopularEditoras(new LivroViewModel());
            livroViewModel.DtPublicacao = DateTime.Now;

            return View(livroViewModel);
        }

        [Route("novo-livro")]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(LivroViewModel livroViewModel)
        {
            livroViewModel = await PopularEditoras(livroViewModel);
            if (!ModelState.IsValid) return View(livroViewModel);

            if (livroViewModel.ImagemCapaUpload != null)
            {
                var imgPrefixo = Guid.NewGuid() + "_";

                if (!await UploadArquivo(livroViewModel.ImagemCapaUpload, imgPrefixo))
                {
                    return View(livroViewModel);
                }

                livroViewModel.ImagemCapa = imgPrefixo + livroViewModel.ImagemCapaUpload.FileName;

                await _livroService.Adicionar(_mapper.Map<Livro>(livroViewModel));

                if (OperacaoValida())
                    return RedirectToAction("Index");
            }

            return View(livroViewModel);
        }

        [Route("editar-livro/{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var livroViewModel = await ObterLivro(id);

            if (livroViewModel == null) return NotFound();

            return View(livroViewModel);
        }

        [Route("editar-livro/{id:guid}")]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid id, LivroViewModel livroViewModel)
        {
            if (id != livroViewModel.Id) return NotFound();

            var livroAtualizacao = await ObterLivro(id);
            livroViewModel.Editora = livroAtualizacao.Editora;
            livroViewModel.ImagemCapa = livroAtualizacao.ImagemCapa;

            if (!ModelState.IsValid) return View(livroViewModel);

            if (livroViewModel.ImagemCapaUpload != null) //Imagem de Capa Alterada
            {
                ExcluirImagemCapa(livroAtualizacao.ImagemCapa);

                var imgPrefixo = Guid.NewGuid() + "_";
                if (!await UploadArquivo(livroViewModel.ImagemCapaUpload, imgPrefixo))
                {
                    return View(livroViewModel);
                }

                livroAtualizacao.ImagemCapa = imgPrefixo + livroViewModel.ImagemCapaUpload.FileName;
            }

            livroAtualizacao.Autor = livroViewModel.Autor;
            livroAtualizacao.DtPublicacao = livroViewModel.DtPublicacao;
            livroAtualizacao.ISBN = livroViewModel.ISBN;
            livroAtualizacao.Sinopse = livroViewModel.Sinopse;
            livroAtualizacao.Titulo = livroViewModel.Titulo;
            livroAtualizacao.Ativo = livroViewModel.Ativo;

            await _livroService.Atualizar(_mapper.Map<Livro>(livroAtualizacao));

            if (!OperacaoValida()) return View(livroViewModel);

            return RedirectToAction("Index");
        }

        [Route("excluir-livro/{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var livro = await ObterLivro(id);

            if (livro == null) return NotFound();
            
            return View(livro);
        }

        [Route("excluir-livro/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var livro = await ObterLivro(id);

            if (livro == null) return NotFound();

            await _livroService.Remover(id);

            if (!OperacaoValida()) return View(livro);

            if (!string.IsNullOrEmpty(livro.ImagemCapa))
                ExcluirImagemCapa(livro.ImagemCapa);

            TempData["Sucesso"] = "Livro excluido com sucesso!";

            return RedirectToAction("Index");
        }

        private void ExcluirImagemCapa(string imgPrefixo)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imgPrefixo);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
        }

        private async Task<LivroViewModel> PopularEditoras(LivroViewModel livro)
        {
            livro.Editoras = _mapper.Map<IEnumerable<EditoraViewModel>>(await _editoraRepository.ObterTodos());
            return livro;
        }

        private async Task<LivroViewModel> ObterLivro(Guid id)
        {
            var livro = _mapper.Map<LivroViewModel>(await _livroRepository.ObterCompleto(id));
            livro.Editoras = _mapper.Map<IEnumerable<EditoraViewModel>>(await _editoraRepository.ObterTodos());
            return livro;
        }

        private async Task<bool> UploadArquivo(IFormFile arquivo, string imgPrefixo)
        {
            if (arquivo.Length <= 0) return false;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imgPrefixo + arquivo.FileName);

            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError(string.Empty, "Já existe um arquivo com este nome");
                return false;
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            return true;
        }

        public async Task<IActionResult> ObterListaLivros(LivroPesquisaViewModel pesquisa, [FromServices]PaginaUtil paginaUtil)
        {
            var livros = await Filtrar(pesquisa);
            return PartialView("_ListaLivros", livros.ToPagedList(pesquisa.Pagina, paginaUtil.QtdItensPagina));
        }

        private async Task<IEnumerable<LivroViewModel>> Filtrar(LivroPesquisaViewModel pesquisa)
        {
            return 
            _mapper.Map<IEnumerable<LivroViewModel>>
                    (await _livroRepository.ObterTodos(pesquisa.PalavraChave,
                                                       pesquisa.DtPublicacaoInicial,
                                                       pesquisa.DtPublicacaoFinal,
                                                       pesquisa.Ordenacao));
        }
    }
}