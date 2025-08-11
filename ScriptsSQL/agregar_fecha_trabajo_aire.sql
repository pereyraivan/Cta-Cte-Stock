-- Script para agregar campo de fecha a la tabla TrabajoAireAcondicionado
-- y actualizar registros existentes

USE [ventas_cta_cte]
GO

-- Verificar si la columna ya existe
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
               WHERE TABLE_NAME = 'TrabajoAireAcondicionado' 
               AND COLUMN_NAME = 'FechaTrabajo')
BEGIN
    -- Agregar la columna FechaTrabajo con valor por defecto
    ALTER TABLE TrabajoAireAcondicionado
    ADD FechaTrabajo DATE NOT NULL DEFAULT GETDATE();
    
    PRINT 'Columna FechaTrabajo agregada exitosamente.';
END
ELSE
BEGIN
    PRINT 'La columna FechaTrabajo ya existe en la tabla.';
END

-- Actualizar registros existentes que podrían tener fecha por defecto
-- Opcional: si quieres asignar fechas específicas a trabajos existentes
-- UPDATE TrabajoAireAcondicionado 
-- SET FechaTrabajo = GETDATE()  -- o cualquier fecha específica
-- WHERE FechaTrabajo = CAST(GETDATE() AS DATE);

PRINT 'Script ejecutado completamente.';
