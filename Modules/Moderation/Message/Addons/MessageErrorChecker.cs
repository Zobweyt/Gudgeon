using Discord;

namespace Gudgeon.Modules.Moderation.Message.Addons
{
    internal static class MessageErrorChecker
    {
        public static MessageEditingError? Check(IMessage message, ulong botId)
        {
            return message == null ? MessageEditingError.Invalid :
                   message.Author.Id != botId ? MessageEditingError.InvalidAuthor :
                   message.Embeds.Count != 1 ? MessageEditingError.InvalidForm :
                   message.Interaction != null ? MessageEditingError.InteractionResult : null;
        }
    }
}