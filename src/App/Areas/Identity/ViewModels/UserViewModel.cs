using System;
using System.ComponentModel.DataAnnotations;

namespace App.Areas.Identity.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "Digite um E-mail Válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Senha { get; set; }

        public string[] Roles { get; set; }
    }
}
