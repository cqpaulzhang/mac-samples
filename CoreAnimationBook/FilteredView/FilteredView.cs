
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.CoreAnimation;
using MonoMac.CoreGraphics;
using MonoMac.CoreImage;

namespace FilteredView
{
	public partial class FilteredView : MonoMac.AppKit.NSView
	{
		#region Constructors

		// Called when created from unmanaged code
		public FilteredView (IntPtr handle) : base(handle)
		{
			Initialize ();
		}

		// Called when created directly from a XIB file
		[Export("initWithCoder:")]
		public FilteredView (NSCoder coder) : base(coder)
		{
			Initialize ();
		}

		// Shared initialization code
		void Initialize ()
		{
		}
		
		#endregion
		
		public override void AwakeFromNib ()
		{
			controls.WantsLayer = true;
		}
		
		public override bool AcceptsFirstResponder ()
		{
			return true;
		}
		
		public override void KeyDown (NSEvent theEvent)
		{
			base.KeyDown (theEvent);
		}
		
		public override void DrawRect (RectangleF dirtyRect)
		{
			RectangleF bounds = Bounds;
			SizeF stripeSize = bounds.Size;
			stripeSize.Width = bounds.Width / 10.0f;
			RectangleF stripe = bounds;
			stripe.Size = stripeSize;
			NSColor[] colors = new NSColor[2] {NSColor.White, NSColor.Blue};
			for (int i = 0; i < 10; i++)
			{
				colors[i % 2].Set();
				NSGraphics.RectFill(stripe);
				PointF origin = stripe.Location;
				origin.X += stripe.Size.Width;
				stripe.Location = origin;
			}
		}
		
		private void pointalize()
		{
			CIVector center = CIVector.Create(Bounds.GetMidX(), Bounds.GetMidY());
			
			CIFilter pointalize = CIFilter.FromName("CIPointillize");
			pointalize.SetValueForKey(NSNumber.FromFloat(1.0f), CIFilter.InputRadiusKey);
			pointalize.SetValueForKey(center, CIFilter.InputCenterKey);
			
			pointalize.Name = "pointalize";
			controls.ContentFilters = new CIFilter[] { pointalize };
			
			
		}
		
		partial void lightPointalize (NSButton sender)
		{
			if (controls.ContentFilters == null || controls.ContentFilters.Count() == 0)
			{
				pointalize();	
			}
			
			string path = string.Format("contentFilters.pointalize.{0}", CIFilter.InputRadiusKey);
			controls.SetValueForKeyPath(NSNumber.FromFloat(1.0f), (NSString)path);
		}
		
		partial void heavyPointalize (NSButton sender)
		{
			if (controls.ContentFilters == null || controls.ContentFilters.Count() == 0)
			{
				pointalize();	
			}
			
			string path = string.Format("contentFilters.pointalize.{0}", CIFilter.InputRadiusKey);
			controls.SetValueForKeyPath(NSNumber.FromFloat(5.0f), (NSString)path);
		}
	}
}

