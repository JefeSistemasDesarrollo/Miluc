using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Sap.LocalizacionSap;
using Miluc.Shared.DTOs.Sap.ActividadEconomica;
using Miluc.Shared.DTOs.Sap.Cliente.HBT_RESPFISCAL;
using Miluc.Shared.DTOs.Sap.Cliente.RegimeNTributario;
using Miluc.Shared.DTOs.Sap.RegimenFiscal;
using Miluc.Shared.DTOs.Sap.Retenciones;

namespace Miluc.Server.Servicios.SapService
{
    public class SapLocalizacionClienteService(SapDbContex sapDbContex) : ILocalizacionSapCliente
    {
        public async Task<(List<HBT_codigosPostalesDto>data, int cantidadRegistros)> GetAllCodigosPostalesAsync(string? buscar = null,int? pagina = null,int? cantidad = null)
        {

            try
            {
                var query = sapDbContex.HBT_CODIGOSPOSTALES.AsNoTracking().AsQueryable();

                if (!string.IsNullOrEmpty(buscar))
                {
                    query = query.Where(x => x.Code.Contains(buscar.ToUpper()) || x.U_HBT_Lugar.Contains(buscar));
                }
                 
                var totalRegistros = await query.CountAsync();

                if (pagina.HasValue && cantidad.HasValue)
                {
                    if (pagina.Value < 0 )
                        throw new ArgumentOutOfRangeException(nameof(pagina), "El número de página no puede ser negativo.");

                    if (cantidad.Value <= 0)
                        throw new ArgumentOutOfRangeException(nameof(cantidad), "La cantidad de registros por página debe ser mayor que cero.");

                    query = query.Skip((pagina.Value - 1) * cantidad.Value).Take(cantidad.Value);
                }

                var codigosPostales = await query.Select(x => new HBT_codigosPostalesDto
                {
                    Code = x.Code,
                    Name = x.Name,
                    U_HBT_Lugar = x.U_HBT_Lugar
                }).ToListAsync();
         
                return (codigosPostales, totalRegistros);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los datos de códigos postales: " + ex.Message);
            }

        }
        public async Task<(List<HbtMunicipiosDto> data, int cantidad)> GetAllMunicipiosAsync(string? buscar = null, int ?pagina = null, int? cantidad = null)
        {
            try
            {
                if (string.IsNullOrEmpty(cantidad?.ToString()))
                {
                    cantidad = 10; // Valor por defecto
                }

                var query = sapDbContex.HBT_MUNICIPIO.AsNoTracking().AsQueryable();

                if (!string.IsNullOrEmpty(buscar?.ToString()))
                {
                    query = query.Where(x => x.Code.Contains(buscar.ToUpper()) || x.Name.Contains(buscar.ToUpper()));
                }


                var totalRegistros = await query.CountAsync();


                if (pagina.HasValue && cantidad.HasValue)
                {
                    if (pagina.Value < 0)
                        throw new ArgumentOutOfRangeException(nameof(pagina), "El número de página no puede ser negativo.");
                    if (cantidad.Value <= 0)
                        throw new ArgumentOutOfRangeException(nameof(cantidad), "La cantidad de registros por página debe ser mayor que cero.");

                    query = query.Skip((pagina.Value - 1) * cantidad.Value).Take(cantidad.Value);


                }
                var municipios = await query.Select(x => new HbtMunicipiosDto
                {
                    Code = x.Code,
                    Name = x.Name
                }).ToListAsync();

                return  (municipios, totalRegistros);
            

            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los datos de municipios: " + ex.Message);
            }
        }
        public async Task<List<RegimenTributarioDto>> GetAllRegimenTributarioAsync()
        {
            try
            {
                var regimenTributario = await sapDbContex.HBT_REGIMTRIB.AsNoTracking().Select(x => new RegimenTributarioDto
                {
                    code = x.code,
                    Name = x.Name
                }).ToListAsync();

                return regimenTributario;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los datos de Régimen Tributario: " + ex.Message);
            }
        }
        public async Task<List<OkiResponsabilidadesFiscalesDto>> GetAllResponsabilidadesFiscalesAsync(string? buscar = null, int ?pagina = null, int? cantidad = null)
        {
            try
            {
                if (string.IsNullOrEmpty(cantidad?.ToString()))
                {
                    cantidad = 10; // Valor por defecto
                }
                var query = sapDbContex.OK1_FE_RESPONFIS.AsNoTracking().AsQueryable();

                if (!string.IsNullOrEmpty(buscar))
                {
                    query = query.Where(x => x.Name.Contains(buscar.ToUpper() ));
                }
                var totalRegistros = await query.CountAsync();
                if (cantidad.HasValue && pagina.HasValue)
                {
                    if(pagina.Value < 0)
                        throw new ArgumentOutOfRangeException(nameof(pagina), "El número de página no puede ser negativo.");

                    if (cantidad.Value <= 0)
                        throw new ArgumentOutOfRangeException(nameof(cantidad), "La cantidad de registros por página debe ser mayor que cero.");    

                    query = query.Skip((Convert.ToInt32(pagina) - 1) * cantidad.Value).Take(cantidad.Value);
                }
                var responsabilidadesFiscales = await query.Select(x => new OkiResponsabilidadesFiscalesDto
                {
                    Code = x.Code,
                    Name = x.Name,
                    U_descripcion = x.U_descripcion,
                    U_Inactivo = x.U_Inactivo
                }).ToListAsync();
                return responsabilidadesFiscales;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error al obtener los datos de Responsabilidades Fiscales: " + ex.Message);
            }
        }

        public async Task<List<TiposDocumentoDto>> GetAllTiposDocumentoAsync()
        {
            try
            {
                var tiposDocumento = await sapDbContex.HBT_TIPODOC.Select(x => new TiposDocumentoDto
                {
                    Code = x.Code,
                    Name = x.Name
                }).ToListAsync();


                if (tiposDocumento == null)
                {
                    throw new InvalidOperationException("No se encontraron datos de Tipos de Documento.");
                }
                return tiposDocumento;
            }

            catch (Exception ex)
            {

                throw new Exception("Error al obtener los datos de Tipos de Documento: " + ex.Message);

            }
        }
        public async Task<List<HBT_RegimenFiscalDto>> GetAllRegimenFiscal()
        {
            try
            {
                var listaRegimenFiscal = await sapDbContex.HBT_REGIMENFISCAL.AsNoTracking().Select(x => new HBT_RegimenFiscalDto
                {
                    Name = x.Name,
                    Code = x.Code,
                }).ToListAsync();

                if (listaRegimenFiscal == null)
                {
                    throw new InvalidOperationException($"No se encontraron regiemn fiscal");
                }

                return listaRegimenFiscal;


            }
            catch (Exception ex)
            {
                throw new Exception($"error {ex.Message}");
            }
        }
        public async Task<List<RetencionDto>> getAllRetenciones()
        {
            try { 
                var responsabilidadesFiscales =
                    await sapDbContex.OWHT.AsNoTracking().Where(x => x.Type=='V').
                    Select(x => new RetencionDto
                {
                   WTCode = x.WTCode,
                   WTName = x.WTName,
                }).ToListAsync();


                if (responsabilidadesFiscales == null || responsabilidadesFiscales.Count == 0)
                {
                    throw new InvalidOperationException("No se encontraron datos de Responsabilidades Fiscales.");
                }
                return responsabilidadesFiscales;
            }
            catch (Exception ex)
            {

                throw new Exception("Error al obtener los datos de Responsabilidades Fiscales: " + ex.Message);
            }
        }
        public async Task<(List<HbtActividadEconomicaDto> data, int cantidad)> GetActividadEconomica(string? buscar = null, int ?pagina = null, int? cantidad = null)
        {
            try
            {

               
                var query = sapDbContex.HBT_ACTIVIDADECO.AsNoTracking().AsQueryable();

                //if (!string.IsNullOrEmpty(buscar))
                //{
                //    query = query.Where(x => x.U_Descripcion.ToUpper().Contains(buscar) || x.Code.ToUpper().Contains(buscar));
                //}




                if (!string.IsNullOrWhiteSpace(buscar))
                {
                    buscar = buscar.ToUpper();

                    query = query.Where(x => 
                        EF.Functions.Collate(x.U_Descripcion, "Modern_Spanish_CI_AI").Contains(buscar) ||
                        EF.Functions.Collate(x.Code, "Modern_Spanish_CI_AI").Contains(buscar));
                }



                var cantidadRegistros = await query.CountAsync();


                if (pagina.HasValue && cantidad.HasValue)
                {
                    if (pagina.Value < 0)
                        throw new ArgumentOutOfRangeException(nameof(pagina), "El número de página no puede ser negativo.");
                    if (cantidad.Value <= 0)
                        throw new ArgumentOutOfRangeException(nameof(cantidad), "La cantidad de registros por página debe ser mayor que cero.");

                    query = query.Skip((pagina.Value - 1) * cantidad.Value).Take(cantidad.Value);
                }




                var actividadEconomicaDtos = await query.Select(x => new HbtActividadEconomicaDto
                {
                    Code = x.Code,
                    Name = x.Name,
                    U_Descripcion=x.U_Descripcion
                }).ToListAsync();

                return (actividadEconomicaDtos, cantidadRegistros);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los datos de la actividad economica : " + ex.Message);
            }
        }

        public async Task<List<SapResponsabilidadFiscalDto>> GetResponsabilidadFiscal1(string? buscar = null, int ?pagina = null , int? cantidad = null)
        {
            try
            {

                if (string.IsNullOrEmpty(cantidad?.ToString()))
                {
                    cantidad = 10; // Valor por defecto
                }

                var query = sapDbContex.HBT_RESPFISCAL.AsQueryable();

                if (!string.IsNullOrEmpty(buscar))
                {
                    query = query.Where(x => x.Name.Contains(buscar.ToUpper()));
                }

                var totalRegistros = await query.CountAsync();

                if (cantidad.HasValue)
                {
                    query = query.Skip((Convert.ToInt32(pagina) - 1) * cantidad.Value).Take(cantidad.Value);
                }

                var responsabilidadFiscalDtos = await query.Select(x => new SapResponsabilidadFiscalDto
                {
                    Code = x.Code,
                    Name = x.Name,
                 
                }).ToListAsync();

                if (responsabilidadFiscalDtos == null || responsabilidadFiscalDtos.Count == 0)
                {
                    throw new InvalidOperationException("No se pudieron encontrar las responsabilidad fiscal.");
                }
                return responsabilidadFiscalDtos;
            }
            catch (Exception ex)
            {

                throw new Exception("Error al obtener los datos de municipios: " + ex.Message);
            }
        }
    }
}
