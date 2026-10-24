using PlanazoPOO.Modelos;

namespace PlanazoPOO.Servicios;

/// <summary>
/// Gestor: contiene las colecciones (List&lt;T&gt;) de eventos, usuarios y reservas
/// y concentra la lógica de negocio. Program.cs solo se encarga del menú.
/// </summary>
public class GestorReservas
{
    private readonly List<Evento> _eventos = new();
    private readonly List<Reserva> _reservas = new();
    private readonly List<Usuario> _usuarios = new();

    private int _siguienteIdReserva = 1;
    private int _siguienteIdUsuario = 1;

    // Se exponen como solo lectura para proteger las listas internas.
    public IReadOnlyList<Evento> Eventos => _eventos;
    public IReadOnlyList<Reserva> Reservas => _reservas;
    public IReadOnlyList<Usuario> Usuarios => _usuarios;

    public void RegistrarEvento(Evento evento)
    {
        ArgumentNullException.ThrowIfNull(evento);
        if (_eventos.Any(e => e.Id == evento.Id))
            throw new InvalidOperationException($"Ya existe un evento con el id {evento.Id}.");
        _eventos.Add(evento);
    }

    public Evento ConsultarEvento(int id) =>
        _eventos.FirstOrDefault(e => e.Id == id)
        ?? throw new KeyNotFoundException($"No existe ningún evento con el id {id}.");

    public Usuario ObtenerOCrearUsuario(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre de la persona no puede estar vacío.");

        var existente = _usuarios.FirstOrDefault(
            u => u.Nombre.Equals(nombre.Trim(), StringComparison.OrdinalIgnoreCase));
        if (existente != null) return existente;

        var nuevo = new Usuario(_siguienteIdUsuario++, nombre);
        _usuarios.Add(nuevo);
        return nuevo;
    }

    public Reserva Reservar(int idEvento, string nombrePersona, int cantidad)
    {
        // Se valida todo ANTES de modificar cualquier dato.
        if (cantidad <= 0)
            throw new ArgumentOutOfRangeException(nameof(cantidad), "La cantidad de entradas debe ser mayor que cero.");

        var evento = ConsultarEvento(idEvento);
        if (cantidad > evento.CuposDisponibles)
            throw new InvalidOperationException(
                $"No hay cupos suficientes. Disponibles: {evento.CuposDisponibles}, solicitados: {cantidad}.");

        var usuario = ObtenerOCrearUsuario(nombrePersona);

        evento.ReservarCupos(cantidad);
        var reserva = new Reserva(_siguienteIdReserva++, usuario, evento, cantidad);
        _reservas.Add(reserva);
        return reserva;
    }

    public Reserva CancelarReserva(int idReserva)
    {
        var reserva = _reservas.FirstOrDefault(r => r.Id == idReserva)
            ?? throw new KeyNotFoundException($"No existe ninguna reserva con el id {idReserva}.");
        reserva.Cancelar();
        return reserva;
    }

    public IEnumerable<Reserva> ReservasDeUsuario(string nombre) =>
        _reservas.Where(r => r.NombrePersona.Equals(nombre.Trim(), StringComparison.OrdinalIgnoreCase));

    public (int ReservasActivas, int EntradasReservadas, decimal ValorTotal) ObtenerResumen()
    {
        var activas = _reservas.Where(r => r.Estado == EstadoReserva.Activa).ToList();
        return (activas.Count, activas.Sum(r => r.Cantidad), activas.Sum(r => r.ValorTotal));
    }

    /// <summary>Carga los eventos iniciales: 2 películas, 2 conciertos, 2 talleres y 1 festival.</summary>
    public void CargarEventosIniciales()
    {
        RegistrarEvento(new Cine(1, "Dune: Parte Tres", new DateTime(2026, 11, 5, 19, 30, 0), 18_000m, 50));
        RegistrarEvento(new Cine(2, "Toy Story 5", new DateTime(2026, 11, 6, 16, 0, 0), 15_000m, 40));
        RegistrarEvento(new Concierto(3, "Concierto Carlos Vives", new DateTime(2026, 11, 14, 20, 0, 0), 50_000m, 100));
        RegistrarEvento(new Concierto(4, "Noche de Rock Nacional", new DateTime(2026, 11, 21, 21, 0, 0), 40_000m, 80));
        RegistrarEvento(new Taller(5, "Taller de Fotografía", new DateTime(2026, 11, 8, 9, 0, 0), 60_000m, 20));
        RegistrarEvento(new Taller(6, "Taller de Cocina Caribeña", new DateTime(2026, 11, 9, 15, 0, 0), 80_000m, 15));
        RegistrarEvento(new Festival(7, "Festival de Verano", new DateTime(2026, 12, 5, 14, 0, 0), 70_000m, 200));
    }
}
