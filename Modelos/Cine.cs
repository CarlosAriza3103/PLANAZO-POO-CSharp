namespace PlanazoPOO.Modelos;

/// <summary>Función de cine: mantiene el precio base.</summary>
public class Cine : Evento
{
    public override string Tipo => "Cine";

    public Cine(int id, string nombre, DateTime fecha, decimal precioBase, int capacidadMaxima)
        : base(id, nombre, fecha, precioBase, capacidadMaxima)
    {
    }

    public override decimal CalcularPrecioFinal() => PrecioBase;
}
