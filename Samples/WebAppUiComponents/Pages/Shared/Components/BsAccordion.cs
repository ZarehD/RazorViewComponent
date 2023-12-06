namespace WebAppUiComponents.Pages.Shared.Components;


[HtmlTargetElement(BsAccordionConst.Accordion.Tag)]
[RestrictChildren(BsAccordionConst.AccordionItem.Tag)]
public class BsAccordion : RazorViewComponent
{
	[HtmlAttributeName("accordionId")]
	public string AccordionId { get; set; } =
		BsAccordionConst.GetUniqueId(BsAccordionConst.Accordion.Prefix);

}


[HtmlTargetElement(BsAccordionConst.AccordionItem.Tag, ParentTag = BsAccordionConst.Accordion.Tag)]
public class BsAccordionItem : RazorViewComponent
{
	public string AccordionItemId { get; set; } =
		BsAccordionConst.GetUniqueId(BsAccordionConst.AccordionItem.Prefix);

	[HtmlAttributeName("show")]
	public bool Show { get; set; }


	internal string ParentAccordionId = string.Empty;

	public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
	{
		var parent = FindParentComponent(context, typeof(BsAccordion)) as BsAccordion;
		if (parent is not null)
		{
			this.ParentAccordionId = parent.AccordionId;
		}

		await base.ProcessAsync(context, output);
	}

	public TagHelperContent RenderHeaderSlot() =>
		RenderSlot(BsAccordionConst.AccordionItem.Slot.Header);
}


[HtmlTargetElement(BsAccordionConst.AccordionItem.Slot.Header, ParentTag = BsAccordionConst.AccordionItem.Tag)]
public class BsAccordionItemHeader : AutoNamedSlotComponent { }





internal class BsAccordionConst
{
	public static string GetUniqueId(string? prefix) =>
		$"{prefix}_{Guid.NewGuid():n}";

	internal class Accordion
	{
		public const string Tag = "bs-accordion";
		public const string Prefix = "accd";
	}

	internal class AccordionItem
	{
		public const string Tag = "bs-accordion-item";
		public const string Prefix = "accdItem";

		internal class Slot
		{
			public const string Header = "accordion-header";
		}
	}
}
