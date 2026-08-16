using System.ComponentModel.DataAnnotations;

namespace CadastroFuncionarios.Models;

// Classe abstrata base, mapeada pelo EF Core como TPH: Gerente, Desenvolvedor e Estagiario
// ficam na mesma tabela "Funcionarios", diferenciados pela coluna "TipoFuncionario".
public abstract class Funcionario
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório."), MaxLength(150)]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O e-mail é obrigatório."), EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, DataType(DataType.Date)]
    public DateTime DataContratacao { get; set; } = DateTime.Today;

    [Range(0, double.MaxValue, ErrorMessage = "O salário base não pode ser negativo.")]
    public decimal SalarioBase { get; set; }

    // Propriedade abstrata: cada subclasse expõe um rótulo de cargo diferente.
    public abstract string Cargo { get; }

    // Método abstrato: cada subclasse tem sua própria fórmula de remuneração total.
    public abstract decimal CalcularSalarioTotal();
}
