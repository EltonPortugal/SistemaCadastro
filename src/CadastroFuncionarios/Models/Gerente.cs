using System.ComponentModel.DataAnnotations;

namespace CadastroFuncionarios.Models;

// Herança: Gerente reaproveita Nome, Email, SalarioBase etc. de Funcionario
// e sobrescreve Cargo/CalcularSalarioTotal com sua própria regra (polimorfismo).
public class Gerente : Funcionario
{
    [Range(0, double.MaxValue)]
    public decimal Bonus { get; set; }

    [Range(0, 500, ErrorMessage = "Informe um tamanho de equipe válido.")]
    public int TamanhoEquipe { get; set; }

    public override string Cargo => "Gerente";

    // Remuneração de gerente: salário base + bônus + um adicional por pessoa gerenciada.
    public override decimal CalcularSalarioTotal() => SalarioBase + Bonus + TamanhoEquipe * 200m;
}
