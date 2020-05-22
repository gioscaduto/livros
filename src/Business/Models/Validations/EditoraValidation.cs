using FluentValidation;

namespace Business.Models.Validations
{
    public class EditoraValidation: AbstractValidator<Editora>
    {
        public EditoraValidation()
        {
            RuleFor(e => e.Nome)
                .NotEmpty().WithMessage("O Campo {PropertyName} precisa ser preenchido")
                .Length(2, 150).WithMessage("O Campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");
        }
    }
}
