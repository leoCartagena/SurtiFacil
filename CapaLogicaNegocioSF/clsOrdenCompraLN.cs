using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using CapaEntidadesSF;

namespace CapaLogicaNegocioSF
{
    /// <summary>
    /// Arma, guarda y gestiona las órdenes de compra.
    /// </summary>
    public class clsOrdenCompraLN
    {
        private clsConexionSQLSF gConexionSF = new clsConexionSQLSF();
        private clsAccesoDatosSF gAccesoSF = new clsAccesoDatosSF();

        public clsOrdenCompraEN CrearOrden(string pStrNombreEmpresaSF, DateTime pDteFechaAnalisisSF,
                                           List<clsPedidoSugeridoEN> pLstPedidoSF)
        {
            clsOrdenCompraEN lEntOrdenSF = new clsOrdenCompraEN();
            lEntOrdenSF.gStrNombreEmpresa = pStrNombreEmpresaSF;
            lEntOrdenSF.gDteFechaAnalisis = pDteFechaAnalisisSF.Date;

            foreach (clsPedidoSugeridoEN lEntPedidoSF in pLstPedidoSF)
            {
                if (lEntPedidoSF.gIntCantidadPedidoSugerido > 0)
                {
                    lEntOrdenSF.gLstDetalles.Add(lEntPedidoSF);
                    lEntOrdenSF.gDecTotalAproximado =
                        lEntOrdenSF.gDecTotalAproximado + lEntPedidoSF.gDecSubtotalSugerido;
                }
            }

            return lEntOrdenSF;
        }

        public int GuardarOrden(clsOrdenCompraEN pEntOrdenSF)
        {
            if (pEntOrdenSF.gLstDetalles.Count == 0)
            {
                throw new InvalidOperationException("La orden no tiene productos para guardar.");
            }

            using (SqlConnection lSqlConexionSF = gConexionSF.CrearConexion())
            {
                lSqlConexionSF.Open();
                SqlTransaction lSqlTransaccionSF = lSqlConexionSF.BeginTransaction();

                try
                {
                    int lIntIdOrdenSF = 0;

                    using (SqlCommand lSqlComandoSF = new SqlCommand(
                        "spInsertarOrdenCompra", lSqlConexionSF, lSqlTransaccionSF))
                    {
                        lSqlComandoSF.CommandType = CommandType.StoredProcedure;
                        lSqlComandoSF.Parameters.AddWithValue("@NombreEmpresa", pEntOrdenSF.gStrNombreEmpresa);
                        lSqlComandoSF.Parameters.AddWithValue("@FechaAnalisis", pEntOrdenSF.gDteFechaAnalisis);
                        lSqlComandoSF.Parameters.AddWithValue("@TotalAproximado", pEntOrdenSF.gDecTotalAproximado);
                        lIntIdOrdenSF = Convert.ToInt32(lSqlComandoSF.ExecuteScalar());
                    }

                    foreach (clsPedidoSugeridoEN lEntDetalleSF in pEntOrdenSF.gLstDetalles)
                    {
                        using (SqlCommand lSqlComandoDetalleSF = new SqlCommand(
                            "spInsertarDetalleOrdenCompra", lSqlConexionSF, lSqlTransaccionSF))
                        {
                            lSqlComandoDetalleSF.CommandType = CommandType.StoredProcedure;
                            lSqlComandoDetalleSF.Parameters.AddWithValue("@IdOrden", lIntIdOrdenSF);
                            lSqlComandoDetalleSF.Parameters.AddWithValue("@IdProducto", lEntDetalleSF.gIntIdProducto);
                            lSqlComandoDetalleSF.Parameters.AddWithValue("@CantidadSugerida", lEntDetalleSF.gIntCantidadPedidoSugerido);
                            lSqlComandoDetalleSF.Parameters.AddWithValue("@CostoUnitario", lEntDetalleSF.gDecCostoUnitario);
                            lSqlComandoDetalleSF.Parameters.AddWithValue("@Subtotal", lEntDetalleSF.gDecSubtotalSugerido);
                            lSqlComandoDetalleSF.ExecuteNonQuery();
                        }
                    }

                    lSqlTransaccionSF.Commit();
                    pEntOrdenSF.gIntIdOrden = lIntIdOrdenSF;
                    return lIntIdOrdenSF;
                }
                catch
                {
                    lSqlTransaccionSF.Rollback();
                    throw;
                }
            }
        }

        public List<clsOrdenCompraEN> ListarOrdenes()
        {
            List<clsOrdenCompraEN> lLstOrdenesSF = new List<clsOrdenCompraEN>();
            DataTable lTblDatosSF = gAccesoSF.ConsultarTabla("spListarOrdenes");

            foreach (DataRow lFilaSF in lTblDatosSF.Rows)
            {
                clsOrdenCompraEN lEntOrdenSF = new clsOrdenCompraEN();
                lEntOrdenSF.gIntIdOrden = Convert.ToInt32(lFilaSF["IdOrden"]);
                lEntOrdenSF.gStrNombreEmpresa = lFilaSF["NombreEmpresa"].ToString();
                lEntOrdenSF.gDteFechaOrden = Convert.ToDateTime(lFilaSF["FechaOrden"]);
                lEntOrdenSF.gDteFechaAnalisis = Convert.ToDateTime(lFilaSF["FechaAnalisis"]);
                lEntOrdenSF.gDecTotalAproximado = Convert.ToDecimal(lFilaSF["TotalAproximado"]);
                lEntOrdenSF.gStrEstado = lFilaSF["Estado"].ToString();
                lLstOrdenesSF.Add(lEntOrdenSF);
            }

            return lLstOrdenesSF;
        }

        public List<clsPedidoSugeridoEN> ObtenerDetalle(int pIntIdOrdenSF)
        {
            List<clsPedidoSugeridoEN> lLstDetalleSF = new List<clsPedidoSugeridoEN>();
            DataTable lTblDatosSF = gAccesoSF.ConsultarTabla("spObtenerDetalleOrden",
                new SqlParameter("@IdOrden", pIntIdOrdenSF));

            foreach (DataRow lFilaSF in lTblDatosSF.Rows)
            {
                clsPedidoSugeridoEN lEntDetalleSF = new clsPedidoSugeridoEN();
                lEntDetalleSF.gIntIdProducto = Convert.ToInt32(lFilaSF["IdProducto"]);
                lEntDetalleSF.gStrNombreProducto = lFilaSF["Nombre"].ToString();
                lEntDetalleSF.gIntCantidadPedidoSugerido = Convert.ToInt32(lFilaSF["CantidadSugerida"]);
                lEntDetalleSF.gDecCostoUnitario = Convert.ToDecimal(lFilaSF["CostoUnitario"]);
                lEntDetalleSF.gDecSubtotalSugerido = Convert.ToDecimal(lFilaSF["Subtotal"]);
                lLstDetalleSF.Add(lEntDetalleSF);
            }

            return lLstDetalleSF;
        }

        public void EliminarOrden(int pIntIdOrdenSF)
        {
            gAccesoSF.EjecutarSinResultado("spEliminarOrden", new SqlParameter("@IdOrden", pIntIdOrdenSF));
        }

        public void RecibirOrden(int pIntIdOrdenSF)
        {
            gAccesoSF.EjecutarSinResultado("spRecibirOrden", new SqlParameter("@IdOrden", pIntIdOrdenSF));
        }
    }
}