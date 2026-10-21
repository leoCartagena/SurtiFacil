using System.Windows.Forms;
using CapaEntidadesSF;
using CapaLogicaNegocioSF;

namespace CapaPresentacionPedidoSF
{
    public class frmCategoriasSF : frmCrudBaseSF
    {
        private clsCategoriaLN gCategoriaLN = new clsCategoriaLN();
        private TextBox txtNombre;

        public frmCategoriasSF() : base("Gestión de Categorías")
        {
            txtNombre = new TextBox { Name = "txtNombre", MaxLength = 100 };
            AgregarCampo("Nombre:", txtNombre);
            AgregarColumna(dgvDatos, "IdCategoria", "gIntIdCategoria", "");
            AgregarColumna(dgvDatos, "NombreCategoria", "gStrNombreCategoria", "");
        }

        protected override object ObtenerDatos() => gCategoriaLN.ListarCategorias();
        protected override void MostrarSeleccion() => txtNombre.Text = ((clsCategoriaEN)ObtenerSeleccionado()).gStrNombreCategoria;
        protected override void LimpiarCampos() { txtNombre.Text = ""; txtNombre.Focus(); }
        protected override void GuardarRegistro()
        {
            if (gBolNuevo) gCategoriaLN.InsertarCategoria(txtNombre.Text);
            else gCategoriaLN.ActualizarCategoria(((clsCategoriaEN)ObtenerSeleccionado()).gIntIdCategoria, txtNombre.Text);
        }
        protected override void EliminarRegistro() => gCategoriaLN.EliminarCategoria(((clsCategoriaEN)ObtenerSeleccionado()).gIntIdCategoria);
    }
}