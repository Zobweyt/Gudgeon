using Discord;
using Gudgeon.Common.Customization;
using Gudgeon.Modules.Moderation.Message.Modals;
using System.Globalization;

namespace Gudgeon.Modules.Moderation.Message.Addons
{
    internal class EmbedConstructor
    {
        public static Embed Build(EmbedModal modal)
        {
            EmbedBuilder builder = AttachImage(modal.ImageUrl)
               .WithTitle(modal.Header)
               .WithDescription(modal.Body);

            AttachColor(builder, modal.Color);

            return builder.Build();
        }
        private static EmbedBuilder AttachImage(string? url)
        {
            try
            {
                return new EmbedBuilder()
                    .WithImageUrl(url)
                    .Build()
                    .ToEmbedBuilder();
            }
            catch (Exception)
            {
                return new EmbedBuilder();
            }
        }
        private static void AttachColor(EmbedBuilder builder, string hex)
        {
            if (int.TryParse(hex.Replace("#", ""), NumberStyles.HexNumber, null, out int color))
            {
                System.Drawing.Color rgb = System.Drawing.Color.FromArgb(color);
                builder.WithColor(new Color(rgb.R, rgb.G, rgb.B));
            }
            else
            {
                builder.WithColor(Colors.Discord);
            }
        }
    }
}