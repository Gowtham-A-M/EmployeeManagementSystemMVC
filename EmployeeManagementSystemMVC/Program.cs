var builder = WebApplication.CreateBuilder(args);


// Add services

builder.Services.AddControllersWithViews();


// Enable Session

builder.Services.AddSession();



var app = builder.Build();



if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseStaticFiles();


app.UseRouting();


// Session middleware

app.UseSession();


app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}"
);



app.Run();