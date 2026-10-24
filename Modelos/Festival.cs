namespace PlanazoPOO.Modelos;

/// <summary>
/// Reto 1: nuevo tipo de evento. Regla propia: 25 % de recargo por servicios
/// del festival más un cargo fijo de $10.000 por seguridad.
/// Se agregó sin modificar Cine, Concierto, Taller ni Evento.
/// </summary>
public class Festival : Evento
{
    private const decimal Recargo = 0.25m;
    private const decimal CargoSeguridad = 10_000m;

    public override string Tipo => "Festival";

    public Festival(int id, string nombre, DateTime fecha, decimal precioBase, int capacidadMaxima)
        : base(id, nombre, fecha, precioBase, capacidadMaxima)
    {
    }

    public override decimal CalcularPrecioFinal() => PrecioBase * (1 + Recargo) + CargoSeguridad;
}
