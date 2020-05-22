using System;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels
{
    public class EditoraViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(150, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Nome { get; set; }

        public bool Ativo { get; set; }
    }
}
