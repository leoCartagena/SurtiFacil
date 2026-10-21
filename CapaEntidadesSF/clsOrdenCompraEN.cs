using System;
using System.Collections.Generic;

namespace CapaEntidadesSF
{
    /// <summary>
    /// Orden de compra con su lista de productos.
    /// </summary>
    public class clsOrdenCompraEN
    {
        public int gIntIdOrden { get; set; }
        public string gStrNombreEmpresa { get; set; }
        public DateTime gDteFechaOrden { get; set; }
        public DateTime gDteFechaAnalisis { get; set; }
        public decimal gDecTotalAproximado { get; set; }
        public string gStrEstado { get; set; }
        public List<clsPedidoSugeridoEN> gLstDetalles { get; set; }

        public clsOrdenCompraEN()
        {
            this.gIntIdOrden = 0;
            this.gStrNombreEmpresa = "";
            this.gDteFechaOrden = DateTime.Today;
            this.gDteFechaAnalisis = DateTime.Today;
            this.gDecTotalAproximado = 0;
            this.gStrEstado = "Pendiente";
            this.gLstDetalles = new List<clsPedidoSugeridoEN>();
        }
    }
}