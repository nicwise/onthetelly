
using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.IO;
using System.Threading;


namespace BigTed.OnTheTelly.Net
{

	/*
	public class Program
	{

		private string id;
		public string Id {
			get { return id; }
			set {
				string[] elements = value.Split (':');
				
				id = elements[elements.Length - 1];
				
			}
		}
		public string Name { get; set; }
		public DateTime Updated { get; set; }

		public string Thumbnail { get; set; }

		
		
		
		public string OutputFilename
		{
			get
			{
				return string.Format("{0}/{0}.mp4", Id);
			}
		}
		
		public string BasePath 
		{
			get { return Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments); }
		}
		public bool DownloadedFileExists
		{
			get 
			{
				return File.Exists(Path.Combine (BasePath, OutputFilename));
			}
		}
		
		public string DownloadFilepath
		{
			get
			{
				return Id;
			}
		}
		
		public bool DownloadPathExists
		{
			get 
			{
				return Directory.Exists(Path.Combine(BasePath,DownloadFilepath));
			}
		}
		
		public event DownloadProgress OnDownloadProgress;
		public event DownloadFinished OnDownloadFinished;
		
		
		public void DoDownloadProgress(float amountDone) 
		{
			if (OnDownloadProgress != null)
			{
				OnDownloadProgress(amountDone);
			}
		}
		
		
		
		
		public void DownloadProgram()
		{
			DoDownloadProgress(0);
			
			ThreadStart tsWorker = new ThreadStart(workerThread);  
            new Thread(tsWorker).Start();
			
			
		}
		
		public string LocalImage { get; set; }
		
		public void workerThread() 
		{
			using (NSAutoreleasePool autoReleasePool = new NSAutoreleasePool()) 
			{
				if (!DownloadPathExists) 
				{
					Directory.CreateDirectory(Path.Combine(BasePath,DownloadFilepath));
				}
				
				LocalImage = ProgramList.DownloadImageToFile(Path.Combine(BasePath,DownloadFilepath), Id, Thumbnail);
				LocalImage = Id + "/" + LocalImage;
				
				ProgramList.DownloadToFile(StreamingUrl, Path.Combine(BasePath,OutputFilename), OnDownloadProgress, OnDownloadFinished);
			}
		}
		
		
		
	}
	*/
	
}
