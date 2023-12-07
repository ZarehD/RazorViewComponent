using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace RazorViewComponentLib
{
	public abstract class RazorViewComponent : TagHelper
	{
		private static readonly string _componentStackKey = $"{nameof(RazorViewComponent)}:Stack";
		protected static readonly TagHelperContent DefaultContent = new DefaultTagHelperContent();

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
		protected Dictionary<string, TagHelperContent> NamedSlots { get; set; } =
#if NET8_0_OR_GREATER
			[];
#else
			new();
#endif

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
				var parentComponentStack = GetComponentStack(context);
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
				SetComponentStack(context, parentComponentStack);
			}

			base.Init(context);
		}

		protected static RazorViewComponent? FindFirstParentOfType(TagHelperContext context, Type parentComponentType)
		{
			var componentStack = GetComponentStack(context);
			// NOTE: first element (index 0) in stack is the current component instance.
			if (componentStack.Count < 2) return null;
			RazorViewComponent? result = null;
			for (var i = 1; i < componentStack.Count; i++)
			{
				if (componentStack.ElementAt(i).GetType() == parentComponentType)
				{
					result = componentStack.ElementAt(i);
					break;
				}
			}
			return result;
		}

		protected static IEnumerable<RazorViewComponent>? GetComponentsOfType(TagHelperContext context, Type componentType)
		{
			Throw.IfNull(context);
			Throw.IfNull(componentType);

			return GetComponentStack(context).Where(c => c.GetType() == componentType);
		}

		private static Stack<RazorViewComponent> GetComponentStack(TagHelperContext context) =>
			(Throw.IfNull(context).Items[_componentStackKey] as Stack<RazorViewComponent>) ??
			new InvalidOperationException(UiSafeMessages.Err_NoComponentStack).Throw<Stack<RazorViewComponent>>();

		private static void SetComponentStack(TagHelperContext context, Stack<RazorViewComponent> parentComponentStack) =>
			Throw.IfNull(context).Items[_componentStackKey] =
			Throw.IfNull(parentComponentStack);

		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			ValidateProcessInputs(context, output);
			await ProcessChildContentAsync(output);
			await ProcessComponentViewAsync(output);
		}

		private void ValidateProcessInputs(TagHelperContext context, TagHelperOutput output)
		{
			Throw.IfNull(context);
			Throw.IfNull(output);

			Throw.IfNull(this.ViewContext,
				ex: _ => new InvalidOperationException(
					UiSafeMessages.Err_NoViewContext));

			Throw.IfNullOrWhitespace(this.PartialViewPathname,
				ex: _ => new InvalidOperationException(
					UiSafeMessages.Err_BadPartialViewName));
		}

		protected virtual async Task ProcessChildContentAsync(TagHelperOutput output)
		{
			var childContent = await output.GetChildContentAsync();

			if (childContent is not null)
			{
				this.ChildContent = childContent;
			}
		}

		protected virtual async Task ProcessComponentViewAsync(TagHelperOutput output)
		{
			var htmlHelper = GetHtmlHelper();

			Throw.IfNull(htmlHelper,
				ex: _ => new InvalidOperationException(
				UiSafeMessages.GetUnableToAcquireInstance(
					nameof(IHtmlHelper))));

			var content = await htmlHelper.PartialAsync(this.PartialViewPathname, this);

			output.Content.SetHtmlContent(content);
			output.TagName = string.Empty;
			output.TagMode = TagMode.StartTagAndEndTag;
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

		public bool HasSlot(string slotName) =>
			this.NamedSlots.ContainsKey(Throw.IfNullOrWhitespace(slotName))
			&& (this.NamedSlots[slotName] is not null)
			//&& !this.NamedSlots[slotName].IsEmptyOrWhiteSpace
			;

		public TagHelperContent RenderChildContent() =>
			this.ChildContent ?? DefaultContent;

		public TagHelperContent RenderSlot(string slotName) =>
			this.NamedSlots.TryGetValue(Throw.IfNullOrWhitespace(slotName),
				out var slot) ? slot : DefaultContent;



		#region UI-safe messages...

		private static class UiSafeMessages
		{
			public static readonly string Err_NoComponentStack = SR.Err_NoComponentStack;

			public static readonly string Err_NoViewContext = SR.Err_NoViewContext;

			public static readonly string Err_BadPartialViewName = SR.Err_InvalidPartialViewPathname;

			public static string GetUnableToAcquireInstance(string objectTypeName) =>
				SR.Msg_UnableToAcquireInstance_Fmt.SF(objectTypeName);

			public static string GetSlotIsNotDefined(string slotName) =>
				SR.Msg_SlotNotDefined_Fmt.SF(slotName);
		}

		#endregion
	}
}
