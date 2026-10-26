using System;
using MiniBanco.ConsoleApp.Exceptions;

namespace MiniBanco.ConsoleApp.Models
{
    public class ContaCorrente : Conta
    {
        public decimal LimiteChequeEspecial { get; }

        public ContaCorrente(
            string titular,
            int numero,
            decimal saldoInicial,
            decimal limiteChequeEspecial)
            : base(titular, numero, saldoInicial)
        {
            if (limiteChequeEspecial < 0)
            {
                throw new ArgumentException(
                    "O limite do cheque especial não pode ser negativo.");
            }

            this.LimiteChequeEspecial = limiteChequeEspecial;
        }

        public override void Sacar(decimal valor)
        {
            if (valor <= 0)
            {
                throw new ArgumentException(
                    "O valor do saque deve ser maior que zero.");
            }

            if (valor > this.Saldo + this.LimiteChequeEspecial)
            {
                throw new SaldoInsuficienteException(
                    "Saldo e limite do cheque especial insuficientes.");
            }

            this.RegistrarTransacao("Saque", "Saque", valor);
            this.Saldo -= valor;
        }

        public override void AplicarFechamentoMensal()
        {
            this.RegistrarTransacao(
                "Tarifa de manutenção",
                "Tarifa",
                10m);

            this.Saldo -= 10m;
        }
    }
}