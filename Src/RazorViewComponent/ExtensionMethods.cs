namespace RazorViewComponentLib
{
	public static class ExtensionMethods
	{
		public static string EnsureNotEndsWith(
			this string? source, string? suffix,
			StringComparison mode = StringComparison.OrdinalIgnoreCase) =>
			(source is null) || string.IsNullOrEmpty(suffix)
			? string.Empty : !source.EndsWith(suffix, mode)
			? source : source[0..^suffix.Length];
	}
}
