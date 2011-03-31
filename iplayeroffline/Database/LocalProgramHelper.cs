
using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.IO;
using System.Threading;

namespace BigTed.OnTheTelly.Database
{


	/// <summary>
	/// helper for managing the locally downloaded shows
	/// </summary>
	public class LocalProgramHelper
	{

		public LocalProgram Program { get; set; }
		
		public LocalProgramHelper (LocalProgram theProgram)
		{
			Program = theProgram;
		}
		
		private UIImage image = null;
		public UIImage ThumbnailImageForTableView 
		{
			get
			{
				if (image == null) 
				{
					using (UIImage tempimage = UIImage.FromFileUncached(Path.Combine(BasePath, Program.ThumbnailPath)))
					{
						image = ImageHelper.ResizeImage(tempimage, -1, 35);
					}
				}
				return image;
			}
		}
		
		public string BasePath 
		{
			get { return Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments); }
		}
		
		public static LocalProgram LocalProgramFromRemoteProgram(RemoteProgram remoteProgram)
		{
			
			LocalProgram program = new LocalProgram();
			program.ProgramId = remoteProgram.ProgramId;
			program.Name = remoteProgram.Name;
			program.Description = remoteProgram.Description;
			program.ThumbnailPath = remoteProgram.ThumbnailPath;
			program.MoviePath = remoteProgram.MoviePath;
			program.State = "D";
			program.Downloaded = DateTime.Now;
			program.Expires = DateTime.Now.AddDays(14);
			program.StreamingUrl = remoteProgram.StreamingUrl;
			program.ThumbnailUrl = remoteProgram.ThumbnailUrl;
				
				
			return program;
			
		}
	}
}
