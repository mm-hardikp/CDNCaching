var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = r =>
    {
        string path = r.File.PhysicalPath;
        if (path.EndsWith(".css") || path.EndsWith(".js") || path.EndsWith(".png") || path.EndsWith(".html"))
        {
            {
                r.Context.Response.Headers.Append("Cache-Control", "public, max-age=86400"); // Cache for 1 day
            }
            //TimeSpan maxAge = TimeSpan.FromMinutes(10);
            //r.Context.Response.Headers.Append("Cache-Control", $"public, max-age={maxAge.TotalSeconds.ToString("0")}");
        }
    }
});
app.UseRouting();

app.UseAuthorization();
app.MapStaticAssets();

app.Use(async (context, next) =>
{
    var path = context.Request.Path.ToString();
    if (path.StartsWith("/css") || path.StartsWith("/js") || path.StartsWith("/html"))
    {
        context.Response.Redirect($"https://cdncaching.pages.dev{path}");
        return;
    }
    await next();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
