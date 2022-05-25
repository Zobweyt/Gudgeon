using Discord;
using Discord.Interactions;

namespace Gudgeon.TypeConverters
{
    internal sealed class UlongTypeConverter : TypeConverter<ulong>
    {
        public override ApplicationCommandOptionType GetDiscordType() => ApplicationCommandOptionType.String;
        public override Task<TypeConverterResult> ReadAsync(IInteractionContext context, IApplicationCommandInteractionDataOption option, IServiceProvider services)
        {
            if (ulong.TryParse((string)option.Value, out ulong result))
            {
                return Task.FromResult(TypeConverterResult.FromSuccess(result));
            }
    
            return Task.FromResult(TypeConverterResult.FromError(InteractionCommandError.ConvertFailed, $"Value **{option.Value}** cannot be converted to **{typeof(ulong)}**"));
        }
    }
}