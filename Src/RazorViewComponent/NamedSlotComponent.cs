using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RazorViewComponentLib
{
	[HtmlTargetElement("slot")]
	public abstract class NamedSlotComponent : RazorViewComponentSlot
	{
		[HtmlAttributeName("name")]
		public string Name
		{
			get => this.SlotName;
			set => this.SlotName = value;
		}
	}
}
