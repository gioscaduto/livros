using System.ComponentModel.DataAnnotations;

namespace App.Areas.Identity.ViewModels
{
    public class RoleViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Nome { get; set; }
    }
}
