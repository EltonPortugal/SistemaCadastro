using System.ComponentModel.DataAnnotations;
using CadastroFuncionarios.Models;

namespace CadastroFuncionarios.ViewModels;

// ViewModel "achatado": representa os campos de todas as subclasses num único formulário.
// O Controller é responsável por montar a subclasse correta (Gerente/Desenvolvedor/Estagiario)
// a partir do campo Tipo, já que o model binding do MVC não consegue ligar direto numa classe abstrata.
public class FuncionarioFormViewModel
{
    public int Id { get; set; }

    [Required]
    public TipoFuncionario Tipo { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório."), MaxLength(150)]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O e-mail é obrigatório."), EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, DataType(DataType.Date)]
    [Display(Name = "Data de contratação")]
    public DateTime DataContratacao { get; set; } = DateTime.Today;

    [Range(0, double.MaxValue, ErrorMessage = "O salário base não pode ser negativo.")]
    [Display(Name = "Salário base")]
    public decimal SalarioBase { get; set; }

    // Campos específicos de Gerente
    [Range(0, double.MaxValue)]
    public decimal Bonus { get; set; }

    [Range(0, 500)]
    [Display(Name = "Tamanho da equipe")]
    public int TamanhoEquipe { get; set; }

    // Campos específicos de Desenvolvedor
    [Display(Name = "Nível de senioridade")]
    public NivelSenioridade NivelSenioridade { get; set; } = NivelSenioridade.Junior;

    [Range(0, 100)]
    [Display(Name = "Quantidade de certificações")]
    public int CertificacoesQtd { get; set; }

    // Campos específicos de Estagiário
    [MaxLength(150)]
    public string? Instituicao { get; set; }

    [Range(0, double.MaxValue)]
    [Display(Name = "Auxílio transporte")]
    public decimal AuxilioTransporte { get; set; }

    public static FuncionarioFormViewModel De(Funcionario funcionario)
    {
        var vm = new FuncionarioFormViewModel
        {
            Id = funcionario.Id,
            Nome = funcionario.Nome,
            Email = funcionario.Email,
            DataContratacao = funcionario.DataContratacao,
            SalarioBase = funcionario.SalarioBase
        };

        // Pattern matching por tipo: só preenche os campos relevantes para a subclasse concreta.
        switch (funcionario)
        {
            case Gerente gerente:
                vm.Tipo = TipoFuncionario.Gerente;
                vm.Bonus = gerente.Bonus;
                vm.TamanhoEquipe = gerente.TamanhoEquipe;
                break;
            case Desenvolvedor dev:
                vm.Tipo = TipoFuncionario.Desenvolvedor;
                vm.NivelSenioridade = dev.NivelSenioridade;
                vm.CertificacoesQtd = dev.CertificacoesQtd;
                break;
            case Estagiario estagiario:
                vm.Tipo = TipoFuncionario.Estagiario;
                vm.Instituicao = estagiario.Instituicao;
                vm.AuxilioTransporte = estagiario.AuxilioTransporte;
                break;
        }

        return vm;
    }
}
