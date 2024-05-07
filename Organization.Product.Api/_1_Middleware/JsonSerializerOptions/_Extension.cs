using Organization.Product.Api.Utils;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Organization.Product.Api._1_Middleware.JsonSerializerOptions
{
#pragma warning disable IDE1006
    public static class _Extension
#pragma warning restore IDE1006
    {
        // https://learn.microsoft.com/ja-jp/aspnet/core/web-api/advanced/formatting?view=aspnetcore-6.0
        // ※ Encoderについて補足
        // 1. デフォルト値はnull
        // 2. null                                        : UnsafeRelaxedJsonEscapingと同じ
        // 3. JavaScriptEncoder.Default                   : BasicLatinの内、「<>&'"+」「\`」以外を許可。「¥」のみ「\\」としてエスケープされる
        // 4. JavaScriptEncoder.UnsafeRelaxedJsonEscaping : 「"¥」の2文字以外は許可。それぞれ「\"」「¥\」としてエスケープされる
        // 5. MyJavaScriptEncoder.Japanese                : Defaultに加え、日本語の文字を許可
        public static void MyAddJsonOptions(this IMvcBuilder mvcBuilder, IConfiguration configuration)
        {
            var cnf = configuration.GetSection("JsonSerializerOptions");
            var encoderName = cnf["Encoder"];
            var encoder =
                string.IsNullOrEmpty(encoderName) ? null :
                encoderName == "Default" ? JavaScriptEncoder.Default :
                encoderName == "UnsafeRelaxedJsonEscaping" ? JavaScriptEncoder.UnsafeRelaxedJsonEscaping :
                encoderName == "Japanese" ? MyJavaScriptEncoder.Japanese :
                null;
            mvcBuilder.AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.AllowTrailingCommas = cnf.GetValue<bool>("AllowTrailingCommas");
                // options.JsonSerializerOptions.Converters
                options.JsonSerializerOptions.DefaultBufferSize = cnf.GetValue<int>("DefaultBufferSize");
                options.JsonSerializerOptions.DefaultIgnoreCondition = Enum.Parse<JsonIgnoreCondition>(cnf["DefaultIgnoreCondition"]);
                options.JsonSerializerOptions.DictionaryKeyPolicy = cnf["DictionaryKeyPolicy"] == "CamelCase" ? JsonNamingPolicy.CamelCase : null;
                options.JsonSerializerOptions.Encoder = encoder;
                options.JsonSerializerOptions.IgnoreReadOnlyFields = cnf.GetValue<bool>("IgnoreReadOnlyFields");
                options.JsonSerializerOptions.IgnoreReadOnlyProperties = cnf.GetValue<bool>("IgnoreReadOnlyProperties");
                options.JsonSerializerOptions.IncludeFields = cnf.GetValue<bool>("IncludeFields");
                options.JsonSerializerOptions.MaxDepth = cnf.GetValue<int>("MaxDepth");
                options.JsonSerializerOptions.NumberHandling = (JsonNumberHandling)cnf.GetValue<int>("NumberHandling");
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = cnf.GetValue<bool>("PropertyNameCaseInsensitive");
                options.JsonSerializerOptions.PropertyNamingPolicy = cnf["PropertyNamingPolicy"] == "CamelCase" ? JsonNamingPolicy.CamelCase : null;
                options.JsonSerializerOptions.ReadCommentHandling = Enum.Parse<JsonCommentHandling>(cnf["ReadCommentHandling"]);
                // options.JsonSerializerOptions.ReferenceHandler
                options.JsonSerializerOptions.UnknownTypeHandling = Enum.Parse<JsonUnknownTypeHandling>(cnf["UnknownTypeHandling"]);
                options.JsonSerializerOptions.WriteIndented = cnf.GetValue<bool>("WriteIndented");
            });
        }
    }
}