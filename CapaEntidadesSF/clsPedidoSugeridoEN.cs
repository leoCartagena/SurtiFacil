using System;
using System.Collections.Generic;
using System.Text;

namespace CapaEntidadesSF
{
    /// <summary>
    /// Una fila de la grilla, con los datos de la base y los calculados.
    /// </summary>
    public class clsPedidoSugeridoEN
    {
        // Datos que vienen de la base de datos

        /// <summary>Número del producto.</summary>
        public int gIntIdProducto { get; set; }

        /// <summary>Nombre del producto.</summary>
        public string gStrNombreProducto { get; set; } = "";

        /// <summary>Unidades que hay hoy en bodega.</summary>
        public int gIntStockActual { get; set; }

        /// <summary>Costo de una unidad.</summary>
        public decimal gDecCostoUnitario { get; set; }

        /// <summary>Días que tarda el proveedor en entregar.</summary>
        public int gIntTiempoEntregaDias { get; set; }

        /// <summary>Días que debe durar el inventario que se pide.</summary>
        public int gIntDiasCobertura { get; set; }

        /// <summary>Colchón de unidades para imprevistos.</summary>
        public int gIntStockSeguridad { get; set; }

        /// <summary>Unidades vendidas en el período analizado.</summary>
        public int gIntUnidadesVendidas { get; set; }

        // Datos que calcula la capa lógica

        /// <summary>Venta promedio por día.</summary>
        public double gDblVentaPromedioDiaria { get; set; }

        /// <summary>Nivel de stock en el que se debe pedir.</summary>
        public double gDblPuntoReorden { get; set; }

        /// <summary>Unidades que se sugiere comprar.</summary>
        public int gIntCantidadPedidoSugerido { get; set; }

        /// <summary>Cantidad sugerida por el costo unitario.</summary>
        public decimal gDecSubtotalSugerido { get; set; }
    }
}
