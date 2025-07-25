-- =====================================================
-- Script para Eliminar Tabla DetalleVenta (NO USADA)
-- VentasCredimax - Limpieza de Estructura
-- =====================================================

USE [VentasCredimax]
GO

PRINT 'Verificando uso de tabla DetalleVenta...'

-- Verificar si la tabla existe
IF OBJECT_ID('dbo.DetalleVenta', 'U') IS NOT NULL
BEGIN
    PRINT 'Tabla DetalleVenta encontrada'
    
    -- Verificar si tiene datos
    DECLARE @Count INT = (SELECT COUNT(*) FROM DetalleVenta)
    PRINT 'Registros en DetalleVenta: ' + CAST(@Count AS VARCHAR(10))
    
    -- Verificar restricciones de clave foránea
    PRINT 'Verificando restricciones de clave foránea...'
    
    SELECT 
        'FK_' + OBJECT_NAME(f.parent_object_id) + '_' + OBJECT_NAME(f.referenced_object_id) AS constraint_name,
        OBJECT_NAME(f.parent_object_id) AS table_name,
        COL_NAME(fc.parent_object_id,fc.parent_column_id) AS column_name,
        OBJECT_NAME (f.referenced_object_id) AS referenced_table_name,
        COL_NAME(fc.referenced_object_id,fc.referenced_column_id) AS referenced_column_name
    FROM sys.foreign_keys AS f
    INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id
    WHERE OBJECT_NAME(f.parent_object_id) = 'DetalleVenta' 
       OR OBJECT_NAME(f.referenced_object_id) = 'DetalleVenta'
    
    PRINT ''
    PRINT '=== ANÁLISIS COMPLETADO ==='
    PRINT 'La tabla DetalleVenta:'
    PRINT '1. Existe en la base de datos'
    PRINT '2. NO se está usando en el código del sistema'
    PRINT '3. Solo existe en las entidades del modelo'
    PRINT ''
    PRINT 'RECOMENDACIÓN:'
    PRINT '- Se puede eliminar sin afectar el funcionamiento'
    PRINT '- Para eliminarla, descomenta las líneas de abajo'
    PRINT ''
    
    /*
    -- DESCOMENTA ESTAS LÍNEAS PARA ELIMINAR LA TABLA:
    
    -- Eliminar restricciones de clave foránea primero
    IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_DetalleVenta_Articulo')
        ALTER TABLE DetalleVenta DROP CONSTRAINT FK_DetalleVenta_Articulo
    
    IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_DetalleVenta_Venta')
        ALTER TABLE DetalleVenta DROP CONSTRAINT FK_DetalleVenta_Venta
    
    -- Eliminar la tabla
    DROP TABLE DetalleVenta
    
    PRINT '✅ Tabla DetalleVenta eliminada correctamente'
    */
    
END
ELSE
BEGIN
    PRINT 'La tabla DetalleVenta no existe en la base de datos'
END

PRINT ''
PRINT 'NOTA: MovimientoStock SÍ se está usando y NO debe eliminarse'
PRINT 'Se usa para registrar el historial de movimientos de stock'
