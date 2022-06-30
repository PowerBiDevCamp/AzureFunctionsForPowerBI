var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSpaStaticFiles(configuration => {
  configuration.RootPath = "wwwRoot";
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles( );
app.UseSpa( spa => {});

// app.UseRouting();

app.Run();
