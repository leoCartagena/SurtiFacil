using System;
using System.Windows.Forms;
using CapaEntidadesSF;
using CapaLogicaNegocioSF;

namespace CapaPresentacionPedidoSF
{
    public class frmProductosSF : frmCrudBaseSF
    {
        private clsProductoLN gProductoLN = new clsProductoLN();
        private clsCategoriaLN gCategoriaLN = new clsCategoriaLN();

        private TextBox txtNombre;
        private ComboBox cmbCategoria;
        private NumericUpDown nudStock, nudCosto, nudEntrega, nudCobertura, nudSeguridad;

        public frmProductosSF() : base("Gestión de Productos")
        {
            txtNombre = new TextBox { Name = "txtNombre", MaxLength = 100 };
            cmbCategoria = new ComboBox { Name = "cmbCategoria", DropDownStyle = ComboBoxStyle.DropDownList };
            nudStock = CrearNumerico("nudStock", 0);
            nudCosto = CrearNumerico("nudCosto", 2);
            nudEntrega = CrearNumerico("nudEntrega", 0);
            nudCobertura = CrearNumerico("nudCobertura", 0);
            nudSeguridad = CrearNumerico("nudSeguridad", 0);

            AgregarCampo("Nombre:", txtNombre);
            AgregarCampo("Categoría:", cmbCategoria);
            AgregarCampo("Stock actual:", nudStock);
            AgregarCampo("Costo unitario:", nudCosto);
            AgregarCampo("Días entrega proveedor:", nudEntrega);
            AgregarCampo("Días cobertura objetivo:", nudCobertura);
            AgregarCampo("Stock seguridad:", nudSeguridad);

            AgregarColumna(dgvDatos, "IdProducto", "gIntIdProducto", "");
            AgregarColumna(dgvDatos, "Nombre", "gStrNombre", "");
            AgregarColumna(dgvDatos, "Categoría", "gStrNombreCategoria", "");
            AgregarColumna(dgvDatos, "Stock Actual", "gIntStockActual", "N0");
            AgregarColumna(dgvDatos, "Costo Unitario", "gDecCostoUnitario", "Q #,##0.00");
        }

        protected override void PrepararFormulario()
        {
            CargarComboCategorias();
        }

        private void CargarComboCategorias()
        {
            cmbCategoria.DataSource = null;
            cmbCategoria.DisplayMember = "gStrNombreCategoria";
            cmbCategoria.ValueMember = "gIntIdCategoria";
            cmbCategoria.DataSource = gCategoriaLN.ListarCategorias();
        }

        protected override object ObtenerDatos() => gProductoLN.ListarProductos();

        protected override void MostrarSeleccion()
        {
            clsProductoEN p = (clsProductoEN)ObtenerSeleccionado();
            if (p == null) return;

            txtNombre.Text = p.gStrNombre;
            CargarComboCategorias();
            cmbCategoria.SelectedValue = p.gIntIdCategoria;
            nudStock.Value = p.gIntStockActual;
            nudCosto.Value = p.gDecCostoUnitario;
            nudEntrega.Value = p.gIntTiempoEntregaDias;
            nudCobertura.Value = p.gIntDiasCobertura;
            nudSeguridad.Value = p.gIntStockSeguridad;
        }

        protected override void LimpiarCampos()
        {
            txtNombre.Text = "";
            CargarComboCategorias();
            if (cmbCategoria.Items.Count > 0) cmbCategoria.SelectedIndex = 0;
            nudStock.Value = 0;
            nudCosto.Value = 0;
            nudEntrega.Value = 5;
            nudCobertura.Value = 30;
            nudSeguridad.Value = 10;
        }

        protected override void GuardarRegistro()
        {
            if (cmbCategoria.SelectedValue == null)
            {
                throw new InvalidOperationException("Debe seleccionar una categoría válida.");
            }

            clsProductoEN p = new clsProductoEN
            {
                gStrNombre = txtNombre.Text,
                gIntIdCategoria = Convert.ToInt32(cmbCategoria.SelectedValue),
                gIntStockActual = (int)nudStock.Value,
                gDecCostoUnitario = nudCosto.Value,
                gIntTiempoEntregaDias = (int)nudEntrega.Value,
                gIntDiasCobertura = (int)nudCobertura.Value,
                gIntStockSeguridad = (int)nudSeguridad.Value
            };

            if (gBolNuevo)
            {
                gProductoLN.InsertarProducto(p);
            }
            else
            {
                p.gIntIdProducto = ((clsProductoEN)ObtenerSeleccionado()).gIntIdProducto;
                gProductoLN.ActualizarProducto(p);
            }
        }

        protected override void EliminarRegistro()
        {
            clsProductoEN p = (clsProductoEN)ObtenerSeleccionado();
            if (p != null)
            {
                gProductoLN.EliminarProducto(p.gIntIdProducto);
            }
        }
    }
}