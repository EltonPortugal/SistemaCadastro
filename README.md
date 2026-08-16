# Cadastro de Funcionários (ASP.NET Core MVC + EF Core + SQLite)

Aplicação MVC (Razor Views) em C# para cadastro de funcionários, usando herança para
modelar diferentes tipos de funcionário com regras de remuneração distintas.

## Conceitos demonstrados

- **Herança mapeada no banco (EF Core TPH)**: `Funcionario` (abstrata) é a base de
  `Gerente`, `Desenvolvedor` e `Estagiario`, todos na mesma tabela `Funcionarios`,
  diferenciados pela coluna `TipoFuncionario`.
- **Polimorfismo**: `Cargo` e `CalcularSalarioTotal()` são abstratos na classe base e
  sobrescritos por cada subclasse com sua própria fórmula. A view `Index.cshtml` chama
  esses membros numa lista de `Funcionario` sem saber o tipo concreto de cada item.
- **ViewModel "achatado" + fábrica no controller**: como o model binding do ASP.NET Core
  MVC não liga diretamente numa classe abstrata, `FuncionarioFormViewModel` reúne os
  campos de todas as subclasses, e `FuncionariosController.MontarFuncionario()` decide
  qual subclasse concreta instanciar a partir do campo `Tipo`.
- **Pattern matching (`switch`)**: usado no controller e nas views (`Details.cshtml`)
  para acessar campos específicos de cada subclasse a partir da referência da classe base.
- **Validação condicional**: o campo `Instituicao` só é obrigatório quando `Tipo == Estagiario`;
  essa regra vive no controller (`ValidarCamposEspecificos`) porque depende de outro campo,
  algo que `DataAnnotations` sozinho não expressa bem.
- **Formulário dinâmico**: JavaScript simples mostra/esconde os campos específicos de
  cada tipo de funcionário conforme o `<select>` de Tipo muda.

## Tipos de funcionário e regra de remuneração

| Tipo          | Fórmula                                             |
|---------------|------------------------------------------------------|
| Gerente       | Salário base + Bônus + (Tamanho da equipe × R$200)   |
| Desenvolvedor | Salário base + adicional por senioridade + (Certificações × R$150) |
| Estagiário    | Salário base + Auxílio transporte                     |

## Estrutura

```
Models/         (Funcionario abstrata, Gerente, Desenvolvedor, Estagiario, enums)
ViewModels/      (FuncionarioFormViewModel)
Data/             (AppDbContext com configuração TPH)
Controllers/       (FuncionariosController: Index/Details/Create/Edit/Delete)
Views/Funcionarios/ (Index, Details, Create, Edit, Delete, _Form parcial)
Migrations/          (histórico de schema do EF Core)
```

## Como rodar

```bash
cd src/CadastroFuncionarios
dotnet run
```

As migrations são aplicadas automaticamente na inicialização, criando `funcionarios.db`
na primeira execução. A tela inicial (`/`) já abre na listagem de funcionários.
