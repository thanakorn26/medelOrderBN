using OrderManagement_BackEnd.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()  
               .AllowAnyMethod() 
               .AllowAnyHeader();
    });
});

builder.Services.AddSingleton<DapperContext>();

AppContext.SetSwitch("Switch.Microsoft.Data.SqlClient.UseManagedNetworkingOnWindows", true);

//builder.WebHost.UseUrls("http://localhost:5050");
builder.WebHost.UseUrls("http://localhost:5050", "https://localhost:5051");


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();  

app.UseAuthorization();

app.MapControllers();

SqlConnectionTester.TestConnection();

app.Run();
