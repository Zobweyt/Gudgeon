using Discord;
using Gudgeon.Common.Styles;

namespace Gudgeon.Common
{
    internal class GudgeonEmbedBuilder : EmbedBuilder
    {
        public GudgeonEmbedBuilder WithStyle(EmbedStyle style, string? name = null)
        {
            style.ApplyStyle(name);

            WithAuthor(author =>
            {
                author
                .WithName(style.Name)
                .WithIconUrl(style.IconUrl);
            })
            .WithColor(style.Color);

            return this;
        }
    }
}