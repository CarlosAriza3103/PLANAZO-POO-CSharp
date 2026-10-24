using PlanazoPOO.Modelos;
using PlanazoPOO.Servicios;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var gestor = new GestorReservas();
gestor.CargarEventosIniciales();

bool salir = false;
while (!salir)
{
    MostrarMenu();
    string? opcion = Console.ReadLine();
    if (opcion is null) break; // fin de la entrada estándar

    Console.WriteLine();
    try
    {
        switch (opcion.Trim())
        {
            case "1": VerEventos(); break;
            case "2": ConsultarEvento(); break;
            case "3": ReservarEntradas(); break;
            case "4": VerReservas(); break;
            case "5": CancelarReserva(); break;
            case "6": VerResumen(); break;
            case "7": VerReservasDeUsuario(); break;
            case "0": salir = true; Console.WriteLine("¡Gracias por usar PLANAZO!"); break;
            default: Console.WriteLine("Opción no válida. Intente de nuevo."); break;
        }
    }
    catch (Exception ex) when (ex is ArgumentException or InvalidOperationException or KeyNotFoundException)
    {
        // Los errores de negocio se informan al usuario sin cerrar el programa.
        Console.WriteLine($"⚠ Error: {ex.Message}");
    }

    if (!salir)
    {
        Console.WriteLine("\nPresione ENTER para continuar...");
        if (Console.ReadLine() is null) break;
    }
}

void MostrarMenu()
{
    Console.WriteLine();
    Console.WriteLine("================================");
    Console.WriteLine("        PLANAZO - EVENTOS");
    Console.WriteLine("================================");
    Console.WriteLine();
    Console.WriteLine("1. Ver eventos disponibles");
    Console.WriteLine("2. Consultar evento");
    Console.WriteLine("3. Reservar entradas");
    Console.WriteLine("4. Ver reservas realizadas");
    Console.WriteLine("5. Cancelar una reserva");
    Console.WriteLine("6. Ver resumen de reservas");
    Console.WriteLine("7. Ver reservas de un usuario");
    Console.WriteLine("0. Salir");
    Console.WriteLine();
    Console.Write("Seleccione una opción: ");
}

int LeerEntero(string mensaje)
{
    Console.Write(mensaje);
    string? texto = Console.ReadLine();
    if (!int.TryParse(texto, out int valor))
        throw new ArgumentException("Debe ingresar un número entero válido.");
    return valor;
}

void VerEventos()
{
    Console.WriteLine("--- EVENTOS DISPONIBLES ---");
    foreach (var evento in gestor.Eventos)
        Console.WriteLine(evento);
}

void ConsultarEvento()
{
    int id = LeerEntero("Ingrese el id del evento: ");
    var e = gestor.ConsultarEvento(id);
    Console.WriteLine($"Tipo:            {e.Tipo}");
    Console.WriteLine($"Nombre:          {e.Nombre}");
    Console.WriteLine($"Fecha:           {e.Fecha:dd/MM/yyyy HH:mm}");
    Console.WriteLine($"Precio base:     {Formato.Moneda(e.PrecioBase)}");
    Console.WriteLine($"Precio final:    {Formato.Moneda(e.CalcularPrecioFinal())}");
    Console.WriteLine($"Capacidad:       {e.CapacidadMaxima}");
    Console.WriteLine($"Cupos libres:    {e.CuposDisponibles}");
}

void ReservarEntradas()
{
    int id = LeerEntero("Id del evento: ");
    Console.Write("Nombre de la persona: ");
    string nombre = Console.ReadLine() ?? string.Empty;
    int cantidad = LeerEntero("Cantidad de entradas: ");

    var reserva = gestor.Reservar(id, nombre, cantidad);
    Console.WriteLine("✔ Reserva registrada correctamente:");
    Console.WriteLine(reserva);
}

void VerReservas()
{
    Console.WriteLine("--- RESERVAS REALIZADAS ---");
    if (gestor.Reservas.Count == 0)
    {
        Console.WriteLine("Aún no hay reservas registradas.");
        return;
    }
    foreach (var r in gestor.Reservas)
        Console.WriteLine(r);
}

void CancelarReserva()
{
    int id = LeerEntero("Id de la reserva a cancelar: ");
    var r = gestor.CancelarReserva(id);
    Console.WriteLine($"✔ Reserva #{r.Id} cancelada. Se devolvieron {r.Cantidad} cupos a \"{r.Evento.Nombre}\" " +
                      $"(cupos disponibles ahora: {r.Evento.CuposDisponibles}).");
}

void VerResumen()
{
    var (activas, entradas, total) = gestor.ObtenerResumen();
    Console.WriteLine("--- RESUMEN DE RESERVAS ACTIVAS ---");
    Console.WriteLine($"Reservas activas:    {activas}");
    Console.WriteLine($"Entradas reservadas: {entradas}");
    Console.WriteLine($"Valor total:         {Formato.Moneda(total)}");
}

void VerReservasDeUsuario()
{
    Console.Write("Nombre del usuario: ");
    string nombre = Console.ReadLine() ?? string.Empty;
    if (string.IsNullOrWhiteSpace(nombre))
        throw new ArgumentException("El nombre no puede estar vacío.");

    var lista = gestor.ReservasDeUsuario(nombre).ToList();
    Console.WriteLine($"--- RESERVAS DE {nombre.Trim().ToUpper()} ---");
    if (lista.Count == 0)
        Console.WriteLine("Este usuario no tiene reservas.");
    foreach (var r in lista)
        Console.WriteLine(r);
}
