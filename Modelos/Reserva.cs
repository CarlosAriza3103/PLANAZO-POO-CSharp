namespace PlanazoPOO.Modelos;

/// <summary>
/// Asocia un usuario con un evento. Guarda la cantidad de entradas, el valor total
/// y el estado. Sabe cancelarse a sí misma devolviendo los cupos al evento.
/// </summary>
public class Reserva
{
    public int Id { get; }
    public Usuario Usuario { get; }
    public Evento Evento { get; }
    public int Cantidad { get; }
    public decimal ValorTotal { get; }
    public EstadoReserva Estado { get; private set; }

    public string NombrePersona => Usuario.Nombre;

    public Reserva(int id, Usuario usuario, Evento evento, int cantidad)
    {
        ArgumentNullException.ThrowIfNull(usuario);
        ArgumentNullException.ThrowIfNull(evento);
        if (cantidad <= 0)
            throw new ArgumentOutOfRangeException(nameof(cantidad), "La cantidad de entradas debe ser mayor que cero.");

        Id = id;
        Usuario = usuario;
        Evento = evento;
        Cantidad = cantidad;
        ValorTotal = evento.CalcularPrecioFinal() * cantidad;
        Estado = EstadoReserva.Activa;
    }

    public void Cancelar()
    {
        if (Estado == EstadoReserva.Cancelada)
            throw new InvalidOperationException($"La reserva {Id} ya fue cancelada anteriormente.");

        Evento.LiberarCupos(Cantidad);
        Estado = EstadoReserva.Cancelada;
    }

    public override string ToString() =>
        $"Reserva #{Id} | {NombrePersona,-15} | {Evento.Nombre,-28} | Entradas: {Cantidad,2} | " +
        $"Total: {Formato.Moneda(ValorTotal),10} | {Estado}";
}
