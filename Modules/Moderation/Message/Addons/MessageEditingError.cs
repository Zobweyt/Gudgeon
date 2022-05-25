namespace Gudgeon.Modules.Moderation.Message.Addons
{
    internal enum MessageEditingError
    {
        /// <summary>
        /// Thrown when <see cref="Discord.IMessage?"/> does not exist.
        /// </summary>
        Invalid,

        /// <summary>
        /// Thrown when <see cref="Discord.IMessage"/> author is not bot.
        /// </summary>
        InvalidAuthor,

        /// <summary>
        /// Thrown when <see cref="Discord.IMessage"/> does not contain only 1 embed.
        /// </summary>
        InvalidForm,

        /// <summary>
        /// Thrown when <see cref="Discord.IMessage"/> received as a result of command execution.
        /// </summary>
        InteractionResult
    }
}