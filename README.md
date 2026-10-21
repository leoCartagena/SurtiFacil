# SurtiFácil - Módulo de Pedido Sugerido (CU-01)

Proyecto académico desarrollado en C# (.NET) y SQL Server Express estructurado en 3 capas.

## Estructura del Diseño

El diseño de la interfaz visual se encuentra construido por código (sin diseñador drag-and-drop de VS) para permitir mantenibilidad y adaptabilidad modular:

- **Colores y Fuentes:** Modificar la clase `clsEstilosSF.cs` en la capa de presentación.
- **Estructura y Controles:** Modificar la clase parcial `frmPedidoSugeridoSF.Diseno.cs`.
- **Lógica de Interfaz y Eventos:** Modificar la clase parcial `frmPedidoSugeridoSF.cs`.

## Requisitos de Entorno

1. SQL Server / SQL Express ejecutándose en localhost.
2. Servidor SQL accesible para la cadena de conexión del proyecto.

## Instrucciones para Restaurar la Base de Datos

Si tienes una versión antigua del proyecto, ejecuta esta sentencia en SQL Server Management Studio para borrar la base de datos previa:

```sql
USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = 'SurtiFacilDB')
BEGIN
    ALTER DATABASE SurtiFacilDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE SurtiFacilDB;
END
GO

