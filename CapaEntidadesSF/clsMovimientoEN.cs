using System;

namespace CapaEntidadesSF
{
    /// <summary>
    /// Movimiento de inventario: Venta, Entrada o Salida.
    /// </summary>
    public class clsMovimientoEN
    {
        public int gIntIdMovimiento { get; set; }
        public int gIntIdProducto { get; set; }
        public string gStrNombreProducto { get; set; } = "";
        public string gStrTipoMovimiento { get; set; } = "";
        public int gIntCantidad { get; set; }
        public DateTime gDteFechaMovimiento { get; set; }
    }
}