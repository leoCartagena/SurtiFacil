using System;
using System.Drawing;
using System.Windows.Forms;

namespace CapaPresentacionPedidoSF
{
    /// <summary>
    /// Parte del formulario que arma el diseño por código.
    /// </summary>
    public partial class frmPedidoSugeridoSF : Form
    {
        // Controles de la pantalla
        private Panel pnlEncabezado;
        private Label lblEmpresa;
        private TextBox txtEmpresa;
        private Label lblTitulo;
        private FlowLayoutPanel pnlFiltros;
        private Label lblCategoria;
        private ComboBox cmbCategoria;
        private Label lblFechaAnalisis;
        private DateTimePicker dtpFechaAnalisis;
        private Button btnCalcular;
        private Button btnExportarOrden;
        private Button btnRegresar;
        private Label lblPeriodo;
        private DataGridView dgvPedidoSugerido;
        private FlowLayoutPanel pnlResumen;
        private Label lblTotalProductos;
        private Label lblValorTotal;
        private ToolTip gAyudaSF;

        /// <summary>
        /// Crea todos los controles y los acomoda en la ventana.
        /// </summary>
        private void ArmarDiseno()
        {
            // Ventana
            this.Text = "Módulo de Pedido Sugerido";
            this.Size = new Size(1100, 650);
            this.MinimumSize = new Size(1000, 510);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;

            CrearEncabezado();
            CrearTitulo();
            CrearFiltros();
            CrearGrilla();
            CrearResumen();

            // Tabla principal, cada fila de la pantalla es una fila de la tabla
            TableLayoutPanel lTblPrincipalSF = new TableLayoutPanel();
            lTblPrincipalSF.Name = "tblPrincipal";
            lTblPrincipalSF.Dock = DockStyle.Fill;
            lTblPrincipalSF.ColumnCount = 1;
            lTblPrincipalSF.RowCount = 5;
            lTblPrincipalSF.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));   // empresa
            lTblPrincipalSF.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));   // título
            lTblPrincipalSF.RowStyles.Add(new RowStyle(SizeType.Absolute, 90));   // filtros
            lTblPrincipalSF.RowStyles.Add(new RowStyle(SizeType.Percent, 100));   // grilla
            lTblPrincipalSF.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));   // resumen

            AgregarFila(lTblPrincipalSF, pnlEncabezado, 0);
            AgregarFila(lTblPrincipalSF, lblTitulo, 1);
            AgregarFila(lTblPrincipalSF, pnlFiltros, 2);
            AgregarFila(lTblPrincipalSF, dgvPedidoSugerido, 3);
            AgregarFila(lTblPrincipalSF, pnlResumen, 4);

            this.Controls.Add(lTblPrincipalSF);
        }

        /// <summary>
        /// Barra superior con el nombre de la empresa.
        /// </summary>
        private void CrearEncabezado()
        {
            pnlEncabezado = new Panel();
            pnlEncabezado.Name = "pnlEncabezado";
            pnlEncabezado.Padding = new Padding(15, 10, 15, 10);

            // Etiqueta que se ve normalmente
            lblEmpresa = new Label();
            lblEmpresa.Name = "lblEmpresa";
            lblEmpresa.Text = gStrNombreEmpresa;
            lblEmpresa.Dock = DockStyle.Fill;
            lblEmpresa.TextAlign = ContentAlignment.MiddleLeft;
            lblEmpresa.Cursor = Cursors.Hand;

            // Caja de texto que aparece al hacer clic en la etiqueta
            txtEmpresa = new TextBox();
            txtEmpresa.Name = "txtEmpresa";
            txtEmpresa.Dock = DockStyle.Top;
            txtEmpresa.MaxLength = 100;
            txtEmpresa.Visible = false;

            gAyudaSF = new ToolTip();
            gAyudaSF.SetToolTip(lblEmpresa, "Clic para cambiar el nombre de la empresa");

            pnlEncabezado.Controls.Add(lblEmpresa);
            pnlEncabezado.Controls.Add(txtEmpresa);
        }

        /// <summary>
        /// Título de la pantalla.
        /// </summary>
        private void CrearTitulo()
        {
            lblTitulo = new Label();
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Text = "Módulo de Cálculo de Pedido Sugerido - Basado en Histórico Anual";
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            lblTitulo.Padding = new Padding(15, 0, 0, 0);
            lblTitulo.AutoSize = false;
        }

        /// <summary>
        /// Categoría, fecha y botones.
        /// </summary>
        private void CrearFiltros()
        {
            pnlFiltros = new FlowLayoutPanel();
            pnlFiltros.Name = "pnlFiltros";
            pnlFiltros.Padding = new Padding(10, 5, 10, 5);

            lblCategoria = CrearEtiqueta("lblCategoria", "Filtrar por categoría:");

            cmbCategoria = new ComboBox();
            cmbCategoria.Name = "cmbCategoria";
            cmbCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategoria.Width = 180;

            lblFechaAnalisis = CrearEtiqueta("lblFechaAnalisis", "Fecha de análisis:");

            dtpFechaAnalisis = new DateTimePicker();
            dtpFechaAnalisis.Name = "dtpFechaAnalisis";
            dtpFechaAnalisis.Format = DateTimePickerFormat.Short;
            dtpFechaAnalisis.Width = 120;
            dtpFechaAnalisis.Value = DateTime.Today;
            dtpFechaAnalisis.MaxDate = DateTime.Today;

            btnCalcular = CrearBoton("btnCalcular", "Calcular Pedido");
            btnExportarOrden = CrearBoton("btnExportarOrden", "Exportar Orden de Compra");
            btnExportarOrden.Enabled = false;
            //Agregado el boton para regresar
            btnRegresar = CrearBoton("btnRegresar", "Regresar al Menú");

            lblPeriodo = CrearEtiqueta("lblPeriodo", "");

            pnlFiltros.Controls.Add(lblCategoria);
            pnlFiltros.Controls.Add(cmbCategoria);
            pnlFiltros.Controls.Add(lblFechaAnalisis);
            pnlFiltros.Controls.Add(dtpFechaAnalisis);
            pnlFiltros.Controls.Add(btnCalcular);
            pnlFiltros.Controls.Add(btnExportarOrden);
            pnlFiltros.Controls.Add(btnRegresar);
            pnlFiltros.Controls.Add(lblPeriodo);

            AcomodarControles(pnlFiltros);
        }

        /// <summary>
        /// Grilla con las columnas del documento más el punto de reorden.
        /// </summary>
        private void CrearGrilla()
        {
            dgvPedidoSugerido = new DataGridView();
            dgvPedidoSugerido.Name = "dgvPedidoSugerido";
            dgvPedidoSugerido.AutoGenerateColumns = false;
            dgvPedidoSugerido.ReadOnly = true;
            dgvPedidoSugerido.AllowUserToAddRows = false;
            dgvPedidoSugerido.AllowUserToDeleteRows = false;
            dgvPedidoSugerido.RowHeadersVisible = false;
            dgvPedidoSugerido.MultiSelect = false;
            dgvPedidoSugerido.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPedidoSugerido.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Título de la columna, propiedad de la entidad y formato
            AgregarColumna("IdProducto", "gIntIdProducto", "");
            AgregarColumna("NombreProducto", "gStrNombreProducto", "");
            AgregarColumna("StockActual", "gIntStockActual", "N0");
            AgregarColumna("VentaPromedioDiaria", "gDblVentaPromedioDiaria", "N2");
            AgregarColumna("StockSeguridad", "gIntStockSeguridad", "N0");
            AgregarColumna("PuntoReorden", "gDblPuntoReorden", "N2");
            AgregarColumna("CantidadPedidoSugerido", "gIntCantidadPedidoSugerido", "N0");
        }

        /// <summary>
        /// Barra inferior con los totales.
        /// </summary>
        private void CrearResumen()
        {
            pnlResumen = new FlowLayoutPanel();
            pnlResumen.Name = "pnlResumen";
            pnlResumen.Padding = new Padding(10, 0, 10, 0);

            lblTotalProductos = CrearEtiqueta("lblTotalProductos", "");
            lblValorTotal = CrearEtiqueta("lblValorTotal", "");

            pnlResumen.Controls.Add(lblTotalProductos);
            pnlResumen.Controls.Add(lblValorTotal);

            AcomodarControles(pnlResumen);
        }

        // Ayudas para no repetir código

        private Label CrearEtiqueta(string pStrNombreSF, string pStrTextoSF)
        {
            Label lLblEtiquetaSF = new Label();
            lLblEtiquetaSF.Name = pStrNombreSF;
            lLblEtiquetaSF.Text = pStrTextoSF;
            lLblEtiquetaSF.AutoSize = true;
            return lLblEtiquetaSF;
        }

        private Button CrearBoton(string pStrNombreSF, string pStrTextoSF)
        {
            Button lBtnBotonSF = new Button();
            lBtnBotonSF.Name = pStrNombreSF;
            lBtnBotonSF.Text = pStrTextoSF;
            lBtnBotonSF.AutoSize = true;
            lBtnBotonSF.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            lBtnBotonSF.Padding = new Padding(10, 4, 10, 4);
            return lBtnBotonSF;
        }

        private void AgregarColumna(string pStrTituloSF, string pStrPropiedadSF, string pStrFormatoSF)
        {
            DataGridViewTextBoxColumn lColColumnaSF = new DataGridViewTextBoxColumn();
            lColColumnaSF.Name = "col" + pStrTituloSF;
            lColColumnaSF.HeaderText = pStrTituloSF;
            lColColumnaSF.DataPropertyName = pStrPropiedadSF;
            lColColumnaSF.DefaultCellStyle.Format = pStrFormatoSF;
            dgvPedidoSugerido.Columns.Add(lColColumnaSF);
        }

        private void AgregarFila(TableLayoutPanel pTblTablaSF, Control pCtlControlSF, int pIntFilaSF)
        {
            pCtlControlSF.Dock = DockStyle.Fill;
            pCtlControlSF.Margin = new Padding(0);
            pTblTablaSF.Controls.Add(pCtlControlSF, 0, pIntFilaSF);
        }

        private void AcomodarControles(FlowLayoutPanel pFlpPanelSF)
        {
            foreach (Control lCtlSF in pFlpPanelSF.Controls)
            {
                // Anchor solo a la izquierda centra el control de arriba a abajo
                lCtlSF.Anchor = AnchorStyles.Left;
                lCtlSF.Margin = new Padding(8);
            }
        }
    }
}