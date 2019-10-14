using JWTSample.Entities;
using JWTSample.Helpers;
using JWTSample.Services.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JWTSample
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Atacağımız isteklerde CORS problemi yaşamamak için:
            services.AddCors();
            services.AddControllers();
            //Projemizde DbContext olarak ApplicationDbContext kullanacağımız belirtliyoruz.
            services.AddDbContext<ApplicationDbContext>();
            // appsettings.json içinde oluşturduğumuz gizli anahtarımızı AppSettings ile çağıracağımızı söylüyoruz.
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // Oluşturduğumuz gizli anahtarımızı byte dizisi olarak alıyoruz.
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.SecretKey);

            //Projede farklı authentication tipleri olabileceği için varsayılan olarak JWT ile kontrol edeceğimizin bilgisini kaydediyoruz.
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                //JWT kullanacağım ve ayarları da şunlar olsun dediğimiz yer ise burasıdır.
                .AddJwtBearer(x =>
                {
                    //Gelen isteklerin sadece HTTPS yani SSL sertifikası olanları kabul etmesi(varsayılan true)
                    x.RequireHttpsMetadata = false;
                    //Eğer token onaylanmış ise sunucu tarafında kayıt edilir.
                    x.SaveToken = true;
                    //Token içinde neleri kontrol edeceğimizin ayarları.
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        //Token 3.kısım(imza) kontrolü
                        ValidateIssuerSigningKey = true,
                        //Neyle kontrol etmesi gerektigi
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        //Bu iki ayar ise "aud" ve "iss" claimlerini kontrol edelim mi diye soruyor
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            //DI için IUserService arayüzünü çağırdığım zaman UserService sınıfını getirmesini söylüyorum.
            services.AddScoped<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //CORS için hangi ayarları kullanacağımızı belirtiyoruz.
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            //Son olarak authentication kullanacağımızı belirtiyoruz.
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
