Use AdresAppDb;
GO
-- ============================================================
-- Catálogo de entidades ADRES  (Campo 12 del archivo ME)
-- Fuente: HU-1.2-BDUA-WS-3877-ME
-- ============================================================
INSERT INTO dbo.ME_CatalogoEntidades (CodigoEntidad, NombreEntidad, CategoriaEntidad) VALUES
-- EPS Contributivo
('EPS037', 'Nueva EPS',                                    'EPS-Contributivo'),
('EPS005', 'EPS Sanitas',                                  'EPS-Contributivo'),
('EPS010', 'EPS Sura',                                     'EPS-Contributivo'),
('EPS002', 'Salud Total',                                  'EPS-Contributivo'),
('EPS017', 'EPS Famisanar',                                'EPS-Contributivo'),
('EPS042', 'Coosalud',                                     'EPS-Contributivo'),
-- EPS Subsidiado
('EPSS37', 'Nueva EPS (Subsidiado)',                       'EPS-Subsidiado'),
('ESS024', 'Coosalud',                                     'EPS-Subsidiado'),
('ESS207', 'Mutual Ser',                                   'EPS-Subsidiado'),
('EPSS34', 'Capital Salud',                                'EPS-Subsidiado'),
('EPSS40', 'Savia Salud',                                  'EPS-Subsidiado'),
('ESS118', 'Emssanar',                                     'EPS-Subsidiado'),
('ESS062', 'Asmet Salud',                                  'EPS-Subsidiado'),
-- EAS Adaptada
('EAS027', 'Fondo Pasivo Ferrocarriles',                   'EAS-Adaptada'),
-- Excepcion / Especial
('FMS001', 'Fuerzas Militares',                            'Excepcion-Especial'),
('POL001', 'Policia Nacional',                             'Excepcion-Especial'),
('RES002', 'Ecopetrol',                                    'Excepcion-Especial'),
('RES004', 'Magisterio (Fomag)',                           'Excepcion-Especial'),
('RES008', 'Universidad Nacional',                         'Excepcion-Especial'),
('RES011', 'Universidad de Antioquia',                     'Excepcion-Especial'),
('RES007', 'Universidad del Valle',                        'Excepcion-Especial'),
('RES009', 'Universidad Industrial de Santander',          'Excepcion-Especial'),
('RES012', 'Universidad de Cordoba',                       'Excepcion-Especial'),
-- Poblacion Reclusa
('CAS001', 'INPEC',                                        'Poblacion-Reclusa'),
-- Planes Voluntarios
('EMP002', 'Medplus (Medicina Prepagada)',                  'Planes-Voluntarios'),
('EMP015', 'Medisanitas',                                  'Planes-Voluntarios'),
('EMP017', 'Colmedica',                                    'Planes-Voluntarios'),
('EMP029', 'Colpatria',                                    'Planes-Voluntarios'),
('EMP023', 'Colsanitas (Medicina Prepagada)',               'Planes-Voluntarios'),
('EMP028', 'Coomeva (Medicina Prepagada)',                  'Planes-Voluntarios'),
('SAP008', 'EMI (Servicio de Ambulancia)',                  'Planes-Voluntarios'),
('SAP026', 'Emermedica (Servicio de Ambulancia)',           'Planes-Voluntarios');
