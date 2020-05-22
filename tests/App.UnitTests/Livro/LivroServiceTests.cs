using App.UnitTests.Extensions;
using Business.Interfaces;
using Business.Services;
using FluentAssertions;
using Moq;
using System.Linq;
using Xunit;

namespace App.UnitTests.Livro
{
    [Collection(nameof(LivroBogusCollection))]
    public class LivroServiceTests
    {
        private readonly LivroTestsBogusFixture _livroTestsBogusFixture;
        private readonly LivroService _livroService;

        public LivroServiceTests(LivroTestsBogusFixture livroTestsBogusFixture)
        {
            _livroTestsBogusFixture = livroTestsBogusFixture;
            _livroService = livroTestsBogusFixture.ObterLivroService();
        }

        [Fact(DisplayName = "Adicionar Livro com Sucesso"), TestPriority(1)]
        [Trait("Categoria", "Livro Service Tests")]
        public void LivroService_Adicionar_DeveExecutarComSucesso()
        {
            // Arrange
            var livro = _livroTestsBogusFixture.GerarLivrosValidos(1, true).FirstOrDefault();

            // Act
            _livroService.Adicionar(livro).GetAwaiter();

            // Assert
            _livroTestsBogusFixture.Mocker.GetMock<ILivroRepository>().Verify(r => r.Adicionar(livro), Times.Once);
            _livroTestsBogusFixture.Mocker.GetMock<INotificador>().Invocations.Should().HaveCount(0);
        }

        [Fact(DisplayName = "Adicionar Livro com Falha"), TestPriority(2)]
        [Trait("Categoria", "Livro Service Tests")]
        public void LivroService_Adicionar_DeveFalharDevidoLivroInvalida()
        {
            // Arrange
            var livro = _livroTestsBogusFixture.GerarLivrosInvalidos(1, true).FirstOrDefault();

            // Act
            _livroService.Adicionar(livro).GetAwaiter();

            // Assert
            _livroTestsBogusFixture.Mocker.GetMock<ILivroRepository>().Verify(r => r.Adicionar(livro), Times.Never);
            _livroTestsBogusFixture.Mocker.GetMock<INotificador>().Invocations.Should().HaveCountGreaterOrEqualTo(1);
        }

        [Fact(DisplayName = "Atualizar Livro com Sucesso"), TestPriority(3)]
        [Trait("Categoria", "Livro Service Tests")]
        public void LivroService_Atualizar_DeveExecutarComSucesso()
        {
            // Arrange
            var livro = _livroTestsBogusFixture.GerarLivrosValidos(1, true).FirstOrDefault();

            // Act
            _livroService.Atualizar(livro).GetAwaiter();

            // Assert
            _livroTestsBogusFixture.Mocker.GetMock<ILivroRepository>().Verify(r => r.Atualizar(livro), Times.Once);
            _livroTestsBogusFixture.Mocker.GetMock<INotificador>().Invocations.Should().HaveCount(0);
        }

        [Fact(DisplayName = "Atualizar Livro com Falha"), TestPriority(4)]
        [Trait("Categoria", "Livro Service Tests")]
        public void LivroService_Atualizar_DeveFalharDevivoLivroInvalida()
        {
            // Arrange
            var livro = _livroTestsBogusFixture.GerarLivrosInvalidos(1, true).FirstOrDefault();

            // Act
            _livroService.Atualizar(livro).GetAwaiter();

            // Assert
            _livroTestsBogusFixture.Mocker.GetMock<ILivroRepository>().Verify(r => r.Atualizar(livro), Times.Never);
            _livroTestsBogusFixture.Mocker.GetMock<INotificador>().Invocations.Should().HaveCountGreaterOrEqualTo(1);
        }

        [Fact(DisplayName = "Remover Livro com Sucesso"), TestPriority(5)]
        [Trait("Categoria", "Livro Service Tests")]
        public void LivroService_Remover_DeveExecutarComSucesso()
        {
            // Arrange
            var livro = _livroTestsBogusFixture.GerarLivrosInvalidos(1, true).FirstOrDefault();

            // Act
            _livroService.Remover(livro.Id).GetAwaiter();

            // Assert
            _livroTestsBogusFixture.Mocker.GetMock<ILivroRepository>().Verify(r => r.Remover(livro.Id), Times.Once);
        }
    }
}
