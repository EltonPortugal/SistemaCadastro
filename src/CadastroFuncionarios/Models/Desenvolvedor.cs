using System.ComponentModel.DataAnnotations;

namespace CadastroFuncionarios.Models;

public class Desenvolvedor : Funcionario
{
    public NivelSenioridade NivelSenioridade { get; set; } = NivelSenioridade.Junior;

    [Range(0, 100)]
    public int CertificacoesQtd { get; set; }

    public override string Cargo => $"Desenvolvedor {NivelSenioridade}";

    // Remuneração de desenvolvedor: salário base + adicional por senioridade + adicional por certificação.
    public override decimal CalcularSalarioTotal()
    {
        var adicionalSenioridade = NivelSenioridade switch
        {
            NivelSenioridade.Junior => 0m,
            NivelSenioridade.Pleno => 800m,
            NivelSenioridade.Senior => 2000m,
            _ => 0m
        };

        return SalarioBase + adicionalSenioridade + CertificacoesQtd * 150m;
    }
}
