using Microsoft.EntityFrameworkCore;
using CadastroFuncionarios.Models;

namespace CadastroFuncionarios.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Funcionario> Funcionarios => Set<Funcionario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // TPH: Gerente, Desenvolvedor e Estagiario compartilham a tabela "Funcionarios".
        modelBuilder.Entity<Funcionario>()
            .HasDiscriminator<string>("TipoFuncionario")
            .HasValue<Gerente>("Gerente")
            .HasValue<Desenvolvedor>("Desenvolvedor")
            .HasValue<Estagiario>("Estagiario");

        modelBuilder.Entity<Funcionario>()
            .Property(f => f.SalarioBase)
            .HasColumnType("decimal(18,2)");
    }
}
