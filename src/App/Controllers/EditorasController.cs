using App.Utils;
using App.ViewModels;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;

namespace App.Controllers
{
    [Authorize]
    public class EditorasController : BaseController
    {
        private readonly IEditoraRepository _editoraRepository;
        private readonly IEditoraService _editoraService;
        private readonly IMapper _mapper;
        
        public EditorasController(IEditoraRepository editoraRepository,
                                  IEditoraService editoraService,
                                  INotificador notificador,
                                  IMapper mapper) : base(notificador)
        {
            _editoraRepository = editoraRepository;
            _editoraService = editoraService;
            _mapper = mapper;
        }

        [Route("lista-de-editoras")]
        public async Task<IActionResult> Index(int? pagina, [FromServices] PaginaUtil paginaUtil)
        {
            var editoras = _mapper.Map<IEnumerable<EditoraViewModel>>(await _editoraRepository.ObterTodos());
            var editorasPagedList = editoras.ToPagedList(pagina ?? 1, paginaUtil.QtdItensPagina);

            return View(editorasPagedList);
        }

        [Route("dados-da-editora/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var editoraViewModel = await ObterEditora(id);

            if (editoraViewModel == null)
            {
                return NotFound();
            }

            return View(editoraViewModel);
        }

        [Route("nova-editora")]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("nova-editora")]
        public async Task<IActionResult> Create(EditoraViewModel editoraViewModel)
        {
            if (!ModelState.IsValid) return View(editoraViewModel);

            var editora = _mapper.Map<Editora>(editoraViewModel);
            await _editoraService.Adicionar(editora);

            if (!OperacaoValida()) return View(editoraViewModel);

            return RedirectToAction("Index");
        }

        [Route("editar-editora/{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var editoraViewModel = await ObterEditora(id);

            if (editoraViewModel == null)
            {
                return NotFound();
            }

            return View(editoraViewModel);
        }

        [HttpPost]
        [Route("editar-editora/{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid id, EditoraViewModel editoraViewModel)
        {
            if (id != editoraViewModel.Id) return NotFound();

            if (!ModelState.IsValid) return View(editoraViewModel);

            var editora = _mapper.Map<Editora>(editoraViewModel);
            await _editoraService.Atualizar(editora);

            if (!OperacaoValida()) return View(editoraViewModel);

            return RedirectToAction("Index");
        }


        [Route("excluir-editora/{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var editoraViewModel = await ObterEditora(id);

            if (editoraViewModel == null) return NotFound();

            return View(editoraViewModel);
        }


        [HttpPost, ActionName("Delete")]
        [Route("excluir-editora/{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var editora = await ObterEditora(id);

            if (editora == null) return NotFound();

            await _editoraService.Remover(id);

            if (!OperacaoValida()) return View(editora);

            return RedirectToAction("Index");
        }

        private async Task<EditoraViewModel> ObterEditora(Guid id)
        {
            return _mapper.Map<EditoraViewModel>(await _editoraRepository.ObterPorId(id));
        }
    }
}