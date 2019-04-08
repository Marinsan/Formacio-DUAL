using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Net.Http;
using System.Web.Http.Routing;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System.Configuration;
using System.IdentityModel.Tokens;
using Thinktecture.IdentityModel.Tokens;
using System.Web.Http;
using System.Net.Http.Formatting;
using Newtonsoft.Json.Serialization;
using Microsoft.Owin.Security.Jwt;
using Owin;
using System.Net.Mail;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Net;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using Microsoft.Owin.Security.Infrastructure;


using System.Threading;

using Newtonsoft.Json.Linq;



[assembly: OwinStartup(typeof(Formacio.App_Start.Startup))]
namespace Formacio.App_Start
{
    

    #region Configuracio Aplicacio. S'inicialitza al carregar l'ensamblat OWIN
    public class Startup
    {

        public static TimeSpan TempsApi { get { return TimeSpan.FromDays(15); } }

        public static TimeSpan TempsWeb { get { return TimeSpan.FromDays(15); } }

      
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            HttpConfiguration config = new HttpConfiguration();

            ConfigureOAuthTokenGeneration(app);

            ConfigureOAuthTokenConsumption(app);

            //ConfigureExternalsLoginProviders(app);

            ConfigureWebApi(config);
            
        }

        //AMJ Comento
        //private static bool IsAjaxRequest(IOwinRequest request)
        //{
        //    if (request.Path.ToString().StartsWith("/Service"))
        //    {
        //        return true;
        //    }
        //    IReadableStringCollection query = request.Query;
        //    if ((query != null) && (query["X-Requested-With"] == "XMLHttpRequest"))
        //    {
        //        return true;
        //    }
        //    IHeaderDictionary headers = request.Headers;
        //    return ((headers != null) && (headers["X-Requested-With"] == "XMLHttpRequest"));
        //}

        private void ConfigureOAuthTokenGeneration(IAppBuilder app)
        {
            //Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(DisiAuthDbContext.Create);
            app.CreatePerOwinContext<DisiUserManager>(DisiUserManager.Create);
            app.CreatePerOwinContext<DisiRoleManager>(DisiRoleManager.Create);
            app.CreatePerOwinContext<DisiSignInManager>(DisiSignInManager.Create);

            //AMJ - NonActionAttribute utilizem cookies
            //Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions CookieOptions = new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions
            //{
            //    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            //    LoginPath = new PathString("/Account/Login"),
            //    ExpireTimeSpan = Startup.TempsWeb,
            //    SlidingExpiration = true,
            //    //CookieSecure = Microsoft.Owin.Security.Cookies.CookieSecureOption.Always,
            //    Provider = new Microsoft.Owin.Security.Cookies.CookieAuthenticationProvider
            //    {
            //        // Enables the application to validate the security stamp when the user logs in.
            //        // This is a security feature which is used when you change a password or add an external login to your account.  
            //        //OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<DisiUserManager, DisiUser, int>(
            //        //    validateInterval: TimeSpan.FromMinutes(30),
            //        //regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager, DefaultAuthenticationTypes.ApplicationCookie))

            //    }
            //};

            //app.UseCookieAuthentication(CookieOptions);


            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                //For Dev enviroment only (on production should be AllowInsecureHttp = false)
                AllowInsecureHttp = true,
                //AllowInsecureHttp = false,
                TokenEndpointPath = new PathString("/oauth/token"),
                AccessTokenExpireTimeSpan = Startup.TempsApi,
                Provider = new DisiOAuthProvider(),
                AccessTokenFormat = new DisiJwtFormat(ConfigurationManager.AppSettings["URLAuthentication"]),
                RefreshTokenProvider = new DisiRefreshTokenProvider()

            };

            app.UseOAuthAuthorizationServer(OAuthServerOptions);


        }
        
        private void ConfigureWebApi(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            jsonFormatter.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;

            //config.SuppressDefaultHostAuthentication();

        }

        private void ConfigureOAuthTokenConsumption(IAppBuilder app)
        {

            var issuer = ConfigurationManager.AppSettings["URLAuthentication"];


            string audienceId = ConfigurationManager.AppSettings["as:AudienceId"];
            byte[] audienceSecret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["as:AudienceSecret"]);

            // Api controllers with an [Authorize] attribute will be validated with JWT
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { audienceId },
                    IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                    {
                        new SymmetricKeyIssuerSecurityTokenProvider(issuer, audienceSecret)
                    }
                });

        }
    }

    #endregion

    #region Entitats

    public class DisiUserRole : IdentityUserRole<int> { }
    public class DisiUserClaim : IdentityUserClaim<int> { }
    public class DisiUserLogin : IdentityUserLogin<int> { }

    public class DisiRole : IdentityRole<int, DisiUserRole>
    {
        public DisiRole() { }
        public DisiRole(string name) { Name = name; }
    }

    public class DisiUserStore : UserStore<DisiUser, DisiRole, int, DisiUserLogin, DisiUserRole, DisiUserClaim>
    {
        public DisiUserStore(DisiAuthDbContext context)
            : base(context)
        {
        }
    }

    public class DisiRoleStore : RoleStore<DisiRole, int, DisiUserRole>
    {
        public DisiRoleStore(DisiAuthDbContext context) : base(context)
        {
        }
    }

    public class RefreshToken
    {
        [Key]
        public string Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Subject { get; set; }
        public DateTime IssuedUtc { get; set; }
        public DateTime ExpiresUtc { get; set; }
        [Required]
        public string ProtectedTicket { get; set; }
    }

    public class DisiUser : IdentityUser<int, DisiUserLogin, DisiUserRole, DisiUserClaim>
    {
        
        [MaxLength(200)]
        public string Nombre { get; set; }

        [MaxLength(200)]
        public string Apellido1 { get; set; }

        [MaxLength(200)]
        public string Apellido2 { get; set; }
        
        [MaxLength(50)]
        public string Documento { get; set; }

        public int? idSede { get; set; }

        public DateTime? FechaBaja { get; set; }

        public DateTime FechaCreacion { get; set; }

        
//Rest of code is removed for brevity
internal async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<DisiUser, int> manager, string authenticationType)
        {
            
            ClaimsIdentity userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            //userIdentity.

            // Add custom user claims here
            return userIdentity;
            
        }

        internal ClaimsIdentity GenerateUserIdentity(UserManager<DisiUser, int> manager, string authenticationType)
        {

            ClaimsIdentity userIdentity = manager.CreateIdentity(this, authenticationType);

            //userIdentity.

            // Add custom user claims here
            return userIdentity;

        }

    }
    #endregion

    #region Models
    public class UserReturnModel
    {
        public string Url { get; set; }
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public DateTime FechaAltaDate { get; set; }
        public IList<string> Roles { get; set; }
        public IList<System.Security.Claims.Claim> Claims { get; set; }
    }

    public class RoleReturnModel
    {
        public string Url { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }


    public class CreateRoleBindingModel
    {
        [Required]
        [StringLength(256, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "Role Name")]
        public string Name { get; set; }

    }

    public class UsersInRoleModel
    {

        public int Id { get; set; }
        public List<int> EnrolledUsers { get; set; }
        public List<int> RemovedUsers { get; set; }
    }


    public class CreateUserModel
    {
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Apellido1")]
        public string Apellido1 { get; set; }

        [Display(Name = "Apellido2")]
        public string Apellido2 { get; set; }

        [Display(Name = "Documento")]
        public string Documento { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Telefono")]
        public string Telefono { get; set; }

        [Display(Name = "IdSede")]
        public int? idSede { get; set; }
      
        [Required]
        [Display(Name = "Usuario")]
        public string Usuario { get; set; }
        
        [Required]
        [StringLength(100, ErrorMessage = "La contraseña debe tener como mínimo {2} carácteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y la confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }

        
        [Display(Name = "Rol")]
        public string Rol { get; set; }

        [Display(Name = "FechaBaja")]
        public DateTime? FechaBaja { get; set; }

        [Display(Name = "FechaCreacion")]
        public DateTime FechaCreacion { get; set; }
       
    }

    public class CheckPasswordModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }


    public class ConfirmarResetPasswordModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva contraseña")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar la nueva contraseña")]
        [Compare("NewPassword", ErrorMessage = "La nueva contraseña y la contraseña de confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }


        public string Mensaje { get; set; }

        public bool Correcto { get; set; }

        public string url { get; set; }
        public string token { get; set; }


    }

    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "La contraseña debe tener como mínimo {2} carácteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "La nueva contraseña y la confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }

    }


    public class ClaimBindingModel
    {
        [Required]
        [Display(Name = "Claim Type")]
        public string Type { get; set; }

        [Required]
        [Display(Name = "Claim Value")]
        public string Value { get; set; }
    }


    //public class ExternalLoginViewModel
    //{
    //    public string Name { get; set; }

    //    public string Url { get; set; }

    //    public string State { get; set; }
    //}

    //public class RegisterExternalBindingModel
    //{
    //    [Required]
    //    public string UserName { get; set; }

    //    [Required]
    //    public string Provider { get; set; }

    //    [Required]
    //    public string ExternalAccessToken { get; set; }

    //    [Required]
    //    [EmailAddress]
    //    [Display(Name = "Email")]
    //    public string Email { get; set; }
        
    //    [Required]
    //    [Display(Name = "Nombre")]
    //    public string Nombre { get; set; }

    //    [Required]
    //    [Display(Name = "Apellido1")]
    //    public string Apellido1 { get; set; }

    //    [Display(Name = "Apellido2")]
    //    public string Apellido2 { get; set; }

    //    [Display(Name = "FechaNacimiento")]
    //    public DateTime FechaNacimiento { get; set; }

    //    [Display(Name = "IdMunicipio")]
    //    public int IdMunicipio { get; set; }

    //    [Display(Name = "TipoDocumento")]
    //    public string TipoDocumento { get; set; }

    //    [Display(Name = "Documento")]
    //    public string Documento { get; set; }

    //    [Display(Name = "CodigoPostal")]
    //    public string CodigoPostal { get; set; }

    //    [Display(Name = "Direccion")]
    //    public string Direccion { get; set; }

    //    [Display(Name = "Sexo")]
    //    public string Sexo { get; set; }

    //    [Display(Name = "Telefono")]
    //    public string Telefono { get; set; }

    //    [Display(Name = "Imagen")]
    //    public byte[] Imagen { get; set; }

    //    [Display(Name = "Rol")]
    //    public string Rol { get; set; }

    //    [Display(Name = "AcceptaLOPD")]
    //    public bool AcceptaLOPD { get; set; }

    //    [Display(Name = "AcceptaMailing")]
    //    public bool AcceptaMailing { get; set; }

    //    [Display(Name = "FechaBaja")]
    //    public DateTime? FechaBaja { get; set; }

    //    [Display(Name = "FechaCreacion")]
    //    public DateTime FechaCreacion { get; set; }
    //}

    //public class ParsedExternalAccessToken
    //{
    //    public string user_id { get; set; }
    //    public string app_id { get; set; }
    //}

    #endregion

    #region BBDD EntityFramework
    public class DisiAuthDbContext : IdentityDbContext<DisiUser, DisiRole, int, DisiUserLogin, DisiUserRole, DisiUserClaim>
    {
        public DisiAuthDbContext() : base("UsersConnection")
        {

        }

        public static DisiAuthDbContext Create()
        {
            return new DisiAuthDbContext();
        }

        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            return base.ValidateEntity(entityEntry, items);
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("dbo");

            modelBuilder.Entity<DisiUserLogin>().Map(c =>
            {
                c.ToTable("Formacion_Usuarios");
                c.Properties(p => new
                {
                    p.UserId,
                    p.LoginProvider,
                    p.ProviderKey,
                });
            }).HasKey(p => new { p.LoginProvider, p.ProviderKey, p.UserId });


            // Mapping for ApiRole
            modelBuilder.Entity<DisiRole>().Map(c =>
            {
                c.ToTable("Formacion_Roles");
                c.Property(p => p.Id).HasColumnName("RoleId");
                c.Properties(p => new
                {
                    p.Name
                });

            }).HasKey(p => p.Id);
            modelBuilder.Entity<DisiRole>().HasMany(c => c.Users).WithRequired().HasForeignKey(c => c.RoleId);

            modelBuilder.Entity<DisiUser>().Map(c =>
            {
                c.ToTable("Usuarios");
                c.Property(p => p.Id).HasColumnName("UserId");
                c.Properties(p => new
                {
                    p.AccessFailedCount,
                    p.Email,
                    p.EmailConfirmed,
                    p.PasswordHash,
                    p.PhoneNumber,
                    p.PhoneNumberConfirmed,
                    p.TwoFactorEnabled,
                    p.SecurityStamp,
                    p.LockoutEnabled,
                    p.LockoutEndDateUtc,
                    p.UserName,
                    p.Nombre,
                    p.Apellido1,
                    p.Apellido2,
                    p.Documento,
                    p.FechaCreacion,
                    p.FechaBaja,
                    p.idSede
                });

              
            }).HasKey(c => c.Id);
            modelBuilder.Entity<DisiUser>().HasMany(c => c.Logins).WithOptional().HasForeignKey(c => c.UserId);
            modelBuilder.Entity<DisiUser>().HasMany(c => c.Claims).WithOptional().HasForeignKey(c => c.UserId);
            modelBuilder.Entity<DisiUser>().HasMany(c => c.Roles).WithRequired().HasForeignKey(c => c.UserId);
            //modelBuilder.Entity<DisiUser>().HasMany(c => c.OrganizationsUser).WithRequired().HasForeignKey(c => c.UserId);


            modelBuilder.Entity<DisiUserRole>().Map(c =>
            {
                c.ToTable("Formacion_UsersInRoles");
                c.Properties(p => new
                {
                    p.UserId,
                    p.RoleId
                });
            })
            .HasKey(c => new { c.UserId, c.RoleId });

            modelBuilder.Entity<DisiUserClaim>().Map(c =>
            {
                c.ToTable("Formacion_UserClaim");
                c.Property(p => p.Id).HasColumnName("UserClaimId");
                c.Properties(p => new
                {
                    p.UserId,
                    p.ClaimValue,
                    p.ClaimType
                });
            }).HasKey(c => c.Id);
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public async Task<bool> AddRefreshToken(RefreshToken token)
        {
            var existingToken = this.RefreshTokens.Where(r => r.Subject == token.Subject).SingleOrDefault();

            if (existingToken != null)
            {
                var result = await RemoveRefreshToken(existingToken);
            }

            this.RefreshTokens.Add(token);

            return await this.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            var refreshToken = await this.RefreshTokens.FindAsync(refreshTokenId);

            if (refreshToken != null)
            {
                this.RefreshTokens.Remove(refreshToken);
                return await this.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            this.RefreshTokens.Remove(refreshToken);
            return await this.SaveChangesAsync() > 0;
        }

        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            var refreshToken = await this.RefreshTokens.FindAsync(refreshTokenId);

            return refreshToken;
        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
            return this.RefreshTokens.ToList();
        }

    }
    #endregion

    #region Classes configuració Seguretat: OWIN , UserIdentity, RoleIdentity, ClaimsIdentity, JWT
    public class ModelFactory
    {
        private UrlHelper _UrlHelper;
        private DisiUserManager _AppUserManager;

        public ModelFactory(HttpRequestMessage request, DisiUserManager appUserManager)
        {
            _UrlHelper = new UrlHelper(request);
            _AppUserManager = appUserManager;
        }

        public UserReturnModel Create(DisiUser appUser)
        {
            return new UserReturnModel
            {
                Url = _UrlHelper.Link("GetUserById", new { id = appUser.Id }),
                Id = appUser.Id,
                UserName = appUser.UserName,
                FullName = string.Format("{0} {1}", appUser.Nombre, appUser.Apellido1),
                //Direccion = appUser.Direccion,
                Email = appUser.Email,
                EmailConfirmed = appUser.EmailConfirmed,
                Roles = _AppUserManager.GetRolesAsync(appUser.Id).Result,
                Claims = _AppUserManager.GetClaimsAsync(appUser.Id).Result
            };
        }

        public RoleReturnModel Create(DisiRole appRole)
        {

            return new RoleReturnModel
            {
                Url = _UrlHelper.Link("GetRoleById", new { id = appRole.Id }),
                Id = appRole.Id,
                Name = appRole.Name
            };
        }
    }



    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            try
            {

                MailMessage myMessage = new MailMessage();
                myMessage.To.Add(new MailAddress(message.Destination));
                myMessage.Subject = message.Subject;
                myMessage.Body = message.Body;
                myMessage.IsBodyHtml = true;

                SmtpClient client = new SmtpClient();
                client.Send(myMessage);

            }
            catch 
            {
            }
        }

    }
    public class DisiUserManager : UserManager<DisiUser, int>
    {

        public DisiUserManager(IUserStore<DisiUser, int> store)
            : base(store)
        {
        }

        public static DisiUserManager Create(IdentityFactoryOptions<DisiUserManager> options, IOwinContext context)
        {


            var appDbContext = context.Get<DisiAuthDbContext>();
            var appUserManager = new DisiUserManager(new DisiUserStore(appDbContext));

            //Rest of code is removed for clarity
            appUserManager.EmailService = new EmailService();

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                appUserManager.UserTokenProvider = new DataProtectorTokenProvider<DisiUser, int>(dataProtectionProvider.Create("ASP.NET Identity"))
                {
                    //Code for email confirmation and reset password life time
                    TokenLifespan = TimeSpan.FromHours(6)
                };
            }

            //appUserManager.UserValidator = new UserValidator<DisiUser>(appUserManager)
            appUserManager.UserValidator = new DisiUserValidator(appUserManager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };


            appUserManager.PasswordValidator = new DisiPasswordValidator
            {
                RequiredLength = 1,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false


            };

            return appUserManager;
        }



        public override async Task<bool> CheckPasswordAsync(DisiUser user, string password)
        {

            return await base.CheckPasswordAsync(user, password);
        }

        public override async Task<DisiUser> FindByNameAsync(string email)
        {
            return await base.FindByNameAsync(email);
        }
       
       


    }


    public class DisiUserValidator : UserValidator<DisiUser, int>
    {

        List<string> _allowedEmailDomains;

        public DisiUserValidator(DisiUserManager appUserManager)
            : base(appUserManager)
        {
            char[] cars = new char[1];
            cars[0] = ',';

            _allowedEmailDomains = System.Configuration.ConfigurationManager.AppSettings["UsersAllowedEmailDomains"].Split(cars).ToList();

        }

        public override async Task<IdentityResult> ValidateAsync(DisiUser user)
        {
            IdentityResult result = await base.ValidateAsync(user);

            if (!String.IsNullOrEmpty(user.Email))
            {


                if (user.Email.Split('@').Length > 1)
                {

                    var emailDomain = user.Email.Split('@')[1];

                    if (!_allowedEmailDomains.Contains(emailDomain.ToLower()) && !_allowedEmailDomains.Contains("*"))
                    {
                        var errors = result.Errors.ToList();

                        errors.Add(String.Format("Email domain '{0}' is not allowed", emailDomain));

                        result = new IdentityResult(errors);
                    }
                }
            }

            return result;
        }
    }

    public class DisiPasswordValidator : PasswordValidator
    {
        public override async Task<IdentityResult> ValidateAsync(string password)
        {
            IdentityResult result = await base.ValidateAsync(password);

            //if (password.Contains("abcdef") || password.Contains("123456"))
            //{
            //    var errors = result.Errors.ToList();
            //    errors.Add("Password can not contain sequence of chars");
            //    result = new IdentityResult(errors);
            //}
            return result;
        }
    }


    public class DisiRoleManager : RoleManager<DisiRole, int>
    {
        public DisiRoleManager(IRoleStore<DisiRole, int> roleStore)
            : base(roleStore)
        {
        }

        public static DisiRoleManager Create(IdentityFactoryOptions<DisiRoleManager> options, IOwinContext context)
        {
            var appRoleManager = new DisiRoleManager(new DisiRoleStore(context.Get<DisiAuthDbContext>()));

            //var appRoleManager = new DisiRoleManager(new RoleStore<DisiRole, int>(context.Get<DisiAuthDbContext>());

            return appRoleManager;
        }


    }

    public class DisiOAuthProvider : OAuthAuthorizationServerProvider
    {

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {

            string clientSecret = ConfigurationManager.AppSettings["as:AudienceSecret"];

            if (context.ClientId == null)
            {
                //Remove the comments from the below line context.SetError, and invalidate context 
                //if you want to force sending clientId/secrects once obtain access tokens. 
                context.Validated();
                //context.SetError("invalid_clientId", "ClientId should be sent.");
                return Task.FromResult<object>(null);
            }


            if (string.IsNullOrWhiteSpace(clientSecret))
            {
                context.SetError("invalid_clientId", "Client secret should be sent.");
                return Task.FromResult<object>(null);
            }


            context.OwinContext.Set<string>("as:clientAllowedOrigin", "*");

            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            var allowedOrigin = "*";

            if (String.IsNullOrEmpty(context.OwinContext.Response.Headers["Access-Control-Allow-Origin"]))
            {
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });
            }

            DisiUserManager userManager = context.OwinContext.GetUserManager<DisiUserManager>();

            DisiUser user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "El nombre o la contraseña son incorrectasa.");
                return;
            }

            if (ConfigurationManager.AppSettings["ValidacioUsuarisEmail"] == "true")
            {
                if (!user.EmailConfirmed)
                {
                    context.SetError("invalid_grant", "User did not confirm email.");
                    return;
                }
            }


            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, "JWT");

            oAuthIdentity.AddClaims(ExtendedClaimsProvider.GetClaims(user, userManager));

            var ticket = new AuthenticationTicket(oAuthIdentity, null);

            context.Validated(ticket);

        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {

            // Change auth ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
            //newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }
    }

   
    public class DisiRefreshTokenProvider : IAuthenticationTokenProvider
    {
        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {


            var refreshTokenId = Guid.NewGuid().ToString("n");

            using (DisiAuthDbContext _repo = new DisiAuthDbContext())
            {


                var token = new RefreshToken()
                {
                    Id = refreshTokenId,
                    Subject = context.Ticket.Identity.Name,
                    IssuedUtc = DateTime.UtcNow,
                    ExpiresUtc = DateTime.UtcNow.Add(Startup.TempsApi)
                };

                context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
                context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

                token.ProtectedTicket = context.SerializeTicket();

                var result = await _repo.AddRefreshToken(token);

                if (result)
                {
                    context.SetToken(refreshTokenId);
                }

            }
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {

            var allowedOrigin = "*";

            if (String.IsNullOrEmpty(context.OwinContext.Response.Headers["Access-Control-Allow-Origin"]))
            {
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });
            }

            //string hashedTokenId = Helper.GetHash(context.Token);

            using (DisiAuthDbContext _repo = new DisiAuthDbContext())
            {
                var refreshToken = await _repo.FindRefreshToken(context.Token);

                if (refreshToken != null)
                {
                    //Get protectedTicket from refreshToken class
                    context.DeserializeTicket(refreshToken.ProtectedTicket);
                    var result = await _repo.RemoveRefreshToken(context.Token);
                }
            }
        }

    }

    // Configure the application sign-in manager which is used in this application.
    public class DisiSignInManager : SignInManager<DisiUser, int>
    {
        public DisiSignInManager(DisiUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(DisiUser user)
        {
            return user.GenerateUserIdentityAsync(this.UserManager, DefaultAuthenticationTypes.ApplicationCookie);
        }

        public static DisiSignInManager Create(IdentityFactoryOptions<DisiSignInManager> options, IOwinContext context)
        {
            return new DisiSignInManager(context.GetUserManager<DisiUserManager>(), context.Authentication);
        }
    }

    public class DisiJwtFormat : ISecureDataFormat<AuthenticationTicket>
    {

        private readonly string _issuer = string.Empty;

        public DisiJwtFormat(string issuer)
        {
            _issuer = issuer;
        }

        public string Protect(AuthenticationTicket data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            string audienceId = ConfigurationManager.AppSettings["as:AudienceId"];

            string symmetricKeyAsBase64 = ConfigurationManager.AppSettings["as:AudienceSecret"];

            var keyByteArray = TextEncodings.Base64Url.Decode(symmetricKeyAsBase64);

            var signingKey = new HmacSigningCredentials(keyByteArray);

            var issued = data.Properties.IssuedUtc;

            var expires = data.Properties.ExpiresUtc;

            var token = new JwtSecurityToken(_issuer, audienceId, data.Identity.Claims, issued.Value.UtcDateTime, expires.Value.UtcDateTime, signingKey);

            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.WriteToken(token);

            return jwt;
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// Métode per assingar permisos "Claim" de forma dinàmica segons les caracteristiques de l'usuari.
    /// (Pot servir per afegir permisos segons regles de negoci)
    /// </summary>
    public static class ExtendedClaimsProvider
    {
        public static IEnumerable<Claim> GetClaims(DisiUser user, DisiUserManager userManager)
        {

            List<Claim> claims = new List<Claim>();

            //claims.Add(CreateClaim("sub", user.Nombre + " " + user.Apellidos));

            //if (userManager.IsInRole(user.Id, "ADMIN"))
            //{
            //    //assigno todos los claims possibles.   
            //}

            try
            {
                string nombre = "";
                string apellidos = "";
                string telefono = "";
                string email = "";
                string dni = "";
                //string direccion = "";
              
                if (!String.IsNullOrEmpty(user.Nombre))
                {
                    nombre = user.Nombre;
                }

                if (!String.IsNullOrEmpty(user.Apellido1))
                {
                    apellidos = user.Apellido1;
                }

                if (!String.IsNullOrEmpty(user.PhoneNumber))
                {
                    telefono = user.PhoneNumber;
                }

                if (!String.IsNullOrEmpty(user.Email))
                {
                    email = user.Email;
                }

                if (!String.IsNullOrEmpty(user.Documento))
                {
                    dni = user.Documento;
                }

                //if (!String.IsNullOrEmpty(user.Direccion))
                //{
                //    direccion = user.Direccion;
                //}

               

                claims.Add(CreateClaim("Nombre", nombre));
                claims.Add(CreateClaim("Apellidos", apellidos));
                claims.Add(CreateClaim("Telefono", telefono));
                claims.Add(CreateClaim("Email", email));
                claims.Add(CreateClaim("DNI", dni));

                //claims.Add(CreateClaim("UserId", user.Id.ToString()));


                //claims.Add(CreateClaim("Direccion", direccion));
                
                //claims.Add(CreateClaim("MunicipioId", ""+user.IdMunicipio));

            }
            catch (Exception ex)
            {
            }

          
            return claims;
        }

        public static Claim CreateClaim(string type, string value)
        {
            return new Claim(type, value, ClaimValueTypes.String);
        }

    }
    

    public class ClaimsAuthorizationAttribute : AuthorizationFilterAttribute
    {
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        public override Task OnAuthorizationAsync(HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken)
        {

            var principal = actionContext.RequestContext.Principal as ClaimsPrincipal;

            if (!principal.Identity.IsAuthenticated)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                return Task.FromResult<object>(null);
            }

            if (!(principal.HasClaim(x => x.Type == ClaimType && x.Value == ClaimValue)))
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                return Task.FromResult<object>(null);
            }

            //User is Authorized, complete execution
            return Task.FromResult<object>(null);

        }
    }

    #endregion


    /// <summary>
    /// Controladors a utilitzar per gestionar Usuaris, Rols i Permisos.
    /// </summary>
    #region Controllers / Base Controller

    public class BaseApiController : ApiController
    {

        private ModelFactory _modelFactory;
        private DisiUserManager _AppUserManager = null;
        private DisiRoleManager _AppRoleManager = null;
        private DisiSignInManager _signInManager = null;

        private DisiOAuthProvider _OAuthProvider = null;

        public BaseApiController()
        {
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_AppUserManager != null)
                {
                    _AppUserManager.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        protected DisiOAuthProvider OAuthProvider
        {
            get
            {
                return _OAuthProvider ?? Request.GetOwinContext().Get<DisiOAuthProvider>();
            }
        }


        protected DisiUserManager UserManager
        {
            get
            {
                return _AppUserManager ?? Request.GetOwinContext().GetUserManager<DisiUserManager>();
            }
        }

        protected DisiRoleManager RoleManager
        {
            get
            {
                return _AppRoleManager ?? Request.GetOwinContext().GetUserManager<DisiRoleManager>();
            }
        }

        public DisiSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? Request.GetOwinContext().Get<DisiSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }


        protected ModelFactory TheModelFactory
        {
            get
            {
                if (_modelFactory == null)
                {
                    _modelFactory = new ModelFactory(this.Request, this.UserManager);
                }
                return _modelFactory;
            }
        }

        protected IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }

    public static class UtilsView
    {
        public static List<String> GetUserRoles(string userID)
        {

            DisiUserManager usr_manager = HttpContext.Current.Request.GetOwinContext().GetUserManager<DisiUserManager>();

            return usr_manager.GetRoles<DisiUser, int>(int.Parse(userID)).ToList();



            //bool isAdmin = UserManager.IsInRole<DisiUser, int>(int.Parse(User.Identity.GetUserId()), "Administrador");
        }

    }


    public class BaseController : System.Web.Mvc.Controller
    {
        private DisiSignInManager _signInManager;
        private DisiUserManager _userManager;
        private DisiRoleManager _roleManager;



        public DisiSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? Request.GetOwinContext().Get<DisiSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public DisiUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<DisiUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public DisiRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? Request.GetOwinContext().GetUserManager<DisiRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }



    }

    [Authorize]
    [RoutePrefix("api/Accounts")]
    public class AccountsController : BaseApiController
    {

        [AllowAnonymous]
        [Route("ConfigInicialApp")]
        [HttpGet]
        public async Task<IHttpActionResult> InicialitzaUsuariDISI(string password)
        {
            if (password == "P@sswordInstalacio")
            {

                var user = new DisiUser()
                {
                    UserName = "disi@disi.es",
                    Email = "disi@disi.es",
                    Nombre = "DISI",
                    Apellido1 = "Usuario Administrador",
                    FechaCreacion = Service.DateTimeUtils.ToServer(DateTime.Now).Value,
                    Documento ="111111111H",
                    PhoneNumber= "649999999",
                    idSede=1
                };

                IdentityResult addUserResult = await this.UserManager.CreateAsync(user, "P@ssword");

                if (!addUserResult.Succeeded)
                {
                    return GetErrorResult(addUserResult);
                }
                else
                {
                    string code = await this.UserManager.GenerateEmailConfirmationTokenAsync(user.Id);

                    IdentityResult result = await this.UserManager.ConfirmEmailAsync(user.Id, code);

                    if (result.Succeeded)
                    {

                        //IdentityResult result_role = await this.RoleManager.CreateAsync(new DisiRole("ADMIN"));

                        //if (result_role.Succeeded)
                        //{
                        //    IdentityResult result_role_assign = await this.UserManager.AddToRoleAsync(user.Id, "ADMIN");
                        //    if (result_role_assign.Succeeded)
                        //    {
                        return Ok();
                        //    }
                        //    else
                        //    {
                        //        return GetErrorResult(result_role_assign);
                        //    }

                        //}
                        //else
                        //{
                        //    return GetErrorResult(result_role);
                        //}
                    }
                    else
                    {
                        return GetErrorResult(result);
                    }
                }
            }
            else
            {
                return BadRequest();
            }
        }

              
        [Route("check/login")]
        [HttpPost]
        public async Task<IHttpActionResult> CheckLogin(CheckPasswordModel model)
        {

            var user = await this.UserManager.FindByNameAsync(model.UserName);
            
            if (user != null)
            {
                return Ok(await this.UserManager.CheckPasswordAsync(user, model.Password));
            }

            return Ok(false);
        }

        //[AllowAnonymous]
        //[Route("createProve")]
        //public async Task<IHttpActionResult> CreateUsersProveedors(CreateUserModel createUserModel)
        //{

        //    var user = new DisiUser()
        //    {

        //        Nombre = "Proveevor XMAS",
        //        UserName = "ProveedorXMAS",
        //        FechaCreacion = Service.DateTimeUtils.ToServer(DateTime.Now).Value,
        //        FechaBaja = null,
        //        Email = "Disi@xmarket.es"

        //    };

        //    IdentityResult addUserResult = null;

        //    addUserResult = await this.UserManager.CreateAsync(user, "P@ssword");


        //    if (!addUserResult.Succeeded)
        //    {
        //        try
        //        {
        //            return BadRequest(addUserResult.Errors.First());
        //        }
        //        catch (Exception e)
        //        {
        //            return GetErrorResult(addUserResult);
        //        }


        //    }

        //    Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));


        //    return Created(locationHeader, TheModelFactory.Create(user));
        //}

        //[AllowAnonymous]
        [Route("create")]
        public async Task<IHttpActionResult> CreateUser(CreateUserModel createUserModel)
        {
            if (!ModelState.IsValid)
            {

                try
                {
                    return BadRequest(ModelState.First().Value.Errors.First().ErrorMessage);
                }
                catch (Exception e)
                {
                    return BadRequest(ModelState);
                }
              
            }

            var user = new DisiUser()
            {

                Nombre = createUserModel.Nombre,
                Apellido1 = createUserModel.Apellido1,
                Apellido2 = createUserModel.Apellido2,
                Documento = createUserModel.Documento,
                UserName = createUserModel.Usuario,
                Email = createUserModel.Email,
                PhoneNumber = createUserModel.Telefono,
                FechaCreacion = Service.DateTimeUtils.ToServer(DateTime.Now).Value,
                FechaBaja = null,
                idSede = createUserModel.idSede
            };

            IdentityResult addUserResult = null;

            addUserResult = await this.UserManager.CreateAsync(user, createUserModel.Password);


            if (!addUserResult.Succeeded)
            {
                try { 
                    return BadRequest(addUserResult.Errors.First());
                }
                catch (Exception e) {
                    return GetErrorResult(addUserResult);
                }
                
                
            }

            if (ConfigurationManager.AppSettings["ValidacioUsuarisEmail"] == "true")
            {
                string code = await this.UserManager.GenerateEmailConfirmationTokenAsync(user.Id);

                var callbackUrl = new Uri(Url.Link("ConfirmEmailRoute", new { userId = user.Id, code = code }));

                await this.UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
            }

            Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));
                

            return Created(locationHeader, TheModelFactory.Create(user));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
        public async Task<IHttpActionResult> ConfirmEmail(int userId, string code = "")
        {
            //if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            //{
            //    ModelState.AddModelError("", "User Id and Code are required");
            //    return BadRequest(ModelState);
            //}

            IdentityResult result = await this.UserManager.ConfirmEmailAsync(userId, code);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return GetErrorResult(result);
            }
        }

        //UpdateUser -> En ConsultaController 
       

        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await this.UserManager.ChangePasswordAsync(int.Parse(User.Identity.GetUserId()), model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }


        [Route("user/{id:int}")]
        public async Task<IHttpActionResult> DeleteUser(int id)
        {

            //Only SuperAdmin or ADMIN can delete users (Later when implement roles)

            var appUser = await this.UserManager.FindByIdAsync(id);

            if (appUser != null)
            {
                IdentityResult result = await this.UserManager.DeleteAsync(appUser);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }

                return Ok();

            }

            return NotFound();

        }

        [Route("users")]
        public IHttpActionResult GetUsers()
        {
            return Ok(this.UserManager.Users.ToList().Select(u => this.TheModelFactory.Create(u)));
        }

        [Route("userdetails/{id:int}")]
        public async Task<IHttpActionResult> GetUserDetails(int Id)
        {
            var user = await this.UserManager.FindByIdAsync(Id);

            if (user != null)
            {
                user.FechaCreacion = Service.DateTimeUtils.ToClient(user.FechaCreacion).Value;

                return Ok(user);
            }

            return NotFound();

        }

        [Route("user/{id:int}", Name = "GetUserById")]
        public async Task<IHttpActionResult> GetUser(int Id)
        {
            var user = await this.UserManager.FindByIdAsync(Id);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();

        }

        [Route("user/{username}")]
        public async Task<IHttpActionResult> GetUserByName(string username)
        {
            var user = await this.UserManager.FindByNameAsync(username);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();
        }

       

        [Authorize(Roles = "ADMIN")]
        [Route("user/{id:int}/roles")]
        [HttpPut]
        public async Task<IHttpActionResult> AssignRolesToUser([FromUri] int id, [FromBody] string[] rolesToAssign)
        {

            var appUser = await this.UserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            var currentRoles = await this.UserManager.GetRolesAsync(appUser.Id);

            var rolesNotExists = rolesToAssign.Except(this.RoleManager.Roles.Select(x => x.Name)).ToArray();

            if (rolesNotExists.Count() > 0)
            {

                ModelState.AddModelError("", string.Format("Roles '{0}' does not exixts in the system", string.Join(",", rolesNotExists)));
                return BadRequest(ModelState);
            }

            IdentityResult removeResult = await this.UserManager.RemoveFromRolesAsync(appUser.Id, currentRoles.ToArray());

            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to remove user roles");
                return BadRequest(ModelState);
            }

            IdentityResult addResult = await this.UserManager.AddToRolesAsync(appUser.Id, rolesToAssign);

            if (!addResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to add user roles");
                return BadRequest(ModelState);
            }

            return Ok();
        }

        [Authorize(Roles = "ADMIN")]
        [Route("user/{id:int}/assignclaims")]
        [HttpPut]
        public async Task<IHttpActionResult> AssignClaimsToUser([FromUri] int id, [FromBody] List<ClaimBindingModel> claimsToAssign)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appUser = await this.UserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            foreach (ClaimBindingModel claimModel in claimsToAssign)
            {
                if (appUser.Claims.Any(c => c.ClaimType == claimModel.Type))
                {

                    await this.UserManager.RemoveClaimAsync(id, ExtendedClaimsProvider.CreateClaim(claimModel.Type, claimModel.Value));
                }

                await this.UserManager.AddClaimAsync(id, ExtendedClaimsProvider.CreateClaim(claimModel.Type, claimModel.Value));
            }

            return Ok();
        }

        [Authorize(Roles = "ADMIN")]
        [Route("user/{id:int}/removeclaims")]
        [HttpPut]
        public async Task<IHttpActionResult> RemoveClaimsFromUser([FromUri] int id, [FromBody] List<ClaimBindingModel> claimsToRemove)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appUser = await this.UserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            foreach (ClaimBindingModel claimModel in claimsToRemove)
            {
                if (appUser.Claims.Any(c => c.ClaimType == claimModel.Type))
                {
                    await this.UserManager.RemoveClaimAsync(id, ExtendedClaimsProvider.CreateClaim(claimModel.Type, claimModel.Value));
                }
            }

            return Ok();
        }
        
    }

    [Authorize(Roles = "ADMIN")]
    [RoutePrefix("api/roles")]
    public class RolesController : BaseApiController
    {

        [Route("{id:int}", Name = "GetRoleById")]
        public async Task<IHttpActionResult> GetRole(int Id)
        {
            var role = await this.RoleManager.FindByIdAsync(Id);

            if (role != null)
            {
                return Ok(TheModelFactory.Create(role));
            }

            return NotFound();

        }

        [Route("", Name = "GetAllRoles")]
        public IHttpActionResult GetAllRoles()
        {
            var roles = this.RoleManager.Roles;

            return Ok(roles);
        }

        [Route("create")]
        public async Task<IHttpActionResult> Create(CreateRoleBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var role = new DisiRole { Name = model.Name };

            var result = await this.RoleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            Uri locationHeader = new Uri(Url.Link("GetRoleById", new { id = role.Id }));

            return Created(locationHeader, TheModelFactory.Create(role));

        }

        [Route("{id:int}")]
        public async Task<IHttpActionResult> DeleteRole(int Id)
        {

            var role = await this.RoleManager.FindByIdAsync(Id);

            if (role != null)
            {
                IdentityResult result = await this.RoleManager.DeleteAsync(role);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }

                return Ok();
            }

            return NotFound();

        }

        [Route("ManageUsersInRole")]
        public async Task<IHttpActionResult> ManageUsersInRole(UsersInRoleModel model)
        {
            var role = await this.RoleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ModelState.AddModelError("", "Role does not exist");
                return BadRequest(ModelState);
            }

            foreach (int user in model.EnrolledUsers)
            {
                var appUser = await this.UserManager.FindByIdAsync(user);

                if (appUser == null)
                {
                    ModelState.AddModelError("", String.Format("User: {0} does not exists", user));
                    continue;
                }

                if (!this.UserManager.IsInRole(user, role.Name))
                {
                    IdentityResult result = await this.UserManager.AddToRoleAsync(user, role.Name);

                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", String.Format("User: {0} could not be added to role", user));
                    }

                }
            }

            foreach (int user in model.RemovedUsers)
            {
                var appUser = await this.UserManager.FindByIdAsync(user);

                if (appUser == null)
                {
                    ModelState.AddModelError("", String.Format("User: {0} does not exists", user));
                    continue;
                }

                IdentityResult result = await this.UserManager.RemoveFromRoleAsync(user, role.Name);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", String.Format("User: {0} could not be removed from role", user));
                }
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok();
        }
    }

    [Authorize]
    [RoutePrefix("api/claims")]
    public class ClaimsController : BaseApiController
    {

        [Route("")]
        public IHttpActionResult GetClaims()
        {
            var identity = User.Identity as ClaimsIdentity;

            var claims = from c in identity.Claims
                         select new
                         {
                             subject = c.Subject.Name,
                             type = c.Type,
                             value = c.Value
                         };

            return Ok(claims);
        }

    }

    #endregion


}