using BlazorAzureSearch.Server.PersonSearch;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<SearchProviderIndex>();
builder.Services.AddScoped<SearchProviderPaging>();
builder.Services.AddScoped<SearchProviderAutoComplete>();
builder.Services.AddHttpClient();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
