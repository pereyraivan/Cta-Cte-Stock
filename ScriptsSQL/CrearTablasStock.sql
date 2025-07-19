-- Crear tabla de Artículos
CREATE TABLE Articulo (
    ArticuloId INT IDENTITY(1,1) PRIMARY KEY,
    Codigo VARCHAR(20) NOT NULL,
    Descripcion VARCHAR(100) NOT NULL,
    PrecioCompra DECIMAL(18,2) NOT NULL,
    PrecioVenta DECIMAL(18,2) NOT NULL,
    Stock INT NOT NULL DEFAULT 0,
    FechaAlta DATETIME NOT NULL DEFAULT GETDATE(),
    FechaAnulacion DATETIME NULL,
    CONSTRAINT UC_Articulo_Codigo UNIQUE (Codigo)
);

-- Crear tabla de Movimientos de Stock (para historial)
CREATE TABLE MovimientoStock (
    MovimientoStockId INT IDENTITY(1,1) PRIMARY KEY,
    ArticuloId INT NOT NULL,
    Fecha DATETIME NOT NULL DEFAULT GETDATE(),
    StockAnterior INT NOT NULL,
    Cantidad INT NOT NULL,
    StockNuevo INT NOT NULL,
    Observaciones VARCHAR(200) NULL,
    CONSTRAINT FK_MovimientoStock_Articulo FOREIGN KEY (ArticuloId) REFERENCES Articulo(ArticuloId)
);

-- Crear tabla de Detalle de Venta
CREATE TABLE DetalleVenta (
    DetalleVentaId INT IDENTITY(1,1) PRIMARY KEY,
    VentaId INT NOT NULL,
    ArticuloId INT NOT NULL,
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(18,2) NOT NULL,
    Subtotal DECIMAL(18,2) NOT NULL,
    CONSTRAINT FK_DetalleVenta_Venta FOREIGN KEY (VentaId) REFERENCES Venta(VentaId),
    CONSTRAINT FK_DetalleVenta_Articulo FOREIGN KEY (ArticuloId) REFERENCES Articulo(ArticuloId),
    CONSTRAINT CK_DetalleVenta_Cantidad CHECK (Cantidad > 0),
    CONSTRAINT CK_DetalleVenta_PrecioUnitario CHECK (PrecioUnitario > 0)
);

-- Índices para mejorar el rendimiento
CREATE INDEX IX_Articulo_Codigo ON Articulo(Codigo);
CREATE INDEX IX_Articulo_FechaAnulacion ON Articulo(FechaAnulacion);
CREATE INDEX IX_MovimientoStock_ArticuloId ON MovimientoStock(ArticuloId);
CREATE INDEX IX_DetalleVenta_VentaId ON DetalleVenta(VentaId);
CREATE INDEX IX_DetalleVenta_ArticuloId ON DetalleVenta(ArticuloId);
