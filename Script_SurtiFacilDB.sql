
-- SCRIPT DE BASE DE DATOS Y DATOS DE PRUEBA: SurtiFacilDB
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'SurtiFacilDB')
BEGIN
    CREATE DATABASE SurtiFacilDB;
END
GO

USE SurtiFacilDB;
GO

-- 1. TABLAS PRINCIPALES Y DE PARÁMETROS
IF OBJECT_ID('MovimientosInventario', 'U') IS NOT NULL DROP TABLE MovimientosInventario;
IF OBJECT_ID('ParametrosReposicion', 'U') IS NOT NULL DROP TABLE ParametrosReposicion;
IF OBJECT_ID('DetalleOrdenCompra', 'U') IS NOT NULL DROP TABLE DetalleOrdenCompra;
IF OBJECT_ID('OrdenCompra', 'U') IS NOT NULL DROP TABLE OrdenCompra;
IF OBJECT_ID('Productos', 'U') IS NOT NULL DROP TABLE Productos;
IF OBJECT_ID('Categoria', 'U') IS NOT NULL DROP TABLE Categoria;

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
    TotalAproximado DECIMAL(12,2) NOT NULL
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

-- 2. INSERCIÓN DE DATOS INICIALES Y PRUEBAS
INSERT INTO Categoria (IdCategoria, NombreCategoria) VALUES
(1, 'Abarrotes'),
(2, 'Limpieza'),
(3, 'Perecederos'),
(4, 'Bebidas'),
(5, 'Cuidado Personal');
GO

INSERT INTO Productos (IdProducto, Nombre, StockActual, CostoUnitario, IdCategoria) VALUES
(1, 'Arroz Extra', 120, 18.50, 1),
(2, 'Frijol Negro', 85, 14.25, 1),
(3, 'Aceite Vegetal', 45, 22.00, 1),
(4, 'Azúcar Refinada', 150, 12.10, 1),
(5, 'Harina de Trigo', 60, 9.80, 1),
(6, 'Desengrasante Líquido', 30, 25.00, 2),
(7, 'Jabón en Polvo', 95, 45.50, 2),
(8, 'Cloro Desinfectante', 110, 15.00, 2),
(9, 'Lavatrastos', 75, 18.25, 2),
(10, 'Suavizante de Telas', 40, 32.00, 2),
(11, 'Leche Entera', 50, 11.50, 3),
(12, 'Queso Fresco', 25, 35.00, 3),
(13, 'Yogurt Natural', 65, 16.80, 3),
(14, 'Mantequilla', 35, 20.00, 3),
(15, 'Crema Pura', 42, 28.50, 3),
(16, 'Agua Pura 600ml', 180, 5.00, 4),
(17, 'Refresco en Polvo', 140, 3.50, 4),
(18, 'Jugo de Naranja', 90, 24.00, 4),
(19, 'Bebida Energética', 105, 19.90, 4),
(20, 'Té Helado', 70, 15.50, 4);
GO

INSERT INTO ParametrosReposicion (IdParametro, IdProducto, TiempoEntregaProveedorDias, DiasCoberturaObjetivo, StockSeguridad) VALUES
(1, 1, 5, 30, 15),
(2, 2, 7, 30, 20),
(3, 3, 4, 15, 10),
(4, 4, 10, 45, 25),
(5, 5, 5, 30, 12),
(6, 6, 6, 20, 8),
(7, 7, 8, 30, 18),
(8, 8, 3, 15, 10),
(9, 9, 5, 30, 14),
(10, 10, 7, 25, 12),
(11, 11, 2, 10, 15),
(12, 12, 3, 7, 10),
(13, 13, 4, 15, 12),
(14, 14, 3, 10, 8),
(15, 15, 4, 12, 10),
(16, 16, 5, 30, 30),
(17, 17, 6, 30, 25),
(18, 18, 5, 20, 15),
(19, 19, 4, 15, 18),
(20, 20, 5, 20, 12);
GO

INSERT INTO MovimientosInventario (IdMovimiento, IdProducto, TipoMovimiento, Cantidad, FechaMovimiento) VALUES
(1, 14, 'Venta', 8, '2025-10-05 10:30:00'),
(2, 3, 'Entrada', 50, '2025-10-08 08:15:00'),
(3, 19, 'Venta', 12, '2025-10-10 14:20:00'),
(4, 7, 'Salida', 5, '2025-10-12 09:00:00'),
(5, 20, 'Venta', 15, '2025-10-15 16:45:00'),
(6, 1, 'Venta', 20, '2025-10-18 11:10:00'),
(7, 11, 'Venta', 10, '2025-10-20 07:30:00'),
(8, 5, 'Entrada', 100, '2025-10-22 13:00:00'),
(9, 9, 'Venta', 6, '2025-10-25 15:15:00'),
(10, 16, 'Venta', 25, '2025-10-28 12:00:00'),
(11, 2, 'Venta', 14, '2025-11-02 09:10:00'),
(12, 4, 'Venta', 30, '2025-11-04 11:25:00'),
(13, 6, 'Venta', 8, '2025-11-06 14:40:00'),
(14, 8, 'Entrada', 60, '2025-11-08 08:00:00'),
(15, 10, 'Venta', 5, '2025-11-10 16:15:00'),
(16, 12, 'Venta', 12, '2025-11-12 10:05:00'),
(17, 13, 'Venta', 18, '2025-11-15 13:50:00'),
(18, 15, 'Venta', 9, '2025-11-18 15:30:00'),
(19, 17, 'Venta', 40, '2025-11-20 17:00:00'),
(20, 18, 'Venta', 22, '2025-11-22 12:10:00'),
(21, 1, 'Entrada', 150, '2025-11-25 08:30:00'),
(22, 3, 'Venta', 11, '2025-11-28 10:45:00'),
(23, 5, 'Venta', 15, '2025-12-01 14:00:00'),
(24, 7, 'Venta', 25, '2025-12-03 16:20:00'),
(25, 9, 'Venta', 10, '2025-12-05 09:35:00'),
(26, 11, 'Venta', 15, '2025-12-08 11:50:00'),
(27, 14, 'Venta', 7, '2025-12-10 15:05:00'),
(28, 16, 'Venta', 35, '2025-12-12 18:00:00'),
(29, 19, 'Venta', 20, '2025-12-15 13:15:00'),
(30, 20, 'Venta', 14, '2025-12-18 10:40:00'),
(31, 2, 'Entrada', 80, '2025-12-20 08:00:00'),
(32, 4, 'Venta', 25, '2025-12-22 12:30:00'),
(33, 6, 'Venta', 11, '2025-12-24 16:10:00'),
(34, 8, 'Venta', 18, '2025-12-27 11:00:00'),
(35, 10, 'Venta', 9, '2025-12-29 14:25:00'),
(36, 12, 'Venta', 15, '2026-01-02 09:15:00'),
(37, 13, 'Venta', 20, '2026-01-05 12:40:00'),
(38, 15, 'Venta', 8, '2026-01-08 15:55:00'),
(39, 17, 'Venta', 30, '2026-01-10 17:30:00'),
(40, 18, 'Venta', 16, '2026-01-12 10:20:00'),
(41, 1, 'Venta', 22, '2026-01-15 11:10:00'),
(42, 3, 'Venta', 14, '2026-01-18 14:35:00'),
(43, 5, 'Venta', 18, '2026-01-20 16:50:00'),
(44, 7, 'Entrada', 100, '2026-01-22 08:15:00'),
(45, 9, 'Venta', 12, '2026-01-25 10:00:00'),
(46, 11, 'Venta', 19, '2026-01-28 13:20:00'),
(47, 14, 'Venta', 10, '2026-01-30 15:45:00'),
(48, 16, 'Venta', 45, '2026-02-02 18:10:00'),
(49, 19, 'Venta', 18, '2026-02-05 11:25:00'),
(50, 20, 'Venta', 12, '2026-02-08 09:40:00'),
(51, 2, 'Venta', 16, '2026-02-10 12:05:00'),
(52, 4, 'Venta', 28, '2026-02-12 14:30:00'),
(53, 6, 'Venta', 7, '2026-02-15 16:55:00'),
(54, 8, 'Venta', 22, '2026-02-18 10:15:00'),
(55, 10, 'Entrada', 50, '2026-02-20 08:30:00'),
(56, 12, 'Venta', 10, '2026-02-22 11:45:00'),
(57, 13, 'Venta', 14, '2026-02-25 14:10:00'),
(58, 15, 'Venta', 11, '2026-02-28 16:25:00'),
(59, 17, 'Venta', 35, '2026-03-02 09:50:00'),
(60, 18, 'Venta', 20, '2026-03-05 12:15:00'),
(61, 1, 'Venta', 25, '2026-03-08 15:00:00'),
(62, 3, 'Entrada', 40, '2026-03-10 08:00:00'),
(63, 5, 'Venta', 12, '2026-03-12 10:30:00'),
(64, 7, 'Venta', 20, '2026-03-15 13:45:00'),
(65, 9, 'Venta', 14, '2026-03-18 16:00:00'),
(66, 11, 'Venta', 16, '2026-03-20 11:15:00'),
(67, 14, 'Venta', 9, '2026-03-22 14:30:00'),
(68, 16, 'Venta', 50, '2026-03-25 17:45:00'),
(69, 19, 'Venta', 15, '2026-03-28 10:00:00'),
(70, 20, 'Venta', 11, '2026-03-30 12:20:00'),
(71, 2, 'Venta', 18, '2026-04-02 15:10:00'),
(72, 4, 'Venta', 32, '2026-04-05 09:25:00'),
(73, 6, 'Venta', 10, '2026-04-08 11:40:00'),
(74, 8, 'Venta', 25, '2026-04-10 14:00:00'),
(75, 10, 'Venta', 8, '2026-04-12 16:15:00'),
(76, 12, 'Entrada', 30, '2026-04-15 08:30:00'),
(77, 13, 'Venta', 17, '2026-04-18 10:50:00'),
(78, 15, 'Venta', 13, '2026-04-20 13:05:00'),
(79, 17, 'Venta', 28, '2026-04-22 15:20:00'),
(80, 18, 'Venta', 19, '2026-04-25 17:35:00'),
(81, 1, 'Venta', 30, '2026-04-28 10:10:00'),
(82, 3, 'Venta', 16, '2026-05-02 12:25:00'),
(83, 5, 'Venta', 20, '2026-05-05 14:40:00'),
(84, 7, 'Venta', 22, '2026-05-08 16:55:00'),
(85, 9, 'Venta', 11, '2026-05-10 09:10:00'),
(86, 11, 'Entrada', 60, '2026-05-12 08:00:00'),
(87, 14, 'Venta', 8, '2026-05-15 11:30:00'),
(88, 16, 'Venta', 40, '2026-05-18 13:45:00'),
(89, 19, 'Venta', 22, '2026-05-20 16:00:00'),
(90, 20, 'Venta', 13, '2026-05-22 10:20:00'),
(91, 2, 'Venta', 21, '2026-05-25 12:35:00'),
(92, 4, 'Venta', 35, '2026-05-28 14:50:00'),
(93, 6, 'Venta', 9, '2026-06-01 17:05:00'),
(94, 8, 'Venta', 20, '2026-06-03 09:30:00'),
(95, 10, 'Venta', 12, '2026-06-05 11:45:00'),
(96, 12, 'Venta', 14, '2026-06-08 14:00:00'),
(97, 13, 'Venta', 19, '2026-06-10 16:15:00'),
(98, 15, 'Entrada', 45, '2026-06-12 08:15:00'),
(99, 17, 'Venta', 32, '2026-06-15 10:40:00'),
(100, 18, 'Venta', 17, '2026-06-18 13:00:00'),
(101, 1, 'Venta', 28, '2026-06-20 15:15:00'),
(102, 3, 'Venta', 18, '2026-06-22 17:30:00'),
(103, 5, 'Venta', 14, '2026-06-25 09:45:00'),
(104, 7, 'Venta', 24, '2026-06-28 12:00:00'),
(105, 9, 'Venta', 13, '2026-07-01 14:15:00'),
(106, 11, 'Venta', 18, '2026-07-03 16:30:00'),
(107, 14, 'Venta', 11, '2026-07-05 10:00:00'),
(108, 16, 'Entrada', 200, '2026-07-08 08:00:00'),
(109, 19, 'Venta', 24, '2026-07-10 12:20:00'),
(110, 20, 'Venta', 16, '2026-07-12 14:35:00'),
(111, 2, 'Venta', 19, '2026-07-15 16:50:00'),
(112, 4, 'Venta', 38, '2026-07-18 09:10:00'),
(113, 6, 'Venta', 12, '2026-07-20 11:25:00'),
(114, 8, 'Venta', 23, '2026-07-22 13:40:00'),
(115, 10, 'Venta', 10, '2026-07-25 15:55:00'),
(116, 12, 'Venta', 11, '2026-07-28 18:10:00'),
(117, 13, 'Venta', 21, '2026-08-01 10:30:00'),
(118, 15, 'Venta', 15, '2026-08-03 12:45:00'),
(119, 17, 'Venta', 36, '2026-08-05 15:00:00'),
(120, 18, 'Venta', 21, '2026-08-08 17:15:00');
GO

-- 3. PROCEDIMIENTOS ALMACENADOS
CREATE PROCEDURE spListarCategorias
AS
BEGIN
    SELECT IdCategoria, NombreCategoria
    FROM Categoria
    ORDER BY NombreCategoria;
END
GO

CREATE PROCEDURE spObtenerDatosPedido
    @FechaAnalisis DATE,
    @IdCategoria INT = 0,
    @DiasHistorico INT = 365
AS
BEGIN
    SELECT
        p.IdProducto,
        p.Nombre,
        p.StockActual,
        p.CostoUnitario,
        pr.TiempoEntregaProveedorDias,
        pr.DiasCoberturaObjetivo,
        pr.StockSeguridad,
        ISNULL(SUM(m.Cantidad), 0) AS UnidadesVendidas
    FROM Productos p
    INNER JOIN ParametrosReposicion pr ON pr.IdProducto = p.IdProducto
    LEFT JOIN MovimientosInventario m
        ON m.IdProducto = p.IdProducto
        AND m.TipoMovimiento = 'Venta'
        AND m.FechaMovimiento >= DATEADD(DAY, -@DiasHistorico, @FechaAnalisis)
        AND m.FechaMovimiento < @FechaAnalisis
    WHERE @IdCategoria = 0 OR p.IdCategoria = @IdCategoria
    GROUP BY p.IdProducto, p.Nombre, p.StockActual, p.CostoUnitario,
             pr.TiempoEntregaProveedorDias, pr.DiasCoberturaObjetivo, pr.StockSeguridad
    ORDER BY p.IdProducto;
END
GO

CREATE PROCEDURE spInsertarOrdenCompra
    @NombreEmpresa VARCHAR(100),
    @FechaAnalisis DATE,
    @TotalAproximado DECIMAL(12,2)
AS
BEGIN
    INSERT INTO OrdenCompra (NombreEmpresa, FechaAnalisis, TotalAproximado)
    VALUES (@NombreEmpresa, @FechaAnalisis, @TotalAproximado);

    SELECT CAST(SCOPE_IDENTITY() AS INT);
END
GO

CREATE PROCEDURE spInsertarDetalleOrdenCompra
    @IdOrden INT,
    @IdProducto INT,
    @CantidadSugerida INT,
    @CostoUnitario DECIMAL(10,2),
    @Subtotal DECIMAL(12,2)
AS
BEGIN
    INSERT INTO DetalleOrdenCompra (IdOrden, IdProducto, CantidadSugerida, CostoUnitario, Subtotal)
    VALUES (@IdOrden, @IdProducto, @CantidadSugerida, @CostoUnitario, @Subtotal);
END
GO