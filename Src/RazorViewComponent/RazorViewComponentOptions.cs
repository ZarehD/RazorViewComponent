namespace RazorViewComponentLib
{
	public class RazorViewComponentOptions
	{
		/// <summary>
		///		Gets or sets the folder where the partial view files 
		///		used for rendering <see cref="RazorViewComponent"/> 
		///		comonents can be found.
		/// </summary>
		/// <remarks>
		///		<para>
		///			If you would like to specify a different location for where 
		///			view files can be found, register an instance of this type 
		///			using the <![CDATA[IServiceCollection.Configure<T>()]]>.
		///		</para>
		///		<para>
		///			The specified location must be relative to the path(s) 
		///			that the Razor view engine is configured to search for 
		///			view files (e.g. /Pages and /Pages/Shared).
		///		</para>
		/// </remarks>
		public string ViewFilesLocation { get; set; } = Constants.DefaultViewsLocation;
	}
}
