namespace PlanazoPOO.Modelos;

/// <summary>Reto 2: persona que realiza reservas.</summary>
public class Usuario
{
    private string _nombre = string.Empty;

    public int Id { get; }

    public string Nombre
    {
        get => _nombre;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("El nombre del usuario no puede estar vacío.");
            _nombre = value.Trim();
        }
    }

    public Usuario(int id, string nombre)
    {
        Id = id;
        Nombre = nombre;
    }

    public override string ToString() => $"Usuario {Id}: {Nombre}";
}
