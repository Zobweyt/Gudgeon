using Discord;

namespace Gudgeon.Common.Styles
{
    internal abstract class EmbedStyle
    {
        public string? Name;
        public string? IconUrl;
        public Color Color;

        public abstract void ApplyStyle(string? name = null);
    }
}