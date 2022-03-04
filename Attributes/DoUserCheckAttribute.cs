using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace Gudgeon.Attributes
{
    class DoUserCheck : PreconditionAttribute
    {   
        public override Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context, ICommandInfo commandInfo, IServiceProvider services)
        {
            if (context.Interaction is not SocketMessageComponent componentContext)
            {
                return Task.FromResult(PreconditionResult.FromError("Context unrecognized as component context."));
            }
            else
            {
                var param = componentContext.Data.CustomId.Split(':');
                if (param.Length > 1 && ulong.TryParse(param[1].Split(',')[0], out ulong id))
                {
                    return (context.User.Id == id)
                        ? Task.FromResult(PreconditionResult.FromSuccess())
                        : Task.FromResult(PreconditionResult.FromError("User ID does not match component ID!"));
                }
                else
                {
                    return Task.FromResult(PreconditionResult.FromError("Parse cannot be done if no userID exists."));
                }
            }
        }
    }
}