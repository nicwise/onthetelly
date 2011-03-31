
using MonoTouch.UIKit;
using System;
using MonoTouch.Foundation;
using BigTed.OnTheTelly.Net;
using System.Collections.Generic;
using BigTed.OnTheTelly.Database;
using System.Threading;

namespace BigTed.OnTheTelly
{
	/// <summary>
	/// View controller for the online programs - ie ones which are not on the device.
	/// </summary>
	partial class OnlineProgramViewController : UITableViewController
	{
		public OnlineProgramViewController() : base() {}
		public OnlineProgramViewController (IntPtr handle) : base(handle)
		{
		}

		public OnlineProgramDefinition ProgramDefinition = null;
		
		public static UINavigationController GetNavigationController(OnlineProgramDefinition programDefinition)
		{
			OnlineProgramViewController svc = new OnlineProgramViewController();
			svc.ProgramDefinition = programDefinition;
			
			UINavigationController navController = new UINavigationController();
			navController.PushViewController(svc, false);
			navController.NavigationBar.BarStyle = UIBarStyle.Black;
			navController.TopViewController.Title = programDefinition.Title;
			navController.TabBarItem = new UITabBarItem(programDefinition.Title, UIImage.FromFile(programDefinition.Image), 0);
			
			return navController;
			
		}
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			//Show an edit button
			//NavigationItem.RightBarButtonItem = EditButtonItem;
			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
			this.TableView.Source = new DataSource (this);
			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
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
			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
			(TableView.Source as DataSource).LoadData();
			TableView.ReloadData();
			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
		}
		
		
		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			
		}
		
		
		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
			
		
		}
		

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
			
			(TableView.Source as DataSource).ClearData();
			
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
			OnlineProgramViewController controller;

			List<RemoteProgram> programs = null;
			public DataSource (OnlineProgramViewController controller)
			{
				this.controller = controller;
				
				
			}
			
			public void LoadData() 
			{
				if (programs == null) 
				{
					programs = RemoteProgramHelper.GetRemoteProgramList(controller.ProgramDefinition.FeedUrl);
				}
			}
			
			public void ClearData()
			{
				if (programs != null)
				{
					programs.Clear();
					programs = null;
				}
			}

			public override int NumberOfSections (UITableView tableView)
			{
				return 1;
			}

			// Customize the number of rows in the table view
			public override int RowsInSection (UITableView tableview, int section)
			{
				if (programs == null) {
					Console.WriteLine("nothign here: {0}", controller.ProgramDefinition.Title);
					return 0;
				}
				return programs.Count;
			}
			
			public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
			{
				return 50;
			}


			// Customize the appearance of table view cells.
			public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
			{
				string cellIdentifier = "Cell";
				var cell = tableView.DequeueReusableCell (cellIdentifier);
				if (cell == null) {
					cell = new UITableViewCell (UITableViewCellStyle.Subtitle, cellIdentifier);
				}
				
				
				RemoteProgramHelper program = new RemoteProgramHelper(programs[indexPath.Row]);
				
				
				
				cell.TextLabel.Text = program.Program.Name;
				
				
				cell.DetailTextLabel.Text = program.Program.Description;
				cell.DetailTextLabel.Font = UIFont.SystemFontOfSize(IplayerConst.DetailTextLabelSize);
				
				
				if (program.DownloadedFileExists) 
				{
					cell.Accessory = UITableViewCellAccessory.Checkmark;
				} else {
					cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
				}
				
				cell.ImageView.Alpha = 1.0f;
				GetImageForCell(cell, program);
				
				//cell.ImageView.Image = program.Image;
				return cell;
			}
			
			
			public void GetImageForCell(UITableViewCell cell, RemoteProgramHelper program)
			{
				if (program.HasImage)
				{
					cell.ImageView.Image = program.Image;
					cell.ImageView.Alpha = 1.0f;
					return;
				}
				
				program.TempCell = cell;
				SetTempImage(program);
				ThreadPool.QueueUserWorkItem(RequestImage, program);
				
				
			}
			
			public void RequestImage(object state) 
			{
				RemoteProgramHelper program = state as RemoteProgramHelper;
				
				program.GetImageIntoCache(delegate { 
					InvokeOnMainThread( delegate {
						DisplayImage(program); 
					}); 
				});
			}
			
			public void SetTempImage(RemoteProgramHelper program)
			{
				program.TempCell.ImageView.Image = ImageHelper.TempImage;
			}
			
			public void DisplayImage(RemoteProgramHelper program)
			{
				
				program.TempCell.ImageView.Image = program.Image;
				
				
				
				//UIView.BeginAnimations("imageThumbnailTransitionIn");
				//UIView.SetAnimationDuration(0.5f);

				
				
				program.TempCell.ImageView.Alpha = 1.0f;
				program.TempCell.SetNeedsDisplay();
				//UIView.CommitAnimations();
				
				
				
				program.TempCell = null;
			}

			/*
			// Override to support conditional editing of the table view.
			public override bool CanEditRow (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
			{
				// Return false if you do not want the specified item to be editable.
				return true;
			}
			*/
			/*
			// Override to support editing the table view.
			public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, MonoTouch.Foundation.NSIndexPath indexPath)
			{
				if (editingStyle == UITableViewCellEditingStyle.Delete) {
					controller.TableView.DeleteRows (new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
				} else if (editingStyle == UITableViewCellEditingStyle.Insert) {
					// Create a new instance of the appropriate class, insert it into the array, and add a new row to the table view.
				}
			}
			*/
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
				
				Console.WriteLine("loading the sub view");
				
				var programDetailViewController = new ProgramDetailViewController ();
				programDetailViewController.SelectedProgram = new RemoteProgramHelper(programs[indexPath.Row]);
				controller.NavigationController.PushViewController(programDetailViewController, true);
			}
		}
	}
	
}
