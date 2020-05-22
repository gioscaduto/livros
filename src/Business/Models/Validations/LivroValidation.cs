using FluentValidation;

namespace Business.Models.Validations
{
    public class LivroValidation: AbstractValidator<Livro>
    {
        public LivroValidation()
        {
            RuleFor(l => l.Titulo)
                .NotEmpty().WithMessage("O Campo {PropertyName} precisa ser preenchido")
                .Length(2, 200).WithMessage("O Campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(l => l.DtPublicacao)
               .NotNull().WithMessage("O Campo {PropertyName} precisa ser preenchido");

            RuleFor(l => l.EditoraId)
               .NotNull().WithMessage("Selecione uma Editora");

            RuleFor(l => l.ImagemCapa)
               .NotEmpty().WithMessage("Selecione uma Imagem de Capa");

            /*CONFORME PESQUISA O ISBN PODE TER 10 OU 13 CARACTERES*/
            RuleFor(l => l.ISBN)
               .NotEmpty().WithMessage("O Campo {PropertyName} precisa ser preenchido")
               .Must(l => l.Length == 10 || l.Length == 13)
               .WithMessage("O Campo {PropertyName} precisa ter 10 ou 13 caracteres");
               
            RuleFor(l => l.Sinopse)
             .NotEmpty().WithMessage("O Campo {PropertyName} precisa ser preenchido")
             .Length(2, 500).WithMessage("O Campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(l => l.Autor)
                .NotEmpty().WithMessage("O Campo {PropertyName} precisa ser preenchido")
                .Length(2, 200).WithMessage("O Campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");
        }
    }
}
