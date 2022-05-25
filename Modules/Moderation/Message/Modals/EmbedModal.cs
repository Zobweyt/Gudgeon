using Discord;
using Discord.Interactions;

namespace Gudgeon.Modules.Moderation.Message.Modals
{
    public class EmbedModal : IModal
    {
        public string Title => "Embed";

        [ModalTextInput("embed_header", placeholder: "Pizza", maxLength: 256)]
        [InputLabel("header")]
        public string Header { get; set; }

        [ModalTextInput("embed_body", TextInputStyle.Paragraph, "I love **pizza**, but...")]
        [InputLabel("body")]
        public string Body { get; set; }

        [ModalTextInput("embed_image", placeholder: "https://image.png")]
        [RequiredInput(false)]
        [InputLabel("image")]
        public string ImageUrl { get; set; }

        [ModalTextInput("embed_color", placeholder: "#5865F2", minLength: 7, maxLength: 7)]
        [RequiredInput(false)]
        [InputLabel("color")]
        public string Color { get; set; }
    }
}