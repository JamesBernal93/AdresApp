Use AdresAppDb;
GO
-- ============================================================
-- HU-1.2-BDUA-WS-3877-ME  |  Tipo de archivo ME
-- Tablas de soporte para validación, catálogos y procesamiento
-- ============================================================

-- ------------------------------------------------------------
-- 1. Catálogo de campos del tipo de registro ME
--    Almacena metadatos de cada campo (posición, longitud,
--    tipo de dato, obligatoriedad).
-- ------------------------------------------------------------
CREATE TABLE dbo.ME_CamposRegistro (
    Id               INT           NOT NULL IDENTITY(1,1),
    TipoRegistro     INT           NOT NULL DEFAULT 0,       -- 0 = ME
    NumeroCampo      INT           NOT NULL,                 -- Posición del campo (1..N)
    NombreCampo      NVARCHAR(120) NOT NULL,
    LongitudMinima   INT           NULL,
    LongitudMaxima   INT           NOT NULL,
    TipoDato         NVARCHAR(30)  NOT NULL,                 -- TEXTO, NUMERICO, FECHA, CODIGO
    EsObligatorio    BIT           NOT NULL DEFAULT 1,
    ValoresPermitidos NVARCHAR(500) NULL,                    -- Lista separada por coma cuando aplica
    FormatoFecha     NVARCHAR(30)  NULL,                     -- DD/MM/AAAA etc.
    Descripcion      NVARCHAR(500) NULL,
    Activo           BIT           NOT NULL DEFAULT 1,
    FechaCreacion    DATETIME2     NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT PK_ME_CamposRegistro PRIMARY KEY (Id),
    CONSTRAINT UQ_ME_CamposRegistro_Tipo_Num UNIQUE (TipoRegistro, NumeroCampo)
);

-- ------------------------------------------------------------
-- 2. Expresiones regulares por campo
--    Permite múltiples regex por campo (una por glosa).
-- ------------------------------------------------------------
CREATE TABLE dbo.ME_ReglasValidacion (
    Id               INT           NOT NULL IDENTITY(1,1),
    CampoRegistroId  INT           NOT NULL,
    CodigoGlosa      NVARCHAR(10)  NOT NULL,                 -- GE0001, GE0002, GE9999, etc.
    DescripcionGlosa NVARCHAR(500) NOT NULL,
    ExpresionRegular NVARCHAR(1000) NULL,                    -- NULL = validación lógica (no regex)
    TipoValidacion   NVARCHAR(30)  NOT NULL,                 -- REGEX, LONGITUD, OBLIGATORIO, CATALOGO, NEGOCIO
    Orden            INT           NOT NULL DEFAULT 1,       -- Orden de evaluación
    Activo           BIT           NOT NULL DEFAULT 1,
    FechaCreacion    DATETIME2     NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT PK_ME_ReglasValidacion PRIMARY KEY (Id),
    CONSTRAINT FK_ME_ReglasValidacion_Campo FOREIGN KEY (CampoRegistroId)
        REFERENCES dbo.ME_CamposRegistro (Id),
    CONSTRAINT UQ_ME_ReglasValidacion_Campo_Glosa UNIQUE (CampoRegistroId, CodigoGlosa)
);

-- ------------------------------------------------------------
-- 3. Catálogo de entidades ADRES (Campo 12)
-- ------------------------------------------------------------
CREATE TABLE dbo.ME_CatalogoEntidades (
    Id               INT           NOT NULL IDENTITY(1,1),
    CodigoEntidad    NVARCHAR(10)  NOT NULL,
    NombreEntidad    NVARCHAR(200) NOT NULL,
    CategoriaEntidad NVARCHAR(100) NOT NULL,                 -- EPS-Contributivo, EPS-Subsidiado, etc.
    Activo           BIT           NOT NULL DEFAULT 1,
    FechaCreacion    DATETIME2     NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT PK_ME_CatalogoEntidades PRIMARY KEY (Id),
    CONSTRAINT UQ_ME_CatalogoEntidades_Codigo UNIQUE (CodigoEntidad)
);

-- ------------------------------------------------------------
-- 4. Log de procesamiento de archivos ME
-- ------------------------------------------------------------
CREATE TABLE dbo.ME_LogProcesamiento (
    Id                   INT           NOT NULL IDENTITY(1,1),
    CargueArchivoId      INT           NULL,                 -- FK a CarguesArchivo (tabla existente)
    NombreArchivo        NVARCHAR(500) NOT NULL,
    TipoArchivo          NVARCHAR(10)  NOT NULL DEFAULT 'ME',
    FechaProcesamiento   DATETIME2     NOT NULL DEFAULT GETUTCDATE(),
    TotalRegistros       INT           NOT NULL DEFAULT 0,
    RegistrosValidos     INT           NOT NULL DEFAULT 0,
    RegistrosRechazados  INT           NOT NULL DEFAULT 0,
    Estado               NVARCHAR(20)  NOT NULL,             -- PROCESADO, RECHAZADO_TOTAL, ERROR
    MensajeError         NVARCHAR(2000) NULL,
    CONSTRAINT PK_ME_LogProcesamiento PRIMARY KEY (Id)
);

-- ------------------------------------------------------------
-- 5. Radicados: registros que pasaron todas las validaciones
-- ------------------------------------------------------------
CREATE TABLE dbo.ME_Radicados (
    Id                     INT           NOT NULL IDENTITY(1,1),
    LogProcesamientoId     INT           NOT NULL,
    NumeroLinea            INT           NOT NULL,
    TipoDocumento          NVARCHAR(2)   NOT NULL,
    NumeroIdentificacion   NVARCHAR(16)  NOT NULL,
    PrimerApellido         NVARCHAR(60)  NOT NULL,
    SegundoApellido        NVARCHAR(60)  NULL,
    PrimerNombre           NVARCHAR(60)  NOT NULL,
    SegundoNombre          NVARCHAR(60)  NULL,
    FechaNacimiento        DATE          NOT NULL,
    SexoBiologico          NVARCHAR(1)   NOT NULL,
    CodigoDepartamento     NVARCHAR(2)   NOT NULL,
    CodigoMunicipio        NVARCHAR(3)   NOT NULL,
    Zona                   NVARCHAR(2)   NOT NULL,
    CodigoEntidad          NVARCHAR(6)   NOT NULL,
    FechaAfiliacion        DATE          NOT NULL,
    EstadoAfiliacion       NVARCHAR(2)   NOT NULL,
    TipoDocumentoCotizante NVARCHAR(2)   NULL,
    RegistroJsonOriginal   NVARCHAR(MAX) NULL,
    FechaCreacion          DATETIME2     NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT PK_ME_Radicados PRIMARY KEY (Id),
    CONSTRAINT FK_ME_Radicados_Log FOREIGN KEY (LogProcesamientoId)
        REFERENCES dbo.ME_LogProcesamiento (Id),
    CONSTRAINT UQ_ME_Radicados_TipoNum UNIQUE (TipoDocumento, NumeroIdentificacion, LogProcesamientoId)
);

-- ------------------------------------------------------------
-- 6. Glosas generadas por registros rechazados
-- ------------------------------------------------------------
CREATE TABLE dbo.ME_GlosasRechazo (
    Id                   INT           NOT NULL IDENTITY(1,1),
    LogProcesamientoId   INT           NOT NULL,
    NumeroLinea          INT           NOT NULL,
    NumeroCampo          INT           NOT NULL,
    CodigoGlosa          NVARCHAR(10)  NOT NULL,
    DescripcionGlosa     NVARCHAR(500) NOT NULL,
    ValorRecibido        NVARCHAR(500) NULL,
    FechaCreacion        DATETIME2     NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT PK_ME_GlosasRechazo PRIMARY KEY (Id),
    CONSTRAINT FK_ME_GlosasRechazo_Log FOREIGN KEY (LogProcesamientoId)
        REFERENCES dbo.ME_LogProcesamiento (Id)
);

-- Índices de apoyo
CREATE INDEX IX_ME_ReglasValidacion_CampoId  ON dbo.ME_ReglasValidacion (CampoRegistroId);
CREATE INDEX IX_ME_Radicados_TipoNum         ON dbo.ME_Radicados (TipoDocumento, NumeroIdentificacion);
CREATE INDEX IX_ME_GlosasRechazo_Log_Linea   ON dbo.ME_GlosasRechazo (LogProcesamientoId, NumeroLinea);
CREATE INDEX IX_ME_LogProcesamiento_Cargue   ON dbo.ME_LogProcesamiento (CargueArchivoId);
