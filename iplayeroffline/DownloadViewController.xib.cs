
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.IO;
using MonoTouch.MediaPlayer;
using BigTed.OnTheTelly.Database;
using System.Drawing;


namespace BigTed.OnTheTelly
{
	/// <summary>
	/// The view controller for the download view - the list of items which have been downloaded.
	/// </summary>
	public partial class DownloadViewController : UITableViewController
	{
		public DownloadViewController (IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			//Show an edit button
			//NavigationItem.RightBarButtonItem = EditButtonItem;
			//UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
			this.TableView.Source = new DataSource (this);
			this.Title = "Downloads";
			
			//UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
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
			DownloadViewController controller;

			List<LocalProgram> downloads = new List<LocalProgram>();
			
			public DataSource (DownloadViewController controller)
			{
				this.controller = controller;
				
				
			}
			
			public void ReloadFromDatabase() 
			{
				Console.WriteLine("reloading");
				downloads = AppDelegate.SessionDatabase.GetLocalDownloadedPrograms();
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
			
			public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
			{
				return 50;
	
			}


			// Customize the appearance of table view cells.
			public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
			{
				string cellIdentifier = "DownloadCell";
				UITableViewCell cell = tableView.DequeueReusableCell (cellIdentifier);
				if (cell == null) {
					cell = new UITableViewCell (UITableViewCellStyle.Subtitle, cellIdentifier);
				}
				
				
				
				LocalProgramHelper localHelper = new LocalProgramHelper(downloads[indexPath.Row]);
				
				
				
				cell.TextLabel.Text = localHelper.Program.Name;
				cell.DetailTextLabel.Text = 
				
				cell.TextLabel.Text = localHelper.Program.Name;
				cell.TextLabel.Font = UIFont.BoldSystemFontOfSize(IplayerConst.TextLabelSize);
				
				cell.DetailTextLabel.Text = string.Format("Expires: {0}", localHelper.Program.Expires.ToShortDateString());
				cell.DetailTextLabel.Font = UIFont.SystemFontOfSize(IplayerConst.DetailTextLabelSize);
				cell.DetailTextLabel.TextAlignment = UITextAlignment.Right;
				
				if (DateTime.Now.AddDays(2) > localHelper.Program.Expires)
				{
					cell.DetailTextLabel.TextColor = UIColor.Red;
				} else {
					cell.DetailTextLabel.TextColor = UIColor.LightGray;
				}
				
				
				
				cell.ImageView.Image = localHelper.ThumbnailImageForTableView;
				cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
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
					
					Directory.Delete(Path.Combine(AppDelegate.DocumentsFolder, SelectedProgram.ProgramId), true);
						
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
			
			public MPMoviePlayerViewController moviePlayer = null;
			
			public override void RowSelected (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
			{
				// Navigation logic may go here -- for example, create and push another view controller.
				//var programDetailViewController = new ProgramDetailViewController ();
				//programDetailViewController.SelectedProgram = programs[indexPath.Row];
				//controller.NavigationController.PushViewController(programDetailViewController, true);
				var SelectedProgram = downloads[indexPath.Row];
				Console.WriteLine("playing " + Path.Combine(AppDelegate.DocumentsFolder, SelectedProgram.MoviePath));
				
				if (moviePlayer != null) 
				{
					moviePlayer.Dispose();
					moviePlayer = null;
				}
				
				moviePlayer = new MPMoviePlayerViewController(new NSUrl(Path.Combine(AppDelegate.DocumentsFolder, SelectedProgram.MoviePath), false));
				
				controller.PresentMoviePlayerViewController(moviePlayer);
			}
		}
		
		
		
	}
}
