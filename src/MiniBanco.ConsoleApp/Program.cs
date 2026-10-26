using System;
using System.Collections.Generic;
using System.Globalization;
using MiniBanco.ConsoleApp.Exceptions;
using MiniBanco.ConsoleApp.Models;
using MiniBanco.ConsoleApp.Services;

namespace MiniBanco.ConsoleApp
{
    internal class Program
    {
        private static readonly CultureInfo Cultura =
            CultureInfo.GetCultureInfo("pt-BR");

        private static void Main(string[] args)
        {
            Console.WriteLine("====================================================================");
            Console.WriteLine(" MiniBanco .NET");
            Console.WriteLine(" Motor de Gestão de Contas Bancárias");
            Console.WriteLine("====================================================================");

            List<Conta> contas = new List<Conta>();
            MotorAnalise motor = new MotorAnalise();

            bool executando = true;

            while (executando)
            {
                ExibirMenu();

                try
                {
                    int opcao = LerInteiro("Escolha uma opção: ");

                    switch (opcao)
                    {
                        case 1:
                            AbrirNovaConta(contas);
                            break;

                        case 2:
                            RegistrarMovimentacao(contas);
                            break;

                        case 3:
                            AplicarFechamentoMensal(contas);
                            break;

                        case 4:
                            ExibirRelatorios(contas, motor);
                            break;

                        case 0:
                            executando = false;
                            Console.WriteLine("Encerrando o MiniBanco.");
                            break;

                        default:
                            Console.WriteLine("Opção inválida.");
                            break;
                    }
                }
                catch (SaldoInsuficienteException ex)
                {
                    Console.WriteLine(
                        "Operação não realizada: " + ex.Message);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(
                        "Dados inválidos: " + ex.Message);
                }
                catch (FormatException)
                {
                    Console.WriteLine(
                        "Entrada inválida. Digite um número no formato solicitado.");
                }
                catch (OverflowException)
                {
                    Console.WriteLine(
                        "O número informado está fora do intervalo permitido.");
                }

                Console.WriteLine();
            }
        }

        private static void ExibirMenu()
        {
            Console.WriteLine();
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("1 - Abrir nova conta");
            Console.WriteLine("2 - Registrar movimentação");
            Console.WriteLine("3 - Aplicar fechamento mensal");
            Console.WriteLine("4 - Exibir relatório das contas");
            Console.WriteLine("0 - Sair");
            Console.WriteLine("----------------------------------------");
        }

        private static void AbrirNovaConta(List<Conta> contas)
        {
            Console.WriteLine();
            Console.WriteLine("TIPOS DE CONTA");
            Console.WriteLine("1 - Conta Corrente");
            Console.WriteLine("2 - Conta Poupança");
            Console.WriteLine("3 - Conta Salário");

            int tipo = LerInteiro("Tipo da conta: ");

            if (tipo < 1 || tipo > 3)
            {
                throw new ArgumentException(
                    "Selecione um tipo de conta entre 1 e 3.");
            }

            string titular = LerTexto("Nome do titular: ");
            int numero = LerInteiro("Número da conta: ");

            for (int i = 0; i < contas.Count; i++)
            {
                if (contas[i].Numero == numero)
                {
                    throw new ArgumentException(
                        "Já existe uma conta com esse número.");
                }
            }

            decimal saldoInicial =
                LerDecimal("Saldo inicial: R$ ");

            Conta novaConta;

            if (tipo == 1)
            {
                decimal limite = LerDecimal(
                    "Limite do cheque especial: R$ ");

                novaConta = new ContaCorrente(
                    titular,
                    numero,
                    saldoInicial,
                    limite);
            }
            else if (tipo == 2)
            {
                double taxa = LerDouble(
                    "Taxa de rendimento mensal (%): ");

                int dia = LerInteiro(
                    "Dia de aniversário da poupança: ");

                novaConta = new ContaPoupanca(
                    titular,
                    numero,
                    saldoInicial,
                    taxa,
                    dia);
            }
            else
            {
                string empregador =
                    LerTexto("Nome do empregador: ");

                int limiteSaques = LerInteiro(
                    "Limite de saques mensais: ");

                novaConta = new ContaSalario(
                    titular,
                    numero,
                    saldoInicial,
                    empregador,
                    limiteSaques);
            }

            contas.Add(novaConta);

            Console.WriteLine("Conta criada com sucesso!");
        }

        private static void RegistrarMovimentacao(
            List<Conta> contas)
        {
            if (contas.Count == 0)
            {
                Console.WriteLine(
                    "Nenhuma conta aberta até o momento.");
                return;
            }

            int numero = LerInteiro("Número da conta: ");

            int indiceConta = -1;

            for (int i = 0; i < contas.Count; i++)
            {
                if (contas[i].Numero == numero)
                {
                    indiceConta = i;
                    break;
                }
            }

            if (indiceConta == -1)
            {
                Console.WriteLine("Conta não encontrada.");
                return;
            }

            Conta conta = contas[indiceConta];

            Console.WriteLine("1 - Depósito");
            Console.WriteLine("2 - Saque");

            int operacao = LerInteiro("Operação: ");
            decimal valor = LerDecimal("Valor: R$ ");

            if (operacao == 1)
            {
                string descricao =
                    LerTexto("Descrição do depósito: ");

                conta.Depositar(valor, descricao);

                Console.WriteLine(
                    "Depósito realizado com sucesso.");
            }
            else if (operacao == 2)
            {
                conta.Sacar(valor);

                Console.WriteLine(
                    "Saque realizado com sucesso.");
            }
            else
            {
                throw new ArgumentException(
                    "Selecione depósito ou saque.");
            }
        }

        private static void AplicarFechamentoMensal(
            List<Conta> contas)
        {
            if (contas.Count == 0)
            {
                Console.WriteLine(
                    "Nenhuma conta aberta até o momento.");
                return;
            }

            foreach (Conta conta in contas)
            {
                conta.AplicarFechamentoMensal();
            }

            Console.WriteLine(
                "Fechamento mensal aplicado às contas.");
        }

        private static void ExibirRelatorios(
            List<Conta> contas,
            MotorAnalise motor)
        {
            if (contas.Count == 0)
            {
                Console.WriteLine(
                    "Nenhuma conta aberta até o momento.");
                return;
            }

            foreach (Conta conta in contas)
            {
                motor.ExibirRelatorio(conta);
            }
        }

        private static string LerTexto(string mensagem)
        {
            Console.Write(mensagem);
            return Console.ReadLine() ?? string.Empty;
        }

        private static int LerInteiro(string mensagem)
        {
            return int.Parse(
                LerTexto(mensagem),
                Cultura);
        }

        private static decimal LerDecimal(string mensagem)
        {
            return decimal.Parse(
                LerTexto(mensagem),
                Cultura);
        }

        private static double LerDouble(string mensagem)
        {
            return double.Parse(
                LerTexto(mensagem),
                Cultura);
        }
    }
}
