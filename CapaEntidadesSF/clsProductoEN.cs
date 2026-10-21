namespace CapaEntidadesSF
{
    /// <summary>
    /// Producto con sus parámetros de reposición.
    /// </summary>
    public class clsProductoEN
    {
        public int gIntIdProducto { get; set; }
        public string gStrNombre { get; set; } = "";
        public int gIntStockActual { get; set; }
        public decimal gDecCostoUnitario { get; set; }
        public int gIntIdCategoria { get; set; }
        public string gStrNombreCategoria { get; set; } = "";
        public int gIntTiempoEntregaDias { get; set; }
        public int gIntDiasCobertura { get; set; }
        public int gIntStockSeguridad { get; set; }
    }
}