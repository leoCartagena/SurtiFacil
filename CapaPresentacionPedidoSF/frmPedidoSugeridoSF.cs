using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using CapaEntidadesSF;
using CapaLogicaNegocioSF;

namespace CapaPresentacionPedidoSF
{
    public partial class frmPedidoSugeridoSF : Form
    {
        private clsCategoriaLN gCategoriaLN = new clsCategoriaLN();
        private clsPedidoSugeridoLN gPedidoLN = new clsPedidoSugeridoLN();
        private clsOrdenCompraLN gOrdenLN = new clsOrdenCompraLN();
        private clsExportadorExcelSF gExportadorSF = new clsExportadorExcelSF();

        private List<clsPedidoSugeridoEN> gLstPedidoActual = new List<clsPedidoSugeridoEN>();
        private string gStrNombreEmpresa = "SurtiFácil, S.A.";

        public frmPedidoSugeridoSF()
        {
            ArmarDiseno();
            clsEstiloSF.AplicarEstilo(this);
            ConectarEventos();
        }

        private void ConectarEventos()
        {
            this.Load += frmPedidoSugeridoSF_Load;
            lblEmpresa.Click += lblEmpresa_Click;
            txtEmpresa.KeyDown += txtEmpresa_KeyDown;
            txtEmpresa.Leave += txtEmpresa_Leave;
            cmbCategoria.SelectedIndexChanged += cmbCategoria_SelectedIndexChanged;
            dtpFechaAnalisis.ValueChanged += dtpFechaAnalisis_ValueChanged;
            btnCalcular.Click += btnCalcular_Click;
            btnExportarOrden.Click += btnExportarOrden_Click;
            btnRegresar.Click += (s, e) => this.Close();
            dgvPedidoSugerido.DataBindingComplete += dgvPedidoSugerido_DataBindingComplete;
        }

        private void frmPedidoSugeridoSF_Load(object sender, EventArgs e)
        {
            try { CargarCategorias(); }
            catch (Exception lExcSF)
            {
                MessageBox.Show("No se pudo conectar con la base de datos: " + lExcSF.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ActualizarPeriodo();
            MostrarResumen();
        }

        private void cmbCategoria_SelectedIndexChanged(object sender, EventArgs e) => LimpiarResultados();
        private void dtpFechaAnalisis_ValueChanged(object sender, EventArgs e)
        {
            ActualizarPeriodo();
            LimpiarResultados();
        }

        private void dgvPedidoSugerido_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e) => clsEstiloSF.ResaltarFilasConPedido(dgvPedidoSugerido);

        private void lblEmpresa_Click(object sender, EventArgs e)
        {
            txtEmpresa.Text = gStrNombreEmpresa;
            lblEmpresa.Visible = false;
            txtEmpresa.Visible = true;
            txtEmpresa.Focus();
            txtEmpresa.SelectAll();
        }

        private void txtEmpresa_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; GuardarNombreEmpresa(); }
            else if (e.KeyCode == Keys.Escape) { txtEmpresa.Text = gStrNombreEmpresa; GuardarNombreEmpresa(); }
        }

        private void txtEmpresa_Leave(object sender, EventArgs e) => GuardarNombreEmpresa();

        private void GuardarNombreEmpresa()
        {
            string lStrNuevoNombreSF = txtEmpresa.Text.Trim();
            if (lStrNuevoNombreSF != "") { gStrNombreEmpresa = lStrNuevoNombreSF; lblEmpresa.Text = gStrNombreEmpresa; }
            txtEmpresa.Visible = false;
            lblEmpresa.Visible = true;
        }

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            try
            {
                int lIntIdCategoriaSF = Convert.ToInt32(cmbCategoria.SelectedValue);
                DateTime lDteFechaSF = dtpFechaAnalisis.Value.Date;

                gLstPedidoActual = gPedidoLN.CalcularPedido(lIntIdCategoriaSF, lDteFechaSF);
                dgvPedidoSugerido.DataSource = null;
                dgvPedidoSugerido.DataSource = gLstPedidoActual;

                MostrarResumen();
                btnExportarOrden.Enabled = gPedidoLN.ContarProductosAReabastecer(gLstPedidoActual) > 0;
            }
            catch (Exception lExcSF)
            {
                MessageBox.Show("No se pudo calcular el pedido: " + lExcSF.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportarOrden_Click(object sender, EventArgs e)
        {
            clsOrdenCompraEN lEntOrdenSF = gOrdenLN.CrearOrden(gStrNombreEmpresa, dtpFechaAnalisis.Value.Date, gLstPedidoActual);
            string lStrMensajeSF = "Se guardará una orden con " + lEntOrdenSF.gLstDetalles.Count + " productos por Q " + lEntOrdenSF.gDecTotalAproximado.ToString("N2") + " y se exportará a Excel. ¿Desea continuar?";

            if (MessageBox.Show(lStrMensajeSF, "Confirmar orden", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            string lStrRutaSF = PedirRutaArchivo();
            if (lStrRutaSF == "") return;

            try { gOrdenLN.GuardarOrden(lEntOrdenSF); }
            catch (Exception lExcSF)
            {
                MessageBox.Show("No se pudo guardar la orden: " + lExcSF.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnExportarOrden.Enabled = false;

            try { gExportadorSF.ExportarOrden(lEntOrdenSF, lStrRutaSF); }
            catch (Exception lExcSF)
            {
                MessageBox.Show("Orden guardada pero falló la creación del Excel: " + lExcSF.Message, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Aquí se le indica al usuario la ruta completa exacta en su pantalla
            string lStrMensajeExitoSF = "Orden exportada exitosamente.\n\n" +
                                        "Ubicación del archivo:\n" + lStrRutaSF + "\n\n" +
                                        "¿Desea abrir el archivo ahora?";

            if (MessageBox.Show(lStrMensajeExitoSF, "Éxito al Exportar", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                AbrirArchivo(lStrRutaSF);
            }
        }

        private void CargarCategorias()
        {
            List<clsCategoriaEN> lLstCategoriasSF = gCategoriaLN.ListarCategorias();
            lLstCategoriasSF.Insert(0, new clsCategoriaEN(0, "Todas"));
            cmbCategoria.DisplayMember = "gStrNombreCategoria";
            cmbCategoria.ValueMember = "gIntIdCategoria";
            cmbCategoria.DataSource = lLstCategoriasSF;
        }

        private string PedirRutaArchivo()
        {
            using (SaveFileDialog lSfdArchivoSF = new SaveFileDialog())
            {
                lSfdArchivoSF.Title = "Guardar orden de compra";
                lSfdArchivoSF.Filter = "Libro de Excel (*.xlsx)|*.xlsx";
                lSfdArchivoSF.FileName = "OrdenCompra_" + DateTime.Today.ToString("yyyyMMdd") + ".xlsx";
                return lSfdArchivoSF.ShowDialog() == DialogResult.OK ? lSfdArchivoSF.FileName : "";
            }
        }

        private void AbrirArchivo(string pStrRutaSF)
        {
            try
            {
                ProcessStartInfo lPsiArchivoSF = new ProcessStartInfo(pStrRutaSF) { UseShellExecute = true };
                Process.Start(lPsiArchivoSF);
            }
            catch (Exception lExcSF)
            {
                MessageBox.Show("No se pudo abrir el archivo: " + lExcSF.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ActualizarPeriodo()
        {
            DateTime lDteFechaSF = dtpFechaAnalisis.Value.Date;
            DateTime lDteInicioSF = lDteFechaSF.AddDays(-clsPedidoSugeridoLN.cIntDiasHistoricoSF);
            lblPeriodo.Text = "Período analizado: " + lDteInicioSF.ToString("dd/MM/yyyy") + " al " + lDteFechaSF.AddDays(-1).ToString("dd/MM/yyyy");
        }

        private void MostrarResumen()
        {
            lblTotalProductos.Text = "Total Productos a Reabastecer: " + gPedidoLN.ContarProductosAReabastecer(gLstPedidoActual);
            lblValorTotal.Text = "Valor Total Aproximado Sugerido: Q " + gPedidoLN.CalcularValorTotal(gLstPedidoActual).ToString("N2");
        }

        private void LimpiarResultados()
        {
            gLstPedidoActual = new List<clsPedidoSugeridoEN>();
            dgvPedidoSugerido.DataSource = null;
            btnExportarOrden.Enabled = false;
            MostrarResumen();
        }
    }
}