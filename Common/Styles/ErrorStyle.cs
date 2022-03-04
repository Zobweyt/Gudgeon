using Gudgeon.Common.Customization;

namespace Gudgeon.Common.Styles
{
    class ErrorStyle : EmbedStyle
    {
        public override void ApplyStyle(string? name = null)
        {
            Name = name ?? Names.Error;
            IconUrl = Icons.Error;
            Color = Colors.Error;
        }
    }
}
