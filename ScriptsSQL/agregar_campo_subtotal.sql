-- Script para agregar el campo Subtotal a la tabla Venta
-- Ejecutar este script en la base de datos antes de usar la nueva funcionalidad

USE [ventas_cta_cte] -- Cambiar por el nombre de tu base de datos
GO

-- Agregar la columna Subtotal a la tabla Venta
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Venta]') AND name = 'Subtotal')
BEGIN
    ALTER TABLE [dbo].[Venta]
    ADD Subtotal DECIMAL(18,2) NULL
    
    PRINT 'Columna Subtotal agregada exitosamente a la tabla Venta'
END
ELSE
BEGIN
    PRINT 'La columna Subtotal ya existe en la tabla Venta'
END
GO

-- Actualizar los registros existentes para calcular el Subtotal (Precio * Cantidad)
UPDATE [dbo].[Venta]
SET Subtotal = ISNULL(Precio, 0) * ISNULL(Cantidad, 0)
WHERE Subtotal IS NULL

PRINT 'Valores de Subtotal calculados para los registros existentes'
GO

-- Opcional: Actualizar el campo Total para que sea Subtotal - Anticipo
-- Solo si quieres aplicar la nueva lógica a los datos existentes
-- DESCOMENTA las siguientes líneas si quieres aplicar la nueva lógica:

/*
UPDATE [dbo].[Venta]
SET Total = ISNULL(Subtotal, 0) - ISNULL(Anticipo, 0)
WHERE Total != (ISNULL(Subtotal, 0) - ISNULL(Anticipo, 0))

PRINT 'Valores de Total recalculados como Subtotal - Anticipo'
*/

PRINT 'Script completado exitosamente'
GO
