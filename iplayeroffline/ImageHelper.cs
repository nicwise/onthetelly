
using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.IO;
using System.Threading;

namespace BigTed.OnTheTelly
{

	
	/// <summary>
	/// Scales a UIImage to a specific size. Specify -1 for width or height and we keep the aspect ratio.
	/// </summary>
	public static class ImageHelper
	{
		public static UIImage TempImage = UIImage.FromBundle("images/tempimage.png");
		                                              
		public static UIImage ResizeImage(UIImage source, float width, float height)
		{
			if (source == null)
			{
				return source;
			}
			
			float imageWidth = source.Size.Width;
			float imageHeight = source.Size.Height;
					
			float newImageHeight = height;
			float newImageWidth = width;
			
			if (width == -1 && height != -1)
			{
				newImageWidth = imageWidth / (imageHeight / newImageHeight);
			} else if (width != -1 && height == -1)
			{
				newImageHeight = imageHeight / (imageWidth / newImageWidth);
			}
			
			try {
				UIGraphics.BeginImageContext(new System.Drawing.SizeF(newImageWidth, newImageHeight));
				source.Draw(new System.Drawing.RectangleF(0,0,newImageWidth, newImageHeight));
			 	return UIGraphics.GetImageFromCurrentImageContext();
			} finally 
			{
				UIGraphics.EndImageContext();
			}
		}

		
	}
}
