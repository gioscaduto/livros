using App.ViewModels;
using AutoMapper;
using Business.Models;

namespace App.AutoMapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Editora, EditoraViewModel>().ReverseMap();
            CreateMap<Livro, LivroViewModel>().ReverseMap();
        }
    }
}
