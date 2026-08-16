using System.ComponentModel.DataAnnotations;

namespace CadastroFuncionarios.Models;

public class Estagiario : Funcionario
{
    [Required(ErrorMessage = "Informe a instituição de ensino."), MaxLength(150)]
    public string Instituicao { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal AuxilioTransporte { get; set; }

    public override string Cargo => "Estagiário";

    // Estagiário não recebe bônus nem adicionais, apenas bolsa + auxílio transporte.
    public override decimal CalcularSalarioTotal() => SalarioBase + AuxilioTransporte;
}
