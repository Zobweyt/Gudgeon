using Discord;
using Discord.Interactions;

namespace Gudgeon.Modules.Admin.CustomEmbed
{
    public class EmbedConfigurationModal : IModal
    {
        public string Title => "Embed configuration";

        [InputLabel("Title")]
        [ModalTextInput("embed-title", maxLength: 256)]
        public string EmbedTitle { get; set; }

        [InputLabel("Description")]
        [ModalTextInput("embed-description", TextInputStyle.Paragraph, maxLength: 2048)]
        public string EmbedDescription { get; set; }
    }
}
