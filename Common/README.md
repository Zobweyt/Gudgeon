# Introduction

The Gudgeon have custom version of `EmbedBuilder` to include styles and make coding easier. It has everything that a regular one has, only styles are added.



## Namespace

The styles are located in [Gudgeon.Common.Styles](https://github.com/Zobweyt/Gudgeon/tree/master/Common/Styles).



## Example

Here is an example of how to use styles.

```cs
public class StylesModule : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("send-success", "Sends an embed with success style.")]
    public async Task SendSuccessAsync()
    {
        var embed = new GudgeonEmbedBuilder()
          // If style name is not attached, then the name of style will be attached i.e. "Success".
          .WithStyle(new SuccessStyle(), "Style name")
          .WithDescription("Description")
          .WithFooter("Footer")
          .Build();

        await RespondAsync(embed: embed);
    }
}

```

## Expected output

When you run the command you should get this.

![image](https://user-images.githubusercontent.com/98274273/156817117-d3f08b6e-cb0e-4be0-81e4-c80c3293b946.png)
