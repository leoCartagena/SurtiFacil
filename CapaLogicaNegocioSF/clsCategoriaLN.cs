using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using CapaEntidadesSF;

namespace CapaLogicaNegocioSF
{
    /// <summary>
    /// Lógica de negocio para las categorías.
    /// </summary>
    public class clsCategoriaLN
    {
        private clsConexionSQLSF gConexionSF = new clsConexionSQLSF();
        private clsAccesoDatosSF gAccesoSF = new clsAccesoDatosSF();

        /// <summary>
        /// Trae las categorías ordenadas por nombre.
        /// </summary>
        public List<clsCategoriaEN> ListarCategorias()
        {
            List<clsCategoriaEN> lLstCategoriasSF = new List<clsCategoriaEN>();

            using (SqlConnection lSqlConexionSF = gConexionSF.CrearConexion())
            {
                lSqlConexionSF.Open();

                using (SqlCommand lSqlComandoSF = new SqlCommand("spListarCategorias", lSqlConexionSF))
                {
                    lSqlComandoSF.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader lSqlLectorSF = lSqlComandoSF.ExecuteReader())
                    {
                        while (lSqlLectorSF.Read())
                        {
                            clsCategoriaEN lEntCategoriaSF = new clsCategoriaEN();
                            lEntCategoriaSF.gIntIdCategoria = Convert.ToInt32(lSqlLectorSF["IdCategoria"]);
                            lEntCategoriaSF.gStrNombreCategoria = lSqlLectorSF["NombreCategoria"].ToString();
                            lLstCategoriasSF.Add(lEntCategoriaSF);
                        }
                    }
                }
            }

            return lLstCategoriasSF;
        }

        /// <summary>
        /// Registra una nueva categoría.
        /// </summary>
        public int InsertarCategoria(string pStrNombreCategoriaSF)
        {
            ValidarCategoria(pStrNombreCategoriaSF);

            return gAccesoSF.EjecutarEscalar("spInsertarCategoria",
                new SqlParameter("@NombreCategoria", pStrNombreCategoriaSF.Trim()));
        }

        /// <summary>
        /// Cambia el nombre de una categoría existente.
        /// </summary>
        public void ActualizarCategoria(int pIntIdCategoriaSF, string pStrNombreCategoriaSF)
        {
            ValidarCategoria(pStrNombreCategoriaSF);

            gAccesoSF.EjecutarSinResultado("spActualizarCategoria",
                new SqlParameter("@IdCategoria", pIntIdCategoriaSF),
                new SqlParameter("@NombreCategoria", pStrNombreCategoriaSF.Trim()));
        }

        /// <summary>
        /// Elimina una categoría si no tiene productos asociados.
        /// </summary>
        public void EliminarCategoria(int pIntIdCategoriaSF)
        {
            gAccesoSF.EjecutarSinResultado("spEliminarCategoria",
                new SqlParameter("@IdCategoria", pIntIdCategoriaSF));
        }

        private void ValidarCategoria(string pStrNombreCategoriaSF)
        {
            if (string.IsNullOrWhiteSpace(pStrNombreCategoriaSF))
            {
                throw new InvalidOperationException("Escriba el nombre de la categoría.");
            }
        }
    }
}