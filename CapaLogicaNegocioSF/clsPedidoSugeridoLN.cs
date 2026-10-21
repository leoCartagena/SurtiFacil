using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using CapaEntidadesSF;

namespace CapaLogicaNegocioSF
{
    /// <summary>
    /// Calcula el pedido sugerido de cada producto.
    /// </summary>
    public class clsPedidoSugeridoLN
    {
        /// <summary>Días del histórico que se analizan.</summary>
        public const int cIntDiasHistoricoSF = 365;

        private clsConexionSQLSF gConexionSF = new clsConexionSQLSF();

        /// <summary>
        /// Trae los datos, calcula cada producto y devuelve la lista completa.
        /// Categoría 0 significa todas.
        /// </summary>
        public List<clsPedidoSugeridoEN> CalcularPedido(int pIntIdCategoriaSF, DateTime pDteFechaAnalisisSF)
        {
            // Primero los datos base de la base de datos
            List<clsPedidoSugeridoEN> lLstPedidoSF = LeerDatosBase(pIntIdCategoriaSF, pDteFechaAnalisisSF);

            // Después se calcula producto por producto
            foreach (clsPedidoSugeridoEN lEntPedidoSF in lLstPedidoSF)
            {
                CalcularProducto(lEntPedidoSF);
            }

            return lLstPedidoSF;
        }

        /// <summary>
        /// Cuenta los productos con cantidad sugerida mayor a cero.
        /// </summary>
        public int ContarProductosAReabastecer(List<clsPedidoSugeridoEN> pLstPedidoSF)
        {
            int lIntContadorSF = 0;

            foreach (clsPedidoSugeridoEN lEntPedidoSF in pLstPedidoSF)
            {
                if (lEntPedidoSF.gIntCantidadPedidoSugerido > 0)
                {
                    lIntContadorSF = lIntContadorSF + 1;
                }
            }

            return lIntContadorSF;
        }

        /// <summary>
        /// Suma los subtotales de todos los productos.
        /// </summary>
        public decimal CalcularValorTotal(List<clsPedidoSugeridoEN> pLstPedidoSF)
        {
            decimal lDecTotalSF = 0;

            foreach (clsPedidoSugeridoEN lEntPedidoSF in pLstPedidoSF)
            {
                lDecTotalSF = lDecTotalSF + lEntPedidoSF.gDecSubtotalSugerido;
            }

            return lDecTotalSF;
        }

        /// <summary>
        /// Lee de la base de datos el stock, los parámetros y lo vendido.
        /// </summary>
        private List<clsPedidoSugeridoEN> LeerDatosBase(int pIntIdCategoriaSF, DateTime pDteFechaAnalisisSF)
        {
            List<clsPedidoSugeridoEN> lLstPedidoSF = new List<clsPedidoSugeridoEN>();

            using (SqlConnection lSqlConexionSF = gConexionSF.CrearConexion())
            using (SqlCommand lSqlComandoSF = new SqlCommand("spObtenerDatosPedido", lSqlConexionSF))
            {
                lSqlComandoSF.CommandType = CommandType.StoredProcedure;
                lSqlComandoSF.Parameters.AddWithValue("@FechaAnalisis", pDteFechaAnalisisSF.Date);
                lSqlComandoSF.Parameters.AddWithValue("@IdCategoria", pIntIdCategoriaSF);
                lSqlComandoSF.Parameters.AddWithValue("@DiasHistorico", cIntDiasHistoricoSF);

                lSqlConexionSF.Open();

                using (SqlDataReader lSqlLectorSF = lSqlComandoSF.ExecuteReader())
                {
                    while (lSqlLectorSF.Read())
                    {
                        clsPedidoSugeridoEN lEntPedidoSF = new clsPedidoSugeridoEN();
                        lEntPedidoSF.gIntIdProducto = Convert.ToInt32(lSqlLectorSF["IdProducto"]);
                        lEntPedidoSF.gStrNombreProducto = lSqlLectorSF["Nombre"].ToString();
                        lEntPedidoSF.gIntStockActual = Convert.ToInt32(lSqlLectorSF["StockActual"]);
                        lEntPedidoSF.gDecCostoUnitario = Convert.ToDecimal(lSqlLectorSF["CostoUnitario"]);
                        lEntPedidoSF.gIntTiempoEntregaDias = Convert.ToInt32(lSqlLectorSF["TiempoEntregaProveedorDias"]);
                        lEntPedidoSF.gIntDiasCobertura = Convert.ToInt32(lSqlLectorSF["DiasCoberturaObjetivo"]);
                        lEntPedidoSF.gIntStockSeguridad = Convert.ToInt32(lSqlLectorSF["StockSeguridad"]);
                        lEntPedidoSF.gIntUnidadesVendidas = Convert.ToInt32(lSqlLectorSF["UnidadesVendidas"]);
                        lLstPedidoSF.Add(lEntPedidoSF);
                    }
                }
            }

            return lLstPedidoSF;
        }

        /// <summary>
        /// Aplica las fórmulas del caso de uso a un producto.
        /// </summary>
        private void CalcularProducto(clsPedidoSugeridoEN pEntPedidoSF)
        {
            // VPD es unidades vendidas entre los días del histórico
            pEntPedidoSF.gDblVentaPromedioDiaria =
                (double)pEntPedidoSF.gIntUnidadesVendidas / cIntDiasHistoricoSF;

            // PR es VPD por tiempo de entrega, más el stock de seguridad
            pEntPedidoSF.gDblPuntoReorden =
                (pEntPedidoSF.gDblVentaPromedioDiaria * pEntPedidoSF.gIntTiempoEntregaDias)
                + pEntPedidoSF.gIntStockSeguridad;

            // CPS es VPD por días de cobertura, más seguridad, menos el stock actual
            double lDblCantidadSF =
                (pEntPedidoSF.gDblVentaPromedioDiaria * pEntPedidoSF.gIntDiasCobertura)
                + pEntPedidoSF.gIntStockSeguridad
                - pEntPedidoSF.gIntStockActual;

            // El redondeo evita errores diminutos de decimales
            lDblCantidadSF = Math.Round(lDblCantidadSF, 2);

            // Si da cero o negativo no se pide nada
            if (lDblCantidadSF <= 0)
            {
                pEntPedidoSF.gIntCantidadPedidoSugerido = 0;
            }
            else
            {
                // Se redondea hacia arriba porque no hay medias unidades
                pEntPedidoSF.gIntCantidadPedidoSugerido = (int)Math.Ceiling(lDblCantidadSF);
            }

            // Subtotal es la cantidad por el costo unitario
            pEntPedidoSF.gDecSubtotalSugerido =
                pEntPedidoSF.gIntCantidadPedidoSugerido * pEntPedidoSF.gDecCostoUnitario;
        }
    }
}