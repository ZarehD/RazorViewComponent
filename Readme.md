<img src="Logo.png" alt="project icon" />

# RazorViewComponent

## What is it?

RazorViewComponent is a mashup of ASP.NET's TagHelper and partial pages technologies that makes it very easy to create and use UI components in your __Razor Pages__ projects.

<br>

Here is a sample of what it lets you do.

> There are more samples available in the repository.

```HTML
<!-- /Pages/Index.cshtml -->
@page
@model IndexModel
@{
	ViewData["Title"] = "Home page";
}

<div>
	<!-- a Bootstrap 5 card component -->
	<bs5-card 
		title="Sample Title" 
		subTitle="Sample Sub-Title"
		cardCss="my-3">

		<p>Some content for my sample card.</p>

		<card-header>
			<p>Some content for the card header</p>
		</card-header>
		<card-footer>
			<p>Some content for the card footer</p>
		</card-footer>
	</bs5-card>
</div>
```

<br>

## Okay, but how easy is it really?

Very easy, actually!

Add the nuget package to your project...
```
dotnet add package RazorViewComponent
```

Update View Imports...

```C#
// /Pages/_ViewImports.cshtml
@using WebApp1
@namespace WebApp1.Pages
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers

// *** ADD THIS LINE ***
@addTagHelper *, WebApp1
```

Define the component...

```C#
// BootstrapCard.cshtml.cs
using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorViewComponent;

namespace WebApp1.Pages.Shared.Components
{
	[HtmlTargetElement("bs5-card")]
	public class BootstrapCard : RazorViewComponent
	{
		[HtmlAttributeName("title")]
		public string? CardTitle { get; set; }

		[HtmlAttributeName("subTitle")]
		public string? CardSubTitle { get; set; }

		[HtmlAttributeName("cardCss")]
		public string? CardCss { get; set; }
	}

	[HtmlTargetElement("card-header")]
	public class BootstrapCardHeader : AutoNamedSlotComponent { }

	[HtmlTargetElement("card-footer")]
	public class BootstrapCardFooter : AutoNamedSlotComponent { }

	// NOTE: Don't worry about 'magic' strings. I used them here only 
	// for the sake of brevity. Take a look at the sample project to 
	// see how to use constants to eliminate 'magic' strings.
}
```

And its view...

```HTML
<!-- Pages/Shared/Components/BootstrapCard.cshtml -->
@model BootstrapCard;

<div class="card @Model.CardCss">
	@if (Model.SlotHasContent("card-header"))
	{
		<div class="card-header">
			@Model.RenderSlot("card-header")
		</div>
	}
	<div class="card-body">
		@if (Model.CardTitle is not null)
		{
			<h5 class="card-title">@Model.CardTitle</h5>
		}
		@if (Model.CardSubTitle is not null)
		{
			<h6 class="card-subtitle">@Model.CardSubTitle</h6>
		}
		@if (Model.HasChildContent())
		{
			<div class="mt-2">
				@Model.RenderChildContent()
			</div>
		}
	</div>
	@if (Model.SlotHasContent("card-footer"))
	{
		<div class="card-footer">
			@Model.RenderSlot("card-footer")
		</div>
	}
</div>
```
And, that's really all there is to it!

<br>

You have quite a bit of flexibility in what your components can do. You could, for instance, inject a service into your component to query a database based on the value of one of the components attributes (e.g. productId) in order to display product data in a consistent, component-based UI element. While that's not an especially practical example, it does illustrate the flexibility available to you. A more realistic approach would be to just provide a pre-populated __Product__ object to your component.

<br>

## Configuration

### Location of view files

By default, `RazorViewComponent` looks for views associated with a component in the `/Shared/Components` subfolder under `/Pages` (i.e. `/Pages/Shared/Components/*.cshtml`). It uses the component's class name (without the '*Component*' suffix, if present) as the view file name. So, as an example, for a component class named `MyViewComponent`, the pathname for its associated view file would be `/Pages/Shared/Components/MyView.cshtml`. 

This path configuration applies only to component view (__*.cshtml__) files; the __*.cs__ file containing the component's class declaration can be located anywhere in your project. It is only a convention to place the __.cs__ and __.cshtml__ files in the same location (e.g. *MyView.cshtml.cs* and *MyView.cshtml*).

The reason why component view files must be located under the `/Pages` folder is that `RazorViewComponent` uses the partial-view rendering capabilities built into ASP.NET to render component views. 
Accordingly, view files must be located somewhere the ASP.NET Razor view engine can find them, which by default in a Razor Pages project is the `/Pages` folder. The configuration for the Razor view engine in ASP.NET can be modified, of course, in the call to `services.AddRazorPages()`.

> __IMPORTANT__: If you change the location where ASP.NET searches for (partial) view files, you must also relocate the __/Pages/Shared/Components/__ folder to that new location (e.g. __/NewPagesFolder/Shared/Components__).

You can specify a different folder where `RazorViewComponent` should look for component view files by configuring an instance of `RazorViewComponentOptions` using a `services.Configure<T>()` call. 
For example, the following will change the folder where `RazorViewComponent` looks for views to `/Pages/MyComponents`.

```C#
services.Configure<RazorViewComponentOptions>(
	options =>
	{
		options.ViewFilesLocation = "/MyComponents";
	});
```

> __IMPORTANT__: In all cases, the folder containing component view files must be a subfolder under the root folder (*or the root folder itself*) where ASP.NET will search for view files (i.e. `/Pages`, by default).

<br>

#### If you like this project or find it useful, please give it a star. Thank you.
