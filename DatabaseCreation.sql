CREATE DATABASE DevsuAccount;
GO

USE DevsuAccount;
GO

-- =========================================================
-- 1. PERSONA
-- =========================================================

CREATE TABLE Persona
(
    PersonaId INT IDENTITY(1,1) NOT NULL,
    Nombre VARCHAR(150) NOT NULL,
    Genero VARCHAR(1) NOT NULL,
    Edad INT NOT NULL,
    Identificacion VARCHAR(20) NOT NULL,
    Direccion VARCHAR(250) NULL,
    Telefono VARCHAR(20) NULL,

    CONSTRAINT PK_Persona
        PRIMARY KEY (PersonaId),

    CONSTRAINT CK_Persona_Genero
        CHECK (Genero IN ('M', 'F'))
);
GO

-- =========================================================
-- 2. CLIENTE
-- =========================================================

CREATE TABLE Cliente
(
    ClienteId INT IDENTITY(1,1) NOT NULL,
    PersonaId INT NOT NULL,
    Contrasena VARCHAR(255) NOT NULL,
    Estado BIT NOT NULL DEFAULT 1,

    CONSTRAINT PK_Cliente
        PRIMARY KEY (ClienteId),

    CONSTRAINT UQ_Cliente_Persona
        UNIQUE (PersonaId),

    CONSTRAINT FK_Cliente_Persona
        FOREIGN KEY (PersonaId)
        REFERENCES Persona(PersonaId)
);
GO


-- =========================================================
-- 3. CUENTA
-- =========================================================

CREATE TABLE Cuenta
(
    CuentaId INT IDENTITY(1,1) NOT NULL,
    NumeroCuenta VARCHAR(30) NOT NULL,
    TipoCuenta VARCHAR(20) NOT NULL,
    SaldoInicial DECIMAL(15,2) NOT NULL DEFAULT 0.00,
    Estado BIT NOT NULL DEFAULT 1,
    ClienteId INT NOT NULL,

    CONSTRAINT PK_Cuenta
        PRIMARY KEY (CuentaId),

    CONSTRAINT UQ_Cuenta_Numero
        UNIQUE (NumeroCuenta),

    CONSTRAINT FK_Cuenta_Cliente
        FOREIGN KEY (ClienteId)
        REFERENCES Cliente(ClienteId)
);
GO


-- =========================================================
-- 4. MOVIMIENTO
-- =========================================================

CREATE TABLE Movimiento
(
    MovimientoId INT IDENTITY(1,1) NOT NULL,
    Fecha DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    TipoMovimiento VARCHAR(20) NOT NULL,
    Valor DECIMAL(15,2) NOT NULL,
    Saldo DECIMAL(15,2) NOT NULL,
    CuentaId INT NOT NULL,

    CONSTRAINT PK_Movimiento
        PRIMARY KEY (MovimientoId),

    CONSTRAINT FK_Movimiento_Cuenta
        FOREIGN KEY (CuentaId)
        REFERENCES Cuenta(CuentaId)
);
GO