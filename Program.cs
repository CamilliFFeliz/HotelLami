using System;
using HotelLami.Models;

namespace HotelLamiApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Criação de Hotel
            Hotel hotel1 = new Hotel("Hotel Transilvania", "12.345.678/0001-99", "Rua Imaginária, 111");
            hotel1.QuantQuartos = 5;
            hotel1.Quartos.Add(new Quarto { Numero = 1, Tipo = "Solteiro", Disponivel = true, PrecoDiaria = 95.00m });
            hotel1.Quartos.Add(new Quarto { Numero = 2, Tipo = "Simples", Disponivel = true, PrecoDiaria = 150.00m });
            hotel1.Quartos.Add(new Quarto { Numero = 3, Tipo = "Simples", Disponivel = true, PrecoDiaria = 150.00m });
            hotel1.Quartos.Add(new Quarto { Numero = 4, Tipo = "Suíte", Disponivel = true, PrecoDiaria = 250.00m });
            hotel1.Quartos.Add(new Quarto { Numero = 5, Tipo = "Suíte", Disponivel = true, PrecoDiaria = 250.00m });

            hotel1.ExibirHotel();

            // Criação de Cliente
            Cliente c1 = new Cliente { Nome = "Camilli Frigeri Feliz", Cpf = "111.222.333-44", Idade = 21 };
            Cliente c2 = new Cliente { Nome = "Alana Cristina Martens", Cpf = "444.333.222-11", Idade = 18 };

            c1.ExibirCliente();
            c2.ExibirCliente();

            // Criação de Reserva
            Reserva reserva = new Reserva
            {
                Cliente = c1,
                Hotel = hotel1
            };

            Console.WriteLine("Digite a data de Check-in (formato: dd/mm/aaaa): ");
            reserva.DataCheckIn = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("Digite a data do Check-out (formato: dd/mm/aaaa): ");
            reserva.DataCheckOut = DateTime.Parse(Console.ReadLine());

            reserva.ExibirReserva();

            // Escolha de Quarto para a Reserva
            reserva.EscolherQuartoDisponivel();
            reserva.ExibirReserva();
        }
    }
}
