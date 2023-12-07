namespace WebAppUiComponents
{
	public class ImageInfo(string source)
	{
		public string Source { get; set; } = source;
		public string? AltText { get; set; }
		public string? CssClass { get; set; }
	}
}
