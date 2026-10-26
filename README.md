# MiniBanco .NET — Motor de Gestão de Contas Bancárias

## Sobre o projeto

O MiniBanco .NET é uma aplicação de console desenvolvida em C# com o objetivo de simular operações bancárias e analisar a saúde financeira das contas por meio de um sistema de pontuação.

O projeto aplica conceitos de Programação Orientada a Objetos (POO), como herança, encapsulamento, polimorfismo e tratamento de exceções.

## Funcionalidades

- Abertura de contas correntes, poupanças e contas salário.
- Depósitos e saques.
- Controle de saldo e limite de cheque especial.
- Aplicação de tarifas e fechamento mensal.
- Cálculo de rendimento mensal da poupança.
- Controle do limite mensal de saques da conta salário.
- Registro de movimentações no extrato.
- Relatórios com pontuação, índice de saúde financeira, classificação e recomendações.
- Tratamento de entradas inválidas e saldo insuficiente.

## Tipos de conta

- **Conta Corrente:** permite utilizar o limite de cheque especial e aplica uma tarifa de manutenção no fechamento mensal.
- **Conta Poupança:** permite configurar a taxa de rendimento mensal e o dia de aniversário.
- **Conta Salário:** possui empregador e limite mensal de saques, com cobrança de tarifa por saques excedentes.

## Sistema de análise

Cada movimentação recebe uma pontuação conforme seu tipo:

| Movimentação | Pontuação |
|---|---:|
| Depósito | 10 pontos |
| Saque | 5 pontos |
| Tarifa | 0 pontos |

O índice de saúde é calculado pela relação entre a pontuação obtida e a pontuação máxima possível para as movimentações registradas.

A classificação apresentada no relatório segue estas faixas:

- **Conta saudável:** índice igual ou superior a 90%.
- **Conta em atenção:** índice igual ou superior a 60% e inferior a 90%.
- **Conta em risco:** índice inferior a 60%.

O relatório também identifica tarifas e saques, apresentando recomendações de acompanhamento.

## Tecnologias utilizadas

- C#
- .NET 10
- Aplicação de console
- Git e GitHub para controle de versão

## Organização do projeto

```text
minibanco-dotnet/
├── src/
│   └── MiniBanco.ConsoleApp/
│       ├── Exceptions/
│       │   └── SaldoInsuficienteException.cs
│       ├── Models/
│       │   ├── Conta.cs
│       │   ├── ContaCorrente.cs
│       │   ├── ContaPoupanca.cs
│       │   ├── ContaSalario.cs
│       │   └── Transacao.cs
│       ├── Services/
│       │   └── MotorAnalise.cs
│       ├── Program.cs
│       └── MiniBanco.ConsoleApp.csproj
├── .gitignore
└── README.md
```

## Como executar

É necessário ter o SDK do .NET 10 instalado.

Na pasta raiz do repositório, execute:

```bash
dotnet run --project src/MiniBanco.ConsoleApp
```

O menu permite abrir contas, registrar movimentações, aplicar o fechamento mensal e consultar relatórios.

## Validações realizadas

Foram realizados testes manuais da aplicação, incluindo:

- Criação das três modalidades de conta.
- Depósitos e saques.
- Geração dos relatórios e cálculo da pontuação.
- Fechamento mensal e rendimento da poupança.
- Cobrança de tarifa por saques excedentes na conta salário.
- Recusa de saque com saldo insuficiente.

## Objetivo acadêmico

Projeto desenvolvido para praticar C#, Programação Orientada a Objetos, organização de código, tratamento de exceções e controle de versão com Git.
