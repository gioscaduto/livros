using Business.Interfaces;
using Business.Models;
using Business.Models.Validations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class EditoraService : BaseService, IEditoraService
    {
        private readonly IEditoraRepository _editoraRepository;

        public EditoraService(IEditoraRepository editoraRepository, INotificador notificador) : base(notificador)
        {
            _editoraRepository = editoraRepository;
        }

        public async Task Adicionar(Editora editora)
        {
            if (!ExecutarValidacao(new EditoraValidation(), editora)) return;

            await _editoraRepository.Adicionar(editora);
        }

        public async Task Atualizar(Editora editora)
        {
            if (!ExecutarValidacao(new EditoraValidation(), editora)) return;

            await _editoraRepository.Atualizar(editora);
        }

        public async Task Remover(Guid id)
        {
            var editora = _editoraRepository.ObterCompleto(id).Result;

            if (editora != null && editora.Livros != null && editora.Livros.Count() > 0)
            {
                Notificar("A Editora possui livros cadastrados");
                return;
            }
            
            await _editoraRepository.Remover(id);
        }

        public void Dispose()
        {
            _editoraRepository?.Dispose();
        }
    }
}
