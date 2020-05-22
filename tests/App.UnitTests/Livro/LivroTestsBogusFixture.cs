using Bogus;
using Bogus.DataSets;
using Business.Services;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using Xunit;
using LivroModel = Business.Models.Livro;

namespace App.UnitTests.Livro
{
    [CollectionDefinition(nameof(LivroBogusCollection))]
    public class LivroBogusCollection : ICollectionFixture<LivroTestsBogusFixture>
    { }

    public class LivroTestsBogusFixture : IDisposable
    {
        public LivroService LivroService;
        public AutoMocker Mocker;

        public IEnumerable<LivroModel> GerarLivrosValidos(int quantidade, bool ativo)
        {
            var genero = new Faker().PickRandom<Name.Gender>();
            var editoraId = Guid.NewGuid();

            var livros =
                new Faker<LivroModel>("pt_BR")
                .CustomInstantiator(f => new
                    LivroModel(f.Name.FirstName(genero),
                               "~/images/",
                               "0123456789",
                               editoraId,
                               f.Name.FullName(genero),
                               "LOREM IPSUM",
                               DateTime.Now,
                               ativo));

            return livros.Generate(quantidade);
        }

        public IEnumerable<LivroModel> GerarLivrosInvalidos(int quantidade, bool ativo)
        {
            var genero = new Faker().PickRandom<Name.Gender>();
            var editoraId = Guid.NewGuid();

            var livros =
                new Faker<LivroModel>("pt_BR")
                .CustomInstantiator(f => new
                    LivroModel("",
                               "",
                               "",
                               editoraId,
                               "",
                               "",
                               DateTime.Now,
                               ativo));

            return livros.Generate(quantidade);
        }

        public LivroService ObterLivroService()
        {
            Mocker = new AutoMocker();

            LivroService = Mocker.CreateInstance<LivroService>();

            return LivroService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
