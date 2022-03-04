using Gudgeon.Common.Customization;

namespace Gudgeon.Common.Styles
{
    class SuccessStyle : EmbedStyle
    {
        public override void ApplyStyle(string? name = null)
        {
            Name = name ?? Names.Success;
            IconUrl = Icons.Success;
            Color = Colors.Success;
        }
    }
}
