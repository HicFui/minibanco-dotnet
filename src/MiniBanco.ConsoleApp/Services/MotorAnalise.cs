using System;
using System.Globalization;
using MiniBanco.ConsoleApp.Models;

namespace MiniBanco.ConsoleApp.Services
{
    public class MotorAnalise
    {
        private readonly CultureInfo cultura =
            CultureInfo.GetCultureInfo("pt-BR");

        public void ExibirRelatorio(Conta conta)
        {
            int pontuacaoObtida = 0;
            int totalTransacoes = 0;
            bool possuiTarifa = false;
            bool possuiSaque = false;

            foreach (Transacao transacao in conta.Extrato)
            {
                totalTransacoes++;

                pontuacaoObtida +=
                    ObterPontuacao(transacao.Tipo);

                if (transacao.Tipo == "Tarifa")
                {
                    possuiTarifa = true;
                }

                if (transacao.Tipo == "Saque")
                {
                    possuiSaque = true;
                }
            }

            int pontuacaoMaxima = totalTransacoes * 10;
            double indiceSaude = 0;

            if (pontuacaoMaxima > 0)
            {
                indiceSaude =
                    (double)pontuacaoObtida /
                    pontuacaoMaxima * 100;
            }

            string tipoConta = "Conta";
            string atributoEspecifico = "";

            if (conta is ContaCorrente corrente)
            {
                tipoConta = "Conta Corrente";
                atributoEspecifico =
                    "Limite cheque especial: " +
                    corrente.LimiteChequeEspecial.ToString(
                        "C2", cultura);
            }
            else if (conta is ContaPoupanca poupanca)
            {
                tipoConta = "Conta Poupanca";
                atributoEspecifico =
                    "Rendimento: " +
                    poupanca.TaxaRendimentoMensal.ToString(
                        "0.##", cultura) +
                    "% ao mes | Aniversario: dia " +
                    poupanca.DiaAniversario;
            }
            else if (conta is ContaSalario salario)
            {
                tipoConta = "Conta Salario";
                atributoEspecifico =
                    "Empregador: " + salario.Empregador +
                    " | Limite de saques: " +
                    salario.LimiteSaquesMensais;
            }

            string classificacao;

            if (indiceSaude >= 90)
            {
                classificacao = "CONTA SAUDAVEL";
            }
            else if (indiceSaude >= 60)
            {
                classificacao = "CONTA EM ATENCAO";
            }
            else
            {
                classificacao = "CONTA EM RISCO";
            }

            Console.WriteLine();
            Console.WriteLine("==================================================");
            Console.WriteLine("          RELATORIO DE ANALISE DA CONTA");
            Console.WriteLine("==================================================");

            Console.WriteLine();
            Console.WriteLine("> DADOS DA CONTA");
            Console.WriteLine("Tipo: " + tipoConta);
            Console.WriteLine("Titular: " + conta.Titular);
            Console.WriteLine("Numero: " + conta.Numero);
            Console.WriteLine(atributoEspecifico);
            Console.WriteLine(
                "Saldo atual: " +
                conta.Saldo.ToString("C2", cultura));

            Console.WriteLine();
            Console.WriteLine(
                "> MOVIMENTACOES (" +
                totalTransacoes + ")");

            if (totalTransacoes == 0)
            {
                Console.WriteLine("Nenhuma transacao registrada.");
            }

            foreach (Transacao transacao in conta.Extrato)
            {
                int pontos = ObterPontuacao(transacao.Tipo);

                Console.WriteLine(
                    "- " + transacao.Descricao +
                    " | " + transacao.Tipo +
                    " | " + transacao.Valor.ToString("C2", cultura) +
                    " | " + pontos + " pontos");
            }

            Console.WriteLine();
            Console.WriteLine("> RESUMO DA PONTUACAO");
            Console.WriteLine(
                "Pontuacao obtida: " +
                pontuacaoObtida + " de " +
                pontuacaoMaxima);

            Console.WriteLine(
                "Indice de saude: " +
                indiceSaude.ToString("F1", cultura) + "%");

            Console.WriteLine(
                "Classificacao: " + classificacao);

            Console.WriteLine();
            Console.WriteLine("> ITENS CRITICOS: TARIFAS");

            if (!possuiTarifa)
            {
                Console.WriteLine("Nenhuma tarifa registrada.");
            }
            else
            {
                foreach (Transacao transacao in conta.Extrato)
                {
                    if (transacao.Tipo == "Tarifa")
                    {
                        Console.WriteLine(
                            "- " + transacao.Descricao +
                            ": " + transacao.Valor.ToString(
                                "C2", cultura) +
                            ". Avaliar revisao ou negociacao.");
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine("> ITENS DE ATENCAO: SAQUES");

            if (!possuiSaque)
            {
                Console.WriteLine("Nenhum saque registrado.");
            }
            else
            {
                foreach (Transacao transacao in conta.Extrato)
                {
                    if (transacao.Tipo == "Saque")
                    {
                        Console.WriteLine(
                            "- Saque de " +
                            transacao.Valor.ToString("C2", cultura) +
                            ": acompanhar movimentacao do cliente.");
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine("> RECOMENDACOES PARA A GERENCIA");

            if (possuiTarifa)
            {
                Console.WriteLine(
                    "- Revisar tarifas e avaliar outro pacote de servicos.");
            }

            if (possuiSaque)
            {
                Console.WriteLine(
                    "- Acompanhar os saques e contatar o cliente se necessario.");
            }

            if (indiceSaude >= 90)
            {
                Console.WriteLine(
                    "- Oferecer investimentos e avaliar aumento de limite.");
            }
            else if (indiceSaude >= 60)
            {
                Console.WriteLine(
                    "- Oferecer orientacao financeira e revisar tarifas.");
            }
            else
            {
                Console.WriteLine(
                    "- Nao aumentar o limite e encaminhar para analise de credito.");
            }

            Console.WriteLine("==================================================");
        }

        private int ObterPontuacao(string tipo)
        {
            if (tipo == "Deposito")
            {
                return 10;
            }
            else if (tipo == "Saque")
            {
                return 5;
            }
            else if (tipo == "Tarifa")
            {
                return 0;
            }

            return 0;
        }
    }
}