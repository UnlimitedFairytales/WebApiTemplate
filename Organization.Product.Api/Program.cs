using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using NLog.Extensions.Logging;
using Organization.Product.Api._1_Middleware.ApiExplorer;
using Organization.Product.Api._1_Middleware.Auth;
using Organization.Product.Api._1_Middleware.CorsPolicy;
using Organization.Product.Api._1_Middleware.JsonSerializerOptions;
using Organization.Product.Api._1_Middleware.Swashbuckle;
using Organization.Product.Api._4_ExceptionFilters;
using Organization.Product.Api._6_ActionFilters;
using Organization.Product.ApplicationServices.Aop;

namespace Organization.Product.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Logging settings
            builder.Logging
                .ClearProviders()
                .AddNLog();

            // Add services to the container.
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<BindModelCacheFilter>();
                options.Filters.Add<ExceptionHandlingFilter>();
            }).MyAddJsonOptions(builder.Configuration);
            builder.Services.MyAddAuthentication(builder.Configuration);
            builder.Services.AddAuthorizationBuilder()
                .SetFallbackPolicy(new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build());
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            // builder.Services.AddEndpointsApiExplorer();
            // builder.Services.AddSwaggerGen();
            builder.Services.MyAddApiVersioning_AddVersionedApiExplorer(builder.Configuration);
            builder.Services.MyAddTransient_AddSwaggerGen(builder.Configuration);
            builder.Services.AddCors(options => { options.MyAddCorsPolicies(builder.Configuration); });
            builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            builder.Services.AddApplicationServices(builder.Configuration);

            var app = builder.Build();

            // Initialize LogAttribute
            var loggerFactory = app.Services.GetService<ILoggerFactory>();
            var logger = loggerFactory!.CreateLogger("Aop");
            LogAttribute.SetLogger(logger);

            // Configure the HTTP request pipeline.
            var fhOption = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
                ForwardLimit = 1,
            };
            fhOption.KnownProxies.Add(System.Net.IPAddress.Parse("192.168.3.249"));
            app.UseForwardedHeaders(fhOption);

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                // app.UseSwaggerUI();
                app.MyUseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.MyUseCorsPolicies(builder.Configuration);

            app.MyUseAuthentication(builder.Configuration);
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

________________________________________________________________________________
# 4. 従来のProject System、.NET Core以降のProject System
________________________________________________________________________________
https://github.com/dotnet/project-system/blob/main/docs/opening-with-new-project-system.md

次のいずれかの場合、Visual Studioはそのプロジェクトに対して.NET Core以降のProject Systemを使う

- .csproj内に、TargetFramework、TargetFrameworksが含まれていれる（TargetFrameworkVersionではない）
- .csproj内に、SDK要素やSDK値がある
- .slnのプロジェクトタイプが、9A19103F-16F7-4668-BE54-9A1E7A4F7556が指定されている

________________________________________________________________________________
# 5. JavascriptEncoder
________________________________________________________________________________
1. nullまたはUnsafeRelaxedJsonEscapingを指定した場合はJSONとして必須の内容しかエスケープされない
2. DefaultやJavaScriptEncoder.Create()で自作した場合、「<>&'"+」「\`」はUnicodeエスケープされる
3. 実際のところ、Unicodeエスケープは「Json直接閲覧XSS」の保険対策でしかなく、htmlエスケープとは異なる点に注意

________________________________________________________________________________
# 6. X-Forwarded-ForとForwardedHeadersOptions
________________________________________________________________________________
https://developer.mozilla.org/ja/docs/Web/HTTP/Headers/X-Forwarded-For

ALB/リバースプロキシなどのポップ段数と設定補足

- ForwardLimitとKnownProxiesは偽装値の読み取りを防ぐためパラメータ
- 1段だけの場合、X-Forwarded-Forの右端の値がクライアントIPである
    - ForwardLimitやKnownProxiesはデフォルトでよく、省略可能
- 2段以上の場合、X-Forwarded-For偽装を考慮する必要がある
    - ForwardLimitやKnownProxiesの設定が必須
    - ForwardLimit : 最大段数。右から数えてこの段数までしか考慮しない（それより左は偽装されたと見なす）
    - KnownProxies : ALB/リバースプロキシのIP群。右から順に探しこれら以外のIPをクライアントと見なす
- 上記の結果はRemoteIpAddressから読み取れる

ForwardLimitとKnownProxiesの動作確認例

```curl
curl -X 'GET' 'https://localhost:7293/v0.1/WeatherForecast' -H 'accept: text/plain' -H 'X-Forwarded-For:192.168.3.247,192.168.3.248,192.168.3.249'
```

X-Forwarded-Proto

- 対応結果はRequest.Schemeから読み取れる

________________________________________________________________________________
# 7. ASP.NET Core 認証関連
________________________________________________________________________________
https://learn.microsoft.com/ja-jp/aspnet/core/security/authentication/?view=aspnetcore-8.0

全体の流れ

1. あるアクションに到達しようとした時、認証や認可が必要か。必要な場合、どの認証(認証スキーム名)が必要か
2. 要求された認証スキームに対応する認証ハンドラを取得して、Initialize、Authenticate、Challenge、Forbidを実施
    - Authenticate : 認証済かどうかを判断する処理
    - Challenge : 「未認証」の場合に、認証フローを開始する処理。OAuthを開始したり、ログインページを表示するなど
    - Forbid : 「認証済」だが「不認可」の場合の処理。403を返したり、403ページを表示するなど
3. ASP.NET Core標準の認証スキームを利用する場合、HttpContext.UserでClaimsPrincipalを取得可能

概念

概念や用語          |概要
--------------------|----------------------------------------------------------------------------------------------
認証サービス        |IAuthenticationService。認証ハンドラの決定、nullのときの調整などを行う薄い層。通常は標準を使用
認証ハンドラ        |IAuthenticationHandler。実際の認証ロジック。Initialize、Authenticate、Challenge、Forbid
認証スキーム        |DIコンテナに登録された認証ハンドラや構成オプションの1セットこと
認可ポリシー        |アプリケーション全体または認証スキームレベルで共通に設定するための認可設定
認可属性            |ControllerやActionに個別に設定するための認可設定。Authorize、AllowAnonymous
既定の認証スキーム名|AddAuthentication()時に指定可能な値。複数の認証スキームを登録した場合は明示的な指定が実質必須
IPrincipal          |認証成功時に返却されるDto。ASP.NET Core標準の認証スキーマはすべてClaimsPrincipalにダウンキャスト可能
クレームセット      |認証成功後、HttpContext.User.Identityで取得可能
マルチテナント認証  |ASP.NET Coreの標準ライブラリでは非対応
AddScheme           |認証スキームを登録するためのメソッド。ほとんどの場合各ヘルパで登録するため、これを直接使用することはあまりない
AddCookie           |AddSchemeヘルパ（Cookie認証）。ステートレス
AddJwtBearer        |AddSchemeヘルパ（JWT Bearer認証）。ステートレス
AddNegotiate        |AddSchemeヘルパ（Windows認証）
AddOAuth            |AddSchemeヘルパ（汎用的なOAuth2）
AddFacebook         |AddSchemeヘルパ（Facebook向けOAuth2）
AddGoogle           |AddSchemeヘルパ（Google向けOAuth2）
AddTwitter          |AddSchemeヘルパ（Twitter向けのOAuth2）

-

ALBによるOAuthの場合

- ALBがOAuthを肩代わりする場合、アプリケーション側はOAuthする必要がない
    - ALBに限らず、リバースプロキシ全般に言える
- 代わりに、X-Amzn-Oidc-Dataで渡されてくるJWTに基づいて認証済みか判断が必要
- AddJwtBearerのオプションを調整すれば対応可能

________________________________________________________________________________
# 8. ASP.NET Core 認可関連
________________________________________________________________________________
DefaultPolicyとFallbackPolicy

- DefaultPolicyは、Authorize属性が指定されたが、Policyが指定されなった時のポリシー
- FallbackPolicyは、属性が何も指定されていない時のポリシー

________________________________________________________________________________
# 9. Sessionについて
________________________________________________________________________________
.NET Core のSession

- SessionKeyの破棄・再作成をサポートしていない
- Cookie側とSession側の両方にTokenを保持し比較することでステートフルにする
    - SignIn  : トークンを発行し、CookieとSessionに保存
    - 検証    : CookieとSessionのトークンを比較
    - SignOut : Sessionをクリア

________________________________________________________________________________
# 10. MiddlewareとFilter
________________________________________________________________________________
https://learn.microsoft.com/ja-jp/aspnet/core/mvc/controllers/filters?view=aspnetcore-8.0

MAREMA Action

1. Middleware（Use順の通り）
2. Authorization filter
3. Resource filter
4. Exception filter
5. Model binding
6. Action filter

*/