USE [ventas_cta_cte]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetVentasByClientId]    Script Date: 24/7/2025 01:54:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_GetVentasByClientId]
    @ClientId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
	cl.Nombre AS NombreCliente,
	cl.Apellido AS ApellidoCliente,
	cl.DNI AS DNICliente,
	cl.Direccion AS DireccionCliente,
	cl.Telefono AS TelefonoCliente,
	v.VentaId,
	v.ClientId,
	v.ArticuloId,
	a.Codigo AS CodigoArticulo,
	a.Descripcion AS Articulo, -- Descripción del artículo desde la tabla Articulo
	v.FechaDeInicio AS FechaDeVenta,
	v.FechaDeCancelacion AS FechaCancelacionCuotas,
	v.FechaAnulacion,
	v.Precio,
	v.Cuotas,
	c.CuotaId,
	c.MontoCuota,
	c.NumeroDeCuota,
	c.FechaProgramada AS FechaProgramadaDeCuota,
	c.FechaPago AS FechaQuePagoCuota,
	c.Estado AS EstadoCuota,
	fdp.Nombre AS FormaDePago
    FROM Venta v
    INNER JOIN Cuota c
    ON v.VentaId = c.VentaId
	INNER JOIN FormaDePago fdp
	ON v.FormaDePagoId = fdp.FormaDePagoId
	INNER JOIN Cliente cl
	ON v.ClientId = cl.ClientId
	INNER JOIN Articulo a
	ON v.ArticuloId = a.ArticuloId -- JOIN con la tabla Articulo
    WHERE v.ClientId = @ClientId
      AND a.FechaAnulacion IS NULL; -- Solo artículos no anulados
END;
