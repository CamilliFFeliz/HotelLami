namespace HotelLami.Models
{
    public class Cliente
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public int Idade { get; set; }

        public void ExibirCliente()
        {
            Console.WriteLine("Dados do Cliente");
            Console.WriteLine($"Nome: {Nome}");
            Console.WriteLine($"Cpf: {Cpf}");
            Console.WriteLine($"Idade: {Idade}");
        }
    }
}
    