using System;

namespace MiniBanco.ConsoleApp.Models
{
    public class ContaPoupanca : Conta
    {
        public double TaxaRendimentoMensal { get; }
        public int DiaAniversario { get; }

        public ContaPoupanca(
            string titular,
            int numero,
            decimal saldoInicial,
            double taxaRendimentoMensal,
            int diaAniversario)
            : base(titular, numero, saldoInicial)
        {
            if (taxaRendimentoMensal < 0)
            {
                throw new ArgumentException(
                    "A taxa de rendimento não pode ser negativa.");
            }

            if (diaAniversario < 1 || diaAniversario > 31)
            {
                throw new ArgumentException(
                    "O dia de aniversário deve estar entre 1 e 31.");
            }

            this.TaxaRendimentoMensal = taxaRendimentoMensal;
            this.DiaAniversario = diaAniversario;
        }

        public override void AplicarFechamentoMensal()
        {
            if (this.Saldo > 0 && this.TaxaRendimentoMensal > 0)
            {
                decimal rendimento =
                    this.Saldo *
                    (decimal)this.TaxaRendimentoMensal / 100m;

                if (rendimento > 0)
                {
                    this.RegistrarTransacao(
                        "Rendimento mensal",
                        "Deposito",
                        rendimento);

                    this.Saldo += rendimento;
                }
            }
        }
    }
}