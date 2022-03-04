using Discord;
using Gudgeon.Common.Styles;
using Gudgeon.Common.Customization;

/// <summary>
/// Custom version of <see cref="EmbedBuilder"/> to include styles.
/// </summary>
namespace Gudgeon.Common
{
    class GudgeonEmbedBuilder
    {
        private readonly EmbedBuilder Builder = new();

        /// <summary>
        /// Attach a title to the embed.
        /// </summary>
        /// <param name="title">The title of the embed.</param>
        /// <returns>A <see cref="GudgeonEmbedBuilder"/> with an attached title.</returns>
        public GudgeonEmbedBuilder WithTitle(string title)
        {
            if (title?.Length > 256)
            {
                throw new ArgumentException(message: $"Title length must be less than or equal to 256.", paramName: nameof(title));
            }

            Builder.WithTitle(title);
            return this;
        }

        /// <summary>
        /// Attach a description to the embed.
        /// </summary>
        /// <param name="description">The description of the embed.</param>
        /// <returns>A <see cref="GudgeonEmbedBuilder"/> with an attached description.</returns>
        public GudgeonEmbedBuilder WithDescription(string description)
        {
            if (description?.Length > 2048)
            {
                throw new ArgumentException(message: $"Description length must be less than or equal to 2048.", paramName: nameof(description));
            }

            Builder.WithDescription(description);
            return this;
        }

        /// <summary>
        /// Attach a footer to the embed.
        /// </summary>
        /// <param name="footer">The footer of the embed.</param>
        /// <returns>A <see cref="GudgeonEmbedBuilder"/> with an attached footer.</returns>
        public GudgeonEmbedBuilder WithFooter(string footer)
        {
            if (footer?.Length > 2048)
            {
                throw new ArgumentException(message: $"Footer length must be less than or equal to 2048.", paramName: nameof(footer));
            }

            Builder.WithFooter(footer);
            return this;
        }

        /// <summary>
        /// Attach a field to the embed.
        /// </summary>
        /// <param name="name">The name of the field.</param>
        /// <param name="value">The value of the field.</param>
        /// <param name="inline">Should the field be inline?</param>
        /// <returns>A <see cref="GudgeonEmbedBuilder"/> with an attached field.</returns>
        public GudgeonEmbedBuilder AddField(string name, string value, bool inline = false)
        {
            if (name?.Length > 256)
            {
                throw new ArgumentException(message: $"Name length must be less than or equal to 256.", paramName: nameof(name));
            }
            if (value?.Length > 2048)
            {
                throw new ArgumentException(message: $"Value length must be less than or equal to 2048.", paramName: nameof(value));
            }

            Builder.AddField(name, value, inline);
            return this;
        }

        /// <summary>
        /// Attach a current timestamp to the embed.
        /// </summary>
        /// <returns>A <see cref="GudgeonEmbedBuilder"/> with an attached current timestamp.</returns>
        public GudgeonEmbedBuilder WithCurrentTimestamp()
        {
            Builder.WithCurrentTimestamp();
            return this;
        }

        /// <summary>
        /// Attach a thumbnail url to the embed.
        /// </summary>
        /// <param name="url">The thumbnail url of the embed.</param>
        /// <returns>A <see cref="GudgeonEmbedBuilder"/> with an attached thumbnail url.</returns>
        public GudgeonEmbedBuilder WithThumbnailUrl(string url)
        {
            Builder.WithThumbnailUrl(url);
            return this;
        }

        /// <summary>
        /// Attach a style to the embed.
        /// </summary>
        /// <param name="style">The style of the embed.</param>
        /// <param name="name">The name of the embed.
        /// If not attached then the <see cref="Names"/> of style will be attached.</param>
        /// <returns>A <see cref="GudgeonEmbedBuilder"/> with an attached style.</returns>
        public GudgeonEmbedBuilder WithStyle(EmbedStyle style, string? name = null)
        {
            if (name?.Length > 256)
            {
                throw new ArgumentException(message: $"Name length must be less than or equal to 256.", paramName: nameof(name));
            }

            style.ApplyStyle(name);

            Builder.WithAuthor(author =>
            {
                author
                .WithName(EmbedStyle.Name)
                .WithIconUrl(EmbedStyle.IconUrl);
            })
            .WithColor(EmbedStyle.Color);

            return this;
        }

        /// <summary>
        /// Builds the <see cref="GudgeonEmbedBuilder"/>.
        /// If no style was attached, setting <see cref="Colors.Default"/>.
        /// </summary>
        /// <returns>The <see cref="Embed"/> with all attached properties.</returns>
        public Embed Build()
        {
            if (!Builder.Color.HasValue)
            {
                Builder.WithColor(Colors.Default);
            }

            return Builder.Build();
        }
    }
}