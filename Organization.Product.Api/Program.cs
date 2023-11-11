using Organization.Product.Api.Middleware.ApiExplorer;
using Organization.Product.Api.Middleware.CorsPolicy;
using Organization.Product.Api.Middleware.Swashbuckle;

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
            // builder.Services.AddEndpointsApiExplorer();
            // builder.Services.AddSwaggerGen();
            builder.Services.MyAddApiVersioning_AddVersionedApiExplorer(builder.Configuration);
            builder.Services.MyAddTransient_AddSwaggerGen();
            builder.Services.AddCors(options => { options.MyAddCorsPolicies(builder.Configuration); });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                // app.UseSwaggerUI();
                app.MyUseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.MyUseCorsPolicies(builder.Configuration);

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

________________________________________________________________________________
# 2. Asp.Versioning.Mvc.ApiExplorer と Swagger
________________________________________________________________________________
Asp.Versioning.Mvc.ApiExplorer（旧名：Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer）

1. URLによるバージョン指定の際、route templateに{version:apiVersion}が使えるようになる。
2. versionの指定方法はURL、クエリ文字列、ヘッダ、メディアタイプのいずれかから選べる（複数も可）。
3. Route属性複数指定(マルチ属性ルーティング)、デフォルトバージョンを指定する、省略許可などを組み合わせれば、省略可能URLベースバージョン指定APIが可能。

Swagger側のVersioning対応

1. AddApiVersioningからチェーンさせてAddApiExplorerでAddすると、IApiVersionDescriptionProviderもDIされて取得できるようになる。
2. IApiVersionDescriptionProviderを取得して、全APIから集計されて用意されたバージョン群の数だけ、SwaggerDocしたりSwaggerEndpointする。

注意点

- ControllerにRoute属性複数指定(マルチ属性ルーティング)している場合、メソッド側ではHttpGet/HttpPost/Route属性などでName(ルート名)は利用できない。
    - ルート名は一意である必要があるが、Route属性複数指定によって同名のNameを複数回登録しようとしてしまうため。
    - templateプロパティに[action]がいること自体は問題ない。飽くまでルート名だけが問題。
    - LinkGeneratorはルート名以外の方法で利用すること

________________________________________________________________________________
# 3. Angular開発環境 と Cors
________________________________________________________________________________
開発環境時のオリジン対応

- AngularなどSPAと連携する構成の場合、開発時にはSPA WebサーバとWeb API Webサーバを別々に立てて別オリジン間通信を行うことになる。
- localhost:4200はAngular開発環境の初期設定。Reactは3000、Vueは8080 
- IsDevelopmentやjsonの切り替えはBuild構成ではなく、環境変数ASPNETCORE_ENVIRONMENT
-- launchSettings.jsonで環境変数違いを用意しておくと楽

*/