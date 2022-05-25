using Discord;
using Discord.Interactions;

namespace Gudgeon.TypeReaders
{
    internal sealed class UlongTypeReader : TypeReader<ulong>
    {
        public override Task<TypeConverterResult> ReadAsync(IInteractionContext context, string option, IServiceProvider services)
        {
            if (ulong.TryParse(option, out ulong result))
            {
                return Task.FromResult(TypeConverterResult.FromSuccess(result));
            }

            return Task.FromResult(TypeConverterResult.FromError(InteractionCommandError.ConvertFailed, $"Value **{option}** cannot be converted to **{typeof(ulong)}**"));
        }
    }
}