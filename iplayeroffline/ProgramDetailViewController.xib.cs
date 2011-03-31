
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using BigTed.OnTheTelly.Net;
using MonoTouch.MediaPlayer;
using BigTed.OnTheTelly.Database;

namespace BigTed.OnTheTelly
{
	/// <summary>
	/// View controller for showing the detail of a program. static form basically.
	/// </summary>
	public partial class ProgramDetailViewController : UIViewController
	{
		#region Constructors

		// The IntPtr and initWithCoder constructors are required for controllers that need 
		// to be able to be created from a xib rather than from managed code
		
		public RemoteProgramHelper SelectedProgram {get; set;}
		

		public ProgramDetailViewController (IntPtr handle) : base(handle)
		{
			Initialize ();
		}

		[Export("initWithCoder:")]
		public ProgramDetailViewController (NSCoder coder) : base(coder)
		{
			Initialize ();
		}

		public ProgramDetailViewController () : base("ProgramDetailViewController", null)
		{
			Initialize ();
		}

		void Initialize ()
		{
		}
		
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			//Show an edit button
			
			//NavigationItem.RightBarButtonItem = EditButtonItem;
			
			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
			SelectedProgram.PopulateExtendedFields();
			
			
			
			this.Title = SelectedProgram.Program.Name;
			this.TitleLabel.Text = SelectedProgram.Program.Name;
			this.SubtitleLabel.Text = SelectedProgram.Program.Description;
			SubtitleLabel.Font = UIFont.SystemFontOfSize(UIFont.SmallSystemFontSize);
			this.ImageView.Image = SelectedProgram.GetFullsizeImage(ImageView.Bounds);
			
			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
			
			if (SelectedProgram.DownloadedFileExists)
			{
				this.DownloadButton.SetTitle("Play", UIControlState.Normal);
			}
			
			this.WatchButton.TouchUpInside += delegate {
			
				// This is for when the user wants to stream the video
				string url = SelectedProgram.StreamingUrl;
				if (string.IsNullOrEmpty(url))
				{
					using (UIAlertView alertView = new UIAlertView("Sorry.", "Sorry, this program is not available for streaming.", null, "OK")) 
					{
						alertView.Show();
						this.WatchButton.SetTitle("Not available", UIControlState.Normal);
					}
					
				} else {
					moviePlayer = new MPMoviePlayerViewController(new NSUrl(SelectedProgram.StreamingUrl));
					PresentMoviePlayerViewController(moviePlayer);
					
				}
			};
			
			this.DownloadButton.TouchUpInside += delegate {
		
				
				if (SelectedProgram.DownloadedFileExists) 
				{
					// if we have the file, we can play it! yay!
					Console.WriteLine("playing");
					moviePlayer = new MPMoviePlayerViewController(new NSUrl(SelectedProgram.OutputFilename, false));
					PresentMoviePlayerViewController(moviePlayer);
				} else {
					
					
					//If you happen to be out of the UK, but want to see how the queueing works, you may want to remove this
					// bit, as it checks if you can download and will not queue if it can't.
					// you still can't download, obviously, but atleast you can see the workflow.
					
					if (SelectedProgram.PrepareForQueue()) 
					{
					
						LocalProgram localQueueProgram = LocalProgramHelper.LocalProgramFromRemoteProgram(SelectedProgram.Program);
				
						localQueueProgram.State = "Q";
						AppDelegate.SessionDatabase.AddLocalProgram(localQueueProgram);
						
						this.DownloadButton.SetTitle("Queued", UIControlState.Normal);
					
					} else {
						
						// could be that you are outside of the UK, on 3G, or just that the show isn't ready yet.
						using (UIAlertView alertView = new UIAlertView("Sorry.", "Sorry, this program is not available for download.", null, "OK")) 
						{
							alertView.Show();
							this.DownloadButton.SetTitle("Not available", UIControlState.Normal);
						}
					}
					this.DownloadButton.Enabled = false;
					
					
					
				}
			};
			
		}
		
		MPMoviePlayerViewController moviePlayer = null;
		
		#endregion
		
		
		
	}
}
