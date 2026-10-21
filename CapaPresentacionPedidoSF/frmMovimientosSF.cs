using System;
using System.Windows.Forms;
using CapaEntidadesSF;
using CapaLogicaNegocioSF;

namespace CapaPresentacionPedidoSF
{
    public class frmMovimientosSF : frmCrudBaseSF
    {
        private clsMovimientoLN gMovimientoLN = new clsMovimientoLN();
        private clsProductoLN gProductoLN = new clsProductoLN();

        private ComboBox cmbProducto, cmbTipo;
        private NumericUpDown nudCantidad;
        private DateTimePicker dtpFecha;

        public frmMovimientosSF() : base("Movimientos de Inventario")
        {
            cmbProducto = new ComboBox { Name = "cmbProducto", DropDownStyle = ComboBoxStyle.DropDownList };
            cmbTipo = new ComboBox { Name = "cmbTipo", DropDownStyle = ComboBoxStyle.DropDownList };
            cmbTipo.Items.AddRange(new object[] { "Venta", "Entrada", "Salida" });
            nudCantidad = CrearNumerico("nudCantidad", 0);
            dtpFecha = new DateTimePicker { Name = "dtpFecha", Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy HH:mm" };

            AgregarCampo("Producto:", cmbProducto);
            AgregarCampo("Tipo movimiento:", cmbTipo);
            AgregarCampo("Cantidad:", nudCantidad);
            AgregarCampo("Fecha y hora:", dtpFecha);

            AgregarColumna(dgvDatos, "IdMovimiento", "gIntIdMovimiento", "");
            AgregarColumna(dgvDatos, "Producto", "gStrNombreProducto", "");
            AgregarColumna(dgvDatos, "Tipo Movimiento", "gStrTipoMovimiento", "");
            AgregarColumna(dgvDatos, "Cantidad", "gIntCantidad", "N0");
            AgregarColumna(dgvDatos, "Fecha y Hora", "gDteFechaMovimiento", "dd/MM/yyyy HH:mm");
        }

        protected override void PrepararFormulario()
        {
            CargarComboProductos();
        }

        private void CargarComboProductos()
        {
            cmbProducto.DataSource = null;
            cmbProducto.DisplayMember = "gStrNombre";
            cmbProducto.ValueMember = "gIntIdProducto";
            cmbProducto.DataSource = gProductoLN.ListarProductos();
        }

        protected override object ObtenerDatos() => gMovimientoLN.ListarMovimientos();

        protected override void MostrarSeleccion()
        {
            clsMovimientoEN m = (clsMovimientoEN)ObtenerSeleccionado();
            if (m == null) return;

            CargarComboProductos();
            cmbProducto.SelectedValue = m.gIntIdProducto;
            cmbTipo.Text = m.gStrTipoMovimiento;
            nudCantidad.Value = m.gIntCantidad;
            dtpFecha.Value = m.gDteFechaMovimiento;
            cmbProducto.Enabled = false;
        }

        protected override void LimpiarCampos()
        {
            cmbProducto.Enabled = true;
            CargarComboProductos();
            if (cmbProducto.Items.Count > 0) cmbProducto.SelectedIndex = 0;
            cmbTipo.SelectedIndex = 0;
            nudCantidad.Value = 1;
            dtpFecha.Value = DateTime.Now;
        }

        protected override void GuardarRegistro()
        {
            if (cmbProducto.SelectedValue == null)
            {
                throw new InvalidOperationException("Debe seleccionar un producto.");
            }

            clsMovimientoEN m = new clsMovimientoEN
            {
                gIntIdProducto = Convert.ToInt32(cmbProducto.SelectedValue),
                gStrTipoMovimiento = cmbTipo.Text,
                gIntCantidad = (int)nudCantidad.Value,
                gDteFechaMovimiento = dtpFecha.Value
            };

            if (gBolNuevo)
            {
                gMovimientoLN.InsertarMovimiento(m);
            }
            else
            {
                m.gIntIdMovimiento = ((clsMovimientoEN)ObtenerSeleccionado()).gIntIdMovimiento;
                gMovimientoLN.ActualizarMovimiento(m);
            }
        }

        protected override void EliminarRegistro()
        {
            clsMovimientoEN m = (clsMovimientoEN)ObtenerSeleccionado();
            if (m != null)
            {
                gMovimientoLN.EliminarMovimiento(m.gIntIdMovimiento);
            }
        }
    }
}