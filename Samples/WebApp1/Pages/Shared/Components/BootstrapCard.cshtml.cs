using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorViewComponentLib;

namespace WebApp1.Pages.Shared.Components
{
	internal class BsCardNames
	{
		public const string Tag = "bs5-card";
		internal class Slot
		{
			public const string Header = "card-header";
			public const string Footer = "card-footer";
			public const string Links = "card-links";
		}
	}

	public class ImageInfo(string source)
	{
		public string Source { get; set; } = source;
		public string? AltText { get; set; }
		public string? CssClass { get; set; }
	}


	[HtmlTargetElement(BsCardNames.Tag)]
	public class BootstrapCard : RazorViewComponent
	{
		[HtmlAttributeName("title")]
		public string? CardTitle { get; set; }

		[HtmlAttributeName("subTitle")]
		public string? CardSubTitle { get; set; }

		[HtmlAttributeName("cardCss")]
		public string? CardCss { get; set; }

		[HtmlAttributeName("cardImg")]
		public ImageInfo? CardImage { get; set; }


		// Convenience methods...
		public bool HasHeaderSlot() => HasSlot(BsCardNames.Slot.Header);
		public bool HasFooterSlot() => HasSlot(BsCardNames.Slot.Footer);
		public TagHelperContent RenderHeaderSlot() => RenderSlot(BsCardNames.Slot.Header);
		public TagHelperContent RenderFooterSlot() => RenderSlot(BsCardNames.Slot.Footer);
	}


	[HtmlTargetElement(BsCardNames.Slot.Header)]
	public class BootstrapCardHeader : AutoNamedSlotComponent { }


	[HtmlTargetElement(BsCardNames.Slot.Footer)]
	public class BootstrapCardFooter : AutoNamedSlotComponent { }


	[HtmlTargetElement(BsCardNames.Slot.Links)]
	public class BootstrapCardLinks : AutoNamedSlotComponent { }
}
