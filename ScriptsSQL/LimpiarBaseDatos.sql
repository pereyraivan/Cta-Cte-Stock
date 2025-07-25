-- =====================================================
-- Script de Limpieza para Instalación del Sistema
-- VentasCredimax - Control de Cuenta Corriente y Stock
-- =====================================================

USE [VentasCredimax]
GO

PRINT 'Iniciando limpieza de base de datos para instalación...'

-- =====================================================
-- 1. LIMPIAR DATOS TRANSACCIONALES (en orden correcto)
-- =====================================================

-- Deshabilitar restricciones de clave foránea temporalmente
EXEC sp_MSforeachtable "ALTER TABLE ? NOCHECK CONSTRAINT all"

PRINT 'Limpiando datos transaccionales...'

-- Limpiar tabla Cuota (depende de Venta)
DELETE FROM [dbo].[Cuota]
DBCC CHECKIDENT ('Cuota', RESEED, 0)
PRINT '- Tabla Cuota limpiada'

-- Limpiar tabla DetalleVenta (si existe - depende de Venta y Articulo)
IF OBJECT_ID('dbo.DetalleVenta', 'U') IS NOT NULL
BEGIN
    DELETE FROM [dbo].[DetalleVenta]
    DBCC CHECKIDENT ('DetalleVenta', RESEED, 0)
    PRINT '- Tabla DetalleVenta limpiada'
END

-- Limpiar tabla MovimientoStock (si existe - depende de Articulo)
IF OBJECT_ID('dbo.MovimientoStock', 'U') IS NOT NULL
BEGIN
    DELETE FROM [dbo].[MovimientoStock]
    DBCC CHECKIDENT ('MovimientoStock', RESEED, 0)
    PRINT '- Tabla MovimientoStock limpiada'
END

-- Limpiar tabla Venta
DELETE FROM [dbo].[Venta]
DBCC CHECKIDENT ('Venta', RESEED, 0)
PRINT '- Tabla Venta limpiada'

-- Limpiar tabla Cliente
DELETE FROM [dbo].[Cliente]
DBCC CHECKIDENT ('Cliente', RESEED, 0)
PRINT '- Tabla Cliente limpiada'

-- Limpiar tabla Articulo (si existe)
IF OBJECT_ID('dbo.Articulo', 'U') IS NOT NULL
BEGIN
    DELETE FROM [dbo].[Articulo]
    DBCC CHECKIDENT ('Articulo', RESEED, 0)
    PRINT '- Tabla Articulo limpiada'
END

-- Limpiar tabla ReciboControl
DELETE FROM [dbo].[ReciboControl]
DBCC CHECKIDENT ('ReciboControl', RESEED, 0)
PRINT '- Tabla ReciboControl limpiada'

-- =====================================================
-- 2. MANTENER DATOS MAESTROS (NO ELIMINAR)
-- =====================================================
PRINT 'Manteniendo datos maestros...'
PRINT '- Tabla FormaDePago (NO se limpia - datos maestros)'
PRINT '- Tabla DiaDeSemana (NO se limpia - datos maestros)'
PRINT '- Tabla Vendedor (NO se limpia - datos maestros)'
PRINT '- Tabla Configuracion (NO se limpia - configuración del sistema)'

-- =====================================================
-- 3. INSERTAR DATOS BÁSICOS NECESARIOS
-- =====================================================
PRINT 'Insertando datos básicos necesarios...'

-- Verificar si existen las formas de pago básicas
IF NOT EXISTS (SELECT 1 FROM FormaDePago WHERE Nombre = 'Contado')
BEGIN
    INSERT INTO FormaDePago (Nombre) VALUES ('Contado')
    PRINT '- Forma de pago "Contado" insertada'
END

IF NOT EXISTS (SELECT 1 FROM FormaDePago WHERE Nombre = 'Cuenta Corriente')
BEGIN
    INSERT INTO FormaDePago (Nombre) VALUES ('Cuenta Corriente')
    PRINT '- Forma de pago "Cuenta Corriente" insertada'
END

-- Verificar si existen los días de la semana
IF NOT EXISTS (SELECT 1 FROM DiaDeSemana WHERE Nombre = 'Lunes')
BEGIN
    INSERT INTO DiaDeSemana (Nombre) VALUES 
    ('Lunes'), ('Martes'), ('Miércoles'), ('Jueves'), ('Viernes'), ('Sábado'), ('Domingo')
    PRINT '- Días de la semana insertados'
END

-- Verificar configuración
IF NOT EXISTS (SELECT 1 FROM Configuracion)
BEGIN
    INSERT INTO Configuracion (isDemo) VALUES (0)
    PRINT '- Configuración básica insertada (Modo PRODUCCIÓN - sin restricciones)'
END
ELSE
BEGIN
    UPDATE Configuracion SET isDemo = 0
    PRINT '- Configuración actualizada a Modo PRODUCCIÓN (sin restricciones)'
END

-- =====================================================
-- 4. REHABILITAR RESTRICCIONES
-- =====================================================
PRINT 'Rehabilitando restricciones de clave foránea...'
EXEC sp_MSforeachtable "ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all"

-- =====================================================
-- 5. VERIFICACIÓN FINAL
-- =====================================================
PRINT '==========================================='
PRINT 'RESUMEN DE LIMPIEZA COMPLETADA:'
PRINT '==========================================='
PRINT 'TABLAS LIMPIADAS (datos transaccionales):'
PRINT '- Cuota: ' + CAST((SELECT COUNT(*) FROM Cuota) AS VARCHAR(10)) + ' registros'
PRINT '- Venta: ' + CAST((SELECT COUNT(*) FROM Venta) AS VARCHAR(10)) + ' registros'
PRINT '- Cliente: ' + CAST((SELECT COUNT(*) FROM Cliente) AS VARCHAR(10)) + ' registros'
PRINT '- ReciboControl: ' + CAST((SELECT COUNT(*) FROM ReciboControl) AS VARCHAR(10)) + ' registros'

IF OBJECT_ID('dbo.DetalleVenta', 'U') IS NOT NULL
    PRINT '- DetalleVenta: ' + CAST((SELECT COUNT(*) FROM DetalleVenta) AS VARCHAR(10)) + ' registros'

IF OBJECT_ID('dbo.MovimientoStock', 'U') IS NOT NULL
    PRINT '- MovimientoStock: ' + CAST((SELECT COUNT(*) FROM MovimientoStock) AS VARCHAR(10)) + ' registros'

IF OBJECT_ID('dbo.Articulo', 'U') IS NOT NULL
    PRINT '- Articulo: ' + CAST((SELECT COUNT(*) FROM Articulo) AS VARCHAR(10)) + ' registros'

PRINT ''
PRINT 'TABLAS MANTENIDAS (datos maestros):'
PRINT '- FormaDePago: ' + CAST((SELECT COUNT(*) FROM FormaDePago) AS VARCHAR(10)) + ' registros'
PRINT '- DiaDeSemana: ' + CAST((SELECT COUNT(*) FROM DiaDeSemana) AS VARCHAR(10)) + ' registros'
PRINT '- Vendedor: ' + CAST((SELECT COUNT(*) FROM Vendedor) AS VARCHAR(10)) + ' registros'
PRINT '- Configuracion: ' + CAST((SELECT COUNT(*) FROM Configuracion) AS VARCHAR(10)) + ' registros'

PRINT ''
PRINT '✅ Base de datos lista para instalación en nueva PC'
PRINT '✅ Sistema configurado en MODO PRODUCCIÓN (sin restricciones)'
PRINT '==========================================='
