using System.Reflection;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RazorViewComponentLib
{
	public abstract class AutoNamedSlotComponent : RazorViewComponentSlot
	{
		public AutoNamedSlotComponent() : base()
		{
			var thisType = GetType();

			var hte = thisType.GetCustomAttribute(typeof(HtmlTargetElementAttribute))
				as HtmlTargetElementAttribute;

			Throw.IfNull(hte, ex: _ => new InvalidOperationException(
				UiSafeMessages.GetMissingHtmlTargetElement(thisType.Name)));

			Throw.IfNullOrWhitespace(hte.Tag, ex: _ => new InvalidOperationException(
				UiSafeMessages.GetMissingHtmlTargetElementName(thisType.Name)));

			this.SlotName = hte.Tag;
		}


		#region UI-safe messages...

		private static class UiSafeMessages
		{
			public static string GetMissingHtmlTargetElement(string typeName) =>
				SR.Err_MissingHtmlTargetElement_Fmt.SF(typeName);

			public static string GetMissingHtmlTargetElementName(string typeName) =>
				SR.Err_MissingHtmlTargetElementName_Fmt.SF(typeName);
		}

		#endregion
	}
}
