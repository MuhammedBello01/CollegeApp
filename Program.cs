using CollegeApp.MyLogging;

var builder = WebApplication.CreateBuilder(args);

//Logging.  by default all loggers are active
builder.Logging.ClearProviders(); //clearing all loggers
builder.Logging.AddConsole(); //logging to console alone
builder.Logging.AddDebug(); 

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
