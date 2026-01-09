
namespace QuePOS.Shared.Helpers
{
    public static class StringExtensions
    {
        public static string Truncate(this string text, int length)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            return text.Length <= length ? text : string.Concat(text.AsSpan(0, length), "...");
        }
    }

}
