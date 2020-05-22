using App.UnitTests.Extensions;
using Business.Interfaces;
using Business.Services;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using System.Linq;
using Xunit;
using EditoraModel = Business.Models.Editora;

namespace App.UnitTests.Editora
{
    [Collection(nameof(EditoraBogusCollection))]
    public class EditoraServiceTests
    {
        private readonly EditoraTestsBogusFixture _editoraTestsBogusFixture;
        private readonly EditoraService _editoraService;
        private EditoraModel _editora;

        public EditoraServiceTests(EditoraTestsBogusFixture editoraTestsBogusFixture)
        {
            _editoraTestsBogusFixture = editoraTestsBogusFixture;
            _editoraService = editoraTestsBogusFixture.ObterEditoraService();
        }

        [Fact(DisplayName = "Adicionar Editora com Sucesso"), TestPriority(1)]
        [Trait("Categoria", "Editora Service Tests")]
        public void EditoraService_Adicionar_DeveExecutarComSucesso()
        {
            // Arrange
            _editora = _editoraTestsBogusFixture.GerarEditorasValidas(1, true).FirstOrDefault();

            // Act
            _editoraService.Adicionar(_editora).GetAwaiter();

            // Assert
            _editoraTestsBogusFixture.Mocker.GetMock<IEditoraRepository>().Verify(r => r.Adicionar(_editora), Times.Once);
            _editoraTestsBogusFixture.Mocker.GetMock<INotificador>().Invocations.Should().HaveCount(0);
        }

        [Fact(DisplayName = "Adicionar Editora com Falha"), TestPriority(2)]
        [Trait("Categoria", "Editora Service Tests")]
        public void EditoraService_Adicionar_DeveFalharDevidoEditoraInvalida()
        {
            // Arrange
            _editora = _editoraTestsBogusFixture.GerarEditorasInvalidas(1, true).FirstOrDefault();
            
            // Act
            _editoraService.Adicionar(_editora).GetAwaiter();

            // Assert
            _editoraTestsBogusFixture.Mocker.GetMock<IEditoraRepository>().Verify(r => r.Adicionar(_editora), Times.Never);
            _editoraTestsBogusFixture.Mocker.GetMock<INotificador>().Invocations.Should().HaveCountGreaterOrEqualTo(1);
        }

        [Fact(DisplayName = "Atualizar Editora com Sucesso"), TestPriority(3)]
        [Trait("Categoria", "Editora Service Tests")]
        public void EditoraService_Atualizar_DeveExecutarComSucesso()
        {
            // Arrange
            _editora = _editoraTestsBogusFixture.GerarEditorasValidas(1, true).FirstOrDefault();

            // Act
            _editoraService.Atualizar(_editora).GetAwaiter();

            // Assert
            _editoraTestsBogusFixture.Mocker.GetMock<IEditoraRepository>().Verify(r => r.Atualizar(_editora), Times.Once);
            _editoraTestsBogusFixture.Mocker.GetMock<INotificador>().Invocations.Should().HaveCount(0);
        }

        [Fact(DisplayName = "Atualizar Editora com Falha"), TestPriority(4)]
        [Trait("Categoria", "Editora Service Tests")]
        public void EditoraService_Atualizar_DeveFalharDevivoEditoraInvalida()
        {
            // Arrange
            var editora = _editoraTestsBogusFixture.GerarEditorasInvalidas(1, true).FirstOrDefault();

            // Act
            _editoraService.Atualizar(editora).GetAwaiter();

            // Assert
            _editoraTestsBogusFixture.Mocker.GetMock<IEditoraRepository>().Verify(r => r.Atualizar(editora), Times.Never);
            _editoraTestsBogusFixture.Mocker.GetMock<INotificador>().Invocations.Should().HaveCountGreaterOrEqualTo(1);
        }
    }
}
