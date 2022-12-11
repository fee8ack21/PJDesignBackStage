using App.BLL;
using App.Common;
using App.DAL.Contexts;
using App.DAL.Repositories;
using App.PL.HostedServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PJ Design",
        Version = "v1",
        Description = "PJ Design APIs"
    });


    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:4200")
           .AllowAnyHeader()
           .AllowAnyMethod()
           .AllowCredentials(); ;
    });
});


builder.Services.AddDbContext<PjdesignContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("PJDesign"));
});
//builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
builder.Services.AddScoped<IAdministratorService, AdministratorService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUnitService, UnitService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IPortfolioService, PortfolioService>();

builder.Services.AddHostedService<EmailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PJ Design");
    });
}
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
    RequestPath = "/Images"
});

app.UseCors("CorsPolicy");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
