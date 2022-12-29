using ITHSCourseSchoolWEB;
using ITHSCourseSchoolWEB.Models.Repository;
using ITHSCourseSchoolWEB.Models.Repository.IRepository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddHttpClient<ICourseRepository, CourseRepository>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddAuthentication
    (options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        //options.DefaultChallengeScheme = "oidc";
    })
              .AddCookie(options =>
              {
                  options.Cookie.HttpOnly = true;
              options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                  options.LoginPath = "/Auth/LoginUser";
                  options.AccessDeniedPath = "/Auth/AccessDenied";
                  options.SlidingExpiration = true;
              });
//.AddOpenIdConnect("oidc", options => {
//options.Authority = builder.Configuration["ServiceUrls:IdentityAPI"];
//options.GetClaimsFromUserInfoEndpoint = true;
//options.ClientId = "magic";
//options.ClientSecret = "secret";
//options.ResponseType = "code";

//options.TokenValidationParameters.NameClaimType = "name";
//options.TokenValidationParameters.RoleClaimType = "role";
//options.Scope.Add("magic");
//options.SaveTokens = true;

//options.ClaimActions.MapJsonKey("role", "role");
//options.LoginPath = "/Auth/LoginUser";
//});




builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
