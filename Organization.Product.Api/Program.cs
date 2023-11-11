namespace Organization.Product.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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
        }
    }
}

/*
________________________________________________________________________________
# 1. .NET5 から .NET6へのテンプレートの変更点
________________________________________________________________________________
ASP.NET Core 6.0 の新しい最小ホスティングモデルに移行されたコードサンプル  
https://docs.microsoft.com/ja-jp/aspnet/core/migration/50-to-60-samples?view=aspnetcore-6.0

.NET5 Builder factory

|.net 5                             |.net6
|-----------------------------------|-------------------------------------------
|WebHost.CreateDefaultBuilder(args) |WebApplication.CreateBuilder(args)

-

.NET5 ConfigureServices()

|.net 5                             |.net6
|-----------------------------------|-------------------------------------------
|services.AddControllers();         |builder.Services.AddControllers();
|-                                  |builder.Services.AddEndpointsApiExplorer();
|services.AddSwaggerGen(...);       |builder.Services.AddSwaggerGen(...);

-

.NET5 Configure()

|.net 5                             |.net6
|-----------------------------------|------------------------------------------
|app.UseDeveloperExceptionPage();   |-
|app.UseSwagger();                  |app.UseSwagger();
|app.UseSwaggerUI(...);             |app.UseSwaggerUI(...);
|app.UseHttpsRedirection();         |app.UseHttpsRedirection();
|app.UseRouting();                  |-
|app.UseAuthorization();            |app.UseAuthorization();
|app.UseEndpoints(x=>{x.MapControllers();}|app.MapControllers();

-

*/