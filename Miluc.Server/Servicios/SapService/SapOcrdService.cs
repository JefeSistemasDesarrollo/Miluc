using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Sap.ConexionSap;
using Miluc.Server.Interfaces.Sap.Ocrd;
using Miluc.Server.Models.Sap;
using Miluc.Shared.DTOs.Sap.Cliente;
using Miluc.Shared.DTOs.Sap.DireccionCliente;
using Miluc.Shared.DTOs.Sap.Impuesto;
using Newtonsoft.Json;
using System.Text;

namespace Miluc.Server.Servicios.SapService
{
    public class SapOcrdService(SapDbContex _sapDbContext, IConexionServiceLayer _serviceConexion) : ISapOcrdService
    {
        public async Task<SapClienteReaderDto> CreateClienteAsync(SapClienteCreateEditDto sapClienteCreateDto)
        {
            var credenciales = await _serviceConexion.ConexionSapService();
            try
            {
                if (credenciales.Valor == null)
                {
                    throw new ArgumentNullException($"No se encontraron las credenciales de SAP en la base de datos {nameof(credenciales)}");
                }
                if (string.IsNullOrEmpty(sapClienteCreateDto.CardCode))
                {
                    throw new ArgumentNullException($"El cliente es inválido {nameof(credenciales)}");
                }
                // throw new ArgumentNullException($"El cliente es inválido {nameof(credenciales)}");
                HttpClientHandler clientHandler = new()
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                };
                using var httpClient = new HttpClient(clientHandler);

                if (credenciales.Valor == null)
                {
                    credenciales = await _serviceConexion.ConexionSapService();
                }

                httpClient.DefaultRequestHeaders.Add("Cookie", $"B1SESSION={credenciales.Valor.B1SESSION}; RouteId={credenciales.Valor.ROUTEID}");

                string url = $"{credenciales.Valor.URLServiceLayer}/BusinessPartners";
                var json = JsonConvert.SerializeObject(sapClienteCreateDto);

                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, data);

                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    string mensajeErrorSap = content;
                    try
                    {
                        dynamic errorObj = JsonConvert.DeserializeObject(content);
                        mensajeErrorSap = errorObj?.error?.message?.value ?? content;
                    }
                    catch { }
                    // 1. MATAMOS LA SESIÓN PRIMERO de forma segura y controlada
                    await _serviceConexion.LogoutAsync(credenciales.Valor.URLServiceLayer, credenciales.Valor.B1SESSION, credenciales.Valor.ROUTEID);
                    // 2. Lanzamos el error real hacia el controlador. Ya no hay finally que estorbe.
                    throw new HttpRequestException($"SAP Service Layer rechazó la creación del cliente. Detalles: {mensajeErrorSap}");
                }
                var result = JsonConvert.DeserializeObject<SapClienteReaderDto>(content);

                //aca vamos a editar las direciones 
                httpClient.DefaultRequestHeaders.Add("B1S-ReplaceCollectionsOnPatch", "true");

                string urlDireccion = $"{credenciales.Valor.URLServiceLayer}/BusinessPartners('{sapClienteCreateDto.CardCode}')";

                var jsonDireccion = JsonConvert.SerializeObject(sapClienteCreateDto.BPAddresses);
                //"BPAddresses": agregar esto para que pase el json 
                jsonDireccion = "{\"BPAddresses\":" + jsonDireccion + "}";

                var dataDireccion = new StringContent(jsonDireccion, Encoding.UTF8, "application/json");

                var responseDireccion = await httpClient.PatchAsync(urlDireccion, dataDireccion);

                var contentDireccion = await responseDireccion.Content.ReadAsStringAsync();

                if (contentDireccion != null)
                {
                    if (!responseDireccion.IsSuccessStatusCode)
                    {
                        string mensajeErrorSapDireccion = contentDireccion;
                        try
                        {
                            dynamic errorObjDireccion = JsonConvert.DeserializeObject(contentDireccion);
                            mensajeErrorSapDireccion = errorObjDireccion?.error?.message?.value ?? contentDireccion;
                        }
                        catch { }
                        // 1. MATAMOS LA SESIÓN PRIMERO de forma segura y controlada
                        await _serviceConexion.LogoutAsync(credenciales.Valor.URLServiceLayer, credenciales.Valor.B1SESSION, credenciales.Valor.ROUTEID);
                        // 2. Lanzamos el error real hacia el controlador. Ya no hay finally que estorbe.
                        throw new HttpRequestException($"SAP Service Layer rechazó la actualización de direcciones del cliente. Detalles: {mensajeErrorSapDireccion}");
                    }
                }
                // 1. MATAMOS LA SESIÓN PRIMERO antes de retornar el éxito
                await _serviceConexion.LogoutAsync(credenciales.Valor.URLServiceLayer, credenciales.Valor.B1SESSION, credenciales.Valor.ROUTEID);
                // 2. Devolvemos la respuesta limpia al controlador
                return new SapClienteReaderDto
                {
                    CardCode = result.CardCode,
                    CardName = result.CardName,
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear el pedido: {ex.Message}", ex);
            }

        }


        public async Task<SapClienteReaderDto> EditarClienteAsunc(SapClienteCreateEditDto sapClienteEditDto)
        {
            var credenciales = await _serviceConexion.ConexionSapService();
            try
            {
                if (credenciales.Valor == null)
                {
                    throw new ArgumentNullException($"No se encontraron las credenciales de SAP en la base de datos {nameof(credenciales)}");
                }
                if (string.IsNullOrEmpty(sapClienteEditDto.CardCode))
                {
                    throw new ArgumentNullException($"El cliente es inválido {nameof(credenciales)}");
                }
                // throw new ArgumentNullException($"El cliente es inválido {nameof(credenciales)}");
                HttpClientHandler clientHandler = new()
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                };

                using var httpClient = new HttpClient(clientHandler);

                if (credenciales.Valor == null)
                {
                    credenciales = await _serviceConexion.ConexionSapService();
                }

                httpClient.DefaultRequestHeaders.Add("Cookie", $"B1SESSION={credenciales.Valor.B1SESSION}; RouteId={credenciales.Valor.ROUTEID}");

                httpClient.DefaultRequestHeaders.Remove("B1S-ReplaceCollectionsOnPatch");
                httpClient.DefaultRequestHeaders.Add("B1S-ReplaceCollectionsOnPatch", "true");

                string url = $"{credenciales.Valor.URLServiceLayer}/BusinessPartners('{sapClienteEditDto.CardCode}')";

                var json = JsonConvert.SerializeObject(sapClienteEditDto);

                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PatchAsync(url, data);

                var content = await response.Content.ReadAsStringAsync();


                if (!response.IsSuccessStatusCode)
                {
                    string mensajeErrorSap = content;

                    try
                    {
                        dynamic errorObj = JsonConvert.DeserializeObject(content);
                        mensajeErrorSap = errorObj?.error?.message?.value ?? content;
                    }
                    catch
                    {
                    }

                    await _serviceConexion.LogoutAsync(
                        credenciales.Valor.URLServiceLayer,
                        credenciales.Valor.B1SESSION,
                        credenciales.Valor.ROUTEID);

                    throw new HttpRequestException($"SAP rechazó la actualización del cliente. {mensajeErrorSap}");
                }

                httpClient.DefaultRequestHeaders.Remove("B1S-ReplaceCollectionsOnPatch");
                httpClient.DefaultRequestHeaders.Add("B1S-ReplaceCollectionsOnPatch", "true");

                var jsonDireccion = JsonConvert.SerializeObject(new
                {
                    BPAddresses = sapClienteEditDto.BPAddresses
                });

                var dataDireccion = new StringContent(jsonDireccion, Encoding.UTF8, "application/json");

                var responseDireccion = await httpClient.PatchAsync(url, dataDireccion);

                var contentDireccion = await responseDireccion.Content.ReadAsStringAsync();

                if (!responseDireccion.IsSuccessStatusCode)
                {
                    string mensajeError = contentDireccion;

                    try
                    {
                        dynamic errorObj = JsonConvert.DeserializeObject(contentDireccion);
                        mensajeError = errorObj?.error?.message?.value ?? contentDireccion;
                    }
                    catch
                    {
                    }

                    await _serviceConexion.LogoutAsync(
                        credenciales.Valor.URLServiceLayer,
                        credenciales.Valor.B1SESSION,
                        credenciales.Valor.ROUTEID);

                    throw new HttpRequestException($"SAP rechazó la actualización de direcciones. {mensajeError}");
                }

                await _serviceConexion.LogoutAsync(
                    credenciales.Valor.URLServiceLayer,
                    credenciales.Valor.B1SESSION,
                    credenciales.Valor.ROUTEID);

                return new SapClienteReaderDto
                {
                    CardCode = sapClienteEditDto.CardCode,
                    CardName = sapClienteEditDto.CardName
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar los clientes: {ex.Message}");

            }
        }

        public async Task<bool> EliminarClienteAsync(string cardCode)
        {
            var credenciales = await _serviceConexion.ConexionSapService();

            try
            {
                if (credenciales.Valor == null)
                    throw new ArgumentNullException(nameof(credenciales), "No se encontraron las credenciales de SAP.");

                if (string.IsNullOrWhiteSpace(cardCode))
                    throw new ArgumentNullException(nameof(cardCode), "El código del cliente es obligatorio.");

                //HttpClientHandler clientHandler = new()
                //{
                //    ServerCertificateCustomValidationCallback =HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                //};



                HttpClientHandler clientHandler = new()
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                };



                using var httpClient = new HttpClient(clientHandler);

                httpClient.DefaultRequestHeaders.Add("Cookie",$"B1SESSION={credenciales.Valor.B1SESSION}; RouteId={credenciales.Valor.ROUTEID}");

                string url = $"{credenciales.Valor.URLServiceLayer}/BusinessPartners('{cardCode}')";

                var response = await httpClient.DeleteAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    string mensaje = content;

                    try
                    {
                        dynamic error = JsonConvert.DeserializeObject(content);
                        mensaje = error?.error?.message?.value ?? content;
                    }
                    catch
                    {
                    }

                    throw new Exception(mensaje);
                }

                return true;
            }
            finally
            {
                if (credenciales?.Valor != null)
                {
                    await _serviceConexion.LogoutAsync(
                        credenciales.Valor.URLServiceLayer,
                        credenciales.Valor.B1SESSION,
                        credenciales.Valor.ROUTEID);
                }
            }
        }
        public async Task<(List<SapClienteReaderDto> Data, int TotalRegistros)> GetallClienteAsync(string? buscar = null, int ? pagina = null, int? cantidad = null, int ?codVendedorSAP = null)
        {
            try
            {
                IQueryable<OcrdClienteSap> queryBusqueda = null;


                if (!string.IsNullOrWhiteSpace(buscar))
                {
                    buscar = buscar.Trim();
                }

               

               int pag =pagina ?? 1;
               
                int codiVendedor = codVendedorSAP ?? -1;


                if (codiVendedor == -1)
                {
                 queryBusqueda = _sapDbContext.OCRD.AsNoTracking()
                   .Include(c => c.OCRG)
                   .Where(c => c.CardType == "C")
                   .AsQueryable();
                }
                else if (codiVendedor != -1)
                {
                    queryBusqueda = _sapDbContext.OCRD.AsNoTracking()
                   .Include(c => c.OCRG)
                   .Where(c => c.CardType == "C" && c.OSLP.SlpCode == codiVendedor)
                   .AsQueryable();
                }

                int cantidadTop = cantidad ?? 20;

                if (!string.IsNullOrEmpty(buscar))
                {
                    queryBusqueda = queryBusqueda.Where(c => c.CardCode.Contains(buscar) || c.CardName.Contains(buscar));
                }
                int totalEncontrados = await queryBusqueda.CountAsync();

                var dataBusqueda = await queryBusqueda.OrderByDescending(x => x.CardName)
                    .Skip((pag - 1) * cantidadTop)
                    .Take(cantidadTop)
                    .Select(C => new SapClienteReaderDto
                    {
                        CardCode = C.CardCode,
                        CardName = C.CardName,
                        CardFName = C.CardFName,
                        CardType = C.CardType,
                        // City=C.CRD1.City,
                        City = C.City,
                        GroupCode = C.OCRG.GroupCode,
                        GroupName = C.OCRG.GroupName,
                        ListNum = C.OPLN.ListNum,//lista de precios 
                        ListName = C.OPLN.ListName,
                        LicTradNum = C.LicTradNum,
                        Address = C.Address,
                        ZipCode = C.ZipCode,
                        MailAddres = C.MailAddres,
                        Phone1 = C.Phone1,
                        Phone2 = C.Phone2,
                        VatStatus = C.VatStatus,
                        Currency = C.Currency,
                        Celular = C.Cellular,
                        County = C.County,
                        E_Mail = C.E_Mail,
                        U_HBT_MailRecep_FE = C.U_HBT_MailRecep_FE,

                        QryGroup1 = C.QryGroup1,
                        QryGroup2 = C.QryGroup2,
                        QryGroup3 = C.QryGroup3,
                        QryGroup4 = C.QryGroup4,
                        QryGroup5 = C.QryGroup5,
                        QryGroup6 = C.QryGroup6,
                        QryGroup7 = C.QryGroup7,
                        CreateDate = C.CreateDate,
                        UpdateDate = C.UpdateDate,
                        CrCardNum = C.CrCardNum,
                        validFor = C.validFor,
                        DebPayAcct = C.DebPayAcct,
                        shipToDef = C.shipToDef,
                        Balance = C.Balance,
                        MailBlock = C.MailBlock,
                        Password = C.Password,
                        Deleted = C.Deleted,
                        DocEntry = C.DocEntry,
                        U_HBT_Nombres = C.U_HBT_Nombres,
                        U_HBT_Apellido1 = C.U_HBT_Apellido1,
                        U_HBT_Apellido2 = C.U_HBT_Apellido2,
                        SlpCode = C.OSLP.SlpCode,
                        SlpName = C.OSLP.SlpName,
                        GroupNum = C.OCTG.GroupNum,
                        PymntGroup = C.OCTG.PymntGroup,
                        PrioCode = C.OBPP != null ? C.OBPP.PrioCode : 0,
                        PrioDesc = C.OBPP != null ? C.OBPP.PrioDesc : null,
                        //PrioCode = C.OBPP.PrioCode,
                        //PrioDesc = C.OBPP.PrioDesc,
                        Block = C.Block,
                        Free_Text = C.Free_Text,
                        // FreeText= C.FreeText,
                        DireccionPrincipal = C.Direcciones.Select(d => new DireccionesCrd1ReaderDto
                        {
                            Street = d.Street,
                            Block = d.Block,
                            ZipCode = d.ZipCode,
                            City = d.City,
                            Country = d.Country,
                            County = d.County,
                            State = d.State,
                            Address = d.Address,
                            CardCode = d.CardCode,
                            U_HBT_DirMM = d.U_HBT_DirMM,
                            AdresType = d.AdresType
                        }).ToList(),
                    }).ToListAsync();

                //.Include(c => c.OCRG).

                return (dataBusqueda, totalEncontrados);

            }
            catch (Exception ex)
            {
                throw new Exception($"Error al listar los clientes: {ex.Message}");
            }

        }


        public async Task<SapClienteReaderDto> GetByIdClienteAsync(string? cardcode = null)
        {
            try
            {
                if (string.IsNullOrEmpty(cardcode))
                {
                    throw new ArgumentOutOfRangeException($"El codigo del cliente es inválido {nameof(cardcode)}");
                }

                var Cliente = await _sapDbContext.OCRD.AsNoTracking()
                    //.Include(c => c.HBT_REGIMTRIB)
                    //.Include(c => c.HBT_TIPODOC)
                    //.Include(c => c.HBT_MUNICIPIO)
                    //.Include(c => c.HBT_TIPODOC)
                    //.Include(c => c.OBPP)
                    //.Include(c => c.HBT_ACTIVIDADECO)
                    //.Include(c => c.HBT_REGIMENFISCAL)
                    //.Include(c => c.HBT_RESPFISCAL)
                    //.Include(c => c.CRD4)
                    .Where(c => c.CardCode == cardcode)
                   .Select(C => new SapClienteReaderDto
                   {
                       U_HBT_TipDoc = C.HBT_TIPODOC.Code,
                       U_HBT_MailRecep_FE = C.U_HBT_MailRecep_FE,
                       CardCode = C.CardCode,
                       CardName = C.CardName,
                       CardFName = C.CardFName,
                       CardType = C.CardType,
                       GroupCode = C.OCRG.GroupCode,
                       ListNum = C.OPLN.ListNum,
                       City = C.City,
                       LicTradNum = C.LicTradNum,
                       GroupName = C.OCRG.GroupName,
                       ListName = C.OPLN.ListName,
                       U_HBT_RegTrib = C.HBT_REGIMTRIB.code, //regimen tributario 
                       U_HBT_TipDocName = C.HBT_TIPODOC.Name,
                       U_HBT_RegTribName = C.HBT_REGIMTRIB.Name,
                       HBT_MUNICIPIOCode = C.HBT_MUNICIPIO.Code,
                       HBT_MUNICIPIOName = C.HBT_MUNICIPIO.Name,
                       U_HBT_ActEcoCode = C.HBT_ACTIVIDADECO.Code,
                       U_HBT_ActEcoName = C.HBT_ACTIVIDADECO.U_Descripcion,
                       U_HBT_MedPag = C.U_HBT_MedPag,
                       U_HBT_Residente = C.U_HBT_Residente,
                       //U_HBT_ReSFisCode = C.HBT_REGIMENFISCAL.Code,
                       //U_HBT_ReSFisCode = C.U_HBT_ResFis,
                       U_HBT_regFisCode = C.HBT_REGIMENFISCAL.Code,
                       U_HBT_regFisName = C.HBT_REGIMENFISCAL.Name,
                       U_AplicaBolsaMercantil = C.U_AplicaBolsaMercantil,
                       U_HBT_ReSFisCode = C.HBT_RESPFISCAL.Code,
                       U_HBT_ResFisCode1 = C.U_HBT_ResFis1,
                       U_HBT_resFisName = C.HBT_REGIMENFISCAL.Name,
                       U_HBT_InfoTrib = C.U_HBT_InfoTrib,
                       //APELLIDOS 
                       U_HBT_Nombres = C.U_HBT_Nombres,
                       U_HBT_Apellido1 = C.U_HBT_Apellido1,
                       U_HBT_Apellido2 = C.U_HBT_Apellido2,
                       //medios magneticos 
                       U_HBT_TipEnt = Convert.ToString(C.U_HBT_TipEnt),//tipo entidad
                       U_HBT_Nacional = C.U_HBT_Nacional.ToString(),
                       U_HBT_TipExt = C.U_HBT_TipExt,
                       Address = C.Address,
                       ZipCode = C.ZipCode,
                       MailAddres = C.MailAddres,
                       Phone1 = C.Phone1,
                       Phone2 = C.Phone2,
                       VatStatus = C.VatStatus,
                       Currency = C.Currency,
                       Celular = C.Cellular,
                       County = C.County,
                       E_Mail = C.E_Mail,
                       QryGroup1 = C.QryGroup1,
                       QryGroup2 = C.QryGroup2,
                       QryGroup3 = C.QryGroup3,
                       QryGroup4 = C.QryGroup4,
                       QryGroup5 = C.QryGroup5,
                       QryGroup6 = C.QryGroup6,
                       QryGroup7 = C.QryGroup7,
                       QryGroup8 = C.QryGroup8,
                       QryGroup9 = C.QryGroup9,
                       QryGroup10 = C.QryGroup10,
                       CreateDate = C.CreateDate,
                       UpdateDate = C.UpdateDate,
                       CrCardNum = C.CrCardNum,
                       validFor = C.validFor,
                       DebPayAcct = C.DebPayAcct,
                       shipToDef = C.shipToDef,
                       Balance = C.Balance,
                       MailBlock = C.MailBlock,
                       Password = C.Password,
                       Deleted = C.Deleted,
                       DocEntry = C.DocEntry,
                       SlpCode = C.OSLP.SlpCode,
                       SlpName = C.OSLP.SlpName,
                       GroupNum = C.OCTG.GroupNum,
                       PymntGroup = C.OCTG.PymntGroup,
                       PrioCode = C.OBPP !=null ? C.OBPP.PrioCode : 0,
                       PrioDesc = C.OBPP !=null ? C.OBPP.PrioDesc : null,
                       Block = C.Block,
                       Free_Text = C.Free_Text,
                       DireccionPrincipal = C.Direcciones.Select(d => new DireccionesCrd1ReaderDto
                       {
                           Street = d.Street,
                           Block = d.Block,
                           ZipCode = d.ZipCode,
                           City = d.City,
                           Country = d.Country,
                           County = d.County,
                           State = d.State,
                           Address = d.Address,
                           CardCode = d.CardCode,
                           U_HBT_DirMM = d.U_HBT_DirMM,
                           AdresType = d.AdresType
                       }).ToList(),
                       BPWithholdingTaxCollection = C.CRD4.Select(w => new SapBPWithholdingTaxDto
                       {
                           WTCode = w.WTCode,
                           BPCode = w.CardCode,
                       }).ToList()
                   }).FirstOrDefaultAsync();

                return Cliente;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error al listar los clientes: {ex.Message}");

            }
        }
    }
}
