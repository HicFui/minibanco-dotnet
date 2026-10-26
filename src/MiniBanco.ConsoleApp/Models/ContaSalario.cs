using System;
using MiniBanco.ConsoleApp.Exceptions;

namespace MiniBanco.ConsoleApp.Models
{
    public class ContaSalario : Conta
    {
        public string Empregador { get; }
        public int LimiteSaquesMensais { get; }

        private int saquesRealizadosNoMes = 0;

        public ContaSalario(
            string titular,
            int numero,
            decimal saldoInicial,
            string empregador,
            int limiteSaquesMensais)
            : base(titular, numero, saldoInicial)
        {
            if (string.IsNullOrWhiteSpace(empregador))
            {
                throw new ArgumentException(
                    "O empregador não pode ser vazio.");
            }

            if (limiteSaquesMensais < 0)
            {
                throw new ArgumentException(
                    "O limite de saques não pode ser negativo.");
            }

            this.Empregador = empregador;
            this.LimiteSaquesMensais = limiteSaquesMensais;
        }

        public override void Sacar(decimal valor)
        {
            if (valor <= 0)
            {
                throw new ArgumentException(
                    "O valor do saque deve ser maior que zero.");
            }

            bool cobrarTarifa =
                this.saquesRealizadosNoMes >= this.LimiteSaquesMensais;

            decimal valorTotal = valor;

            if (cobrarTarifa)
            {
                valorTotal += 5m;
            }

            if (valorTotal > this.Saldo)
            {
                throw new SaldoInsuficienteException(
                    "Saldo insuficiente para o saque e eventual tarifa.");
            }

            this.RegistrarTransacao("Saque", "Saque", valor);
            this.Saldo -= valor;

            this.saquesRealizadosNoMes++;

            if (cobrarTarifa)
            {
                this.RegistrarTransacao(
                    "Tarifa por saque excedente",
                    "Tarifa",
                    5m);

                this.Saldo -= 5m;
            }
        }

        public override void AplicarFechamentoMensal()
        {
            this.saquesRealizadosNoMes = 0;
        }
    }
}