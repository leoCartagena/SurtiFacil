using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using CapaEntidadesSF;

namespace CapaLogicaNegocioSF
{
    /// <summary>
    /// Lógica de los movimientos de inventario.
    /// </summary>
    public class clsMovimientoLN
    {
        private clsAccesoDatosSF gAccesoSF = new clsAccesoDatosSF();

        /// <summary>Trae todos los movimientos, los más nuevos primero.</summary>
        public List<clsMovimientoEN> ListarMovimientos()
        {
            List<clsMovimientoEN> lLstMovimientosSF = new List<clsMovimientoEN>();
            DataTable lTblDatosSF = gAccesoSF.ConsultarTabla("spListarMovimientos");

            foreach (DataRow lFilaSF in lTblDatosSF.Rows)
            {
                clsMovimientoEN lEntMovimientoSF = new clsMovimientoEN();
                lEntMovimientoSF.gIntIdMovimiento = Convert.ToInt32(lFilaSF["IdMovimiento"]);
                lEntMovimientoSF.gIntIdProducto = Convert.ToInt32(lFilaSF["IdProducto"]);
                lEntMovimientoSF.gStrNombreProducto = lFilaSF["Nombre"].ToString();
                lEntMovimientoSF.gStrTipoMovimiento = lFilaSF["TipoMovimiento"].ToString();
                lEntMovimientoSF.gIntCantidad = Convert.ToInt32(lFilaSF["Cantidad"]);
                lEntMovimientoSF.gDteFechaMovimiento = Convert.ToDateTime(lFilaSF["FechaMovimiento"]);
                lLstMovimientosSF.Add(lEntMovimientoSF);
            }

            return lLstMovimientosSF;
        }

        /// <summary>Registra un movimiento y actualiza el stock.</summary>
        public int InsertarMovimiento(clsMovimientoEN pEntMovimientoSF)
        {
            ValidarMovimiento(pEntMovimientoSF);

            return gAccesoSF.EjecutarEscalar("spInsertarMovimiento",
                new SqlParameter("@IdProducto", pEntMovimientoSF.gIntIdProducto),
                new SqlParameter("@TipoMovimiento", pEntMovimientoSF.gStrTipoMovimiento),
                new SqlParameter("@Cantidad", pEntMovimientoSF.gIntCantidad),
                new SqlParameter("@FechaMovimiento", pEntMovimientoSF.gDteFechaMovimiento));
        }

        /// <summary>Cambia tipo, cantidad o fecha y corrige el stock.</summary>
        public void ActualizarMovimiento(clsMovimientoEN pEntMovimientoSF)
        {
            ValidarMovimiento(pEntMovimientoSF);

            gAccesoSF.EjecutarSinResultado("spActualizarMovimiento",
                new SqlParameter("@IdMovimiento", pEntMovimientoSF.gIntIdMovimiento),
                new SqlParameter("@TipoMovimiento", pEntMovimientoSF.gStrTipoMovimiento),
                new SqlParameter("@Cantidad", pEntMovimientoSF.gIntCantidad),
                new SqlParameter("@FechaMovimiento", pEntMovimientoSF.gDteFechaMovimiento));
        }

        /// <summary>Elimina un movimiento y devuelve el stock a como estaba.</summary>
        public void EliminarMovimiento(int pIntIdMovimientoSF)
        {
            gAccesoSF.EjecutarSinResultado("spEliminarMovimiento",
                new SqlParameter("@IdMovimiento", pIntIdMovimientoSF));
        }

        private void ValidarMovimiento(clsMovimientoEN pEntMovimientoSF)
        {
            if (pEntMovimientoSF.gIntIdProducto <= 0)
            {
                throw new InvalidOperationException("Seleccione un producto.");
            }

            if (pEntMovimientoSF.gStrTipoMovimiento == "")
            {
                throw new InvalidOperationException("Seleccione el tipo de movimiento.");
            }

            if (pEntMovimientoSF.gIntCantidad <= 0)
            {
                throw new InvalidOperationException("La cantidad debe ser mayor a cero.");
            }
        }
    }
}