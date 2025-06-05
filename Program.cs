using HotelLami.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}   

var clientes = new List<Cliente>
{
    new Cliente { Nome = "Camilli Frigeri Feliz", Cpf = "111.222.333-44", Idade = 21 },
    new Cliente { Nome = "Alana Cristina Martens", Cpf = "444.333.222-11", Idade = 18 }
};

var hotel1 = new Hotel("Hotel Transilvania", "12.345.678/0001-99", "Rua Imaginária, 111")
{
    QuantQuartos = 5,
    Quartos = new List<Quarto>
    {
        new Quarto { Numero = 1, Tipo = "Solteiro", Disponivel = true, PrecoDiaria = 95.00m },
        new Quarto { Numero = 2, Tipo = "Simples", Disponivel = true, PrecoDiaria = 150.00m },
        new Quarto { Numero = 3, Tipo = "Simples", Disponivel = true, PrecoDiaria = 150.00m },
        new Quarto { Numero = 4, Tipo = "Suíte", Disponivel = true, PrecoDiaria = 250.00m },
        new Quarto { Numero = 5, Tipo = "Suíte", Disponivel = true, PrecoDiaria = 250.00m }
    }
};

var hoteis = new List<Hotel> { hotel1 };
var reservas = new List<Reserva>();


app.MapGet("/clientes", () => Results.Ok(clientes));

app.MapGet("/clientes/{cpf}", (string cpf) =>
{
    var cliente = clientes.FirstOrDefault(c => c.Cpf == cpf);
    return cliente != null ? Results.Ok(cliente) : Results.NotFound("Cliente não encontrado.");
});

app.MapPost("/clientes", (Cliente c) =>
{
    clientes.Add(c);
    return Results.Created($"/clientes/{c.Cpf}", c);
});

app.MapPut("/clientes/{cpf}", (string cpf, Cliente c) =>
{
    var clienteExistente = clientes.FirstOrDefault(x => x.Cpf == cpf);
    if (clienteExistente == null) return Results.NotFound("Cliente não encontrado.");

    clienteExistente.Nome = c.Nome;
    clienteExistente.Idade = c.Idade;

    return Results.Ok(clienteExistente);
});

app.MapDelete("/clientes/{cpf}", (string cpf) =>
{
    var cliente = clientes.FirstOrDefault(c => c.Cpf == cpf);
    if (cliente == null) return Results.NotFound("Cliente não encontrado.");

    clientes.Remove(cliente);
    return Results.NoContent();
});


app.MapGet("/hoteis", () => Results.Ok(hoteis));

app.MapGet("/hoteis/{cnpj}", (string cnpj) =>
{
    var hotel = hoteis.FirstOrDefault(h => h.Cnpj == cnpj);
    return hotel != null ? Results.Ok(hotel) : Results.NotFound("Hotel não encontrado.");
});

app.MapPost("/hoteis", (Hotel h) =>
{
    hoteis.Add(h);
    return Results.Created($"/hoteis/{h.Cnpj}", h);
});

app.MapPut("/hoteis/{cnpj}", (string cnpj, Hotel h) =>
{
    var hotelExistente = hoteis.FirstOrDefault(x => x.Cnpj == cnpj);
    if (hotelExistente == null) return Results.NotFound("Hotel não encontrado.");

    hotelExistente.Nome = h.Nome;
    hotelExistente.Endereco = h.Endereco;
    hotelExistente.QuantQuartos = h.QuantQuartos;

    return Results.Ok(hotelExistente);
});

app.MapDelete("/hoteis/{cnpj}", (string cnpj) =>
{
    var hotel = hoteis.FirstOrDefault(h => h.Cnpj == cnpj);
    if (hotel == null) return Results.NotFound("Hotel não encontrado.");

    hoteis.Remove(hotel);
    return Results.NoContent();
});

app.MapGet("/reservas", () => Results.Ok(reservas));

app.MapGet("/reservas/{id}", (int id) =>
{
    var reserva = reservas.FirstOrDefault(r => r.Id == id);
    return reserva != null ? Results.Ok(reserva) : Results.NotFound("Reserva não encontrada.");
});

app.MapPost("/reservas", (Reserva r) =>
{
    var hotel = hoteis.Find(h => h.Nome == r.Hotel.Nome);
    if (hotel == null) return Results.NotFound("Hotel não encontrado.");

    var cliente = clientes.Find(c => c.Cpf == r.Cliente.Cpf);
    if (cliente == null) return Results.NotFound("Cliente não encontrado.");

    var quartoDisponivel = hotel.Quartos.FirstOrDefault(q => q.Numero == r.Quarto.Numero && q.Disponivel);
    if (quartoDisponivel == null) return Results.BadRequest("Quarto não disponível.");

    r.Id = reservas.Count > 0 ? reservas.Max(x => x.Id) + 1 : 1;
    r.Cliente = cliente;
    r.Hotel = hotel;
    r.Quarto = quartoDisponivel; 
    quartoDisponivel.Disponivel = false;

    reservas.Add(r);
    return Results.Created($"/reservas/{r.Id}", r);
});

app.MapPut("/reservas/{id}", (int id, Reserva r) =>
{
    var reservaExistente = reservas.FirstOrDefault(res => res.Id == id);
    if (reservaExistente == null) return Results.NotFound("Reserva não encontrada.");

    reservaExistente.Cliente = r.Cliente;
    reservaExistente.Hotel = r.Hotel;
    reservaExistente.Quarto = r.Quarto;

    return Results.Ok(reservaExistente);
});

app.MapDelete("/reservas/{id}", (int id) =>
{
    var reserva = reservas.FirstOrDefault(r => r.Id == id);
    if (reserva == null) return Results.NotFound("Reserva não encontrada.");

    var quarto = reserva.Hotel.Quartos.FirstOrDefault(q => q.Numero == reserva.Quarto.Numero);
    if (quarto != null) quarto.Disponivel = true;

    reservas.Remove(reserva);
    return Results.NoContent();
});


var clienteAlana = clientes.FirstOrDefault(c => c.Cpf == "444.333.222-11");
var hotelTransilvania = hoteis.FirstOrDefault(h => h.Nome == "Hotel Transilvania");
var quartoEscolhido = hotelTransilvania?.Quartos.FirstOrDefault(q => q.Numero == 2 && q.Disponivel);

if (clienteAlana != null && hotelTransilvania != null && quartoEscolhido != null)
{
    var novaReserva = new Reserva
    {
        Id = reservas.Count > 0 ? reservas.Max(r => r.Id) + 1 : 1,
        Cliente = clienteAlana,
        Hotel = hotelTransilvania,
        Quarto = quartoEscolhido,
        DataCheckIn = new DateTime(2025, 6, 1),
        DataCheckOut = new DateTime(2025, 6, 5)
    };

    quartoEscolhido.Disponivel = false;
    reservas.Add(novaReserva);
    novaReserva.ExibirReserva();
}
else
{
    Console.WriteLine("Erro ao criar reserva: cliente, hotel ou quarto não encontrado.");
}

app.Run();
