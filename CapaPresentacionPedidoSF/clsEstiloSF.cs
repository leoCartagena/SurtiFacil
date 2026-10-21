using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CapaPresentacionPedidoSF
{
    public static class clsEstiloSF
    {
        // Paleta de colores (Design System)
        public static readonly Color gColMarinoPetroleoSF = Color.FromArgb(4, 61, 80);      // #043D50
        public static readonly Color gColTealPrincipalSF = Color.FromArgb(14, 116, 144);    // #0E7490
        public static readonly Color gColFondoPaginaSF = Color.FromArgb(233, 243, 245);     // #E9F3F5
        public static readonly Color gColFilaResaltadaSF = Color.FromArgb(213, 233, 239);   // #D5E9EF
        public static readonly Color gColTarjetaSF = Color.White;                           // #FFFFFF
        public static readonly Color gColTextoSF = Color.FromArgb(12, 36, 44);              // #0C242C
        public static readonly Color gColDegradadoInicioSF = Color.FromArgb(20, 184, 166);  // #14B8A6
        public static readonly Color gColDegradadoFinSF = Color.FromArgb(21, 158, 189);     // #159EBD
        public static readonly Color gColAcentoAmbarSF = Color.FromArgb(227, 163, 59);      // #E3A33B
        public static readonly Color gColAvisoSF = Color.FromArgb(208, 85, 85);             // #D05555

        public static void AplicarEstilo(Form pFormSF)
        {
            pFormSF.BackColor = gColFondoPaginaSF;
            pFormSF.ForeColor = gColTextoSF;
            //Icono de la aplicación
            pFormSF.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            foreach (Control lControlSF in pFormSF.Controls)
            {
                AplicarEstiloControl(lControlSF);
            }
        }

        private static void AplicarEstiloControl(Control pControlSF)
        {
            string lStrPadreSF = pControlSF.Parent?.Name ?? "";
            bool lBolSobreOscuroSF = lStrPadreSF == "pnlEncabezado" || lStrPadreSF == "pnlLogo";

            if (pControlSF is Label lLblSF)
            {
                if (lBolSobreOscuroSF) lLblSF.BackColor = Color.Transparent;

                if (lLblSF.Name == "lblTitulo")
                {
                    lLblSF.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
                    lLblSF.ForeColor = lBolSobreOscuroSF ? Color.White : gColMarinoPetroleoSF;
                }
                else if (lLblSF.Name == "lblEmpresa")
                {
                    lLblSF.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
                    lLblSF.ForeColor = Color.White;
                }
                else if (lLblSF.Name == "lblTotalProductos" || lLblSF.Name == "lblValorTotal")
                {
                    lLblSF.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
                    lLblSF.ForeColor = gColMarinoPetroleoSF;
                }
                else if (lLblSF.Name == "lblMensaje")
                {
                    lLblSF.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                    lLblSF.ForeColor = gColTealPrincipalSF;
                }
                else
                {
                    lLblSF.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
                    lLblSF.ForeColor = gColTextoSF;
                }
            }
            else if (pControlSF is Button lBtnSF)
            {
                EstilizarBoton(lBtnSF);
            }
            else if (pControlSF is DataGridView lDgvSF)
            {
                EstilizarGrilla(lDgvSF);
            }
            else if (pControlSF is TextBox || pControlSF is ComboBox || pControlSF is DateTimePicker || pControlSF is NumericUpDown)
            {
                pControlSF.Font = new Font("Segoe UI", 9.5F);
                pControlSF.ForeColor = gColTextoSF;
            }
            else if (pControlSF is Panel || pControlSF is FlowLayoutPanel || pControlSF is TableLayoutPanel)
            {
                EstilizarContenedor(pControlSF);
            }

            if (pControlSF.HasChildren)
            {
                foreach (Control lHijoSF in pControlSF.Controls)
                {
                    AplicarEstiloControl(lHijoSF);
                }
            }
        }

        private static void EstilizarContenedor(Control pContenedorSF)
        {
            switch (pContenedorSF.Name)
            {
                case "pnlEncabezado":
                    pContenedorSF.BackColor = gColMarinoPetroleoSF;
                    pContenedorSF.Paint += PintarLineaAcento;
                    pContenedorSF.Resize += (s, e) => pContenedorSF.Invalidate();
                    break;
                case "pnlLogo":
                    pContenedorSF.Paint += PintarDegradadoLogo;
                    pContenedorSF.Resize += (s, e) => pContenedorSF.Invalidate();
                    break;
                case "pnlFiltros":
                case "pnlResumen":
                case "pnlBotones":
                case "tblCampos":
                    pContenedorSF.BackColor = gColTarjetaSF;
                    break;
            }
        }

        private static void EstilizarBoton(Button pBtnSF)
        {
            pBtnSF.FlatStyle = FlatStyle.Flat;
            pBtnSF.FlatAppearance.BorderSize = 0;
            pBtnSF.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            //Centrar letras
            pBtnSF.TextAlign = ContentAlignment.MiddleCenter;
            pBtnSF.Padding = new Padding(0);
            pBtnSF.MinimumSize = new Size(0, 36);


            Color lColBaseSF = ObtenerColorBoton(pBtnSF);
            Color lColHoverSF = lColBaseSF == gColMarinoPetroleoSF ? gColTealPrincipalSF
                              : lColBaseSF == gColAvisoSF ? ControlPaint.Dark(gColAvisoSF, 0.1F)
                              : gColMarinoPetroleoSF;
            pBtnSF.FlatAppearance.MouseOverBackColor = lColHoverSF;
            pBtnSF.FlatAppearance.MouseDownBackColor = lColHoverSF;
            
            pBtnSF.EnabledChanged += (s, e) => ActualizarEstadoBoton(pBtnSF);
            ActualizarEstadoBoton(pBtnSF);
        }

        private static Color ObtenerColorBoton(Button pBtnSF)
        {
            if (pBtnSF.Name == "btnEliminar") return gColAvisoSF;
            if (pBtnSF.Name == "btnSalir") return gColMarinoPetroleoSF;
            if (pBtnSF.Name == "btnRegresar") return gColMarinoPetroleoSF;
            return gColTealPrincipalSF;
        }

        private static void ActualizarEstadoBoton(Button pBtnSF)
        {
            if (pBtnSF.Enabled)
            {
                pBtnSF.BackColor = ObtenerColorBoton(pBtnSF);
                pBtnSF.ForeColor = Color.White;
                pBtnSF.Cursor = Cursors.Hand;
            }
            else
            {
                pBtnSF.BackColor = gColFilaResaltadaSF;
                pBtnSF.ForeColor = Color.FromArgb(120, 140, 148);
                pBtnSF.Cursor = Cursors.Default;
            }
        }

        private static void EstilizarGrilla(DataGridView pDgvSF)
        {
            pDgvSF.BackgroundColor = gColTarjetaSF;
            pDgvSF.BorderStyle = BorderStyle.None;
            pDgvSF.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            pDgvSF.GridColor = gColFilaResaltadaSF;
            pDgvSF.EnableHeadersVisualStyles = false;
            pDgvSF.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            pDgvSF.ColumnHeadersDefaultCellStyle.BackColor = gColMarinoPetroleoSF;
            pDgvSF.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            pDgvSF.ColumnHeadersDefaultCellStyle.SelectionBackColor = gColMarinoPetroleoSF;
            pDgvSF.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            pDgvSF.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            pDgvSF.ColumnHeadersHeight = 38;
            pDgvSF.DefaultCellStyle.BackColor = gColTarjetaSF;
            pDgvSF.DefaultCellStyle.ForeColor = gColTextoSF;
            pDgvSF.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            pDgvSF.DefaultCellStyle.Padding = new Padding(4, 0, 4, 0);
            pDgvSF.DefaultCellStyle.SelectionBackColor = gColTealPrincipalSF;
            pDgvSF.DefaultCellStyle.SelectionForeColor = Color.White;
            pDgvSF.RowTemplate.Height = 30;
            // Alterna el color de las lineas.
            pDgvSF.AlternatingRowsDefaultCellStyle.BackColor = gColFondoPaginaSF;
            pDgvSF.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // Barra de color marino con una línea ámbar en la parte inferior
        private static void PintarLineaAcento(object? pObjSenderSF, PaintEventArgs pPeaSF)
        {
            if (pObjSenderSF is not Control lCtlSF) return;
            using (SolidBrush lBrhSF = new SolidBrush(gColAcentoAmbarSF))
            {
                pPeaSF.Graphics.FillRectangle(lBrhSF, 0, lCtlSF.Height - 3, lCtlSF.Width, 3);
            }
        }

        // Fondo con el degradado del logotipo y línea ámbar inferior
        private static void PintarDegradadoLogo(object? pObjSenderSF, PaintEventArgs pPeaSF)
        {
            if (pObjSenderSF is not Control lCtlSF || lCtlSF.Width <= 0 || lCtlSF.Height <= 0) return;
            using (LinearGradientBrush lBrhSF = new LinearGradientBrush(lCtlSF.ClientRectangle, gColDegradadoInicioSF, gColDegradadoFinSF, LinearGradientMode.Horizontal))
            {
                pPeaSF.Graphics.FillRectangle(lBrhSF, lCtlSF.ClientRectangle);
            }
            PintarLineaAcento(pObjSenderSF, pPeaSF);
        }

        public static void ResaltarFilasConPedido(DataGridView pDgvSF)
        {
            foreach (DataGridViewRow lFilaSF in pDgvSF.Rows)
            {
                if (lFilaSF.Cells["colCantidadPedidoSugerido"].Value != null)
                {
                    int lIntCantidadSF = System.Convert.ToInt32(lFilaSF.Cells["colCantidadPedidoSugerido"].Value);
                    if (lIntCantidadSF > 0)
                    {
                        lFilaSF.DefaultCellStyle.BackColor = gColFilaResaltadaSF;
                        lFilaSF.Cells["colCantidadPedidoSugerido"].Style.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                        lFilaSF.Cells["colCantidadPedidoSugerido"].Style.ForeColor = gColMarinoPetroleoSF;
                    }
                }
            }
        }
    }
}
