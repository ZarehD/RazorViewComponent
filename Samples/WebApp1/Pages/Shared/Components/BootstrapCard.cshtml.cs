using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorViewComponentLib;

namespace WebApp1.Pages.Shared.Components
{
	internal class BsCard
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


	[HtmlTargetElement(BsCard.Tag)]
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
		public bool HasHeaderContent() => SlotHasContent(BsCard.Slot.Header);
		public bool HasFooterContent() => SlotHasContent(BsCard.Slot.Footer);
		public TagHelperContent RenderHeaderContent() => RenderSlot(BsCard.Slot.Header);
		public TagHelperContent RenderFooterContent() => RenderSlot(BsCard.Slot.Footer);
	}


	[HtmlTargetElement(BsCard.Slot.Header)]
	public class BootstrapCardHeader : AutoNamedSlotComponent { }


	[HtmlTargetElement(BsCard.Slot.Footer)]
	public class BootstrapCardFooter : AutoNamedSlotComponent { }


	[HtmlTargetElement(BsCard.Slot.Links)]
	public class BootstrapCardLinks : AutoNamedSlotComponent { }
}
