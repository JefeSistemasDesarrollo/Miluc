using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Sap.Oitm;
using Miluc.Shared.DTOs.Sap.Articulos;

namespace Miluc.Server.Servicios.SapService
{
    public class SapOitmService(SapDbContex _dbContex) : ISapOitmService
    {
        public async Task<(List<SapOitmDto> Data, int TotalRegistros)> GetListOitmAsync(string? buscar = null, int ? pagina = null, int? cantidad = null)
        {
            try
            {
                int cantidadTop = cantidad ?? 10;
                // var skip = (pagina - 1) * cantidadTop; select * from articulo que esten activos y se articlo para la venta 
                var query = _dbContex.OITM.AsNoTracking()
                    .Where(c => c.validFor == 'Y' && c.SellItem == 'Y').AsQueryable();

                //busca por coincidencia en el código o nombre del artículo
                if (!string.IsNullOrEmpty(buscar))
                {
                    query = query.Where(c => c.ItemCode.Contains(buscar) || c.ItemName.Contains(buscar));
                }
                else
                {
                    throw new ArgumentException("Por favor ingrese el codigo que desea buscar");
                }
                if (cantidad <= 0)
                    throw new ArgumentException("Quantity debe ser mayor a 0");

                int totalRegistros = await query.CountAsync();

                var lista = await query
                    .Skip((Convert.ToInt32(pagina) - 1) * cantidadTop)
                    .Take(cantidadTop)
                    .Select(o => new SapOitmDto
                    {
                        ItemCode = o.ItemCode,
                        ItemName = o.ItemName,
                        //BuyUnitMsr = o.BuyUnitMsr,
                        //SWeight1 = o.SWeight1,
                    }).ToListAsync();
                return (lista, totalRegistros);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener la lista de OITM: {ex.Message}", ex);
            }
        }


        public async Task<ArticuloPorListaDePreciosDto> GetListOitmCarcodeItemcodeAsync
           (string cardcode, string itemCode, int? cantidad = null)
        {

            ArticuloPorListaDePreciosDto? preciosCliente = new();
            ArticuloPorListaDePreciosDto? preciosEspeciales = new();
            try
            {
                if (string.IsNullOrWhiteSpace(cardcode))
                    throw new ArgumentException("CardCode es obligatorio");

                if (string.IsNullOrWhiteSpace(itemCode))
                    throw new ArgumentException("ItemCode es obligatorio");

                //consulta la tabla de precios especiales para el cliente y artículo  y los trae 
                //si no encuentra precios especiales, consulta la lista de precios del cliente y los trae
                preciosEspeciales = await _dbContex.OSPP.AsNoTracking()
                    .Include(o => o.OITM)
                    .Include(o => o.OITM.OSTC)
                    .Where(o => o.CardCode == cardcode && o.OITM.ItemCode == itemCode)
                    .Select(o => new ArticuloPorListaDePreciosDto
                    {
                        ItemCode = o.ItemCode,
                        ItemName = o.OITM.ItemName,
                        BuyUnitMsr = o.OITM.BuyUnitMsr,// U_CantidadKg = o.OITM.U_CantidadKg,
                        Quantity = cantidad,
                        SWeight1 = o.OITM.SWeight1,
                        PriceAcobrar = o.Price,
                        PriceEspecial = o.Price,
                        BaseSum = Convert.ToInt32(cantidad) * o.Price,
                        CodeTaxcode=o.OITM.OSTC.Code,
                        Rate = o.OITM.OSTC.Rate,
                        TaxCode = o.OITM.OSTC.Name,
                        
                        // U_CantidadKg=(cantidad /o.OITM.SWeight1),
                        //U_UnidadSal= ( cantidad * o.OITM.SWeight1)

                    }).FirstOrDefaultAsync();
                if (preciosEspeciales != null)
                {
                    var PrecioAlterno = await
                                           (from socio in _dbContex.OCRD
                                            join precios in _dbContex.OPLN
                                                           on socio.ListNum equals precios.ListNum
                                            join detItm in _dbContex.ITM1
                                                            on precios.ListNum equals detItm.PriceList
                                            join item in _dbContex.OITM
                                                            on detItm.ItemCode equals item.ItemCode
                                            where ((socio.CardCode == cardcode && item.ItemCode == itemCode) && (item.validFor == 'Y' && item.SellItem == 'Y'))
                                            select (new ArticuloPorListaDePreciosDto
                                            {
                                                ItemCode = item.ItemCode,
                                                ItemName = item.ItemName,
                                                PriceAsignado = detItm.Price
                                            })).FirstOrDefaultAsync();

                    if (PrecioAlterno != null && PrecioAlterno.ItemCode.ToUpper() == itemCode.ToUpper())
                    {
                        preciosEspeciales.PriceAsignado = PrecioAlterno.PriceAsignado;

                    }
                }

                if (preciosEspeciales == null)
                {
                    preciosCliente = await
                                          (from socio in _dbContex.OCRD
                                           join precios in _dbContex.OPLN
                                                          on socio.ListNum equals precios.ListNum
                                           join detItm in _dbContex.ITM1
                                                           on precios.ListNum equals detItm.PriceList
                                           join item in _dbContex.OITM
                                                           on detItm.ItemCode equals item.ItemCode
                                           where ((socio.CardCode == cardcode && item.ItemCode == itemCode) && (item.validFor == 'Y' && item.SellItem == 'Y'))
                                           select (new ArticuloPorListaDePreciosDto
                                           {
                                               ItemCode = item.ItemCode,
                                               ItemName = item.ItemName,
                                               BuyUnitMsr = item.BuyUnitMsr,
                                               Quantity = cantidad,
                                               SWeight1 = item.SWeight1,
                                               PriceAcobrar = detItm.Price,
                                               PriceAsignado = detItm.Price,
                                               BaseSum = Convert.ToInt32(cantidad) * Convert.ToDecimal(detItm.Price),
                                               CodeTaxcode=item.OSTC.Code,
                                               Rate = item.OSTC.Rate,
                                               TaxCode = item.OSTC.Name,
                                               // U_CantidadKg = (cantidad/ item.SWeight1),
                                               //U_UnidadSal = ( cantidad* item.SWeight1)
                                               //PriceAlternoEspecial = detItm.Price

                                           })).FirstOrDefaultAsync();

                    return preciosCliente ?? new ArticuloPorListaDePreciosDto();
                }
                return preciosEspeciales ?? new ArticuloPorListaDePreciosDto();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener la lista de OITM: {ex.Message}", ex);
            }
        }

    }
}
