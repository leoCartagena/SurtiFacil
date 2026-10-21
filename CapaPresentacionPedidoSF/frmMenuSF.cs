using System;
using System.Drawing;
using System.Windows.Forms;

namespace CapaPresentacionPedidoSF
{
    public class frmMenuSF : Form
    {
        public frmMenuSF()
        {
            this.Text = "SurtiFácil - Gestión de Inventario";
            this.Size = new Size(480, 520);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            TableLayoutPanel lTblMenuSF = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 7 };
            lTblMenuSF.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));
            for (int i = 1; i <= 6; i++) lTblMenuSF.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));

            Panel lPnlLogoSF = new Panel { Name = "pnlLogo", Dock = DockStyle.Fill, Margin = new Padding(0) };
            Label lLblTituloSF = new Label { Name = "lblTitulo", Text = "SurtiFácil, S.A. - Gestión de Inventario", TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill };
            lPnlLogoSF.Controls.Add(lLblTituloSF);
            lTblMenuSF.Controls.Add(lPnlLogoSF, 0, 0);

            AgregarBoton(lTblMenuSF, "btnPedidoSugerido", "Pedido Sugerido", 1);
            AgregarBoton(lTblMenuSF, "btnProductos", "Productos", 2);
            AgregarBoton(lTblMenuSF, "btnCategorias", "Categorías", 3);
            AgregarBoton(lTblMenuSF, "btnMovimientos", "Movimientos de Inventario", 4);
            AgregarBoton(lTblMenuSF, "btnOrdenes", "Órdenes de Compra", 5);
            AgregarBoton(lTblMenuSF, "btnSalir", "Salir", 6);

            this.Controls.Add(lTblMenuSF);
            clsEstiloSF.AplicarEstilo(this);
        }

        private void AgregarBoton(TableLayoutPanel pTblMenuSF, string pStrNombreSF, string pStrTextoSF, int pIntFilaSF)
        {
            Button lBtnBotonSF = new Button { Name = pStrNombreSF, Text = pStrTextoSF, Size = new Size(320, 42), Anchor = AnchorStyles.None };
            lBtnBotonSF.Click += btnMenu_Click;
            pTblMenuSF.Controls.Add(lBtnBotonSF, 0, pIntFilaSF);
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            string lStrNombreBotonSF = ((Button)sender).Name;
            Form lFrmDestinoSF = null;

            switch (lStrNombreBotonSF)
            {
                case "btnPedidoSugerido": lFrmDestinoSF = new frmPedidoSugeridoSF(); break;
                case "btnProductos": lFrmDestinoSF = new frmProductosSF(); break;
                case "btnCategorias": lFrmDestinoSF = new frmCategoriasSF(); break;
                case "btnMovimientos": lFrmDestinoSF = new frmMovimientosSF(); break;
                case "btnOrdenes": lFrmDestinoSF = new frmOrdenesSF(); break;
                case "btnSalir": this.Close(); break;
            }

            if (lFrmDestinoSF != null)
            {
                //Cambio para ocultar
                //lFrmDestinoSF.ShowDialog(this);
                //lFrmDestinoSF.Dispose();
                //Incio del cambio
                this.Hide();
                lFrmDestinoSF.ShowDialog(this);
                lFrmDestinoSF.Dispose();
                this.Show();
            }
        }
    }
}
