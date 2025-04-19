namespace HotelLami.Models
{
    using System.Collections.Generic;

    public class Hotel
    {
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public string Endereco { get; set; }
        public int QuantQuartos { get; set; }
        public List<Quarto> Quartos { get; set; }

        public Hotel(string nome, string cnpj, string endereco)
        {
            Nome = nome;
            Cnpj = cnpj;
            Endereco = endereco;
            Quartos = new List<Quarto>();
        }

        public void ExibirHotel()
        {
            Console.WriteLine($"Nome do Hotel: {Nome}");
            Console.WriteLine($"CNPJ: {Cnpj}");
            Console.WriteLine($"Endereço: {Endereco}");
            Console.WriteLine($"Quantidade de Quartos: {QuantQuartos}");
        }
    }
}
