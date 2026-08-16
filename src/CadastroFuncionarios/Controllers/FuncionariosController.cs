using CadastroFuncionarios.Data;
using CadastroFuncionarios.Models;
using CadastroFuncionarios.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CadastroFuncionarios.Controllers;

public class FuncionariosController : Controller
{
    private readonly AppDbContext _context;

    public FuncionariosController(AppDbContext context)
    {
        _context = context;
    }

    // GET: /Funcionarios
    // Consulta polimórfica: o EF Core devolve Gerente, Desenvolvedor e Estagiario juntos,
    // e a view chama funcionario.CalcularSalarioTotal() sem saber o tipo concreto de cada um.
    public async Task<IActionResult> Index()
    {
        var funcionarios = await _context.Funcionarios
            .OrderBy(f => f.Nome)
            .ToListAsync();

        return View(funcionarios);
    }

    // GET: /Funcionarios/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var funcionario = await _context.Funcionarios.FirstOrDefaultAsync(f => f.Id == id);
        if (funcionario is null) return NotFound();

        return View(funcionario);
    }

    // GET: /Funcionarios/Create
    public IActionResult Create()
    {
        return View(new FuncionarioFormViewModel());
    }

    // POST: /Funcionarios/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(FuncionarioFormViewModel vm)
    {
        if (!ValidarCamposEspecificos(vm))
        {
            return View(vm);
        }

        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        Funcionario funcionario = MontarFuncionario(vm);

        _context.Funcionarios.Add(funcionario);
        await _context.SaveChangesAsync();

        TempData["Mensagem"] = $"{funcionario.Cargo} \"{funcionario.Nome}\" cadastrado com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Funcionarios/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var funcionario = await _context.Funcionarios.FirstOrDefaultAsync(f => f.Id == id);
        if (funcionario is null) return NotFound();

        return View(FuncionarioFormViewModel.De(funcionario));
    }

    // POST: /Funcionarios/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, FuncionarioFormViewModel vm)
    {
        if (id != vm.Id) return BadRequest();

        var funcionarioExistente = await _context.Funcionarios.FirstOrDefaultAsync(f => f.Id == id);
        if (funcionarioExistente is null) return NotFound();

        if (!ValidarCamposEspecificos(vm) || !ModelState.IsValid)
        {
            return View(vm);
        }

        funcionarioExistente.Nome = vm.Nome;
        funcionarioExistente.Email = vm.Email;
        funcionarioExistente.DataContratacao = vm.DataContratacao;
        funcionarioExistente.SalarioBase = vm.SalarioBase;

        // O tipo do funcionário não muda na edição: cada subclasse atualiza só os campos que lhe pertencem.
        switch (funcionarioExistente)
        {
            case Gerente gerente:
                gerente.Bonus = vm.Bonus;
                gerente.TamanhoEquipe = vm.TamanhoEquipe;
                break;
            case Desenvolvedor dev:
                dev.NivelSenioridade = vm.NivelSenioridade;
                dev.CertificacoesQtd = vm.CertificacoesQtd;
                break;
            case Estagiario estagiario:
                estagiario.Instituicao = vm.Instituicao ?? string.Empty;
                estagiario.AuxilioTransporte = vm.AuxilioTransporte;
                break;
        }

        await _context.SaveChangesAsync();

        TempData["Mensagem"] = $"{funcionarioExistente.Cargo} \"{funcionarioExistente.Nome}\" atualizado com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Funcionarios/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var funcionario = await _context.Funcionarios.FirstOrDefaultAsync(f => f.Id == id);
        if (funcionario is null) return NotFound();

        return View(funcionario);
    }

    // POST: /Funcionarios/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var funcionario = await _context.Funcionarios.FindAsync(id);
        if (funcionario is not null)
        {
            _context.Funcionarios.Remove(funcionario);
            await _context.SaveChangesAsync();
            TempData["Mensagem"] = $"{funcionario.Cargo} \"{funcionario.Nome}\" removido.";
        }

        return RedirectToAction(nameof(Index));
    }

    // Fábrica simples: converte o ViewModel achatado na subclasse concreta correta de Funcionario.
    private static Funcionario MontarFuncionario(FuncionarioFormViewModel vm)
    {
        Funcionario funcionario = vm.Tipo switch
        {
            TipoFuncionario.Gerente => new Gerente
            {
                Bonus = vm.Bonus,
                TamanhoEquipe = vm.TamanhoEquipe
            },
            TipoFuncionario.Desenvolvedor => new Desenvolvedor
            {
                NivelSenioridade = vm.NivelSenioridade,
                CertificacoesQtd = vm.CertificacoesQtd
            },
            TipoFuncionario.Estagiario => new Estagiario
            {
                Instituicao = vm.Instituicao ?? string.Empty,
                AuxilioTransporte = vm.AuxilioTransporte
            },
            _ => throw new ArgumentOutOfRangeException(nameof(vm.Tipo))
        };

        funcionario.Nome = vm.Nome;
        funcionario.Email = vm.Email;
        funcionario.DataContratacao = vm.DataContratacao;
        funcionario.SalarioBase = vm.SalarioBase;

        return funcionario;
    }

    // Validação condicional: o campo obrigatório de Estagiário (Instituição) só se aplica a esse tipo.
    private bool ValidarCamposEspecificos(FuncionarioFormViewModel vm)
    {
        if (vm.Tipo == TipoFuncionario.Estagiario && string.IsNullOrWhiteSpace(vm.Instituicao))
        {
            ModelState.AddModelError(nameof(vm.Instituicao), "Informe a instituição de ensino do estagiário.");
            return false;
        }

        return true;
    }
}
