using ChatterboxApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IAzureOpenAIService, AzureOpenAIService>();
builder.Services.AddSingleton<ChatHistoryService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

var chatFilesPath = Path.Combine(Directory.GetCurrentDirectory(), "ChatFiles");
Console.WriteLine($"=== CHATFILES PATH: {chatFilesPath} ===");

if (!Directory.Exists(chatFilesPath))
{
    Directory.CreateDirectory(chatFilesPath);
    Console.WriteLine("ChatFiles-mapp skapad");
}
else
{
    Console.WriteLine("ChatFiles-mapp finns redan");
}

var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();

lifetime.ApplicationStopping.Register(() =>
{
    Console.WriteLine("========================================");
    Console.WriteLine("APPLICATION STOPPING - FÖRSÖKER SPARA!");
    Console.WriteLine("========================================");

    try
    {
        var historyService = app.Services.GetRequiredService<ChatHistoryService>();
        Console.WriteLine("ChatHistoryService hämtad från DI");

        historyService.SaveAllChatsToFile();

        Console.WriteLine("SaveAllChatsToFile() anropad");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"FEL VID SPARANDE: {ex.Message}");
        Console.WriteLine($"STACK TRACE: {ex.StackTrace}");
    }

    Console.WriteLine("========================================");
});

Console.WriteLine("========================================");
Console.WriteLine("APPLIKATION STARTAR");
Console.WriteLine($"Working Directory: {Directory.GetCurrentDirectory()}");
Console.WriteLine("STÄNG MED CTRL+C FÖR ATT SPARA!");
Console.WriteLine("========================================");

app.Run();

Console.WriteLine("app.Run() avslutad");