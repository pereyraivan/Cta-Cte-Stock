USE [ventas_cta_cte]
GO
/****** Object:  StoredProcedure [dbo].[ReciboDePago]    Script Date: 24/7/2025 01:45:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[ReciboDePago]
    @VentaId INT,
    @NumeroDeCuota INT  
AS
BEGIN
    SET NOCOUNT ON;

    -- Declarar variables para el recibo
    DECLARE @Serie INT;
    DECLARE @NumeroSecuencial INT;
    DECLARE @NuevoNumeroSecuencial INT;
    DECLARE @NumeroRecibo VARCHAR(13);

    -- Obtener valores actuales de Serie y NumeroSecuencial
    SELECT @Serie = Serie, @NumeroSecuencial = NumeroSecuencial FROM ReciboControl;

    -- Incrementar el secuencial
    SET @NuevoNumeroSecuencial = @NumeroSecuencial + 1;

    -- Verificar si el secuencial supera el límite
    IF @NuevoNumeroSecuencial > 99999999
    BEGIN
        SET @NuevoNumeroSecuencial = 1;
        SET @Serie = @Serie + 1;
    END;

    -- Actualizar valores en la tabla de control
    UPDATE ReciboControl
    SET Serie = @Serie, NumeroSecuencial = @NuevoNumeroSecuencial;

    -- Formatear el número de recibo
    SET @NumeroRecibo = RIGHT('0000' + CAST(@Serie AS VARCHAR(4)), 4) + '-' + 
                        RIGHT('00000000' + CAST(@NuevoNumeroSecuencial AS VARCHAR(8)), 8);

    -- Obtener los detalles de la cuota específica con fecha actual
    SELECT
        c.VentaId,
        GETDATE() AS FechaPago, -- Fecha actual del sistema
        cl.Nombre,
        cl.Apellido,
        c.MontoCuota,
        a.Descripcion AS Articulo, -- Ahora obtenemos la descripción desde la tabla Articulo
        c.NumeroDeCuota,
        ISNULL(
            SUM(CASE WHEN c.Estado = 1 THEN c.MontoCuota ELSE 0 END), 0
        ) + c.MontoCuota AS TotalPagado,
        v.Precio - ISNULL(
		(SELECT SUM(MontoCuota) FROM Cuota 
		WHERE VentaId = @VentaId AND Estado = 1), 0) AS SaldoRestante,
        (SELECT COUNT(*) FROM Cuota WHERE VentaId = @VentaId AND Estado = 0) AS CuotasPendientes,
        @NumeroRecibo AS NumeroRecibo
    FROM Venta v
    JOIN Cuota c ON v.VentaId = c.VentaId
    JOIN Cliente cl ON v.ClientId = cl.ClientId
    JOIN Articulo a ON v.ArticuloId = a.ArticuloId -- JOIN con la tabla Articulo
    WHERE c.VentaId = @VentaId 
      AND c.NumeroDeCuota = @NumeroDeCuota
      AND a.FechaAnulacion IS NULL -- Solo artículos no anulados
    GROUP BY 
        c.VentaId,
        cl.Nombre,
        cl.Apellido,
        c.MontoCuota,
        a.Descripcion, -- Cambiado de v.Articulo a a.Descripcion
        c.NumeroDeCuota,
        v.Precio;
END;
