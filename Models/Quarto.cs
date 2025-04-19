namespace HotelLami.Models
{
    public class Quarto
    {
        public int Numero { get; set; }
        public string Tipo { get; set; }
        public bool Disponivel { get; set; }
        public decimal PrecoDiaria { get; set; }

        public void ExibirQuarto()
        {
            string status = Disponivel ? "Disponível" : "Indisponível";
            Console.WriteLine($"Número do Quarto: {Numero}, Tipo: {Tipo}, Status: {status}, Preço por diária: R$ {PrecoDiaria}");
        }
    }
}
