using System.Globalization;

namespace PlanazoPOO.Modelos;

/// <summary>Utilidad para mostrar valores monetarios con el formato colombiano ($57.500).</summary>
public static class Formato
{
    private static readonly CultureInfo Cultura = ObtenerCultura();

    private static CultureInfo ObtenerCultura()
    {
        try { return CultureInfo.GetCultureInfo("es-CO"); }
        catch (CultureNotFoundException) { return CultureInfo.InvariantCulture; }
    }

    public static string Moneda(decimal valor) => "$" + valor.ToString("N0", Cultura);
}
