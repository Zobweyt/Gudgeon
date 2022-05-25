using Discord;

namespace Gudgeon.Modules.Moderation.Message.Modals
{
    internal class ProcessedEmbedModal
    {
        public static Modal Build(IMessage message)
        {
            IEmbed embed = message.Embeds.ToList()[0];

            TextInputBuilder header = new TextInputBuilder()
                .WithLabel("header")
                .WithMinLength(1).WithMaxLength(256)
                .WithValue(embed.Title).WithPlaceholder("Pizza")
                .WithCustomId("embed_header");

            TextInputBuilder body = new TextInputBuilder()
                .WithLabel("body")
                .WithMinLength(1)
                .WithStyle(TextInputStyle.Paragraph)
                .WithValue(embed.Description).WithPlaceholder("I love **pizza**, but...")
                .WithCustomId("embed_body");

            TextInputBuilder image = new TextInputBuilder()
                .WithLabel("image").WithRequired(false)
                .WithMinLength(1)
                .WithValue(embed.Image == null ? "Invalid" : embed.Image.Value.Url).WithPlaceholder("https://image.png")
                .WithCustomId("embed_image");

            TextInputBuilder color = new TextInputBuilder()
                .WithLabel("color").WithRequired(false)
                .WithMinLength(7).WithMaxLength(7)
                .WithValue(embed.Color.ToString()).WithPlaceholder("#5865F2")
                .WithCustomId("embed_color");

            return new ModalBuilder()
                    .WithTitle("Embed")
                    .WithCustomId($"embed_edit_modal_submit:{message.Id}")
                    .AddTextInput(header).AddTextInput(body).AddTextInput(image).AddTextInput(color)
                    .Build();
        }
    }
}