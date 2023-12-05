using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace RazorViewComponentLib
{
	public abstract class RazorViewComponent : TagHelper
	{
		protected static readonly string _componentStackKey = $"{nameof(RazorViewComponent)}:Stack";

		protected readonly RazorViewComponentOptions PartialViewComponentOptions;

		protected string PartialViewPathname { get; set; }

		[HtmlAttributeNotBound]
		[ViewContext]
		public ViewContext? ViewContext { get; set; }

		[HtmlAttributeNotBound]
		protected RazorViewComponent? ParentComponent { get; set; }

		[HtmlAttributeNotBound]
		protected TagHelperContent? ChildContent { get; set; }

		[HtmlAttributeNotBound]
		protected Dictionary<string, TagHelperContent> NamedSlots { get; set; } = new();


		public RazorViewComponent(
			IOptions<RazorViewComponentOptions>? optionsAccessor = default)
		{
			this.PartialViewComponentOptions = optionsAccessor?.Value ?? new();
			this.PartialViewPathname =
				Path.Combine(
					this.PartialViewComponentOptions.ViewFilesLocation.NormalizeForPlatform(),
					$"{GetType().Name.EnsureNotEndsWith(Constants.Component)}.cshtml");
		}

		public RazorViewComponent(
			string viewPathname,
			IOptions<RazorViewComponentOptions>? optionsAccessor = default)
		{
			this.PartialViewComponentOptions = optionsAccessor?.Value ?? new();
			this.PartialViewPathname = viewPathname;
		}


		public override sealed void Init(TagHelperContext context)
		{
			if (context.Items.ContainsKey(_componentStackKey))
			{
				var parentComponentStack = GetParentComponentStack(context);
				this.ParentComponent = parentComponentStack.Peek();
				if (this is not RazorViewComponentSlot)
				{
					parentComponentStack.Push(this);
				}
			}
			else
			{
				var parentComponentStack = new Stack<RazorViewComponent>();
				this.ParentComponent = null;
				parentComponentStack.Push(this);
				SetParentComponentStack(context, parentComponentStack);
			}

			base.Init(context);
		}

		private static Stack<RazorViewComponent> GetParentComponentStack(TagHelperContext context) =>
			(context.Items[_componentStackKey] as Stack<RazorViewComponent>)!;

		private static void SetParentComponentStack(TagHelperContext context, Stack<RazorViewComponent> parentComponentStack) =>
			Throw.IfNull(context).Items[_componentStackKey] =
			Throw.IfNull(parentComponentStack);

		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			Throw.IfNull(this.ViewContext,
				ex: _ => new InvalidOperationException(
					UiSafeMessages.Err_NoViewContext));
			Throw.IfNullOrWhitespace(this.PartialViewPathname);

			await RenderPartialView(output, this.PartialViewPathname);
		}

		protected async Task RenderPartialView(TagHelperOutput output, string viewPathname)
		{
			Throw.IfNull(output);
			Throw.IfNullOrWhitespace(viewPathname);

			var childContent = await output.GetChildContentAsync();

			if (childContent is not null)
			{
				this.ChildContent = childContent;
			}

			var htmlHelper = GetHtmlHelper();

			Throw.IfNull(htmlHelper,
				ex: _ => new InvalidOperationException(
				UiSafeMessages.GetUnableToAcquireInstance(
					nameof(IHtmlHelper))));

			var content = await htmlHelper.PartialAsync(viewPathname, this);

			output.Content.SetHtmlContent(content);
			output.TagName = string.Empty;
		}

		protected IHtmlHelper GetHtmlHelper()
		{
			Throw.IfNull(this.ViewContext,
				ex: _ => new InvalidOperationException(
					UiSafeMessages.Err_NoViewContext));

			var htmlHelper =
				this.ViewContext.HttpContext.RequestServices
				.GetRequiredService<IHtmlHelper>();

			if (htmlHelper is IViewContextAware h)
			{
				h.Contextualize(this.ViewContext);
			}

			return htmlHelper;
		}

		public bool CanAddSlot(string slotName) =>
			!this.NamedSlots.ContainsKey(
				Throw.IfNullOrWhitespace(slotName));

		public bool TryAddSlot(string slotName, TagHelperContent content)
		{
			Throw.IfNullOrWhitespace(slotName);
			Throw.IfNull(content);

			return
				CanAddSlot(slotName) &&
				this.NamedSlots.TryAdd(slotName, content)
				;
		}

		public bool HasChildContent() =>
			(this.ChildContent is not null) &&
			!this.ChildContent.IsEmptyOrWhiteSpace;

		public bool SlotHasContent(string slotName) =>
			this.NamedSlots.ContainsKey(Throw.IfNullOrWhitespace(slotName)) &&
			(this.NamedSlots[slotName] is not null) &&
			!this.NamedSlots[slotName].IsEmptyOrWhiteSpace;

		public TagHelperContent RenderChildContent() =>
			this.ChildContent ?? new DefaultTagHelperContent();

		public TagHelperContent RenderSlot(string slotName)
		{
			Throw.IfNullOrWhitespace(slotName);

			if (!this.NamedSlots.TryGetValue(slotName, out var slot))
			{
				Throw.InvalidOp(UiSafeMessages.GetSlotIsNotDefined(slotName));
			}

			return slot;
		}



		#region UI-safe messages...

		private static class UiSafeMessages
		{
			public static readonly string Err_NoViewContext = SR.Err_NoViewContext;


			public static string GetUnableToAcquireInstance(string objectTypeName) =>
				SR.Msg_UnableToAcquireInstance_Fmt.SF(objectTypeName);

			public static string GetSlotIsNotDefined(string slotName) =>
				SR.Msg_SlotNotDefined_Fmt.SF(slotName);
		}

		#endregion
	}
}
