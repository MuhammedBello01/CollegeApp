using CollegeApp.MyLogging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//Logging.  by default all loggers are active
// builder.Logging.ClearProviders(); //clearing all loggers
// builder.Logging.AddConsole(); //logging to console alone
// builder.Logging.AddDebug(); 


Log.Logger = new LoggerConfiguration()
   // .WriteTo.File("Log/log.txt")//this is if u want to log everything into one file
    .WriteTo.File("Log/log.txt", rollingInterval: RollingInterval.Day)// if u want to log into new file everyday
    .MinimumLevel.Information()
    .CreateLogger();

//builder.Host.UseSerilog();// if u want to use only serilog
builder.Logging.AddSerilog();// if u want to use both inbuilt loggers and serilog

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson().AddXmlDataContractSerializerFormatters( );
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IMyLogger, LogToFile>();  //Dependency injection
// builder.Services.AddSingleton<IMyLogger, LogToFile>();
// builder.Services.AddTransient<IMyLogger, LogToFile>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
