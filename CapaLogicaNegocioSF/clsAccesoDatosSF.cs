using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace CapaLogicaNegocioSF
{
    /// <summary>
    /// Ayudante para ejecutar procedimientos almacenados.
    /// </summary>
    public class clsAccesoDatosSF
    {
        private clsConexionSQLSF gConexionSF = new clsConexionSQLSF();

        /// <summary>Ejecuta un procedimiento que devuelve filas.</summary>
        public DataTable ConsultarTabla(string pStrProcedimientoSF, params SqlParameter[] pSqlParametrosSF)
        {
            DataTable lTblResultadoSF = new DataTable();

            using (SqlConnection lSqlConexionSF = gConexionSF.CrearConexion())
            using (SqlCommand lSqlComandoSF = CrearComando(pStrProcedimientoSF, lSqlConexionSF, pSqlParametrosSF))
            {
                lSqlConexionSF.Open();

                using (SqlDataReader lSqlLectorSF = lSqlComandoSF.ExecuteReader())
                {
                    lTblResultadoSF.Load(lSqlLectorSF);
                }
            }

            return lTblResultadoSF;
        }

        /// <summary>Ejecuta un procedimiento que devuelve un número, como el id nuevo.</summary>
        public int EjecutarEscalar(string pStrProcedimientoSF, params SqlParameter[] pSqlParametrosSF)
        {
            using (SqlConnection lSqlConexionSF = gConexionSF.CrearConexion())
            using (SqlCommand lSqlComandoSF = CrearComando(pStrProcedimientoSF, lSqlConexionSF, pSqlParametrosSF))
            {
                lSqlConexionSF.Open();
                return Convert.ToInt32(lSqlComandoSF.ExecuteScalar());
            }
        }

        /// <summary>Ejecuta un procedimiento que no devuelve nada.</summary>
        public void EjecutarSinResultado(string pStrProcedimientoSF, params SqlParameter[] pSqlParametrosSF)
        {
            using (SqlConnection lSqlConexionSF = gConexionSF.CrearConexion())
            using (SqlCommand lSqlComandoSF = CrearComando(pStrProcedimientoSF, lSqlConexionSF, pSqlParametrosSF))
            {
                lSqlConexionSF.Open();
                lSqlComandoSF.ExecuteNonQuery();
            }
        }

        private SqlCommand CrearComando(string pStrProcedimientoSF, SqlConnection pSqlConexionSF,
                                        SqlParameter[] pSqlParametrosSF)
        {
            SqlCommand lSqlComandoSF = new SqlCommand(pStrProcedimientoSF, pSqlConexionSF);
            lSqlComandoSF.CommandType = CommandType.StoredProcedure;
            lSqlComandoSF.Parameters.AddRange(pSqlParametrosSF);
            return lSqlComandoSF;
        }
    }
}