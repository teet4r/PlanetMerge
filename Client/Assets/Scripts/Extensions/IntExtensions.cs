public static class IntExtensions
{
    public static string Comma(this int number) => string.Format("{0:#,###0}", number);
}