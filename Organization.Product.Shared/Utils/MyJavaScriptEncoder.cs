using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace Organization.Product.Shared.Utils
{
    // Thread safe singleton
    // https://csharpindepth.com/Articles/Singleton
    public static class MyJavaScriptEncoder
    {
        // 「<>&'"+」「\`」はエスケープされる
        public static readonly JavaScriptEncoder Japanese = JavaScriptEncoder.Create(
            UnicodeRanges.BasicLatin,
            UnicodeRanges.CjkRadicalsSupplement, //          2E80-2EFF
            UnicodeRanges.KangxiRadicals, //                 2F00-2FDF
            UnicodeRanges.CjkSymbolsandPunctuation, //       3000-303F
            UnicodeRanges.Hiragana, //                       3040-309F
            UnicodeRanges.Katakana, //                       30A0-30FF
            UnicodeRanges.KatakanaPhoneticExtensions, //     31F0-31FF
            UnicodeRanges.EnclosedCjkLettersandMonths, //    3200-32FF
            UnicodeRanges.CjkCompatibility, //               3300-33FF
            UnicodeRanges.CjkUnifiedIdeographsExtensionA, // 3400-4DB5
            UnicodeRanges.CjkUnifiedIdeographs, //           4E00-9FCC
            UnicodeRanges.CjkCompatibilityIdeographs, //     F900-FAD9
            UnicodeRanges.HalfwidthandFullwidthForms); //    FF00-FFEE

        // Do not mark as beforefieldinit
        static MyJavaScriptEncoder() { }
    }
}