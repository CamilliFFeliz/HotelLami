namespace HotelLami.Models
{
    using System;

    public class Reserva
    {
        public Cliente Cliente { get; set; }
        public Hotel Hotel { get; set; }
        public Quarto Quarto { get; set; }
        public DateTime DataCheckIn { get; set; }
        public DateTime DataCheckOut { get; set; }

        public void ExibirReserva()
        {
            if (Quarto != null)
            {
                Console.WriteLine("\n==== Detalhes da Reserva ====");
                Console.WriteLine($"Cliente: {Cliente.Nome}");
                Console.WriteLine($"Hotel: {Hotel.Nome}");
                Console.WriteLine($"Quarto: {Quarto.Numero} - {Quarto.Tipo}");
                Console.WriteLine($"Check-in: {DataCheckIn:dd/MM/yyyy}");
                Console.WriteLine($"Check-out: {DataCheckOut:dd/MM/yyyy}");
                Console.WriteLine($"Total de Diárias: {(DataCheckOut - DataCheckIn).Days}");
                Console.WriteLine($"Valor Total: R$ {(DataCheckOut - DataCheckIn).Days * Quarto.PrecoDiaria}");
            }
            else
            {
                Console.WriteLine("Nenhum quarto foi reservado.");
            }
        }

        public void EscolherQuartoDisponivel()
        {
            Console.WriteLine("\n---- Quartos Disponíveis ----");
            foreach (var quarto in Hotel.Quartos)
            {
                if (quarto.Disponivel)
                {
                    quarto.ExibirQuarto();
                }
            }

            int numeroEscolhido;
            bool valido = false;

            do
            {
                Console.Write("\nDigite o número do quarto que você deseja: ");
                string entrada = Console.ReadLine();

                if (int.TryParse(entrada, out numeroEscolhido))
                {
                    foreach (var quarto in Hotel.Quartos)
                    {
                        if (quarto.Numero == numeroEscolhido && quarto.Disponivel)
                        {
                            Quarto = quarto;
                            quarto.Disponivel = false;
                            Console.WriteLine("Quarto reservado com sucesso!");
                            valido = true;
                            break;
                        }
                    }

                    if (!valido)
                    {
                        Console.WriteLine("Quarto não encontrado ou indisponível. Tente novamente.");
                    }
                }
                else
                {
                    Console.WriteLine("Entrada inválida. Digite um número válido.");
                }

            } while (!valido);
        }
    }
}
