using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels
{
    public class LivroViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(200, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        [DisplayName("Título")]
        public string Titulo { get; set; }

        public IFormFile ImagemCapaUpload { get; set; }
        
        [DisplayName("Imagem de Capa")]
        public string ImagemCapa { get;  set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(13, ErrorMessage = "O campo {0} precisa ter {2} ou {1} caracteres", MinimumLength = 10)]
        public string ISBN { get;  set; }

        [Required(ErrorMessage = "Selecione uma Editora")]
        public Guid EditoraId { get;  set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(200, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Autor { get;  set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(500, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Sinopse { get;  set; }

        [DisplayName("Data de Publicação")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DtPublicacao { get;  set; }

        public EditoraViewModel Editora { get; set; }

        public IEnumerable<EditoraViewModel> Editoras { get; set; }

        public bool Ativo { get;  set; }
    }
}
