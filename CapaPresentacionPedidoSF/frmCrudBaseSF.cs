using System;
using System.Drawing;
using System.Windows.Forms;

namespace CapaPresentacionPedidoSF
{
    public class frmCrudBaseSF : Form
    {
        protected Label lblTitulo;
        protected TableLayoutPanel tblCampos;
        protected FlowLayoutPanel pnlBotones;
        protected Button btnNuevo, btnGuardar, btnEliminar, btnRegresar;
        protected DataGridView dgvDatos;
        protected Label lblMensaje;
        protected bool gBolNuevo = true;

        public frmCrudBaseSF(string pStrTituloSF)
        {
            ArmarBase(pStrTituloSF);

            this.Load += (s, e) =>
            {
                clsEstiloSF.AplicarEstilo(this);
                try
                {
                    PrepararFormulario();
                    RecargarGrilla();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error de Carga", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnNuevo.Click += (s, e) => PrepararNuevo();

            btnRegresar.Click += (s, e) => this.Close();

            btnGuardar.Click += (s, e) =>
            {
                try
                {
                    GuardarRegistro();
                    RecargarGrilla();
                    MostrarMensaje("Registro guardado exitosamente.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error al Guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            btnEliminar.Click += (s, e) =>
            {
                if (dgvDatos.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Seleccione un registro para eliminar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (MessageBox.Show("¿Está seguro de eliminar el registro seleccionado?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        EliminarRegistro();
                        RecargarGrilla();
                        MostrarMensaje("Registro eliminado correctamente.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "No se pudo eliminar", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
            };

            

            dgvDatos.SelectionChanged += (s, e) =>
            {
                if (dgvDatos.SelectedRows.Count > 0 && dgvDatos.SelectedRows[0].DataBoundItem != null)
                {
                    gBolNuevo = false;
                    btnGuardar.Text = "Guardar Cambios";
                    MostrarSeleccion();
                }
            };
        }

        protected virtual void PrepararFormulario() { }
        protected virtual object ObtenerDatos() => null;
        protected virtual void MostrarSeleccion() { }
        protected virtual void LimpiarCampos() { }
        protected virtual void GuardarRegistro() { }
        protected virtual void EliminarRegistro() { }

        protected void RecargarGrilla()
        {
            dgvDatos.DataSource = null;
            dgvDatos.DataSource = ObtenerDatos();
            PrepararNuevo();
        }

        protected void PrepararNuevo()
        {
            dgvDatos.ClearSelection();
            gBolNuevo = true;
            btnGuardar.Text = "Agregar Nuevo";
            LimpiarCampos();
            lblMensaje.Text = "";
        }

        protected object ObtenerSeleccionado()
        {
            return (dgvDatos.SelectedRows.Count > 0 && dgvDatos.SelectedRows[0].DataBoundItem != null)
                ? dgvDatos.SelectedRows[0].DataBoundItem
                : null;
        }

        protected void MostrarMensaje(string pStrMensajeSF)
        {
            lblMensaje.Text = pStrMensajeSF;
            lblMensaje.ForeColor = clsEstiloSF.gColTealPrincipalSF;
        }

        protected void AgregarCampo(string pStrTextoSF, Control pCtlCampoSF)
        {
            int lIntFilaSF = tblCampos.RowCount++;
            tblCampos.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            Label lLblSF = new Label
            {
                Text = pStrTextoSF,
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Margin = new Padding(3, 6, 15, 6)
            };

            tblCampos.Controls.Add(lLblSF, 0, lIntFilaSF);
            pCtlCampoSF.Width = 320;
            tblCampos.Controls.Add(pCtlCampoSF, 1, lIntFilaSF);
        }

        protected void AgregarControlAncho(Control pCtlSF)
        {
            int lIntFilaSF = tblCampos.RowCount++;
            tblCampos.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            pCtlSF.Dock = DockStyle.Fill;
            tblCampos.Controls.Add(pCtlSF, 0, lIntFilaSF);
            tblCampos.SetColumnSpan(pCtlSF, 2);
        }

        protected DataGridView CrearGrilla(string pStrNombreSF)
        {
            return new DataGridView
            {
                Name = pStrNombreSF,
                AutoGenerateColumns = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Dock = DockStyle.Fill
            };
        }

        protected void AgregarColumna(DataGridView pGrdGrillaSF, string pStrTituloSF, string pStrPropiedadSF, string pStrFormatoSF)
        {
            pGrdGrillaSF.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "col" + pStrTituloSF,
                HeaderText = pStrTituloSF,
                DataPropertyName = pStrPropiedadSF,
                DefaultCellStyle = { Format = pStrFormatoSF }
            });
        }

        protected Button CrearBoton(string pStrNombreSF, string pStrTextoSF)
        {
            return new Button
            {
                Name = pStrNombreSF,
                Text = pStrTextoSF,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(10, 4, 10, 4),
                Margin = new Padding(5)
            };
        }

        protected NumericUpDown CrearNumerico(string pStrNombreSF, int pIntDecimalesSF)
        {
            return new NumericUpDown
            {
                Name = pStrNombreSF,
                DecimalPlaces = pIntDecimalesSF,
                Maximum = 1000000,
                Width = 320
            };
        }

        private void ArmarBase(string pStrTituloSF)
        {
            this.Text = pStrTituloSF;
            this.Size = new Size(1000, 700);
            this.MinimumSize = new Size(720, 612);
            this.StartPosition = FormStartPosition.CenterParent;

            lblTitulo = new Label
            {
                Name = "lblTitulo",
                Text = pStrTituloSF,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Padding = new Padding(15, 0, 0, 0)
            };

            Panel lPnlEncabezadoSF = new Panel { Name = "pnlEncabezado", Dock = DockStyle.Fill, Margin = new Padding(0) };
            lPnlEncabezadoSF.Controls.Add(lblTitulo);

            tblCampos = new TableLayoutPanel
            {
                Name = "tblCampos",
                AutoSize = true,
                ColumnCount = 2,
                Padding = new Padding(15, 10, 15, 10),
                Dock = DockStyle.Top
            };

            btnNuevo = CrearBoton("btnNuevo", "Nuevo Registro");
            btnGuardar = CrearBoton("btnGuardar", "Agregar Nuevo");
            btnEliminar = CrearBoton("btnEliminar", "Eliminar Seleccionado");
            btnRegresar = CrearBoton("btnRegresar", "Regresar al Menú");

            pnlBotones = new FlowLayoutPanel
            {
                Name = "pnlBotones",
                Padding = new Padding(10, 5, 10, 5),
                Dock = DockStyle.Fill
            };
            pnlBotones.Controls.AddRange(new Control[] { btnNuevo, btnGuardar, btnEliminar, btnRegresar });

            dgvDatos = CrearGrilla("dgvDatos");

            lblMensaje = new Label
            {
                Name = "lblMensaje",
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Padding = new Padding(15, 0, 0, 0)
            };

            TableLayoutPanel lTblPrincipalSF = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 5
            };

            lTblPrincipalSF.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            lTblPrincipalSF.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            lTblPrincipalSF.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            lTblPrincipalSF.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            lTblPrincipalSF.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));

            lTblPrincipalSF.Controls.Add(lPnlEncabezadoSF, 0, 0);
            lTblPrincipalSF.Controls.Add(tblCampos, 0, 1);
            lTblPrincipalSF.Controls.Add(pnlBotones, 0, 2);
            lTblPrincipalSF.Controls.Add(dgvDatos, 0, 3);
            lTblPrincipalSF.Controls.Add(lblMensaje, 0, 4);

            this.Controls.Add(lTblPrincipalSF);
        }
    }
}