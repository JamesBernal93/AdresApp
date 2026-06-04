Use AdresAppDb;
GO
-- ============================================================
-- Reglas de validación (expresiones regulares + tipo de val.)
-- Cada fila = una glosa + su regex para un campo específico
-- ============================================================

-- ── Glosas globales (aplican a cualquier campo) ─────────────
-- CampoRegistroId = 0 indica glosa global; se resuelven antes
-- de entrar a las validaciones por campo.
-- Usamos Id fijo -1 como "campo genérico" (no FK real).
-- En la implementación se evalúan por separado.

INSERT INTO dbo.ME_ReglasValidacion
    (CampoRegistroId, CodigoGlosa, DescripcionGlosa, ExpresionRegular, TipoValidacion, Orden)
VALUES

-- ── CAMPO 1: Tipo de documento del asegurado ────────────────
(1, 'GE0001',  'La entidad no corresponde',
    '^(CN|MS|RC|TI|CC|CE|SC|AS|PA|PT)$',
    'REGEX', 1),

(1, 'GE0001A', 'El código de la entidad no es válido',
    '^(CN|MS|RC|TI|CC|CE|SC|AS|PA|PT)$',
    'REGEX', 2),

-- ── CAMPO 2: Número de identificación del asegurado ─────────
(2, 'GE0002',  'El tipo de documento no es válido',
    '^[A-Z0-9]{3,16}$',
    'REGEX', 1),

(2, 'GE0002B', 'El tipo de identificación debe venir vacío',
    NULL,
    'NEGOCIO', 2),

-- ── CAMPO 3: Primer apellido del asegurado ──────────────────
(3, 'GE0003',  'Debe ser alfanumérico, longitud mínima 3 y máxima 16 (cuando es afiliado)',
    '^[A-ZÁÉÍÓÚÜÑ ]{1,60}$',
    'REGEX', 1),

(3, 'GE0003B', 'El número de identificación debe venir vacío',
    NULL,
    'NEGOCIO', 2),

(3, 'GE0003C', 'El número de documento del cotizante no corresponde al tipo de documento',
    NULL,
    'NEGOCIO', 3),

-- ── CAMPO 4: Segundo apellido del asegurado ─────────────────
(4, 'GE0004',  'El tipo de documento no es válido',
    '^[A-ZÁÉÍÓÚÜÑ ]{0,60}$',
    'REGEX', 1),

(4, 'GE0004A', 'Dato no válido para el tipo de campo',
    '^[A-ZÁÉÍÓÚÜÑ ]{0,60}$',
    'REGEX', 2),

-- ── CAMPO 5: Primer nombre del asegurado ────────────────────
(5, 'GE0005',  'Debe ser alfanumérico, longitud mínima 3 y máxima 16',
    '^[A-ZÁÉÍÓÚÜÑ ]{1,60}$',
    'REGEX', 1),

(5, 'GE0005A', 'Dato no válido para el tipo de campo',
    '^[A-ZÁÉÍÓÚÜÑ ]{1,60}$',
    'REGEX', 2),

(5, 'GE0005B', 'El Tipo y Número de Documento del Afiliado no debe repetirse en el archivo',
    NULL,
    'NEGOCIO', 3),

(5, 'GE0005C', 'El número de documento del afiliado no corresponde al tipo de documento',
    NULL,
    'NEGOCIO', 4),

-- ── CAMPO 6: Segundo nombre del asegurado ───────────────────
(6, 'GE0006',  'No se aceptan espacios ni caracteres especiales, longitud 1 a 20',
    '^[A-ZÁÉÍÓÚÜÑ ]{0,60}$',
    'REGEX', 1),

-- ── CAMPO 7: Fecha de nacimiento ────────────────────────────
(7, 'GE0007',  'No se aceptan espacios al principio ni al final, no se aceptan números ni caracteres especiales',
    '^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[0-2])/[0-9]{4}$',
    'REGEX', 1),

(7, 'GE9996',  'El formato de la fecha no corresponde al especificado',
    '^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[0-2])/[0-9]{4}$',
    'REGEX', 2),

-- ── CAMPO 8: Sexo biológico ──────────────────────────────────
(8, 'GE0008',  'No se aceptan espacios ni caracteres especiales, longitud mínima 1 y máxima 20',
    '^[MF]$',
    'REGEX', 1),

(8, 'GE0011',  'El tipo de sexo debe ser M o F',
    '^[MF]$',
    'REGEX', 2),

-- ── CAMPO 9: Código departamento de afiliación ──────────────
(9, 'GE0009',  'No se aceptan espacios al principio ni al final, no se aceptan números ni caracteres especiales',
    '^[0-9]{2}$',
    'REGEX', 1),

(9, 'GE0013',  'El código de departamento no existe',
    NULL,
    'CATALOGO', 2),

-- ── CAMPO 10: Código municipio de afiliación ────────────────
(10, 'GE0010',  'La fecha debe estar en formato DD-MM-YYYY',
    '^[0-9]{3}$',
    'REGEX', 1),

(10, 'GE0010A', 'La fecha no existe',
    NULL,
    'CATALOGO', 2),

(10, 'GE0010B', 'Fecha de nacimiento superior a la fecha inicial de afiliación',
    NULL,
    'NEGOCIO', 3),

(10, 'GE0010C', 'Fecha de nacimiento superior a la fecha de desafiliación',
    NULL,
    'NEGOCIO', 4),

(10, 'GE0010D', 'La edad no corresponde al tipo de documento',
    NULL,
    'NEGOCIO', 5),

(10, 'GE0014',  'El código de municipio no existe',
    NULL,
    'CATALOGO', 6),

(10, 'GE0014A', 'Código de municipio no corresponde',
    NULL,
    'CATALOGO', 7),

-- ── CAMPO 11: Zona ───────────────────────────────────────────
(11, 'GE0012',  'El tipo de afiliado debe ser C, T o B',
    '^(U1|U2|R1|R2)$',
    'REGEX', 1),

-- ── CAMPO 12: Código de la entidad ──────────────────────────
-- La validación real es contra el catálogo ME_CatalogoEntidades
(12, 'GE0001',  'La entidad no corresponde',
    '^[A-Z0-9]{6}$',
    'REGEX', 1),

-- ── CAMPO 13: Fecha de la afiliación y/o novedad ────────────
(13, 'GE0013',  'El código de departamento no existe',
    '^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[0-2])/[0-9]{4}$',
    'REGEX', 1),

(13, 'GE0015',  'La fecha de afiliación debe estar en formato DD/MM/YYYY',
    '^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[0-2])/[0-9]{4}$',
    'REGEX', 2),

(13, 'GE0015A', 'La fecha no existe',
    NULL,
    'NEGOCIO', 3),

(13, 'GE0015B', 'La fecha debe ser menor o igual a la fecha actual',
    NULL,
    'NEGOCIO', 4),

(13, 'GE0013B', 'El número de Identificación debe venir vacío',
    NULL,
    'NEGOCIO', 5),

-- ── CAMPO 14: Estado actual de la afiliación ────────────────
(14, 'GE0012',  'El tipo de afiliado debe ser C, T o B',
    '^(AC|RE|AF|SM|PL|SD)$',
    'REGEX', 1),

-- ── CAMPO 15: Tipo de documento del cotizante principal ─────
(15, 'GE0015',  'La fecha de afiliación debe estar en formato DD/MM/YYYY',
    '^(CN|TI|CC|CD|CE|SC|PA|MS|AS|PT|PE)$',
    'REGEX', 1),

(15, 'GE0015A', 'La fecha no existe',
    '^(CN|TI|CC|CD|CE|SC|PA|MS|AS|PT|PE)$',
    'REGEX', 2),

(15, 'GE0015B', 'La fecha debe ser menor o igual a la fecha actual',
    NULL,
    'NEGOCIO', 3);


-- ── Reglas globales de estructura (no asociadas a campo) ────
-- Se insertan referenciando el campo 1 con orden negativo
-- para que el motor las evalúe antes que cualquier campo.
INSERT INTO dbo.ME_ReglasValidacion
    (CampoRegistroId, CodigoGlosa, DescripcionGlosa, ExpresionRegular, TipoValidacion, Orden)
VALUES
(1, 'GE0000',  'La cadena de texto contiene caracteres no válidos',
    '^[^\x00-\x08\x0B\x0C\x0E-\x1F\x7F]*$',
    'REGEX', -10),

(1, 'GE9992',  'El campo debe ser vacío',
    NULL,
    'NEGOCIO', -9),

(1, 'GE9997',  'El número de campos del registro no coincide con los permitidos para este tipo de registro',
    NULL,
    'ESTRUCTURA', -8),

(1, 'GE9998',  'El tipo del registro no existe en el contexto actual',
    NULL,
    'ESTRUCTURA', -7),

(1, 'GE9999',  'La longitud del registro no concuerda con los tamaños permitidos',
    NULL,
    'LONGITUD', -6);
