using Discord;

namespace Gudgeon.Common.Styles
{
    abstract class EmbedStyle
    {
        public static string? Name;
        public static string? IconUrl;
        public static Color Color;

        public abstract void ApplyStyle(string? name = null);
    }
}
