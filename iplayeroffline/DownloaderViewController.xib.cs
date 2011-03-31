
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using BigTed.OnTheTelly.Database;
using BigTed.OnTheTelly.Net;

namespace BigTed.OnTheTelly
{
	/// <summary>
	/// The view which controls the downloader - the bit which does the downloading.
	/// </summary>
	public partial class DownloaderViewController : UIViewController
	{
		#region Constructors

		// The IntPtr and initWithCoder constructors are required for controllers that need 
		// to be able to be created from a xib rather than from managed code

		public DownloaderViewController (IntPtr handle) : base(handle)
		{
			Initialize ();
		}

		[Export("initWithCoder:")]
		public DownloaderViewController (NSCoder coder) : base(coder)
		{
			Initialize ();
		}

		public DownloaderViewController () : base("DownloaderViewController", null)
		{
			Initialize ();
		}

		void Initialize ()
		{
		}
		
		public QueueDownloader queueDownloader = null;
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			CloseButton.TouchUpInside += delegate {
				DismissModalViewControllerAnimated(true);
			};
			
			CloseButton.Hidden = true;
			
			queueDownloader = new QueueDownloader();
			queueDownloader.Queue = AppDelegate.SessionDatabase.GetLocalQueuedPrograms();
			
			if (queueDownloader.Queue.Count > 0)
			{
				LocalProgram firstProgram = queueDownloader.Queue[0];
			
				TitleLabel.Text = firstProgram.Name;
			
				ImageView.Image = RemoteProgramHelper.GetFullsizeImageFromFile(firstProgram.ThumbnailPath, ImageView.Bounds);
				int queueSize = queueDownloader.Queue.Count;
				if (queueSize == 0)
				{
					SubLabel.Text = "Downloading. No more items in the queue";
				} else if (queueSize == 1) {
					SubLabel.Text = String.Format("Downloading. {0} item remaining to download", queueSize);
				} else {
					SubLabel.Text = string.Format("Downloading. {0} items remaining to download", queueSize);
				}
				
				
				ProgressBar.Progress = 0;
				
			
			}
			queueDownloader.OnStart += HandleQueueDownloaderOnStart;
			queueDownloader.OnProgress += HandleQueueDownloaderOnProgress;
			queueDownloader.OnEnd += HandleQueueDownloaderOnEnd;
			queueDownloader.OnNoMoreItems += HandleQueueDownloaderOnNoMoreItems;
			
			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
			UIApplication.SharedApplication.IdleTimerDisabled = true;
			queueDownloader.Start();
		}

		void HandleQueueDownloaderOnNoMoreItems ()
		{
			InvokeOnMainThread(delegate {
				Console.WriteLine("All done!");
				SubLabel.Text = "No more items to download";
				CloseButton.Hidden = false;
				UIApplication.SharedApplication.IdleTimerDisabled = false;
				UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
			});
		}

		void HandleQueueDownloaderOnEnd (LocalProgram finishedProgram)
		{
			InvokeOnMainThread(delegate {
				Console.WriteLine("{0} is finished", finishedProgram.Name);
				
				AppDelegate.SessionDatabase.MarkProgramAsDownloaded(finishedProgram.Id);
			});
		}

		void HandleQueueDownloaderOnProgress (LocalProgram currentProgram, float progress)
		{
			InvokeOnMainThread(delegate {
				Console.WriteLine("{0} has progressed", currentProgram.Name);
				ProgressBar.Progress = progress;
				ProgressBar.SetNeedsDisplay();
			});
		}

		void HandleQueueDownloaderOnStart (LocalProgram startedProgram, int queueSize)
		{
			InvokeOnMainThread(delegate {
				Console.WriteLine("{0} has started", startedProgram.Name);
				
				TitleLabel.Text = startedProgram.Name;
				TitleLabel.SetNeedsDisplay();
				ImageView.Image = RemoteProgramHelper.GetFullsizeImageFromFile(startedProgram.ThumbnailPath, ImageView.Bounds);
				if (queueSize == 0)
				{
					SubLabel.Text = "Downloading. No more items in the queue";
				} else if (queueSize == 1) {
					SubLabel.Text = String.Format("Downloading. {0} item remaining to download", queueSize);
				} else {
					SubLabel.Text = string.Format("Downloading. {0} items remaining to download", queueSize);
				}
				
				SubLabel.SetNeedsDisplay();
				ProgressBar.Progress = 0;
				ProgressBar.SetNeedsDisplay();
			
				
				
			});
		}

		
		#endregion
		
		
		
	}
}
