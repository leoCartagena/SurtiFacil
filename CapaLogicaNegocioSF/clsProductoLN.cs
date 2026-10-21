using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using CapaEntidadesSF;

namespace CapaLogicaNegocioSF
{
    /// <summary>
    /// Lógica de los productos.
    /// </summary>
    public class clsProductoLN
    {
        private clsAccesoDatosSF gAccesoSF = new clsAccesoDatosSF();

        /// <summary>Trae los productos con su categoría y parámetros.</summary>
        public List<clsProductoEN> ListarProductos()
        {
            List<clsProductoEN> lLstProductosSF = new List<clsProductoEN>();
            DataTable lTblDatosSF = gAccesoSF.ConsultarTabla("spListarProductos");

            foreach (DataRow lFilaSF in lTblDatosSF.Rows)
            {
                clsProductoEN lEntProductoSF = new clsProductoEN();
                lEntProductoSF.gIntIdProducto = Convert.ToInt32(lFilaSF["IdProducto"]);
                lEntProductoSF.gStrNombre = lFilaSF["Nombre"].ToString();
                lEntProductoSF.gIntStockActual = Convert.ToInt32(lFilaSF["StockActual"]);
                lEntProductoSF.gDecCostoUnitario = Convert.ToDecimal(lFilaSF["CostoUnitario"]);
                lEntProductoSF.gIntIdCategoria = Convert.ToInt32(lFilaSF["IdCategoria"]);
                lEntProductoSF.gStrNombreCategoria = lFilaSF["NombreCategoria"].ToString();
                lEntProductoSF.gIntTiempoEntregaDias = Convert.ToInt32(lFilaSF["TiempoEntregaProveedorDias"]);
                lEntProductoSF.gIntDiasCobertura = Convert.ToInt32(lFilaSF["DiasCoberturaObjetivo"]);
                lEntProductoSF.gIntStockSeguridad = Convert.ToInt32(lFilaSF["StockSeguridad"]);
                lLstProductosSF.Add(lEntProductoSF);
            }

            return lLstProductosSF;
        }

        /// <summary>Crea un producto y devuelve su id.</summary>
        public int InsertarProducto(clsProductoEN pEntProductoSF)
        {
            ValidarProducto(pEntProductoSF);

            return gAccesoSF.EjecutarEscalar("spInsertarProducto",
                new SqlParameter("@Nombre", pEntProductoSF.gStrNombre.Trim()),
                new SqlParameter("@StockActual", pEntProductoSF.gIntStockActual),
                new SqlParameter("@CostoUnitario", pEntProductoSF.gDecCostoUnitario),
                new SqlParameter("@IdCategoria", pEntProductoSF.gIntIdCategoria),
                new SqlParameter("@TiempoEntrega", pEntProductoSF.gIntTiempoEntregaDias),
                new SqlParameter("@DiasCobertura", pEntProductoSF.gIntDiasCobertura),
                new SqlParameter("@StockSeguridad", pEntProductoSF.gIntStockSeguridad));
        }

        /// <summary>Guarda los cambios de un producto.</summary>
        public void ActualizarProducto(clsProductoEN pEntProductoSF)
        {
            ValidarProducto(pEntProductoSF);

            gAccesoSF.EjecutarSinResultado("spActualizarProducto",
                new SqlParameter("@IdProducto", pEntProductoSF.gIntIdProducto),
                new SqlParameter("@Nombre", pEntProductoSF.gStrNombre.Trim()),
                new SqlParameter("@StockActual", pEntProductoSF.gIntStockActual),
                new SqlParameter("@CostoUnitario", pEntProductoSF.gDecCostoUnitario),
                new SqlParameter("@IdCategoria", pEntProductoSF.gIntIdCategoria),
                new SqlParameter("@TiempoEntrega", pEntProductoSF.gIntTiempoEntregaDias),
                new SqlParameter("@DiasCobertura", pEntProductoSF.gIntDiasCobertura),
                new SqlParameter("@StockSeguridad", pEntProductoSF.gIntStockSeguridad));
        }

        /// <summary>Elimina un producto si no tiene historial.</summary>
        public void EliminarProducto(int pIntIdProductoSF)
        {
            gAccesoSF.EjecutarSinResultado("spEliminarProducto",
                new SqlParameter("@IdProducto", pIntIdProductoSF));
        }

        /// <summary>Revisa que los datos del producto tengan sentido.</summary>
        private void ValidarProducto(clsProductoEN pEntProductoSF)
        {
            if (pEntProductoSF.gStrNombre.Trim() == "")
            {
                throw new InvalidOperationException("Escriba el nombre del producto.");
            }

            if (pEntProductoSF.gIntIdCategoria <= 0)
            {
                throw new InvalidOperationException("Seleccione una categoría.");
            }

            if (pEntProductoSF.gDecCostoUnitario <= 0)
            {
                throw new InvalidOperationException("El costo unitario debe ser mayor a cero.");
            }

            if (pEntProductoSF.gIntDiasCobertura <= 0)
            {
                throw new InvalidOperationException("Los días de cobertura deben ser mayores a cero.");
            }
        }
    }
}
