using System;
using System.Collections.Generic;
using System.Text;

namespace CapaEntidadesSF
{
    /// <summary>
    /// Categoría de productos
    /// </summary>
    public class clsCategoriaEN
    {
        /// <summary>Número de la categoría</summary>
        public int gIntIdCategoria { get; set; }

        /// <summary>Nombre que se ve en el ComboBox</summary>
        public string gStrNombreCategoria { get; set; }

        /// <summary>
        /// Constructor vacío, deja los valores iniciales
        /// </summary>
        public clsCategoriaEN() : this(0, "") { }

        /// <summary>
        /// Constructor que recibe todos los datos
        /// </summary>
        public clsCategoriaEN(int pIntIdCategoriaSF, string pStrNombreCategoriaSF)
        {
            this.gIntIdCategoria = pIntIdCategoriaSF;
            this.gStrNombreCategoria = pStrNombreCategoriaSF;
        }
    }
}
