namespace WebAppUiComponents.Pages.Shared.Components
{
	[HtmlTargetElement(BsCarouselConst.Carousel.Tag)]
	[RestrictChildren(BsCarouselConst.CarouselItem.Tag)]
	public class BsCarousel : RazorViewComponent
	{
		[HtmlAttributeName("carouselId")]
		public string CarouselId { get; set; } =
			IdHelper.GetUniqueId(BsCarouselConst.Carousel.Prefix);

		[HtmlAttributeName("rideMode")]
		public CarouselRideMode RideMode { get; set; } = CarouselRideMode.None;

		[HtmlAttributeName("hideControls")]
		public bool HideControls { get; set; }

		[HtmlAttributeName("showIndicators")]
		public bool ShowIndicators { get; set; }

		[HtmlAttributeName("imgHeight")]
		public int ImageHeight { get; set; }


		public int ImageItemCount { get; private set; }
		public int ActiveImageItemNum { get; private set; } = -1;
		public bool HasImageItems => this.ImageItemCount > 0;


		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			await GetCarouselItemInfoAsync(context, output);
			await base.ProcessAsync(context, output);
		}

		private async Task GetCarouselItemInfoAsync(TagHelperContext context, TagHelperOutput output)
		{
			await ProcessChildContentAsync(output);

			var itemComponents = GetComponentsOfType(context, typeof(BsCarouselItem));
			this.ImageItemCount = itemComponents?.Count() ?? 0;
			if (this.ImageItemCount > 0)
			{
				itemComponents = itemComponents!.Reverse();

				for (var i = 0; i < this.ImageItemCount; i++)
				{
					var c = (BsCarouselItem) itemComponents!.ElementAt(i);
					if (c.IsActive)
					{
						this.ActiveImageItemNum = i;
						break;
					}
				}
			}
		}
	}


	[HtmlTargetElement(BsCarouselConst.CarouselItem.Tag, ParentTag = BsCarouselConst.Carousel.Tag)]
	[RestrictChildren(BsCarouselConst.CarouselItem.Slot.Caption)]
	public class BsCarouselItem : RazorViewComponent
	{
		[HtmlAttributeName("imgInfo")]
		public ImageInfo ImageInfo { get; set; } = null!;

		[HtmlAttributeName("altText")]
		public string? AltText { get; set; }

		[HtmlAttributeName("isActive")]
		public bool IsActive { get; set; }

		[HtmlAttributeName("interval")]
		public uint Interval { get; set; } = 2500;

		public int ImageHeight { get; private set; }


		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			GetImageHeightFromCarousel(context);
			await base.ProcessAsync(context, output);
		}

		private void GetImageHeightFromCarousel(TagHelperContext context)
		{
			var carousel = FindFirstParentOfType(context, typeof(BsCarousel)) as BsCarousel;
			if (carousel is not null)
			{
				this.ImageHeight = carousel.ImageHeight;
			}
		}


		public bool HasCaptionSlot() => HasSlot(BsCarouselConst.CarouselItem.Slot.Caption);
		public TagHelperContent RenderCaptionSlot() => RenderSlot(BsCarouselConst.CarouselItem.Slot.Caption);
	}


	[HtmlTargetElement(BsCarouselConst.CarouselItem.Slot.Caption, ParentTag = BsCarouselConst.CarouselItem.Tag)]
	public class BsCarouselItemCaptionSlot : AutoNamedSlotComponent { }



	#region Related types...

	public enum CarouselRideMode { None, Carousel, True }


	internal class BsCarouselConst
	{
		public class Carousel
		{
			public const string Tag = "bs-carousel";
			public const string Prefix = "crsl";
		}

		public class CarouselItem
		{
			public const string Tag = "bs-carousel-item";

			public class Slot
			{
				public const string Caption = "bs-carousel-item-caption";
			}
		}
	}

	#endregion
}
