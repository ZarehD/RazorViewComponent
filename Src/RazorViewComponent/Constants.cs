namespace RazorViewComponentLib
{
	internal static class Constants
	{
		public static readonly string DefaultViewsLocation = "/Shared/Components";


		public static readonly string Component = "Component";

		public static readonly char BakSlash = '\\';
		public static readonly char FwdSlash = '/';

		public static string NormalizeForPlatform(this string source) =>
			Throw.IfNull(source)
			.Replace(BakSlash, Path.DirectorySeparatorChar)
			.Replace(FwdSlash, Path.DirectorySeparatorChar)
			;
	}
}
