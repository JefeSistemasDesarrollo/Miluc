using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Autorizacion;
using Miluc.Server.Interfaces.Encriptacion;
using Miluc.Server.Interfaces.LogErrores;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Server.Interfaces.Permisos;
using Miluc.Server.Interfaces.Roles;
using Miluc.Server.Interfaces.Sap.BussnesParnerGrup;
using Miluc.Server.Interfaces.Sap.CiudadMM;
using Miluc.Server.Interfaces.Sap.ConexionSap;
using Miluc.Server.Interfaces.Sap.Itm1Sap;
using Miluc.Server.Interfaces.Sap.LocalizacionSap;
using Miluc.Server.Interfaces.Sap.Obpp;
using Miluc.Server.Interfaces.Sap.Ocrd;
using Miluc.Server.Interfaces.Sap.Octg;
using Miluc.Server.Interfaces.Sap.Oitm;
using Miluc.Server.Interfaces.Sap.Opln;
using Miluc.Server.Interfaces.Sap.Ordr;
using Miluc.Server.Interfaces.Sap.Oslp;
using Miluc.Server.Interfaces.Sap.Ospp;
using Miluc.Server.Interfaces.Usuarios;
using Miluc.Server.Models.Nomina;
using Miluc.Server.Models.Sap;
using Miluc.Server.Security;
using Miluc.Server.Servicios;
using Miluc.Server.Servicios.Autorizacion;
using Miluc.Server.Servicios.Autorizacion.Rol;
using Miluc.Server.Servicios.LogService;
using Miluc.Server.Servicios.Nomina;
using Miluc.Server.Servicios.PermisosService;
using Miluc.Server.Servicios.SapService;
using Miluc.Server.Servicios.Usuarios;
using Scalar.AspNetCore;
using System.Security.Claims;
using System.Text;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
//base de datos 
builder.Services.AddDbContext<MilucDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("miluc"));
});
builder.Services.AddDbContext<SapDbContex>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sap"));
});

builder.Services.AddDbContext<NominaDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("nomina"));
});

// 2. Registro de tus servicios e interfacesbuilder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IRolService, RolService>();
builder.Services.AddScoped<ITipoUsuario, TipoUsuarioService>();
builder.Services.AddScoped<IPermisosService, PermisosService>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IOtpService, OtpService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IEstadoCivilService, EstadoCivilService>();
builder.Services.AddScoped<ILocalizacionSapCliente, SapLocalizacionClienteService>();
builder.Services.AddScoped<ISapOctgService, SapOctgService>();

//nomina 
builder.Services.AddScoped<IVacunaService, VacunaService>();
builder.Services.AddScoped<IEsquemaVacunacionService, EsquemaVacunacionService>();
builder.Services.AddScoped<IEstadoCivilService, EstadoCivilService>();
builder.Services.AddScoped<IParentescoService, ParentescoService>();
builder.Services.AddScoped<IInfoFamiliarService, InfoFamiliarService>();
builder.Services.AddScoped<ITipoDocumentoService, TipoDocumentoService>();
builder.Services.AddScoped<ITipoContratoService, TipoContratoService>();
builder.Services.AddScoped<IEmpresaService, EmpresaService>();
builder.Services.AddScoped<IContratoLaboralService, ContratoLaboralService>();
builder.Services.AddScoped<IContratoLaboralDetalleService, ContratoLaboralDetalleService>();
builder.Services.AddScoped<IEstadoCivilService, EstadoCivilService>();
builder.Services.AddScoped<IDepartamentoService, DepartamentoService>();
builder.Services.AddScoped<IMunicipioService, MunicipioService>();
builder.Services.AddScoped<IEpsService, EpsService>();
builder.Services.AddScoped<IArlService, ArlService>();
builder.Services.AddScoped<IAfpService, AfpService>();
builder.Services.AddScoped<ICajaCompensacionService, CajaCompensacionService>();
builder.Services.AddScoped<IAfiliacionSeguridadSocialService, AfiliacionSeguridadSocialService>();
builder.Services.AddScoped<IGeneroService, GeneroService>();
builder.Services.AddScoped<IPaisService, PaisService>();
builder.Services.AddScoped<IDeporteService, DeporteService>();
builder.Services.AddScoped<ICondicionMedicaService, CondicionMedicaService>();
builder.Services.AddScoped<IMedioTransporteService, MedioTransporteService>();
builder.Services.AddScoped<ITipoViviendaService, TipoViviendaService>();
builder.Services.AddScoped<IClaseViviendaService, ClaseViviendaService>();
builder.Services.AddScoped<INivelAcademicoService, NivelAcademicoService>();
builder.Services.AddScoped<IMatrizSociodemograficaService, MatrizSociodemograficaService>();
builder.Services.AddScoped<IEmpleadoService, EmpleadoService>();

////sap
builder.Services.AddScoped<IBusinessPartnerGroups, BusinessPartnerGroupsService>();
builder.Services.AddScoped<ISapOcrdService, SapOcrdService>();
builder.Services.AddScoped<ISapOslpService, SapOslpService>();
builder.Services.AddScoped<ISapOitmService, SapOitmService>();
builder.Services.AddScoped<ISapItm1Service, SapItm1Service>();
builder.Services.AddScoped<ISapOplnService, SapOplnService>();
builder.Services.AddScoped<ISapObppService, SapObppService>();
builder.Services.AddScoped<ISapCiudadMMService, SapCiudadMMService>();
builder.Services.AddScoped<ISapOsppService, OsppService>();
builder.Services.AddScoped<ISapOitmService, SapOitmService>();
builder.Services.AddScoped<IConexionServiceLayer, SapConexionService>();
builder.Services.AddScoped<ISapOrdrService, SapOrderService>();
builder.Services.AddScoped<IEncryptionService, EncryptionService>();


// 3. CONFIGURACIÓN DE COOKIES (Seguridad BFF)
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "SmartScheme";
    options.DefaultChallengeScheme = "SmartScheme";
})
.AddPolicyScheme("SmartScheme", "Cookie or JWT", options =>
{
    options.ForwardDefaultSelector = context =>
    {
        var authHeader = context.Request.Headers["Authorization"];
        if (!string.IsNullOrEmpty(authHeader) &&
            authHeader.ToString().StartsWith("Bearer "))
        {
            return JwtBearerDefaults.AuthenticationScheme;
        }
        return CookieAuthenticationDefaults.AuthenticationScheme;
    };
})
.AddCookie(options =>
{
    options.Cookie.Name = "OpcionErp_Auth";
    options.Cookie.HttpOnly = true;
    // IMPORTANTE para cross-origin (puertos distintos)
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
   // options.ExpireTimeSpan = TimeSpan.FromMinutes(builder.Configuration["Jwt:AccessTokenExpirationMinutes"]);
    options.ExpireTimeSpan = TimeSpan.FromMinutes(Convert.ToDouble(builder.Configuration["Jwt:AccessTokenExpirationMinutes"])
);
    //options.Cookie.Domain = "avicolamiluc.ddns.net";

    //Nuevo 
    options.Events = new CookieAuthenticationEvents
    {
        OnValidatePrincipal = context =>
        {
            // Opcional: Aquí puedes depurar si llegan los roles
            // Esto te permite ver en el debugger qué Claims tiene la cookie que llegó
            var identity = context.Principal?.Identity as ClaimsIdentity;
            var roles = identity?.FindAll(ClaimTypes.Role).Select(c => c.Value);
            return Task.CompletedTask;
        }
    };
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        RoleClaimType = ClaimTypes.Role,
        NameClaimType = ClaimTypes.Name,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
        )
    };
});

// 4. Configuración de CORS (Para que Blazor pueda llamar a la API)
builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorCors", policy =>
    {
     
       policy.WithOrigins("https://localhost:7198")
       .AllowAnyHeader()
       .AllowAnyMethod()
     .AllowCredentials(); // OBLIGATORIO para enviar cookies
    });
});
//builder.Services.AddControllers();


// Modifica la línea de los controladores para romper los ciclos infinitos de SAP
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Esto evita que Scalar truene al procesar relaciones circulares (ej. ORDR -> OSLP -> ORDR)
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });


// Learn more about configuring OpenAPI at 
builder.Services.AddOpenApi();


var app = builder.Build();
// 5. ORDEN DEL MIDDLEWARE (El orden es vital)
// Configure the HTTP request pipeline.
// Configuración 
// Redirigir siempre a HTTPS
app.UseHttpsRedirection();

app.UseCors("BlazorCors");

// Autenticación
app.UseAuthentication();
app.UseAuthorization();

// Middleware de errores
app.UseMiddleware<ErrorMiddleware>();


app.MapOpenApi();

app.MapScalarApiReference(options =>
{
    options.WithTitle("OpcionErp API")
           .WithTheme(ScalarTheme.Moon)
           .WithDefaultHttpClient(
                ScalarTarget.CSharp,
                ScalarClient.HttpClient);
});


app.UseHttpsRedirection();//para manejar solo el trafico por https 

//app.UseAuthentication(); // Quién eres
//app.UseAuthorization(); //  permiso
app.MapControllers();
//app.UseMiddleware<ErrorMiddleware>();

app.Run();
