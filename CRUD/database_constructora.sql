CREATE DATABASE IF NOT EXISTS crud_db
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_general_ci;

USE crud_db;

CREATE TABLE IF NOT EXISTS Users (
    Id INT NOT NULL AUTO_INCREMENT,
    Nombre VARCHAR(100) NOT NULL,
    Dni VARCHAR(20) NOT NULL,
    Rol VARCHAR(50) NOT NULL,
    Clave VARCHAR(255) NOT NULL,
    PRIMARY KEY (Id),
    UNIQUE KEY IX_Users_Dni (Dni)
);

CREATE TABLE IF NOT EXISTS Proveedores (
    Id INT NOT NULL AUTO_INCREMENT,
    Nombre VARCHAR(100) NOT NULL,
    RazonSocial VARCHAR(150) NOT NULL,
    Cuit VARCHAR(20) NOT NULL,
    Mail VARCHAR(150) NOT NULL,
    Telefono VARCHAR(30) NOT NULL,
    PRIMARY KEY (Id),
    UNIQUE KEY IX_Proveedores_Cuit (Cuit)
);

CREATE TABLE IF NOT EXISTS Articulos (
    Id INT NOT NULL AUTO_INCREMENT,
    Nombre VARCHAR(100) NOT NULL,
    UnidadMedida VARCHAR(50) NOT NULL,
    Descripcion VARCHAR(500) NOT NULL,
    PRIMARY KEY (Id)
);

CREATE TABLE IF NOT EXISTS Pedidos (
    Id INT NOT NULL AUTO_INCREMENT,
    UsuarioId INT NOT NULL,
    Fecha DATETIME(6) NOT NULL,
    Estado VARCHAR(30) NOT NULL,
    PRIMARY KEY (Id),
    KEY IX_Pedidos_UsuarioId (UsuarioId),
    CONSTRAINT FK_Pedidos_Users_UsuarioId
        FOREIGN KEY (UsuarioId) REFERENCES Users (Id)
        ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS PedidoDetalles (
    Id INT NOT NULL AUTO_INCREMENT,
    PedidoId INT NOT NULL,
    ArticuloId INT NOT NULL,
    Cantidad DECIMAL(10, 2) NOT NULL,
    PRIMARY KEY (Id),
    KEY IX_PedidoDetalles_PedidoId (PedidoId),
    KEY IX_PedidoDetalles_ArticuloId (ArticuloId),
    CONSTRAINT FK_PedidoDetalles_Pedidos_PedidoId
        FOREIGN KEY (PedidoId) REFERENCES Pedidos (Id)
        ON DELETE CASCADE,
    CONSTRAINT FK_PedidoDetalles_Articulos_ArticuloId
        FOREIGN KEY (ArticuloId) REFERENCES Articulos (Id)
        ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS OrdenesCompra (
    Id INT NOT NULL AUTO_INCREMENT,
    ProveedorId INT NOT NULL,
    UsuarioId INT NOT NULL,
    FechaEntregaReal DATETIME(6) NULL,
    Total DECIMAL(12, 2) NOT NULL,
    NumeroFactura VARCHAR(50) NULL,
    Estado VARCHAR(30) NOT NULL,
    PRIMARY KEY (Id),
    KEY IX_OrdenesCompra_ProveedorId (ProveedorId),
    KEY IX_OrdenesCompra_UsuarioId (UsuarioId),
    CONSTRAINT FK_OrdenesCompra_Proveedores_ProveedorId
        FOREIGN KEY (ProveedorId) REFERENCES Proveedores (Id)
        ON DELETE CASCADE,
    CONSTRAINT FK_OrdenesCompra_Users_UsuarioId
        FOREIGN KEY (UsuarioId) REFERENCES Users (Id)
        ON DELETE CASCADE
);
