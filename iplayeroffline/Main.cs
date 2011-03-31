
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.IO;
using BigTed.OnTheTelly.Database;

namespace BigTed.OnTheTelly
{
	public class Application
	{
		static void Main (string[] args)
		{
			UIApplication.Main (args);
		}
	}

	// The name AppDelegate is referenced in the MainWindow.xib file.
	public partial class AppDelegate : UIApplicationDelegate
	{
		
		public static Database.Database SessionDatabase { get; set; }
		
		public static string DocumentsFolder { 
			get { return Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments); }
		}
		
		// This method is invoked when the application has loaded its UI and its ready to run
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			
			//File.Delete(Path.Combine (DocumentsFolder, "onthetele.db"));
			SessionDatabase = new Database.Database(Path.Combine (DocumentsFolder, "onthetele.db") );
			
			//add the removing of old stuff in here
			
			RemoteProgramHelper.ClearImageCache();
			RemoteProgramHelper.CleanupCache();
			
			List<UIViewController> viewControllers = new List<UIViewController>(tabbarController.ViewControllers);
			
			// add in a view controller for each category
			viewControllers.Add(OnlineProgramViewController.GetNavigationController(OnlineProgramDefinition.News));
			viewControllers.Add(OnlineProgramViewController.GetNavigationController(OnlineProgramDefinition.Children));
			viewControllers.Add(OnlineProgramViewController.GetNavigationController(OnlineProgramDefinition.Comedy));
			viewControllers.Add(OnlineProgramViewController.GetNavigationController(OnlineProgramDefinition.Drama));
			viewControllers.Add(OnlineProgramViewController.GetNavigationController(OnlineProgramDefinition.Entertainment));
			viewControllers.Add(OnlineProgramViewController.GetNavigationController(OnlineProgramDefinition.Factual));
			viewControllers.Add(OnlineProgramViewController.GetNavigationController(OnlineProgramDefinition.Films));
			viewControllers.Add(OnlineProgramViewController.GetNavigationController(OnlineProgramDefinition.LifestyleAndLeisure));
			viewControllers.Add(OnlineProgramViewController.GetNavigationController(OnlineProgramDefinition.Music));
			viewControllers.Add(OnlineProgramViewController.GetNavigationController(OnlineProgramDefinition.ReligionAndEthics));
			viewControllers.Add(OnlineProgramViewController.GetNavigationController(OnlineProgramDefinition.Sport));
			viewControllers.Add(OnlineProgramViewController.GetNavigationController(OnlineProgramDefinition.SignZone));
			viewControllers.Add(OnlineProgramViewController.GetNavigationController(OnlineProgramDefinition.AudioDescribed));
			viewControllers.Add(OnlineProgramViewController.GetNavigationController(OnlineProgramDefinition.NorthernIreland));
			viewControllers.Add(OnlineProgramViewController.GetNavigationController(OnlineProgramDefinition.Scotland));
			viewControllers.Add(OnlineProgramViewController.GetNavigationController(OnlineProgramDefinition.Wales));
			
			
			
			tabbarController.ViewControllers = viewControllers.ToArray();
			tabbarController.MoreNavigationController.NavigationBar.BarStyle = UIBarStyle.Black;
			tabbarController.CustomizableViewControllers = null;
			
			window.AddSubview (tabbarController.View);
			window.MakeKeyAndVisible ();
			
			return true;
		}

		// This method is required in iPhoneOS 3.0
		public override void OnActivated (UIApplication application)
		{
		}
		
		public override void WillTerminate (UIApplication application)
		{
			// clean up when we die.
			
			Console.WriteLine("terminating");
			UIApplication.SharedApplication.ApplicationIconBadgeNumber = SessionDatabase.LastSeenDownloadCount;
			SessionDatabase.Dispose();
			SessionDatabase = null;
			
			UIApplication.SharedApplication.IdleTimerDisabled = false;
		}
		
	}
		
		
			
	}
