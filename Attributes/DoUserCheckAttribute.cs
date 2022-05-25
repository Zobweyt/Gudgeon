using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace Gudgeon.Attributes
{
    internal class DoUserCheck : PreconditionAttribute
    {   
        public override Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context, ICommandInfo commandInfo, IServiceProvider services)
        {
            if (context.Interaction is not SocketMessageComponent componentContext)
            {
                return Task.FromResult(PreconditionResult.FromError("Context unrecognized as component context."));
            }

            var param = componentContext.Data.CustomId.Split(':');

            if (param.Length > 1 && ulong.TryParse(param[1].Split(',')[0], out ulong id))
            {
                var user = context.Client.GetUserAsync(id).Result;

                return (context.User.Id == id)
                    ? Task.FromResult(PreconditionResult.FromSuccess())
                    : Task.FromResult(PreconditionResult.FromError($"Only {user.Mention} can control this menu."));
            }

            return Task.FromResult(PreconditionResult.FromError("Parse cannot be done if no userID exists."));
        }
    }
}