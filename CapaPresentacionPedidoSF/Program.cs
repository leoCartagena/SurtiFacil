using CapaLogicaNegocioSF;

namespace CapaPresentacionPedidoSF
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            try
            {
                clsBaseDatosSF lBaseDatosSF = new clsBaseDatosSF();
                bool lBolCreadaSF = lBaseDatosSF.AsegurarBaseDatos();

                if (lBolCreadaSF)
                {
                    MessageBox.Show("La base de datos SurtiFacilDB se creó correctamente.",
                                    "Primera ejecución", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception lExcSF)
            {
                MessageBox.Show("No se pudo preparar la base de datos.\n\n" + lExcSF.Message +
                                "\n\nRevise el servidor en la clase clsConexionSQLSF.",
                                "Error de base de datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Application.Run(new frmMenuSF());
        }
    }
}