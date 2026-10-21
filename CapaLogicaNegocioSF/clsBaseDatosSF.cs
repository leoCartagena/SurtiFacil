using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;

namespace CapaLogicaNegocioSF
{
    public class clsBaseDatosSF
    {
        private clsConexionSQLSF gConexionSF = new clsConexionSQLSF();

        public bool AsegurarBaseDatos()
        {
            using (SqlConnection lSqlConexionSF = gConexionSF.CrearConexionMaster())
            {
                lSqlConexionSF.Open();

                if (ExisteBaseDatos(lSqlConexionSF))
                {
                    VerificarVersion(lSqlConexionSF);
                    return false;
                }

                try
                {
                    EjecutarScript(lSqlConexionSF, clsScriptBaseDatosSF.cStrScriptSF);
                }
                catch
                {
                    EliminarBaseDatos(lSqlConexionSF);
                    throw;
                }

                return true;
            }
        }

        private bool ExisteBaseDatos(SqlConnection pSqlConexionSF)
        {
            string lStrSqlSF = "SELECT COUNT(*) FROM sys.databases WHERE name = @Nombre";
            using (SqlCommand lSqlComandoSF = new SqlCommand(lStrSqlSF, pSqlConexionSF))
            {
                lSqlComandoSF.Parameters.AddWithValue("@Nombre", clsConexionSQLSF.cStrBaseDatosSF);
                int lIntCantidadSF = Convert.ToInt32(lSqlComandoSF.ExecuteScalar());
                return lIntCantidadSF > 0;
            }
        }

        private void VerificarVersion(SqlConnection pSqlConexionSF)
        {
            string lStrSqlSF = "SELECT COUNT(*) FROM " + clsConexionSQLSF.cStrBaseDatosSF + ".sys.procedures WHERE name = 'spListarProductos'";
            using (SqlCommand lSqlComandoSF = new SqlCommand(lStrSqlSF, pSqlConexionSF))
            {
                int lIntCantidadSF = Convert.ToInt32(lSqlComandoSF.ExecuteScalar());
                if (lIntCantidadSF == 0)
                {
                    throw new InvalidOperationException("La base SurtiFacilDB existe pero es de una versión anterior. Elimínela en SQL Server Management Studio para recrearla automáticamente.");
                }
            }
        }

        private void EjecutarScript(SqlConnection pSqlConexionSF, string pStrScriptSF)
        {
            string[] lStrLotesSF = Regex.Split(pStrScriptSF, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            foreach (string lStrLoteSF in lStrLotesSF)
            {
                if (lStrLoteSF.Trim() == "") continue;
                using (SqlCommand lSqlComandoSF = new SqlCommand(lStrLoteSF, pSqlConexionSF))
                {
                    lSqlComandoSF.CommandTimeout = 120;
                    lSqlComandoSF.ExecuteNonQuery();
                }
            }
        }

        private void EliminarBaseDatos(SqlConnection pSqlConexionSF)
        {
            string lStrNombreSF = clsConexionSQLSF.cStrBaseDatosSF;
            string lStrSqlSF = "USE master; IF DB_ID('" + lStrNombreSF + "') IS NOT NULL BEGIN ALTER DATABASE " + lStrNombreSF + " SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE " + lStrNombreSF + "; END";
            try
            {
                using (SqlCommand lSqlComandoSF = new SqlCommand(lStrSqlSF, pSqlConexionSF))
                {
                    lSqlComandoSF.ExecuteNonQuery();
                }
            }
            catch { }
        }
    }
}