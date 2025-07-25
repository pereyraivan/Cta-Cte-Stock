-- CONSULTA DE DIAGNÓSTICO PARA VERIFICAR DATOS

-- 1. Verificar si hay ventas en la tabla
SELECT COUNT(*) AS TotalVentas FROM Venta;

-- 2. Verificar si hay ventas no anuladas
SELECT COUNT(*) AS VentasNoAnuladas FROM Venta WHERE FechaAnulacion IS NULL;

-- 3. Verificar si hay ventas con todos los datos relacionados
SELECT COUNT(*) AS VentasCompletas
FROM Venta v
    INNER JOIN Cliente c ON v.ClientId = c.ClientId
    INNER JOIN FormaDePago fp ON v.FormaDePagoId = fp.FormaDePagoId
    INNER JOIN Articulo a ON v.IdArticulo = a.ArticuloId
WHERE v.FechaAnulacion IS NULL;

-- 4. Ver una muestra de las primeras 5 ventas
SELECT TOP 5 
    v.VentaId,
    v.ClientId,
    v.IdArticulo,
    v.FormaDePagoId,
    v.FechaAnulacion,
    c.Nombre + ' ' + c.Apellido AS Cliente,
    a.Descripcion AS Articulo,
    fp.Nombre AS FormaPago
FROM Venta v
    LEFT JOIN Cliente c ON v.ClientId = c.ClientId
    LEFT JOIN FormaDePago fp ON v.FormaDePagoId = fp.FormaDePagoId
    LEFT JOIN Articulo a ON v.IdArticulo = a.ArticuloId;

-- 5. CONSULTA PRINCIPAL EQUIVALENTE AL LINQ
SELECT 
    v.VentaId,
    c.ClientId AS IdCliente,
    (c.Apellido + ' ' + c.Nombre) AS NombreCliente,
    v.IdArticulo,
    a.Descripcion AS Articulo,
    fp.Nombre AS FormaDePago,
    v.Precio,
    v.Cuotas,
    v.FechaDeInicio,
    v.FechaDeCancelacion,
    v.Cantidad,
    v.Total,
    v.FechaAnulacion,
    CASE 
        WHEN EXISTS (
            SELECT 1 
            FROM Cuota cu 
            WHERE cu.VentaId = v.VentaId 
              AND cu.FechaProgramada < GETDATE() 
              AND cu.FechaPago IS NULL
        ) 
        THEN 1 
        ELSE 0 
    END AS CuotasVencidas
FROM Venta v
    INNER JOIN Cliente c ON v.ClientId = c.ClientId
    INNER JOIN FormaDePago fp ON v.FormaDePagoId = fp.FormaDePagoId
    INNER JOIN Articulo a ON v.IdArticulo = a.ArticuloId
WHERE v.FechaAnulacion IS NULL
ORDER BY v.FechaDeInicio DESC;

-- 6. Verificar si hay problemas con claves foráneas
SELECT 'Ventas sin Cliente' AS Problema, COUNT(*) AS Cantidad
FROM Venta v
LEFT JOIN Cliente c ON v.ClientId = c.ClientId
WHERE c.ClientId IS NULL
UNION ALL
SELECT 'Ventas sin Artículo', COUNT(*)
FROM Venta v
LEFT JOIN Articulo a ON v.IdArticulo = a.ArticuloId
WHERE a.ArticuloId IS NULL
UNION ALL
SELECT 'Ventas sin Forma de Pago', COUNT(*)
FROM Venta v
LEFT JOIN FormaDePago fp ON v.FormaDePagoId = fp.FormaDePagoId
WHERE fp.FormaDePagoId IS NULL;
