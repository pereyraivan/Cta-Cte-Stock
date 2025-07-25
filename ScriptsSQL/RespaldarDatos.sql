-- =====================================================
-- Script de Respaldo de Datos Importantes
-- VentasCredimax - Control de Cuenta Corriente y Stock
-- =====================================================

USE [VentasCredimax]
GO

PRINT 'Creando respaldo de datos importantes...'

-- =====================================================
-- 1. CREAR TABLAS DE RESPALDO
-- =====================================================

-- Respaldo de Clientes
IF OBJECT_ID('dbo.Cliente_Backup', 'U') IS NOT NULL
    DROP TABLE [dbo].[Cliente_Backup]

SELECT * 
INTO [dbo].[Cliente_Backup]
FROM [dbo].[Cliente]
PRINT 'Respaldo de Clientes creado: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' registros'

-- Respaldo de Ventas
IF OBJECT_ID('dbo.Venta_Backup', 'U') IS NOT NULL
    DROP TABLE [dbo].[Venta_Backup]

SELECT * 
INTO [dbo].[Venta_Backup]
FROM [dbo].[Venta]
PRINT 'Respaldo de Ventas creado: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' registros'

-- Respaldo de Cuotas
IF OBJECT_ID('dbo.Cuota_Backup', 'U') IS NOT NULL
    DROP TABLE [dbo].[Cuota_Backup]

SELECT * 
INTO [dbo].[Cuota_Backup]
FROM [dbo].[Cuota]
PRINT 'Respaldo de Cuotas creado: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' registros'

-- Respaldo de Artículos (si existe)
IF OBJECT_ID('dbo.Articulo', 'U') IS NOT NULL
BEGIN
    IF OBJECT_ID('dbo.Articulo_Backup', 'U') IS NOT NULL
        DROP TABLE [dbo].[Articulo_Backup]

    SELECT * 
    INTO [dbo].[Articulo_Backup]
    FROM [dbo].[Articulo]
    PRINT 'Respaldo de Artículos creado: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' registros'
END

-- Respaldo de DetalleVenta (si existe)
IF OBJECT_ID('dbo.DetalleVenta', 'U') IS NOT NULL
BEGIN
    IF OBJECT_ID('dbo.DetalleVenta_Backup', 'U') IS NOT NULL
        DROP TABLE [dbo].[DetalleVenta_Backup]

    SELECT * 
    INTO [dbo].[DetalleVenta_Backup]
    FROM [dbo].[DetalleVenta]
    PRINT 'Respaldo de DetalleVenta creado: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' registros'
END

-- Respaldo de MovimientoStock (si existe)
IF OBJECT_ID('dbo.MovimientoStock', 'U') IS NOT NULL
BEGIN
    IF OBJECT_ID('dbo.MovimientoStock_Backup', 'U') IS NOT NULL
        DROP TABLE [dbo].[MovimientoStock_Backup]

    SELECT * 
    INTO [dbo].[MovimientoStock_Backup]
    FROM [dbo].[MovimientoStock]
    PRINT 'Respaldo de MovimientoStock creado: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' registros'
END

PRINT ''
PRINT '✅ Respaldo completado. Las tablas *_Backup contienen los datos originales.'
PRINT 'Para restaurar los datos, puedes usar:'
PRINT 'INSERT INTO [Tabla] SELECT * FROM [Tabla_Backup]'
