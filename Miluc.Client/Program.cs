using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Miluc.Client;
using Miluc.Client.Interfaces;
using Miluc.Client.Interfaces.LogInterfaceClient;
using Miluc.Client.Interfaces.Nomina;
using Miluc.Client.Interfaces.Nomina.SeguridadSocial;
using Miluc.Client.Interfaces.SapInterfaces.Cliente;
using Miluc.Client.Interfaces.SapInterfaces.LocalizacionSap;
using Miluc.Client.Interfaces.SapInterfaces.Octg;
using Miluc.Client.Interfaces.SapInterfaces.Opln;
using Miluc.Client.Interfaces.SapInterfaces.Oslp;
using Miluc.Client.Interfaces.SapInterfaces.Rutas;
using Miluc.Client.Interfaces.SapInterfaces.SapOitm;
using Miluc.Client.Interfaces.SapInterfaces.SapOrdr;
using Miluc.Client.Interfaces.UsuariosRolesPermisos;
using Miluc.Client.Servicios.Autorizacion;
using Miluc.Client.Servicios.LocalizacionSapService;
using Miluc.Client.Servicios.LogService;
using Miluc.Client.Servicios.Nomina;
using Miluc.Client.Servicios.SapService;
using Miluc.Client.Servicios.UsuariosRolesPermisos;
using Miluc.Shared.DTOs.Nomina.AfiliacionSeguridadSocialDto;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//  GESTIÓN DE RED Y COOKIES
builder.Services.AddTransient<CookieHandler>();

builder.Services.AddScoped(sp =>
{
    var handler = sp.GetRequiredService<CookieHandler>();

    return new HttpClient(handler)
    {
      BaseAddress = new Uri("https://localhost:7222/")
    // BaseAddress = new Uri("https://avicolamiluc.ddns.net:91/")

    };
});

//  AUTENTICACIÓN
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
// SERVICIOS DE APLICACIÓN
builder.Services.AddScoped<IAuthClientService, AuthFrontService>();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<CierrePorInactividad>();
builder.Services.AddScoped<RefrescarToken>();
// En el Program.cs del proyecto Client
builder.Services.AddScoped<ITipoUsuarioClienteService, TipoUsuarioClientService>();
builder.Services.AddScoped<IRolClientService, RoleClientService>();
builder.Services.AddScoped<IUsuarioClientService, UsuarioClientService>();
builder.Services.AddScoped<IPermisoClientService, PermisosClientService>();
builder.Services.AddScoped<ILogClientService, LogClientService>();
//builder.Services.AddScoped<ISapOslpService, SapOslpService>();
//builder.Services.AddScoped<ISapOcrdClientService, SapOcrdClientService>();
//builder.Services.AddScoped<ISapOitmClientService, SapOitmClientService>();

//sap 
builder.Services.AddScoped<ISapOslpService, SapOslpService>();
builder.Services.AddScoped<ISapOcrdClientService, SapOcrdClientService>();
builder.Services.AddScoped<ISapOitmClientService, SapOitmClientService>();
builder.Services.AddScoped<ISapOrdrService, SapOrdrService>();
builder.Services.AddScoped<ILocalizacionClientSap, LocalizacionSapService>();
builder.Services.AddScoped<ISapObppClientService, SapObppClientService>();
builder.Services.AddScoped<ISapOctgClientService, SapOctgClientService>();
builder.Services.AddScoped<ISapOplnClientService, SapOplnClientService>();



//Nomina
builder.Services.AddScoped<IEmpleadoClientService, EmpleadoClientService>();
builder.Services.AddScoped<IDepartamentosClientService, DepartamentoClienteService>();
builder.Services.AddScoped<ITipoDocumentoClienteService, TipoDocumentoClienteService>();
builder.Services.AddScoped<IEstadoCivilClientService, EstadoCivilClientService>();
builder.Services.AddScoped<IMatrizSociodemograficaClientService, MatrizSociodemograficaClientService>();
builder.Services.AddScoped<IInfoFamiliarClientService, InfoFamiliarClientService>();
builder.Services.AddScoped<IAfSeguridadSocialClientService, AfSeguridadSocialClientService>();
builder.Services.AddScoped<IContratoLaboralClientService, ContratoLaboralClientService>();
builder.Services.AddScoped<IEsquemaVacunacionClientService, EsquemaVacinacionClientService>();
builder.Services.AddScoped<IParentescoClient, ParentescoClientService>();
builder.Services.AddScoped<IEpsClientService, EpsClientService>();
builder.Services.AddScoped<IArlClientService, ArlClientService>();
builder.Services.AddScoped<IAfpClientService, AfpClientService>();
builder.Services.AddScoped<ICajaCompensacionClientService, CajaCompClientService>();
// Abre el Program.cs del proyecto Client y añade esta línea junto a tus otros servicios de nómina:





//  INICIALIZACIÓN DE SESIÓN (ANTES DE MOSTRAR UI)
var host = builder.Build();
var authService = host.Services.GetRequiredService<IAuthClientService>();
await authService.InitializeAsync(); // Recupera cookie + usuario

await host.RunAsync(); // SE USA EL MISMO HOST
