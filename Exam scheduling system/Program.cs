var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "StudentLogin",
    pattern: "student/login",
    defaults: new { controller = "Home", action = "StudentLogin" });

app.MapControllerRoute(
    name: "FacultyLogin",
    pattern: "faculty/login",
    defaults: new { controller = "Home", action = "FacultyLogin" });

app.MapControllerRoute(
    name: "AdminLogin",
    pattern: "admin/login",
    defaults: new { controller = "Home", action = "AdminLogin" });

app.MapControllerRoute(
    name: "RequestPasswordReset",
    pattern: "forgot-password",
    defaults: new { controller = "Home", action = "RequestPasswordReset" });

app.MapControllerRoute(
    name: "VerifyIdentity",
    pattern: "verify-identity",
    defaults: new { controller = "Home", action = "VerifyIdentity" });

app.MapControllerRoute(
    name: "ResetPassword",
    pattern: "reset-password",
    defaults: new { controller = "Home", action = "ResetPassword" });

app.MapControllerRoute(
    name: "SignUp",
    pattern: "signup",
    defaults: new { controller = "Home", action = "SignUp" });

app.Run();
