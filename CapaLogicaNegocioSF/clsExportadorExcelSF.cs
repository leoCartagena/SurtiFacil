using System.Globalization;
using System.IO.Compression;
using System.Security;
using System.Text;
using CapaEntidadesSF;

namespace CapaLogicaNegocioSF
{
    public class clsExportadorExcelSF
    {
        private const string cStrTiposSF = "<?xml version='1.0' encoding='UTF-8' standalone='yes'?><Types xmlns='http://schemas.openxmlformats.org/package/2006/content-types'><Default Extension='rels' ContentType='application/vnd.openxmlformats-package.relationships+xml'/><Default Extension='xml' ContentType='application/xml'/><Override PartName='/xl/workbook.xml' ContentType='application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml'/><Override PartName='/xl/worksheets/sheet1.xml' ContentType='application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml'/><Override PartName='/xl/styles.xml' ContentType='application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml'/></Types>";
        private const string cStrRelacionesSF = "<?xml version='1.0' encoding='UTF-8' standalone='yes'?><Relationships xmlns='http://schemas.openxmlformats.org/package/2006/relationships'><Relationship Id='rId1' Type='http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument' Target='xl/workbook.xml'/></Relationships>";
        private const string cStrLibroSF = "<?xml version='1.0' encoding='UTF-8' standalone='yes'?><workbook xmlns='http://schemas.openxmlformats.org/spreadsheetml/2006/main' xmlns:r='http://schemas.openxmlformats.org/officeDocument/2006/relationships'><sheets><sheet name='Orden de Compra' sheetId='1' r:id='rId1'/></sheets></workbook>";
        private const string cStrRelacionesLibroSF = "<?xml version='1.0' encoding='UTF-8' standalone='yes'?><Relationships xmlns='http://schemas.openxmlformats.org/package/2006/relationships'><Relationship Id='rId1' Type='http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet' Target='worksheets/sheet1.xml'/><Relationship Id='rId2' Type='http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles' Target='styles.xml'/></Relationships>";
        private const string cStrEstilosSF = "<?xml version='1.0' encoding='UTF-8' standalone='yes'?><styleSheet xmlns='http://schemas.openxmlformats.org/spreadsheetml/2006/main'><fonts count='2'><font><sz val='11'/><name val='Calibri'/></font><font><b/><sz val='11'/><name val='Calibri'/></font></fonts><fills count='2'><fill><patternFill patternType='none'/></fill><fill><patternFill patternType='gray125'/></fill></fills><borders count='1'><border><left/><right/><top/><bottom/><diagonal/></border></borders><cellStyleXfs count='1'><xf numFmtId='0' fontId='0' fillId='0' borderId='0'/></cellStyleXfs><cellXfs count='4'><xf numFmtId='0' fontId='0' fillId='0' borderId='0' xfId='0'/><xf numFmtId='0' fontId='1' fillId='0' borderId='0' xfId='0' applyFont='1'/><xf numFmtId='4' fontId='0' fillId='0' borderId='0' xfId='0' applyNumberFormat='1'/><xf numFmtId='4' fontId='1' fillId='0' borderId='0' xfId='0' applyNumberFormat='1' applyFont='1'/></cellXfs><cellStyles count='1'><cellStyle name='Normal' xfId='0' builtinId='0'/></cellStyles></styleSheet>";

        public void ExportarOrden(clsOrdenCompraEN pEntOrdenSF, string pStrRutaSF)
        {
            using (FileStream lFsArchivoSF = new FileStream(pStrRutaSF, FileMode.Create))
            using (ZipArchive lZipSF = new ZipArchive(lFsArchivoSF, ZipArchiveMode.Create))
            {
                AgregarArchivo(lZipSF, "[Content_Types].xml", cStrTiposSF);
                AgregarArchivo(lZipSF, "_rels/.rels", cStrRelacionesSF);
                AgregarArchivo(lZipSF, "xl/workbook.xml", cStrLibroSF);
                AgregarArchivo(lZipSF, "xl/_rels/workbook.xml.rels", cStrRelacionesLibroSF);
                AgregarArchivo(lZipSF, "xl/styles.xml", cStrEstilosSF);
                AgregarArchivo(lZipSF, "xl/worksheets/sheet1.xml", CrearHoja(pEntOrdenSF));
            }
        }

        private string CrearHoja(clsOrdenCompraEN pEntOrdenSF)
        {
            StringBuilder lSbHojaSF = new StringBuilder();
            lSbHojaSF.Append("<?xml version='1.0' encoding='UTF-8' standalone='yes'?><worksheet xmlns='http://schemas.openxmlformats.org/spreadsheetml/2006/main'><cols><col min='1' max='1' width='14' customWidth='1'/><col min='2' max='2' width='30' customWidth='1'/><col min='3' max='5' width='16' customWidth='1'/></cols><sheetData>");
            lSbHojaSF.Append(CrearFila(1, CeldaTexto("A", 1, pEntOrdenSF.gStrNombreEmpresa, 1)));
            lSbHojaSF.Append(CrearFila(2, CeldaTexto("A", 2, "Orden de compra No. " + pEntOrdenSF.gIntIdOrden, 1)));
            lSbHojaSF.Append(CrearFila(3, CeldaTexto("A", 3, "Fecha de análisis:", 0), CeldaTexto("B", 3, pEntOrdenSF.gDteFechaAnalisis.ToString("dd/MM/yyyy"), 0)));
            lSbHojaSF.Append(CrearFila(5, CeldaTexto("A", 5, "IdProducto", 1), CeldaTexto("B", 5, "NombreProducto", 1), CeldaTexto("C", 5, "Cantidad", 1), CeldaTexto("D", 5, "CostoUnitario", 1), CeldaTexto("E", 5, "Subtotal", 1)));

            int lIntFilaSF = 6;
            foreach (clsPedidoSugeridoEN lEntDetalleSF in pEntOrdenSF.gLstDetalles)
            {
                lSbHojaSF.Append(CrearFila(lIntFilaSF, CeldaNumero("A", lIntFilaSF, lEntDetalleSF.gIntIdProducto, 0), CeldaTexto("B", lIntFilaSF, lEntDetalleSF.gStrNombreProducto, 0), CeldaNumero("C", lIntFilaSF, lEntDetalleSF.gIntCantidadPedidoSugerido, 0), CeldaNumero("D", lIntFilaSF, lEntDetalleSF.gDecCostoUnitario, 2), CeldaNumero("E", lIntFilaSF, lEntDetalleSF.gDecSubtotalSugerido, 2)));
                lIntFilaSF++;
            }

            lSbHojaSF.Append(CrearFila(lIntFilaSF, CeldaTexto("D", lIntFilaSF, "TOTAL", 1), CeldaNumero("E", lIntFilaSF, pEntOrdenSF.gDecTotalAproximado, 3)));
            lSbHojaSF.Append("</sheetData></worksheet>");
            return lSbHojaSF.ToString();
        }

        private string CrearFila(int pIntFilaSF, params string[] pStrCeldasSF) => "<row r='" + pIntFilaSF + "'>" + string.Concat(pStrCeldasSF) + "</row>";
        private string CeldaTexto(string pStrColumnaSF, int pIntFilaSF, string pStrTextoSF, int pIntEstiloSF) => "<c r='" + pStrColumnaSF + pIntFilaSF + "' s='" + pIntEstiloSF + "' t='inlineStr'><is><t>" + SecurityElement.Escape(pStrTextoSF) + "</t></is></c>";
        private string CeldaNumero(string pStrColumnaSF, int pIntFilaSF, decimal pDecValorSF, int pIntEstiloSF) => "<c r='" + pStrColumnaSF + pIntFilaSF + "' s='" + pIntEstiloSF + "'><v>" + pDecValorSF.ToString(CultureInfo.InvariantCulture) + "</v></c>";

        private void AgregarArchivo(ZipArchive pZipSF, string pStrNombreSF, string pStrContenidoSF)
        {
            ZipArchiveEntry lEntradaSF = pZipSF.CreateEntry(pStrNombreSF);
            using (Stream lStreamSF = lEntradaSF.Open())
            using (StreamWriter lWriterSF = new StreamWriter(lStreamSF, new UTF8Encoding(false)))
            {
                lWriterSF.Write(pStrContenidoSF);
            }
        }
    }
}