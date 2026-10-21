using System;
using System.Drawing;
using System.Windows.Forms;
using CapaEntidadesSF;
using CapaLogicaNegocioSF;

namespace CapaPresentacionPedidoSF
{
    public class frmOrdenesSF : frmCrudBaseSF
    {
        private clsOrdenCompraLN gOrdenLN = new clsOrdenCompraLN();
        private DataGridView dgvDetalle;
        private Button btnRecibir;

        public frmOrdenesSF() : base("Órdenes de Compra")
        {
            btnNuevo.Visible = false;
            btnGuardar.Visible = false;

            btnRecibir = CrearBoton("btnRecibir", "Marcar como Recibida");
            btnRecibir.Click += btnRecibir_Click;
            pnlBotones.Controls.Add(btnRecibir);
            //Orden de los botones
            pnlBotones.Controls.SetChildIndex(btnRegresar, pnlBotones.Controls.Count - 1);

            dgvDetalle = CrearGrilla("dgvDetalle");
            AgregarControlAncho(dgvDetalle);

            AgregarColumna(dgvDatos, "IdOrden", "gIntIdOrden", "");
            AgregarColumna(dgvDatos, "Empresa", "gStrNombreEmpresa", "");
            AgregarColumna(dgvDatos, "Fecha Orden", "gDteFechaOrden", "dd/MM/yyyy HH:mm");
            AgregarColumna(dgvDatos, "Total Aproximado", "gDecTotalAproximado", "Q #,##0.00");
            AgregarColumna(dgvDatos, "Estado", "gStrEstado", "");

            AgregarColumna(dgvDetalle, "Producto", "gStrNombreProducto", "");
            AgregarColumna(dgvDetalle, "Cantidad Sugerida", "gIntCantidadPedidoSugerido", "N0");
            AgregarColumna(dgvDetalle, "Costo Unitario", "gDecCostoUnitario", "Q #,##0.00");
            AgregarColumna(dgvDetalle, "Subtotal", "gDecSubtotalSugerido", "Q #,##0.00");
        }

        protected override object ObtenerDatos() => gOrdenLN.ListarOrdenes();

        protected override void MostrarSeleccion()
        {
            clsOrdenCompraEN o = (clsOrdenCompraEN)ObtenerSeleccionado();
            if (o != null)
            {
                dgvDetalle.DataSource = gOrdenLN.ObtenerDetalle(o.gIntIdOrden);
            }
        }

        protected override void LimpiarCampos() => dgvDetalle.DataSource = null;

        protected override void EliminarRegistro()
        {
            clsOrdenCompraEN o = (clsOrdenCompraEN)ObtenerSeleccionado();
            if (o != null)
            {
                gOrdenLN.EliminarOrden(o.gIntIdOrden);
            }
        }

        private void btnRecibir_Click(object sender, EventArgs e)
        {
            clsOrdenCompraEN o = (clsOrdenCompraEN)ObtenerSeleccionado();
            if (o == null)
            {
                MessageBox.Show("Seleccione una orden para marcar como recibida.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("¿Marcar la orden No. " + o.gIntIdOrden + " como recibida e ingresar productos al stock?", "Confirmar Recepción", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    gOrdenLN.RecibirOrden(o.gIntIdOrden);
                    RecargarGrilla();
                    MostrarMensaje("Orden recibida exitosamente. El stock ha sido actualizado.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error al Recibir Orden", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}