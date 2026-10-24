namespace PlanazoPOO.Modelos;

/// <summary>
/// Clase abstracta que agrupa lo común a cualquier evento de PLANAZO.
/// Cada tipo de evento define su propia regla de precio (polimorfismo).
/// </summary>
public abstract class Evento
{
    // Encapsulamiento: los campos son privados y solo se exponen con propiedades.
    private string _nombre = string.Empty;
    private decimal _precioBase;
    private int _capacidadMaxima;
    private int _cuposDisponibles;

    public int Id { get; }

    public string Nombre
    {
        get => _nombre;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("El nombre del evento no puede estar vacío.");
            _nombre = value.Trim();
        }
    }

    public DateTime Fecha { get; }

    public decimal PrecioBase
    {
        get => _precioBase;
        private set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(PrecioBase), "El precio base no puede ser negativo.");
            _precioBase = value;
        }
    }

    public int CapacidadMaxima
    {
        get => _capacidadMaxima;
        private set
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(CapacidadMaxima), "La capacidad debe ser mayor que cero.");
            _capacidadMaxima = value;
        }
    }

    // Solo se puede modificar a través de ReservarCupos y LiberarCupos.
    public int CuposDisponibles => _cuposDisponibles;

    /// <summary>Nombre del tipo de evento (Cine, Concierto, Taller...).</summary>
    public abstract string Tipo { get; }

    protected Evento(int id, string nombre, DateTime fecha, decimal precioBase, int capacidadMaxima)
    {
        Id = id;
        Nombre = nombre;
        Fecha = fecha;
        PrecioBase = precioBase;
        CapacidadMaxima = capacidadMaxima;
        _cuposDisponibles = capacidadMaxima;
    }

    /// <summary>Método abstracto: cada clase hija implementa su propia regla de precio.</summary>
    public abstract decimal CalcularPrecioFinal();

    public bool TieneCupos(int cantidad) => cantidad > 0 && cantidad <= _cuposDisponibles;

    public void ReservarCupos(int cantidad)
    {
        if (cantidad <= 0)
            throw new ArgumentOutOfRangeException(nameof(cantidad), "La cantidad debe ser mayor que cero.");
        if (cantidad > _cuposDisponibles)
            throw new InvalidOperationException(
                $"No hay cupos suficientes. Disponibles: {_cuposDisponibles}, solicitados: {cantidad}.");
        _cuposDisponibles -= cantidad;
    }

    public void LiberarCupos(int cantidad)
    {
        if (cantidad <= 0)
            throw new ArgumentOutOfRangeException(nameof(cantidad), "La cantidad debe ser mayor que cero.");
        if (_cuposDisponibles + cantidad > _capacidadMaxima)
            throw new InvalidOperationException("No se pueden devolver más cupos que la capacidad máxima.");
        _cuposDisponibles += cantidad;
    }

    public override string ToString() =>
        $"[{Id}] {Tipo,-9} | {Nombre,-28} | {Fecha:dd/MM/yyyy HH:mm} | " +
        $"Precio final: {Formato.Moneda(CalcularPrecioFinal()),9} | Cupos: {_cuposDisponibles}/{_capacidadMaxima}";
}
