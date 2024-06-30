using Housing.Data;
using Housing.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Runtime.Loader;

var builder = WebApplication.CreateBuilder(args);

// Log to ensure path is correct
Assembly entry = Assembly.GetEntryAssembly();
//找到当前执行文件所在路径
string dir = Path.GetDirectoryName(entry.Location);
string dllPath = Path.Combine(dir, "DLL");
string entryName = entry.GetName().Name;
//获取执行文件同一目录下的其他dll
foreach (string dll in Directory.GetFiles(dllPath, "*.dll"))
{
    if (entryName.Equals(Path.GetFileNameWithoutExtension(dll))) { continue; }
    //非程序集类型的关联load时会报错
    try
    {
        AssemblyLoadContext.Default.LoadFromAssemblyPath(dll);
    }
    catch (Exception ex)
    {
    }
}

// Add services to the container.
builder.Services.AddControllers();
// Add HttpClient
builder.Services.AddHttpClient();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:8080")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

// Configure Entity Framework and MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(8, 0, 21))));

// Register services
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IWebScraperService, WebScraperService>();
builder.Services.AddScoped<IAgentService, AgentService>();
builder.Services.AddScoped<ICommunityService, CommunityService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure CORS
app.UseCors("AllowVueApp");

// Uncomment to enforce HTTPS redirection if needed
// app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
