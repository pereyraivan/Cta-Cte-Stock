-- =====================================================
-- Script de Instalación Completa del Sistema
-- VentasCredimax - Control de Cuenta Corriente y Stock
-- =====================================================

-- PASO 1: Crear la base de datos (si no existe)
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'VentasCredimax')
BEGIN
    CREATE DATABASE [VentasCredimax]
    PRINT 'Base de datos VentasCredimax creada'
END
ELSE
BEGIN
    PRINT 'Base de datos VentasCredimax ya existe'
END
GO

USE [VentasCredimax]
GO

PRINT 'Iniciando instalación del sistema VentasCredimax...'
PRINT '===================================================='

-- PASO 2: Crear todas las tablas necesarias
-- (Aquí deberías pegar el contenido completo de scriptSQL.sql)

-- PASO 3: Insertar datos maestros básicos
PRINT 'Insertando datos maestros básicos...'

-- Formas de Pago
IF NOT EXISTS (SELECT 1 FROM FormaDePago WHERE Nombre = 'Contado')
    INSERT INTO FormaDePago (Nombre) VALUES ('Contado')

IF NOT EXISTS (SELECT 1 FROM FormaDePago WHERE Nombre = 'Cuenta Corriente')
    INSERT INTO FormaDePago (Nombre) VALUES ('Cuenta Corriente')

IF NOT EXISTS (SELECT 1 FROM FormaDePago WHERE Nombre = 'Tarjeta de Crédito')
    INSERT INTO FormaDePago (Nombre) VALUES ('Tarjeta de Crédito')

PRINT 'Formas de pago insertadas'

-- Días de la semana
IF NOT EXISTS (SELECT 1 FROM DiaDeSemana WHERE Nombre = 'Lunes')
BEGIN
    INSERT INTO DiaDeSemana (Nombre) VALUES 
    ('Lunes'), ('Martes'), ('Miércoles'), ('Jueves'), ('Viernes'), ('Sábado'), ('Domingo')
    PRINT 'Días de la semana insertados'
END

-- Vendedor por defecto
IF NOT EXISTS (SELECT 1 FROM Vendedor WHERE Nombre = 'Vendedor')
BEGIN
    INSERT INTO Vendedor (Nombre, Apellido, Telefono) 
    VALUES ('Vendedor', 'Principal', '000-000-0000')
    PRINT 'Vendedor por defecto insertado'
END

-- Configuración del sistema
IF NOT EXISTS (SELECT 1 FROM Configuracion)
BEGIN
    INSERT INTO Configuracion (isDemo) VALUES (0)
    PRINT 'Configuración del sistema insertada (Modo Producción)'
END

PRINT 'Datos maestros básicos completados'
PRINT ''

-- PASO 4: Verificación final
PRINT 'VERIFICACIÓN DE INSTALACIÓN:'
PRINT '============================'
PRINT 'Tablas creadas:'
SELECT 
    TABLE_NAME as 'Tabla',
    (SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = t.TABLE_NAME) as 'Columnas'
FROM INFORMATION_SCHEMA.TABLES t
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME

PRINT ''
PRINT '✅ INSTALACIÓN COMPLETADA EXITOSAMENTE'
PRINT '============================'
PRINT 'El sistema VentasCredimax está listo para usar.'
PRINT 'Datos maestros configurados:'
PRINT '- Formas de pago disponibles'
PRINT '- Días de la semana configurados'
PRINT '- Vendedor principal creado'
PRINT '- Sistema en modo producción'
PRINT ''
PRINT 'Próximos pasos:'
PRINT '1. Configurar cadena de conexión en el sistema'
PRINT '2. Agregar artículos al inventario'
PRINT '3. Registrar clientes'
PRINT '4. Comenzar a registrar ventas'
