using System.Text.Json;
using HotelLami.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

string caminhoClientes = "clientes.json";
string caminhoHoteis = "hoteis.json";
string caminhoReservas = "reservas.json";

List<Cliente> clientes = File.Exists(caminhoClientes)
    ? JsonSerializer.Deserialize<List<Cliente>>(File.ReadAllText(caminhoClientes)) ?? new List<Cliente>()
    : new List<Cliente>();

List<Hotel> hoteis = File.Exists(caminhoHoteis)
    ? JsonSerializer.Deserialize<List<Hotel>>(File.ReadAllText(caminhoHoteis)) ?? new List<Hotel>()
    : new List<Hotel>();

List<Reserva> reservas = File.Exists(caminhoReservas)
    ? JsonSerializer.Deserialize<List<Reserva>>(File.ReadAllText(caminhoReservas)) ?? new List<Reserva>()
    : new List<Reserva>();

void SalvarDados()
{
    File.WriteAllText(caminhoClientes, JsonSerializer.Serialize(clientes, new JsonSerializerOptions { WriteIndented = true }));
    File.WriteAllText(caminhoHoteis, JsonSerializer.Serialize(hoteis, new JsonSerializerOptions { WriteIndented = true }));
    File.WriteAllText(caminhoReservas, JsonSerializer.Serialize(reservas, new JsonSerializerOptions { WriteIndented = true }));
}

app.MapGet("/clientes", () => Results.Ok(clientes));

app.MapGet("/clientes/{cpf}", (string cpf) =>
{
    var cliente = clientes.FirstOrDefault(c => c.Cpf == cpf);
    return cliente != null ? Results.Ok(cliente) : Results.NotFound("Cliente não encontrado.");
});

app.MapPost("/clientes", (Cliente c) =>
{
    clientes.Add(c);
    SalvarDados();
    return Results.Created($"/clientes/{c.Cpf}", c);
});

app.MapPut("/clientes/{cpf}", (string cpf, Cliente c) =>
{
    var clienteExistente = clientes.FirstOrDefault(x => x.Cpf == cpf);
    if (clienteExistente == null) return Results.NotFound("Cliente não encontrado.");

    clienteExistente.Nome = c.Nome;
    clienteExistente.Idade = c.Idade;
    SalvarDados();
    return Results.Ok(clienteExistente);
});

app.MapDelete("/clientes/{cpf}", (string cpf) =>
{
    var cliente = clientes.FirstOrDefault(c => c.Cpf == cpf);
    if (cliente == null) return Results.NotFound("Cliente não encontrado.");

    clientes.Remove(cliente);
    SalvarDados();
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
    SalvarDados();
    return Results.Created($"/hoteis/{h.Cnpj}", h);
});

app.MapPut("/hoteis/{cnpj}", (string cnpj, Hotel h) =>
{
    var hotelExistente = hoteis.FirstOrDefault(x => x.Cnpj == cnpj);
    if (hotelExistente == null) return Results.NotFound("Hotel não encontrado.");

    hotelExistente.Nome = h.Nome;
    hotelExistente.Endereco = h.Endereco;
    hotelExistente.QuantQuartos = h.QuantQuartos;
    hotelExistente.Quartos = h.Quartos;
    SalvarDados();
    return Results.Ok(hotelExistente);
});

app.MapDelete("/hoteis/{cnpj}", (string cnpj) =>
{
    var hotel = hoteis.FirstOrDefault(h => h.Cnpj == cnpj);
    if (hotel == null) return Results.NotFound("Hotel não encontrado.");

    hoteis.Remove(hotel);
    SalvarDados();
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
    SalvarDados();
    return Results.Created($"/reservas/{r.Id}", r);
});

app.MapPut("/reservas/{id}", (int id, Reserva r) =>
{
    var reservaExistente = reservas.FirstOrDefault(res => res.Id == id);
    if (reservaExistente == null) return Results.NotFound("Reserva não encontrada.");

    reservaExistente.Cliente = r.Cliente;
    reservaExistente.Hotel = r.Hotel;
    reservaExistente.Quarto = r.Quarto;
    SalvarDados();
    return Results.Ok(reservaExistente);
});

app.MapDelete("/reservas/{id}", (int id) =>
{
    var reserva = reservas.FirstOrDefault(r => r.Id == id);
    if (reserva == null) return Results.NotFound("Reserva não encontrada.");

    var quarto = reserva.Hotel.Quartos.FirstOrDefault(q => q.Numero == reserva.Quarto.Numero);
    if (quarto != null) quarto.Disponivel = true;

    reservas.Remove(reserva);
    SalvarDados();
    return Results.NoContent();
});

app.Run();