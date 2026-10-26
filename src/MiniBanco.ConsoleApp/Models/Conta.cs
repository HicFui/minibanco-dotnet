using System;
using System.Collections.Generic;
using MiniBanco.ConsoleApp.Exceptions;

namespace MiniBanco.ConsoleApp.Models
{
    public abstract class Conta
    {
        public string Titular { get; }
        public int Numero { get; }
        public decimal Saldo { get; protected set; }
        public List<Transacao> Extrato { get; }

        public Conta(string titular, int numero, decimal saldoInicial)
        {
            if (string.IsNullOrWhiteSpace(titular))
            {
                throw new ArgumentException(
                    "O titular não pode ser vazio.");
            }

            if (numero <= 0)
            {
                throw new ArgumentException(
                    "O número da conta deve ser positivo.");
            }

            this.Titular = titular;
            this.Numero = numero;
            this.Saldo = saldoInicial;
            this.Extrato = new List<Transacao>();
        }

        public void RegistrarTransacao(
            string descricao,
            string tipo,
            decimal valor)
        {
            Transacao transacao = new Transacao(
                descricao,
                tipo,
                valor);

            this.Extrato.Add(transacao);
        }

        public void Depositar(decimal valor, string descricao)
        {
            this.RegistrarTransacao(
                descricao,
                "Deposito",
                valor);

            this.Saldo += valor;
        }

        public virtual void Sacar(decimal valor)
        {
            if (valor <= 0)
            {
                throw new ArgumentException(
                    "O valor do saque deve ser maior que zero.");
            }

            if (valor > this.Saldo)
            {
                throw new SaldoInsuficienteException(
                    "Saldo insuficiente para realizar o saque.");
            }

            this.RegistrarTransacao(
                "Saque",
                "Saque",
                valor);

            this.Saldo -= valor;
        }

        public virtual void AplicarFechamentoMensal()
        {
        }
    }
}