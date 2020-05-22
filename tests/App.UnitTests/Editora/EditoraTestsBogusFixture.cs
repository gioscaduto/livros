using Bogus;
using Bogus.DataSets;
using Business.Services;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using Xunit;
using EditoraModel = Business.Models.Editora;

namespace App.UnitTests.Editora
{
    [CollectionDefinition(nameof(EditoraBogusCollection))]
    public class EditoraBogusCollection : ICollectionFixture<EditoraTestsBogusFixture>
    { }

    public class EditoraTestsBogusFixture : IDisposable
    {
        public EditoraService EditoraService;
        public AutoMocker Mocker;

        public IEnumerable<EditoraModel> GerarEditorasValidas(int quantidade, bool ativo)
        {
            var genero = new Faker().PickRandom<Name.Gender>();

            var editoras =
                new Faker<EditoraModel>("pt_BR")
                .CustomInstantiator(f => new EditoraModel(f.Name.FirstName(genero), ativo));

            return editoras.Generate(quantidade);
        }

        public IEnumerable<EditoraModel> GerarEditorasInvalidas(int quantidade, bool ativo)
        {
            var genero = new Faker().PickRandom<Name.Gender>();

            var editoras =
                new Faker<EditoraModel>("pt_BR")
                .CustomInstantiator(f => new EditoraModel("", ativo));

            return editoras.Generate(quantidade);
        }

        public EditoraService ObterEditoraService()
        {
            Mocker = new AutoMocker();

            EditoraService = Mocker.CreateInstance<EditoraService>();

            return EditoraService;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
