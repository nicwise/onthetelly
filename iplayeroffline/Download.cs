
using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.IO;
using System.Threading;

namespace BigTed.OnTheTelly
{


	public class Download
	{

		public Download ()
		{
		}
		
		public string Id { get; set; }
		public string Name { get; set; }
		public string Thumbnail { get; set; }
		public string VideoPath { get; set; }
		public int DatabaseId { get; set; }
		
		
		private UIImage image = null;
		public UIImage ThumbnailImageForTableView
		{
			get 
			{
				if (image == null) 
				{

					if (File.Exists(Path.Combine(AppDelegate.DocumentsFolder,Thumbnail))) 
					{
						using (UIImage tempimage = UIImage.FromFileUncached(Path.Combine(AppDelegate.DocumentsFolder, Thumbnail)))
						{
					
							float imageWidth = tempimage.Size.Width;
							float imageHeight = tempimage.Size.Height;
							
							float newImageHeight = 44;
							float newImageWidth = imageWidth / (imageHeight / newImageHeight);
							
							UIGraphics.BeginImageContext(new System.Drawing.SizeF(newImageWidth, newImageHeight));
							tempimage.Draw(new System.Drawing.RectangleF(0,0,newImageWidth, newImageHeight));
						 	image = UIGraphics.GetImageFromCurrentImageContext();
							UIGraphics.EndImageContext();
						}
					} else {
						Console.WriteLine("Thumbnail doesn't exist: {0}", Thumbnail);
					}
					
				}
				return image;
			}
		}
		
	}
}
