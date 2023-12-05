using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RazorViewComponentLib
{
	public abstract class RazorViewComponentSlot : RazorViewComponent
	{
		public string SlotName { get; protected set; } = string.Empty;


		public RazorViewComponentSlot() { }


		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			Throw.IfNullOrWhitespace(this.SlotName,
				ex: _ => new InvalidOperationException(
					UiSafeMessages.Err_MissingSlotNameAttributeValue));

			Throw.IfNull(this.ParentComponent,
				ex: _ => new InvalidOperationException(
					UiSafeMessages.GetCannotUseSlotWithoutParent(this.SlotName)));

			Throw.InvalidOpWhen(
				() => !this.ParentComponent.CanAddSlot(this.SlotName),
				UiSafeMessages.GetSlotAlreadyExists(this.SlotName));

			var childContent = await output.GetChildContentAsync();

			Throw.IfNull(childContent,
				ex: _ => new InvalidOperationException(
					UiSafeMessages.GetSlotHasNoContent(this.SlotName)));

			Throw.InvalidOpWhen(
				() => !this.ParentComponent.TryAddSlot(this.SlotName, childContent),
				UiSafeMessages.GetFailedToAddSlotToParent(this.SlotName));

			output.SuppressOutput();
		}



		#region UI-safe messages...

		private static class UiSafeMessages
		{
			public static readonly string Err_MissingSlotNameAttributeValue = SR.Err_MissingRequiredAttribute_Fmt.SF("name");


			public static string GetCannotUseSlotWithoutParent(string slotName) =>
				SR.Err_CannotUseSlotWithoutParent_Fmt.SF(slotName);

			public static string GetSlotAlreadyExists(string slotName) =>
				SR.Err_SlotAlreadyExists_Fmt.SF(slotName);

			public static string GetSlotHasNoContent(string slotName) =>
				SR.Err_SlotHasNoContent_Fmt.SF(slotName);

			public static string GetFailedToAddSlotToParent(string slotName) =>
				SR.Err_FailedToAddSlot_Fmt.SF(slotName);
		}

		#endregion
	}
}
