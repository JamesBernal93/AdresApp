Use AdresAppDb;
GO
-- ============================================================
-- Inserción de los 15 campos del tipo de registro ME
-- Basado en la HU-1.2-BDUA-WS-3877-ME
-- ============================================================
SET IDENTITY_INSERT dbo.ME_CamposRegistro ON;

INSERT INTO dbo.ME_CamposRegistro
    (Id, TipoRegistro, NumeroCampo, NombreCampo, LongitudMinima, LongitudMaxima, TipoDato, EsObligatorio, ValoresPermitidos, FormatoFecha, Descripcion)
VALUES
-- Campo 1: Tipo de documento del asegurado
(1,  0,  1, 'Tipo de documento del asegurado',                          2,   2,  'CODIGO',   1, 'CN,MS,RC,TI,CC,CE,SC,AS,PA,PT',   NULL,           'Tipo de documento de identidad del afiliado'),

-- Campo 2: Número de identificación del asegurado
(2,  0,  2, 'Número de identificación del asegurado',                   3,  16,  'ALFANUM',  1,  NULL,                               NULL,           'Número de documento del afiliado, entre 3 y 16 caracteres'),

-- Campo 3: Primer apellido del asegurado
(3,  0,  3, 'Primer apellido del asegurado',                            1,  60,  'TEXTO',    1,  NULL,                               NULL,           'Primer apellido en mayúsculas, sin caracteres especiales'),

-- Campo 4: Segundo apellido del asegurado
(4,  0,  4, 'Segundo apellido del asegurado',                           0,  60,  'TEXTO',    1,  NULL,                               NULL,           'Segundo apellido en mayúsculas'),

-- Campo 5: Primer nombre del asegurado
(5,  0,  5, 'Primer nombre del asegurado',                              1,  60,  'TEXTO',    1,  NULL,                               NULL,           'Primer nombre en mayúsculas, sin caracteres especiales'),

-- Campo 6: Segundo nombre del asegurado
(6,  0,  6, 'Segundo nombre del asegurado',                             0,  60,  'TEXTO',    0,  NULL,                               NULL,           'Segundo nombre en mayúsculas'),

-- Campo 7: Fecha de nacimiento del asegurado
(7,  0,  7, 'Fecha de nacimiento del asegurado',                       10,  10,  'FECHA',    1,  NULL,                               'DD/MM/AAAA',   'Fecha de nacimiento formato DD/MM/AAAA'),

-- Campo 8: Sexo biológico del asegurado
(8,  0,  8, 'Sexo biológico del asegurado',                             1,   1,  'CODIGO',   1,  'M,F',                              NULL,           'M = Masculino, F = Femenino'),

-- Campo 9: Código departamento de afiliación
(9,  0,  9, 'Código departamento de afiliación',                        2,   2,  'NUMERICO', 1,  NULL,                               NULL,           'Codificación DIVIPOLA, 2 dígitos'),

-- Campo 10: Código municipio de afiliación
(10, 0, 10, 'Código municipio de afiliación',                           3,   3,  'NUMERICO', 1,  NULL,                               NULL,           'Codificación DIVIPOLA, 3 dígitos'),

-- Campo 11: Zona
(11, 0, 11, 'Zona',                                                     2,   2,  'CODIGO',   1,  'U1,U2,R1,R2',                      NULL,           'Zona de afiliación'),

-- Campo 12: Código de la entidad
(12, 0, 12, 'Código de la entidad',                                     6,   6,  'CODIGO',   1,  NULL,                               NULL,           'Código ADRES de la EPS, EAS u otra entidad'),

-- Campo 13: Fecha de la afiliación y/o novedad
(13, 0, 13, 'Fecha de la afiliación y/o novedad',                      10,  10,  'FECHA',    1,  NULL,                               'DD/MM/AAAA',   'Fecha de radicación del formulario único de afiliación'),

-- Campo 14: Estado actual de la afiliación
(14, 0, 14, 'Estado actual de la afiliación',                           2,   2,  'CODIGO',   1,  'AC,RE,AF,SM,PL,SD',                NULL,           'Estado vigente de la afiliación'),

-- Campo 15: Tipo de documento del cotizante principal
(15, 0, 15, 'Tipo de documento del cotizante principal',                2,   2,  'CODIGO',   1,  'CN,TI,CC,CD,CE,SC,PA,MS,AS,PT,PE', NULL,           'Tipo de documento del cabeza de familia / titular / PPL');

SET IDENTITY_INSERT dbo.ME_CamposRegistro OFF;
