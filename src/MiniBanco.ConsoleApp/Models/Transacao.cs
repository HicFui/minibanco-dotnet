namespace MiniBanco.ConsoleApp.Models

{
    public class Transacao

    {
        public string Descricao { get; }
        public string Tipo { get; }
        public decimal Valor { get; }

        public Transacao(string descricao, string tipo, decimal valor)
        {
            if (string.IsNullOrWhiteSpace(descricao))
            {
                throw new ArgumentException(
                    "A descrição não pode ser vazia.",
                    nameof(descricao));
            }
            if (tipo != "Deposito" &&
                tipo != "Saque" &&
                tipo != "Tarifa")
            {
                throw new ArgumentException(
                    "Tipo inválido. Use Deposito, Saque ou Tarifa.",
                    nameof(tipo));
            }
            if (valor <= 0)
            {
                throw new ArgumentException(
                    "O valor deve ser maior que zero.",
                    nameof(valor));
            }

            this.Descricao = descricao;
            this.Tipo = tipo;
            this.Valor = valor;
        }
    }
}