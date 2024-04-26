namespace Organization.Product.Domain.ValueObjects
{
    public class AppMessage
    {
        public string ID { get; private set; }
        public string[]? TextParam { get; private set; }
        public string TextTemplate { get; private set; }

        private AppMessage(string id, string[]? textParam, string textTemplate)
        {
            this.ID = id;
            this.TextParam = textParam;
            this.TextTemplate = textTemplate;
        }

        public string FormattedText
        {
            get
            {
                string text = "";
                try
                {
                    text = TextParam == null ?
                        this.TextTemplate :
                        string.Format(this.TextTemplate, this.TextParam);
                }
                catch
                {
                }
                return text;
            }
        }

        public static AppMessage E9999(string[]? textParam = null) => new(nameof(E9999), textParam, "システムエラーが発生しました。管理者にお問い合わせください。");

        public static AppMessage E9997(string[]? textParam = null) => new(nameof(E9997), textParam, "DBエラーが発生しました。管理者にお問い合わせください。");
        public static AppMessage E9996(string[]? textParam = null) => new(nameof(E9996), textParam, "帳票出力に失敗しました。");
        public static AppMessage E9995(string[]? textParam = null) => new(nameof(E9995), textParam, "FAX送信に失敗しました。");
        public static AppMessage E9994(string[]? textParam = null) => new(nameof(E9994), textParam, "ファイルの取得に失敗しました。");

        public static AppMessage W6002(string[]? textParam = null) => new(nameof(W6002), textParam, "現在サービス停止中です。");
        public static AppMessage W6001(string[]? textParam = null) => new(nameof(W6001), textParam, "Web画面は利用禁止としています。管理者に問い合わせてください。");

        public static AppMessage W5001(string[]? textParam = null) => new(nameof(W5001), textParam, "ログインに失敗しました。");
        public static AppMessage W5002(string[]? textParam = null) => new(nameof(W5002), textParam, "該当するユーザコードが存在しません。");
        public static AppMessage W5003(string[]? textParam = null) => new(nameof(W5003), textParam, "パスワードが正しくありません。");
        public static AppMessage W5004(string[]? textParam = null) => new(nameof(W5004), textParam, "ログインが許可されていません。");
        public static AppMessage W5005(string[]? textParam = null) => new(nameof(W5005), textParam, "権限がありません。");
        public static AppMessage W5006(string[]? textParam = null) => new(nameof(W5006), textParam, "これはステートレス認証です。ログアウトは無意味です。");

        public static AppMessage W4001(string[]? textParam = null) => new(nameof(W4001), textParam, "セッションタイムアウトしました。お手数ですが、再度ログインしてください。");
        public static AppMessage W4002(string[]? textParam = null) => new(nameof(W4002), textParam, "一定時間（{0}分以上）操作をしなかったため、画面を再読み込みしてください。");

        public static AppMessage W3001(string[]? textParam = null) => new(nameof(W3001), textParam, "抽出最大件数({0})を超えています。抽出条件を変更してください。");
        public static AppMessage W3002(string[]? textParam = null) => new(nameof(W3002), textParam, "該当データが存在しません。");
        public static AppMessage W3003(string[]? textParam = null) => new(nameof(W3003), textParam, "{0}を入力してください。");
        public static AppMessage W3004(string[]? textParam = null) => new(nameof(W3004), textParam, "{0}を選択してください。");
        public static AppMessage W3005(string[]? textParam = null) => new(nameof(W3005), textParam, "{0}は{1}つ以下で選択してください。");
        public static AppMessage W3006(string[]? textParam = null) => new(nameof(W3006), textParam, "{0}は{1}件以下で選択してください。");

        public static AppMessage W2000(string[]? textParam = null) => new(nameof(W2000), textParam, "入力チェックエラーが発生しました。内容をご確認ください。");

        public static AppMessage W2023(string[]? textParam = null) => new(nameof(W2023), textParam, "{0}となる行が複数存在します。");
        public static AppMessage W2024(string[]? textParam = null) => new(nameof(W2024), textParam, "入力した{0}は既に存在しています。別の{1}を入力してください。");
        public static AppMessage W2025(string[]? textParam = null) => new(nameof(W2025), textParam, "有効期間の前後エラーです。{0}");

        public static AppMessage W2099(string[]? textParam = null) => new(nameof(W2099), textParam, "該当データは他のユーザに更新されました。画面を再読み込みしてください。");
    }
}
