
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.IO;
using MonoTouch.MediaPlayer;
using BigTed.OnTheTelly.Database;

namespace BigTed.OnTheTelly
{
	
	/// <summary>
	/// The view controller for the queue list. Shows programs in the queue, allows for downloading of shows via DownloaderViewController
	/// </summary>
	public partial class QueueViewController : UITableViewController
	{
		public QueueViewController (IntPtr handle) : base(handle)
		{
		}
		


		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this.TableView.Source = new DataSource (this);
			this.Title = "Queue";
			
			this.NavigationItem.RightBarButtonItem = new UIBarButtonItem("Download", UIBarButtonItemStyle.Bordered,
			                                                             this, new MonoTouch.ObjCRuntime.Selector("DownloadQueue"));
		}

		/// <summary>
		/// Called when the right bar button gets pressed. Export is because we have to use an Obj-C selector!
		/// </summary>
		[MonoTouch.Foundation.Export("DownloadQueue")]
		public void DownloadQueue() 
		{
			if ((TableView.Source as DataSource).downloads.Count == 0)
			{
				using (UIAlertView alertView = new UIAlertView("Nothing to download", "You need to add a program to the queue before downloading.", null, "OK"))
				{
					alertView.Show();
				}
				return;
			}
			
			var downloader = new DownloaderViewController();
			
			downloader.ModalTransitionStyle = UIModalTransitionStyle.FlipHorizontal;
			NavigationController.PresentModalViewController(downloader, true);
			
		}
		

		/*
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			
		}
		*/
		
		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			if (TableView.Source != null) 
			{
				(TableView.Source as DataSource).ReloadFromDatabase();
				TableView.ReloadData();
			}
		}
		
		/*
		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}
		*/
		/*
		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}
		*/

		/*
		// Override to allow orientations other than the default portrait orientation
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			//return true for supported orientations
			return (InterfaceOrientation == UIInterfaceOrientation.Portrait);
		}
		*/

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidUnload ()
		{
			// Release anything that can be recreated in viewDidLoad or on demand.
			// e.g. this.myOutlet = null;
			
			base.ViewDidUnload ();
		}

		class DataSource : UITableViewSource
		{
			QueueViewController controller;

			public List<LocalProgram> downloads = new List<LocalProgram>();
			
			public DataSource (QueueViewController controller)
			{
				this.controller = controller;
			}
			
			public void ReloadFromDatabase() 
			{
				Console.WriteLine("reloading");
			    downloads = AppDelegate.SessionDatabase.GetLocalQueuedPrograms();
					
								
				
			}

			public override int NumberOfSections (UITableView tableView)
			{
				return 1;
			}

			// Customize the number of rows in the table view
			public override int RowsInSection (UITableView tableview, int section)
			{
				return downloads.Count;
			}

			// Customize the appearance of table view cells.
			public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
			{
				string cellIdentifier = "QueueCell";
				var cell = tableView.DequeueReusableCell (cellIdentifier);
				if (cell == null) {
					cell = new UITableViewCell (UITableViewCellStyle.Subtitle, cellIdentifier);
				}
				
				
				
				LocalProgramHelper localProgram = new LocalProgramHelper(downloads[indexPath.Row]);
				cell.TextLabel.Text = localProgram.Program.Name;
		
				
				cell.DetailTextLabel.Text = localProgram.Program.Description;
				cell.DetailTextLabel.Font = UIFont.SystemFontOfSize(IplayerConst.DetailTextLabelSize);
				
				
				cell.ImageView.Image = localProgram.ThumbnailImageForTableView;
				cell.Accessory = UITableViewCellAccessory.None;
				
				
				return cell;
			}

			
			// Override to support conditional editing of the table view.
			public override bool CanEditRow (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
			{
				// Return false if you do not want the specified item to be editable.
				return true;
			}
			
			
			// Override to support editing the table view.
			public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, MonoTouch.Foundation.NSIndexPath indexPath)
			{
				if (editingStyle == UITableViewCellEditingStyle.Delete) {
					
					
					var SelectedProgram = downloads[indexPath.Row];
					
					AppDelegate.SessionDatabase.DeleteLocalProgramById(SelectedProgram.Id);
					
					try 
					{
						Directory.Delete(Path.Combine(AppDelegate.DocumentsFolder, SelectedProgram.ProgramId), true);
					} catch 
					{
					}
						
					downloads.RemoveAt(indexPath.Row);
					
					controller.TableView.DeleteRows (new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
					
				} else if (editingStyle == UITableViewCellEditingStyle.Insert) {
					// Create a new instance of the appropriate class, insert it into the array, and add a new row to the table view.
				}
			}
			
			/*
			// Override to support rearranging the table view.
			public override void MoveRow (UITableView tableView, NSIndexPath sourceIndexPath, NSIndexPath destinationIndexPath)
			{
			}
			*/
			/*
			// Override to support conditional rearranging of the table view.
			public override bool CanMoveRow (UITableView tableView, NSIndexPath indexPath)
			{
				// Return false if you do not want the item to be re-orderable.
				return true;
			}
			*/

			// Override to support row selection in the table view.
			
			
			
			public override void RowSelected (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
			{
				// Navigation logic may go here -- for example, create and push another view controller.
				//var programDetailViewController = new ProgramDetailViewController ();
				//programDetailViewController.SelectedProgram = programs[indexPath.Row];
				//controller.NavigationController.PushViewController(programDetailViewController, true);
				
			}
		}
		
		
		
	}
}
