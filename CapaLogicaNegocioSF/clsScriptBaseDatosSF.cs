namespace CapaLogicaNegocioSF
{
    public static class clsScriptBaseDatosSF
    {
        public const string cStrScriptSF = @"
CREATE DATABASE SurtiFacilDB;
GO
USE SurtiFacilDB;
GO

CREATE TABLE Categoria (
    IdCategoria INT PRIMARY KEY,
    NombreCategoria VARCHAR(100) NOT NULL
);
GO

CREATE TABLE Productos (
    IdProducto INT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    StockActual INT NOT NULL,
    CostoUnitario DECIMAL(10,2) NOT NULL,
    IdCategoria INT FOREIGN KEY REFERENCES Categoria(IdCategoria)
);
GO

CREATE TABLE ParametrosReposicion (
    IdParametro INT PRIMARY KEY,
    IdProducto INT FOREIGN KEY REFERENCES Productos(IdProducto),
    TiempoEntregaProveedorDias INT NOT NULL,
    DiasCoberturaObjetivo INT NOT NULL,
    StockSeguridad INT NOT NULL
);
GO

CREATE TABLE MovimientosInventario (
    IdMovimiento INT PRIMARY KEY,
    IdProducto INT FOREIGN KEY REFERENCES Productos(IdProducto),
    TipoMovimiento VARCHAR(20) NOT NULL,
    Cantidad INT NOT NULL,
    FechaMovimiento DATETIME NOT NULL
);
GO

CREATE TABLE OrdenCompra (
    IdOrden INT IDENTITY(1,1) PRIMARY KEY,
    NombreEmpresa VARCHAR(100) NOT NULL,
    FechaOrden DATETIME NOT NULL DEFAULT GETDATE(),
    FechaAnalisis DATE NOT NULL,
    TotalAproximado DECIMAL(12,2) NOT NULL,
    Estado VARCHAR(20) NOT NULL DEFAULT 'Pendiente'
);
GO

CREATE TABLE DetalleOrdenCompra (
    IdDetalle INT IDENTITY(1,1) PRIMARY KEY,
    IdOrden INT NOT NULL FOREIGN KEY REFERENCES OrdenCompra(IdOrden),
    IdProducto INT NOT NULL FOREIGN KEY REFERENCES Productos(IdProducto),
    CantidadSugerida INT NOT NULL,
    CostoUnitario DECIMAL(10,2) NOT NULL,
    Subtotal DECIMAL(12,2) NOT NULL
);
GO

INSERT INTO Categoria (IdCategoria, NombreCategoria) VALUES
(1, 'Abarrotes'), (2, 'Limpieza'), (3, 'Perecederos'), (4, 'Bebidas'), (5, 'Cuidado Personal');
GO

INSERT INTO Productos (IdProducto, Nombre, StockActual, CostoUnitario, IdCategoria) VALUES
(1, 'Arroz Extra', 10, 18.50, 1), (2, 'Frijol Negro', 12, 14.25, 1), (3, 'Aceite Vegetal', 5, 22.00, 1),
(4, 'Azúcar Refinada', 15, 12.10, 1), (5, 'Harina de Trigo', 8, 9.80, 1), (6, 'Desengrasante Líquido', 4, 25.00, 2),
(7, 'Jabón en Polvo', 15, 45.50, 2), (8, 'Cloro Desinfectante', 8, 15.00, 2), (9, 'Lavatrastos', 10, 18.25, 2),
(10, 'Suavizante de Telas', 6, 32.00, 2), (11, 'Leche Entera', 8, 11.50, 3), (12, 'Queso Fresco', 3, 35.00, 3),
(13, 'Yogurt Natural', 10, 16.80, 3), (14, 'Mantequilla', 2, 20.00, 3), (15, 'Crema Pura', 5, 28.50, 3),
(16, 'Agua Pura 600ml', 25, 5.00, 4), (17, 'Refresco en Polvo', 20, 3.50, 4), (18, 'Jugo de Naranja', 12, 24.00, 4),
(19, 'Bebida Energética', 15, 19.90, 4), (20, 'Té Helado', 10, 15.50, 4);
GO

INSERT INTO ParametrosReposicion (IdParametro, IdProducto, TiempoEntregaProveedorDias, DiasCoberturaObjetivo, StockSeguridad) VALUES
(1, 1, 5, 30, 15), (2, 2, 7, 30, 20), (3, 3, 4, 15, 10), (4, 4, 10, 45, 25), (5, 5, 5, 30, 12),
(6, 6, 6, 20, 8), (7, 7, 8, 30, 18), (8, 8, 3, 15, 10), (9, 9, 5, 30, 14), (10, 10, 7, 25, 12),
(11, 11, 2, 10, 15), (12, 12, 3, 7, 10), (13, 13, 4, 15, 12), (14, 14, 3, 10, 8), (15, 15, 4, 12, 10),
(16, 16, 5, 30, 30), (17, 17, 6, 30, 25), (18, 18, 5, 20, 15), (19, 19, 4, 15, 18), (20, 20, 5, 20, 12);
GO

INSERT INTO MovimientosInventario (IdMovimiento, IdProducto, TipoMovimiento, Cantidad, FechaMovimiento) VALUES
(1, 14, 'Venta', 8, '2025-10-05 10:30:00'), (2, 3, 'Entrada', 50, '2025-10-08 08:15:00'), (3, 19, 'Venta', 12, '2025-10-10 14:20:00'),
(4, 7, 'Salida', 5, '2025-10-12 09:00:00'), (5, 20, 'Venta', 15, '2025-10-15 16:45:00'), (6, 1, 'Venta', 20, '2025-10-18 11:10:00'),
(7, 11, 'Venta', 10, '2025-10-20 07:30:00'), (8, 5, 'Entrada', 100, '2025-10-22 13:00:00'), (9, 9, 'Venta', 6, '2025-10-25 15:15:00'),
(10, 16, 'Venta', 25, '2025-10-28 12:00:00'), (11, 2, 'Venta', 14, '2025-11-02 09:10:00'), (12, 4, 'Venta', 30, '2025-11-04 11:25:00'),
(13, 6, 'Venta', 8, '2025-11-06 14:40:00'), (14, 8, 'Entrada', 60, '2025-11-08 08:00:00'), (15, 10, 'Venta', 5, '2025-11-10 16:15:00'),
(16, 12, 'Venta', 12, '2025-11-12 10:05:00'), (17, 13, 'Venta', 18, '2025-11-15 13:50:00'), (18, 15, 'Venta', 9, '2025-11-18 15:30:00'),
(19, 17, 'Venta', 40, '2025-11-20 17:00:00'), (20, 18, 'Venta', 22, '2025-11-22 12:10:00'), (21, 1, 'Entrada', 150, '2025-11-25 08:30:00'),
(22, 3, 'Venta', 11, '2025-11-28 10:45:00'), (23, 5, 'Venta', 15, '2025-12-01 14:00:00'), (24, 7, 'Venta', 25, '2025-12-03 16:20:00'),
(25, 9, 'Venta', 10, '2025-12-05 09:35:00'), (26, 11, 'Venta', 15, '2025-12-08 11:50:00'), (27, 14, 'Venta', 7, '2025-12-10 15:05:00'),
(28, 16, 'Venta', 35, '2025-12-12 18:00:00'), (29, 19, 'Venta', 20, '2025-12-15 13:15:00'), (30, 20, 'Venta', 14, '2025-12-18 10:40:00'),
(31, 2, 'Entrada', 80, '2025-12-20 08:00:00'), (32, 4, 'Venta', 25, '2025-12-22 12:30:00'), (33, 6, 'Venta', 11, '2025-12-24 16:10:00'),
(34, 8, 'Venta', 18, '2025-12-27 11:00:00'), (35, 10, 'Venta', 9, '2025-12-29 14:25:00'), (36, 12, 'Venta', 15, '2026-01-02 09:15:00'),
(37, 13, 'Venta', 20, '2026-01-05 12:40:00'), (38, 15, 'Venta', 8, '2026-01-08 15:55:00'), (39, 17, 'Venta', 30, '2026-01-10 17:30:00'),
(40, 18, 'Venta', 16, '2026-01-12 10:20:00'), (41, 1, 'Venta', 22, '2026-01-15 11:10:00'), (42, 3, 'Venta', 14, '2026-01-18 14:35:00'),
(43, 5, 'Venta', 18, '2026-01-20 16:50:00'), (44, 7, 'Entrada', 100, '2026-01-22 08:15:00'), (45, 9, 'Venta', 12, '2026-01-25 10:00:00'),
(46, 11, 'Venta', 19, '2026-01-28 13:20:00'), (47, 14, 'Venta', 10, '2026-01-30 15:45:00'), (48, 16, 'Venta', 45, '2026-02-02 18:10:00'),
(49, 19, 'Venta', 18, '2026-02-05 11:25:00'), (50, 20, 'Venta', 12, '2026-02-08 09:40:00'), (51, 2, 'Venta', 16, '2026-02-10 12:05:00'),
(52, 4, 'Venta', 28, '2026-02-12 14:30:00'), (53, 6, 'Venta', 7, '2026-02-15 16:55:00'), (54, 8, 'Venta', 22, '2026-02-18 10:15:00'),
(55, 10, 'Entrada', 50, '2026-02-20 08:30:00'), (56, 12, 'Venta', 10, '2026-02-22 11:45:00'), (57, 13, 'Venta', 14, '2026-02-25 14:10:00'),
(58, 15, 'Venta', 11, '2026-02-28 16:25:00'), (59, 17, 'Venta', 35, '2026-03-02 09:50:00'), (60, 18, 'Venta', 20, '2026-03-05 12:15:00'),
(61, 1, 'Venta', 25, '2026-03-08 15:00:00'), (62, 3, 'Entrada', 40, '2026-03-10 08:00:00'), (63, 5, 'Venta', 12, '2026-03-12 10:30:00'),
(64, 7, 'Venta', 20, '2026-03-15 13:45:00'), (65, 9, 'Venta', 14, '2026-03-18 16:00:00'), (66, 11, 'Venta', 16, '2026-03-20 11:15:00'),
(67, 14, 'Venta', 9, '2026-03-22 14:30:00'), (68, 16, 'Venta', 50, '2026-03-25 17:45:00'), (69, 19, 'Venta', 15, '2026-03-28 10:00:00'),
(70, 20, 'Venta', 11, '2026-03-30 12:20:00'), (71, 2, 'Venta', 18, '2026-04-02 15:10:00'), (72, 4, 'Venta', 32, '2026-04-05 09:25:00'),
(73, 6, 'Venta', 10, '2026-04-08 11:40:00'), (74, 8, 'Venta', 25, '2026-04-10 14:00:00'), (75, 10, 'Venta', 8, '2026-04-12 16:15:00'),
(76, 12, 'Entrada', 30, '2026-04-15 08:30:00'), (77, 13, 'Venta', 17, '2026-04-18 10:50:00'), (78, 15, 'Venta', 13, '2026-04-20 13:05:00'),
(79, 17, 'Venta', 28, '2026-04-22 15:20:00'), (80, 18, 'Venta', 19, '2026-04-25 17:35:00'), (81, 1, 'Venta', 30, '2026-04-28 10:10:00'),
(82, 3, 'Venta', 16, '2026-05-02 12:25:00'), (83, 5, 'Venta', 20, '2026-05-05 14:40:00'), (84, 7, 'Venta', 22, '2026-05-08 16:55:00'),
(85, 9, 'Venta', 11, '2026-05-10 09:10:00'), (86, 11, 'Entrada', 60, '2026-05-12 08:00:00'), (87, 14, 'Venta', 8, '2026-05-15 11:30:00'),
(88, 16, 'Venta', 40, '2026-05-18 13:45:00'), (89, 19, 'Venta', 22, '2026-05-20 16:00:00'), (90, 20, 'Venta', 13, '2026-05-22 10:20:00'),
(91, 2, 'Venta', 21, '2026-05-25 12:35:00'), (92, 4, 'Venta', 35, '2026-05-28 14:50:00'), (93, 6, 'Venta', 9, '2026-06-01 17:05:00'),
(94, 8, 'Venta', 20, '2026-06-03 09:30:00'), (95, 10, 'Venta', 12, '2026-06-05 11:45:00'), (96, 12, 'Venta', 14, '2026-06-08 14:00:00'),
(97, 13, 'Venta', 19, '2026-06-10 16:15:00'), (98, 15, 'Entrada', 45, '2026-06-12 08:15:00'), (99, 17, 'Venta', 32, '2026-06-15 10:40:00'),
(100, 18, 'Venta', 17, '2026-06-18 13:00:00'), (101, 1, 'Venta', 28, '2026-06-20 15:15:00'), (102, 3, 'Venta', 18, '2026-06-22 17:30:00'),
(103, 5, 'Venta', 14, '2026-06-25 09:45:00'), (104, 7, 'Venta', 24, '2026-06-28 12:00:00'), (105, 9, 'Venta', 13, '2026-07-01 14:15:00'),
(106, 11, 'Venta', 18, '2026-07-03 16:30:00'), (107, 14, 'Venta', 11, '2026-07-05 10:00:00'), (108, 16, 'Entrada', 200, '2026-07-08 08:00:00'),
(109, 19, 'Venta', 24, '2026-07-10 12:20:00'), (110, 20, 'Venta', 16, '2026-07-12 14:35:00'), (111, 2, 'Venta', 19, '2026-07-15 16:50:00'),
(112, 4, 'Venta', 38, '2026-07-18 09:10:00'), (113, 6, 'Venta', 12, '2026-07-20 11:25:00'), (114, 8, 'Venta', 23, '2026-07-22 13:40:00'),
(115, 10, 'Venta', 10, '2026-07-25 15:55:00'), (116, 12, 'Venta', 11, '2026-07-28 18:10:00'), (117, 13, 'Venta', 21, '2026-08-01 10:30:00'),
(118, 15, 'Venta', 15, '2026-08-03 12:45:00'), (119, 17, 'Venta', 36, '2026-08-05 15:00:00'), (120, 18, 'Venta', 21, '2026-08-08 17:15:00');
GO

CREATE PROCEDURE spListarCategorias
AS
BEGIN
    SELECT IdCategoria, NombreCategoria FROM Categoria ORDER BY NombreCategoria;
END
GO

CREATE PROCEDURE spObtenerDatosPedido
    @FechaAnalisis DATE,
    @IdCategoria INT = 0,
    @DiasHistorico INT = 365
AS
BEGIN
    SELECT
        p.IdProducto, p.Nombre, p.StockActual, p.CostoUnitario,
        pr.TiempoEntregaProveedorDias, pr.DiasCoberturaObjetivo, pr.StockSeguridad,
        ISNULL(SUM(m.Cantidad), 0) AS UnidadesVendidas
    FROM Productos p
    INNER JOIN ParametrosReposicion pr ON pr.IdProducto = p.IdProducto
    LEFT JOIN MovimientosInventario m
        ON m.IdProducto = p.IdProducto AND m.TipoMovimiento = 'Venta'
        AND m.FechaMovimiento >= DATEADD(DAY, -@DiasHistorico, @FechaAnalisis)
        AND m.FechaMovimiento < @FechaAnalisis
    WHERE @IdCategoria = 0 OR p.IdCategoria = @IdCategoria
    GROUP BY p.IdProducto, p.Nombre, p.StockActual, p.CostoUnitario,
             pr.TiempoEntregaProveedorDias, pr.DiasCoberturaObjetivo, pr.StockSeguridad
    ORDER BY p.IdProducto;
END
GO

CREATE PROCEDURE spInsertarOrdenCompra
    @NombreEmpresa VARCHAR(100), @FechaAnalisis DATE, @TotalAproximado DECIMAL(12,2)
AS
BEGIN
    INSERT INTO OrdenCompra (NombreEmpresa, FechaAnalisis, TotalAproximado)
    VALUES (@NombreEmpresa, @FechaAnalisis, @TotalAproximado);
    SELECT CAST(SCOPE_IDENTITY() AS INT);
END
GO

CREATE PROCEDURE spInsertarDetalleOrdenCompra
    @IdOrden INT, @IdProducto INT, @CantidadSugerida INT, @CostoUnitario DECIMAL(10,2), @Subtotal DECIMAL(12,2)
AS
BEGIN
    INSERT INTO DetalleOrdenCompra (IdOrden, IdProducto, CantidadSugerida, CostoUnitario, Subtotal)
    VALUES (@IdOrden, @IdProducto, @CantidadSugerida, @CostoUnitario, @Subtotal);
END
GO

CREATE PROCEDURE spInsertarCategoria @NombreCategoria VARCHAR(100)
AS
BEGIN
    SET XACT_ABORT ON;
    IF EXISTS (SELECT 1 FROM Categoria WHERE NombreCategoria = @NombreCategoria)
        THROW 50001, 'Ya existe una categoría con ese nombre.', 1;

    BEGIN TRANSACTION;
        DECLARE @IdNuevo INT = (SELECT ISNULL(MAX(IdCategoria), 0) + 1 FROM Categoria WITH (UPDLOCK, HOLDLOCK));
        INSERT INTO Categoria (IdCategoria, NombreCategoria) VALUES (@IdNuevo, @NombreCategoria);
    COMMIT TRANSACTION;
    SELECT @IdNuevo;
END
GO

CREATE PROCEDURE spActualizarCategoria @IdCategoria INT, @NombreCategoria VARCHAR(100)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Categoria WHERE NombreCategoria = @NombreCategoria AND IdCategoria <> @IdCategoria)
        THROW 50001, 'Ya existe una categoría con ese nombre.', 1;
    UPDATE Categoria SET NombreCategoria = @NombreCategoria WHERE IdCategoria = @IdCategoria;
END
GO

CREATE PROCEDURE spEliminarCategoria @IdCategoria INT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Productos WHERE IdCategoria = @IdCategoria)
        THROW 50002, 'No se puede eliminar la categoría porque hay productos asignados a ella.', 1;
    DELETE FROM Categoria WHERE IdCategoria = @IdCategoria;
END
GO

CREATE PROCEDURE spListarProductos
AS
BEGIN
    SELECT p.IdProducto, p.Nombre, p.StockActual, p.CostoUnitario,
           ISNULL(p.IdCategoria, 0) AS IdCategoria, ISNULL(c.NombreCategoria, 'Sin categoría') AS NombreCategoria,
           ISNULL(pr.TiempoEntregaProveedorDias, 0) AS TiempoEntregaProveedorDias,
           ISNULL(pr.DiasCoberturaObjetivo, 0) AS DiasCoberturaObjetivo,
           ISNULL(pr.StockSeguridad, 0) AS StockSeguridad
    FROM Productos p
    LEFT JOIN Categoria c ON c.IdCategoria = p.IdCategoria
    LEFT JOIN ParametrosReposicion pr ON pr.IdProducto = p.IdProducto
    ORDER BY p.IdProducto;
END
GO

CREATE PROCEDURE spInsertarProducto
    @Nombre VARCHAR(100), @StockActual INT, @CostoUnitario DECIMAL(10,2), @IdCategoria INT,
    @TiempoEntrega INT, @DiasCobertura INT, @StockSeguridad INT
AS
BEGIN
    SET XACT_ABORT ON;
    BEGIN TRANSACTION;
        DECLARE @IdNuevo INT = (SELECT ISNULL(MAX(IdProducto), 0) + 1 FROM Productos WITH (UPDLOCK, HOLDLOCK));
        DECLARE @IdParametro INT = (SELECT ISNULL(MAX(IdParametro), 0) + 1 FROM ParametrosReposicion WITH (UPDLOCK, HOLDLOCK));

        INSERT INTO Productos (IdProducto, Nombre, StockActual, CostoUnitario, IdCategoria)
        VALUES (@IdNuevo, @Nombre, @StockActual, @CostoUnitario, @IdCategoria);

        INSERT INTO ParametrosReposicion (IdParametro, IdProducto, TiempoEntregaProveedorDias, DiasCoberturaObjetivo, StockSeguridad)
        VALUES (@IdParametro, @IdNuevo, @TiempoEntrega, @DiasCobertura, @StockSeguridad);
    COMMIT TRANSACTION;
    SELECT @IdNuevo;
END
GO

CREATE PROCEDURE spActualizarProducto
    @IdProducto INT, @Nombre VARCHAR(100), @StockActual INT, @CostoUnitario DECIMAL(10,2), @IdCategoria INT,
    @TiempoEntrega INT, @DiasCobertura INT, @StockSeguridad INT
AS
BEGIN
    SET XACT_ABORT ON;
    BEGIN TRANSACTION;
        UPDATE Productos
        SET Nombre = @Nombre, StockActual = @StockActual, CostoUnitario = @CostoUnitario, IdCategoria = @IdCategoria
        WHERE IdProducto = @IdProducto;

        UPDATE ParametrosReposicion
        SET TiempoEntregaProveedorDias = @TiempoEntrega, DiasCoberturaObjetivo = @DiasCobertura, StockSeguridad = @StockSeguridad
        WHERE IdProducto = @IdProducto;

        IF @@ROWCOUNT = 0
            INSERT INTO ParametrosReposicion (IdParametro, IdProducto, TiempoEntregaProveedorDias, DiasCoberturaObjetivo, StockSeguridad)
            SELECT ISNULL(MAX(IdParametro), 0) + 1, @IdProducto, @TiempoEntrega, @DiasCobertura, @StockSeguridad
            FROM ParametrosReposicion;
    COMMIT TRANSACTION;
END
GO

CREATE PROCEDURE spEliminarProducto @IdProducto INT
AS
BEGIN
    SET XACT_ABORT ON;
    IF EXISTS (SELECT 1 FROM MovimientosInventario WHERE IdProducto = @IdProducto)
        THROW 50010, 'No se puede eliminar el producto porque tiene movimientos de inventario registrados.', 1;

    IF EXISTS (SELECT 1 FROM DetalleOrdenCompra WHERE IdProducto = @IdProducto)
        THROW 50011, 'No se puede eliminar el producto porque está incluido en órdenes de compra.', 1;

    BEGIN TRANSACTION;
        DELETE FROM ParametrosReposicion WHERE IdProducto = @IdProducto;
        DELETE FROM Productos WHERE IdProducto = @IdProducto;
    COMMIT TRANSACTION;
END
GO

CREATE PROCEDURE spListarMovimientos
AS
BEGIN
    SELECT m.IdMovimiento, m.IdProducto, p.Nombre, m.TipoMovimiento, m.Cantidad, m.FechaMovimiento
    FROM MovimientosInventario m
    INNER JOIN Productos p ON p.IdProducto = m.IdProducto
    ORDER BY m.FechaMovimiento DESC, m.IdMovimiento DESC;
END
GO

CREATE PROCEDURE spInsertarMovimiento
    @IdProducto INT, @TipoMovimiento VARCHAR(20), @Cantidad INT, @FechaMovimiento DATETIME
AS
BEGIN
    SET XACT_ABORT ON;
    IF @TipoMovimiento NOT IN ('Venta', 'Entrada', 'Salida') THROW 50030, 'El tipo de movimiento no es válido.', 1;
    IF @Cantidad <= 0 THROW 50031, 'La cantidad debe ser mayor a cero.', 1;

    BEGIN TRANSACTION;
        DECLARE @Stock INT = (SELECT StockActual FROM Productos WITH (UPDLOCK) WHERE IdProducto = @IdProducto);
        IF @Stock IS NULL THROW 50034, 'El producto no existe.', 1;

        DECLARE @Efecto INT = CASE WHEN @TipoMovimiento = 'Entrada' THEN @Cantidad ELSE -@Cantidad END;
        IF @Stock + @Efecto < 0 THROW 50032, 'No hay stock suficiente para realizar este movimiento.', 1;

        DECLARE @IdNuevo INT = (SELECT ISNULL(MAX(IdMovimiento), 0) + 1 FROM MovimientosInventario WITH (UPDLOCK, HOLDLOCK));

        INSERT INTO MovimientosInventario (IdMovimiento, IdProducto, TipoMovimiento, Cantidad, FechaMovimiento)
        VALUES (@IdNuevo, @IdProducto, @TipoMovimiento, @Cantidad, @FechaMovimiento);

        UPDATE Productos SET StockActual = @Stock + @Efecto WHERE IdProducto = @IdProducto;
    COMMIT TRANSACTION;
    SELECT @IdNuevo;
END
GO

CREATE PROCEDURE spActualizarMovimiento
    @IdMovimiento INT, @TipoMovimiento VARCHAR(20), @Cantidad INT, @FechaMovimiento DATETIME
AS
BEGIN
    SET XACT_ABORT ON;
    IF @TipoMovimiento NOT IN ('Venta', 'Entrada', 'Salida') THROW 50030, 'El tipo de movimiento no es válido.', 1;
    IF @Cantidad <= 0 THROW 50031, 'La cantidad debe ser mayor a cero.', 1;

    BEGIN TRANSACTION;
        DECLARE @IdProducto INT, @TipoViejo VARCHAR(20), @CantidadVieja INT;
        SELECT @IdProducto = IdProducto, @TipoViejo = TipoMovimiento, @CantidadVieja = Cantidad FROM MovimientosInventario WHERE IdMovimiento = @IdMovimiento;
        IF @IdProducto IS NULL THROW 50033, 'El movimiento no existe.', 1;

        DECLARE @EfectoViejo INT = CASE WHEN @TipoViejo = 'Entrada' THEN @CantidadVieja ELSE -@CantidadVieja END;
        DECLARE @EfectoNuevo INT = CASE WHEN @TipoMovimiento = 'Entrada' THEN @Cantidad ELSE -@Cantidad END;
        DECLARE @Stock INT = (SELECT StockActual FROM Productos WITH (UPDLOCK) WHERE IdProducto = @IdProducto);

        IF @Stock - @EfectoViejo + @EfectoNuevo < 0 THROW 50032, 'No hay stock suficiente para este cambio.', 1;

        UPDATE Productos SET StockActual = @Stock - @EfectoViejo + @EfectoNuevo WHERE IdProducto = @IdProducto;
        UPDATE MovimientosInventario SET TipoMovimiento = @TipoMovimiento, Cantidad = @Cantidad, FechaMovimiento = @FechaMovimiento WHERE IdMovimiento = @IdMovimiento;
    COMMIT TRANSACTION;
END
GO

CREATE PROCEDURE spEliminarMovimiento @IdMovimiento INT
AS
BEGIN
    SET XACT_ABORT ON;
    BEGIN TRANSACTION;
        DECLARE @IdProducto INT, @Tipo VARCHAR(20), @Cantidad INT;
        SELECT @IdProducto = IdProducto, @Tipo = TipoMovimiento, @Cantidad = Cantidad FROM MovimientosInventario WHERE IdMovimiento = @IdMovimiento;
        IF @IdProducto IS NULL THROW 50033, 'El movimiento no existe.', 1;

        DECLARE @Efecto INT = CASE WHEN @Tipo = 'Entrada' THEN @Cantidad ELSE -@Cantidad END;
        DECLARE @Stock INT = (SELECT StockActual FROM Productos WITH (UPDLOCK) WHERE IdProducto = @IdProducto);

        IF @Stock - @Efecto < 0 THROW 50035, 'No se puede eliminar porque el stock de productos quedaría en números negativos.', 1;

        UPDATE Productos SET StockActual = @Stock - @Efecto WHERE IdProducto = @IdProducto;
        DELETE FROM MovimientosInventario WHERE IdMovimiento = @IdMovimiento;
    COMMIT TRANSACTION;
END
GO

CREATE PROCEDURE spListarOrdenes
AS
BEGIN
    SELECT IdOrden, NombreEmpresa, FechaOrden, FechaAnalisis, TotalAproximado, Estado FROM OrdenCompra ORDER BY IdOrden DESC;
END
GO

CREATE PROCEDURE spObtenerDetalleOrden @IdOrden INT
AS
BEGIN
    SELECT d.IdProducto, p.Nombre, d.CantidadSugerida, d.CostoUnitario, d.Subtotal
    FROM DetalleOrdenCompra d
    INNER JOIN Productos p ON p.IdProducto = d.IdProducto
    WHERE d.IdOrden = @IdOrden ORDER BY d.IdDetalle;
END
GO

CREATE PROCEDURE spEliminarOrden @IdOrden INT
AS
BEGIN
    SET XACT_ABORT ON;
    BEGIN TRANSACTION;
        DELETE FROM DetalleOrdenCompra WHERE IdOrden = @IdOrden;
        DELETE FROM OrdenCompra WHERE IdOrden = @IdOrden;
    COMMIT TRANSACTION;
END
GO

CREATE PROCEDURE spRecibirOrden @IdOrden INT
AS
BEGIN
    SET XACT_ABORT ON;
    IF NOT EXISTS (SELECT 1 FROM OrdenCompra WHERE IdOrden = @IdOrden AND Estado = 'Pendiente')
        THROW 50040, 'La orden no existe o ya fue marcada como recibida.', 1;

    BEGIN TRANSACTION;
        DECLARE @Base INT = (SELECT ISNULL(MAX(IdMovimiento), 0) FROM MovimientosInventario WITH (UPDLOCK, HOLDLOCK));

        INSERT INTO MovimientosInventario (IdMovimiento, IdProducto, TipoMovimiento, Cantidad, FechaMovimiento)
        SELECT @Base + ROW_NUMBER() OVER (ORDER BY IdDetalle), IdProducto, 'Entrada', CantidadSugerida, GETDATE()
        FROM DetalleOrdenCompra WHERE IdOrden = @IdOrden;

        UPDATE p SET p.StockActual = p.StockActual + d.CantidadSugerida
        FROM Productos p INNER JOIN DetalleOrdenCompra d ON d.IdProducto = p.IdProducto WHERE d.IdOrden = @IdOrden;

        UPDATE OrdenCompra SET Estado = 'Recibida' WHERE IdOrden = @IdOrden;
    COMMIT TRANSACTION;
END
GO
";
    }
}