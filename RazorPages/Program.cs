using AspNetCoreAzureSearch;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<SearchProviderIndex>();
builder.Services.AddScoped<SearchProviderPaging>();
builder.Services.AddScoped<SearchProviderAutoComplete>();
builder.Services.AddHttpClient();
builder.Services.AddRazorPages();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.Run();
