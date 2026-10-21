using Microsoft.Data.SqlClient;

namespace CapaLogicaNegocioSF
{
    /// <summary>
    /// Crea las conexiones a SQL Server.
    /// </summary>
    public class clsConexionSQLSF
    {
        // En caso de estar usando otra instancia (ej: localhost o .\SQLEXPRESS), modificar solo esta constante
        // private const string cStrServidorSF = ".";
        //Con esto deberia poder servir donde sea que tengan SQL 
        private static readonly string cStrServidorSF = LeerServidorSF();
        private static string LeerServidorSF()
        {
            string lStrRutaSF = Path.Combine(AppContext.BaseDirectory, "servidor.txt");
            if (File.Exists(lStrRutaSF))
            {
                string lStrTextoSF = File.ReadAllText(lStrRutaSF).Trim();
                if (lStrTextoSF != "") return lStrTextoSF;
            }

            string[] lArrCandidatosSF = { "localhost", @".\SQLEXPRESS", @"(localdb)\MSSQLLocalDB", "." };
            foreach (string lStrCandidatoSF in lArrCandidatosSF)
            {
                try
                {
                    using (SqlConnection lSqlPruebaSF = new SqlConnection("Server=" + lStrCandidatoSF + ";Database=master;" + cStrAutenticacionSF + "Connect Timeout=3;"))
                    {
                        lSqlPruebaSF.Open();
                        return lStrCandidatoSF;
                    }
                }
                catch { }
            }
            return "localhost";
        }


        //Final de la conexión
        public const string cStrBaseDatosSF = "SurtiFacilDB";
        private const string cStrAutenticacionSF = "Trusted_Connection=True;TrustServerCertificate=True;";

        public SqlConnection CrearConexion()
        {
            return new SqlConnection("Server=" + cStrServidorSF + ";Database=" + cStrBaseDatosSF + ";" + cStrAutenticacionSF);
        }

        public SqlConnection CrearConexionMaster()
        {
            return new SqlConnection("Server=" + cStrServidorSF + ";Database=master;" + cStrAutenticacionSF);
        }
    }
}