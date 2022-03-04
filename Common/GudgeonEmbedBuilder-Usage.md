# Introduction

The Gudgeon have the custom version of EmbedBuilder to include styles and make to code easier. It has everything that a regular one has, only styles are added.
- To start sending style embeds, import [Gudgeon.Common.Styles](https://github.com/Zobweyt/Gudgeon/tree/master/Common/Styles)

## Examples
Here is some examples of how to use them.

### Sending a success style message
```cs
[SlashCommand("success-style", "Sends an embed with success style.")]
public async Task SendSuccessStyleAsync()
{
  var embed = new GudgeonEmbedBuilder()
    // if style name is not attached, then the name of style will be attached i.e. "Success".
    .WithStyle(new SuccessStyle())
    .WithDescription("I made a success style message!")
    // you do not need to make a lambda expression here as in the original builder.
    .WithFooter("Yay, an easy footer!")
    .Build();
    
  await RespondAsync(embed: embed);
}
```

### Expected output:
![image](https://user-images.githubusercontent.com/98274273/156772963-a8821a78-115e-4efb-a25d-6ce34eaecd12.png)



### Error style message
```cs
[SlashCommand("error-style", "Sends an embed with error style.")]
public async Task SendErrorStyleAsync()
{
  var embed = new GudgeonEmbedBuilder()
    // of course you can attach a name to the style.
    .WithStyle(new ErrorStyle(), "This is the name")
    .WithDescription("I made an error style message!")
    // you do not need to make a lambda expression here as in the original builder.
    .WithFooter("Yay, an easy footer!")
    .Build();
    
  await RespondAsync(embed: embed);
}
```

### Expected output:
![image](https://user-images.githubusercontent.com/98274273/156773035-6b54efa5-3a5c-4661-96fa-d422c2146766.png)
