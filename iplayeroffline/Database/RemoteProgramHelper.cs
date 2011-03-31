
using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.IO;
using System.Threading;
using System.Web;
using BigTed.OnTheTelly.Net;
using System.Text.RegularExpressions;
using System.Net;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace BigTed.OnTheTelly.Database
{


	/// <summary>
	/// helper for managing and downloading the shows from iPlayer
	/// </summary>
	public class RemoteProgramHelper
	{
		
		
		public RemoteProgram Program { get; set; }
		
		private static CookieContainer cookieContainer = new CookieContainer();
		
		public RemoteProgramHelper (RemoteProgram theProgram)
		{
			Program = theProgram;
		}
		
		/// <summary>
		/// Use the Atom feed to download a list of shows.
		/// </summary>
		public static List<RemoteProgram> GetRemoteProgramList(string atomUrl, bool doSortAndLimit)
		{
			List<RemoteProgram> list = new List<RemoteProgram>();
			
		
			using (WebClient client = new WebClient())
			{
			
				string atomContent = client.DownloadString(atomUrl);
				XElement root = null;
				try {
					root = XElement.Parse(atomContent);
				} catch (Exception ex)
				{
					return list;
				}
				
				XNamespace atomNS = "http://www.w3.org/2005/Atom";
				XNamespace mediaNS = "http://search.yahoo.com/mrss/";
				
				var items = from item in root.Descendants(atomNS + "entry")
					select new RemoteProgram 
					{
						ProgramId = ExtractProgramId(item.Element(atomNS + "id").Value),
						Name = item.Element(atomNS + "title").Value,
						Updated = DateTime.Parse(item.Element(atomNS + "updated").Value),
						ThumbnailUrl = item.Element(atomNS + "link").Element(mediaNS + "content").Element(mediaNS + "thumbnail").Attribute("url").Value,
						Description = ExtractDescription(item.Element(atomNS + "content").Value)
						
					};
				
				
	
				if (doSortAndLimit) 
				{
					var 	shortList = (from item in items orderby item.Updated descending select item);
					list.AddRange(shortList);
					shortList = null;
				} else {
					list.AddRange(items);
				}
				
				items = null;
				
				
			}
			
			return list;
		}
		
		/// <summary>
		/// Gets the content for a lot of atom feeds. Not used.
		/// </summary>
		public static List<RemoteProgram> GetRemoteProgramList(params string[] atomUrls)
		{
			List<RemoteProgram> list = new List<RemoteProgram>();
			
			foreach(string atomUrl in atomUrls)
			{
				list.AddRange(GetRemoteProgramList(atomUrl, true));
			}
			
			return list;
			
		}
		
		
		/// <summary>
		/// Program IDs look like this:
		/// 
		/// tag:bbc.co.uk,2008:pips:b00rgk0p
		/// 
		/// we just want the last bit.
		/// </summary>
		public static string ExtractProgramId(string longId) 
		{
			string[] elements = longId.Split (':');
			return elements[elements.Length - 1];
		}
		
		/// <summary>
		/// The description is an HTML block, and we just need the last paragraph
		/// </summary>
		public static string ExtractDescription(string desc) 
		{
			// we want the last paragraph
			
			Regex regex = new Regex(@"<p\b[^>]*>(.*?)</p>", RegexOptions.Singleline);
			string res = "";
			foreach(Match match in regex.Matches(desc))
			{
				if (match.Groups.Count > 1) 
				  res = match.Groups[1].Value;
			}
			
			return res.Trim();
		}
		
		/// <summary>
		/// Clean up the thumbnail cache - if something is over 14 days old, remove it, chances are it's not
		/// on iPlayer anymore
		/// </summary>
		public static void CleanupCache() 
		{
			if (!Directory.Exists(Path.Combine(BasePath, "cache")))
			{
				Directory.CreateDirectory(Path.Combine(BasePath, "cache"));
			}
			
			foreach(var file in Directory.GetFiles(Path.Combine(BasePath, "cache"), "*.jpg"))
			{
				FileInfo f = new FileInfo(file);
				if (f.CreationTime < DateTime.Now.AddDays(-14)) 
				{
					f.Delete();
				}
			}
		}
		
		public static void ClearImageCache() 
		{
			if (!Directory.Exists(Path.Combine(BasePath, "cache")))
			{
				Directory.CreateDirectory(Path.Combine(BasePath, "cache"));
			}
			
			foreach(var file in Directory.GetFiles(Path.Combine(BasePath, "cache"), "*.jpg"))
			{
				FileInfo f = new FileInfo(file);
				
				f.Delete();
				
			}
		}
		
		
		private UIImage image = null;
		public UIImage Image 
		{
			get
			{
				if (image == null) 
				{
					if (CachedThumbExists(Program.ProgramId)) 
					{
						
						image = UIImage.FromFileUncached(CachedThumbFullpath(Program.ProgramId));
					} else 
					{
					
						using (NSData imgData = NSData.FromUrl(new NSUrl(Program.ThumbnailUrl))) 
						{
							using (UIImage tempimage = UIImage.LoadFromData(imgData))
							{
								image = ImageHelper.ResizeImage(tempimage, -1, 35);
								NSData data = image.AsJPEG();
								NSError error;
								data.Save(CachedThumbFullpath(Program.ProgramId), false, out error);
								
							}
						}
					}
				}
				return image;
					
			}
		}
		
		
		public void GetImageIntoCache (NSAction doWhenDone)
		{
			if (image == null) 
			{
				if (CachedThumbExists(Program.ProgramId)) 
				{
					
					image = UIImage.FromFileUncached(CachedThumbFullpath(Program.ProgramId));
				} else 
				{
				
					using (NSData imgData = NSData.FromUrl(new NSUrl(Program.ThumbnailUrl))) 
					{
						using (UIImage tempimage = UIImage.LoadFromData(imgData))
						{
							image = ImageHelper.ResizeImage(tempimage, -1, 35);
							NSData data = image.AsJPEG();
							NSError error;
							data.Save(CachedThumbFullpath(Program.ProgramId), false, out error);
							
						}
					}
				}
			}
			
			doWhenDone();
			
			
		}

		
		
		public UITableViewCell TempCell = null;
		
		public bool HasImage 
		{
			get
			{
				if (image != null) return true;
				return (CachedThumbExists(Program.ProgramId));
				
			}
		}
		
		
		public bool CachedThumbExists(string id)
		{
			return File.Exists(CachedThumbFullpath(id));
		}
		
		public string CachedThumbFullpath(string id) 
		{
			if (!Directory.Exists(Path.Combine(BasePath, "cache")))
			{
				Directory.CreateDirectory(Path.Combine(BasePath, "cache"));
			}
			return Path.Combine(BasePath, "cache/" + id + ".jpg");
		}
		
		public string FullsizeImageUrl { get; set; }
		
		private UIImage fullsizeImage = null;
		
		/// <summary>
		/// Grabs the fullsized image (640x320) for use, as it looks a lot better than the smaller
		/// ones on screen.
		/// </summary>
		public UIImage GetFullsizeImage(System.Drawing.RectangleF bounds)
		{
			
			if (fullsizeImage == null) 
			{
				string sourceImageUrl = Program.ThumbnailUrl;
				if (!String.IsNullOrEmpty(FullsizeImageUrl)) sourceImageUrl = FullsizeImageUrl;
				
				using (NSData imgData = NSData.FromUrl(new NSUrl(sourceImageUrl))) 
				{
					using (UIImage tempimage = UIImage.LoadFromData(imgData))
					{
						fullsizeImage = ImageHelper.ResizeImage(tempimage, bounds.Width, -1);
					}
				}
			}
			return fullsizeImage;
			
		}
		
		public static UIImage GetFullsizeImageFromFile(string filename, System.Drawing.RectangleF bounds)
		{
		
			UIImage fullsizeImage = null;
			
			
			using (UIImage tempimage = UIImage.FromFileUncached(Path.Combine(BasePath, filename)))
			{
				fullsizeImage = ImageHelper.ResizeImage(tempimage, bounds.Width, -1);
			
			}
			
			return fullsizeImage;
			
		}
		
		/// <summary>
		/// Once we drill into a program, we can go to the network
		/// and get more info from the playlist.
		/// </summary>
		public void PopulateExtendedFields()
		{
			using (WebClient client = new WebClient())
			{
			
				string url = string.Format(IplayerConst.PlaylistUrlTemplate, Program.ProgramId);
				string atomContent = client.DownloadString(url);
				
				
				XElement root = XElement.Parse(atomContent);
				
				
				XNamespace playlistNS = IplayerConst.PlaylistNS;

				var items = from item in root.Elements(playlistNS + "summary")
					select new
					{
						Description = item.Value
					};
				
				foreach(var prog in items) 
				{
					Program.Description = prog.Description;
				}
				
				
				
			    var items2 = from item in 
					  root.Elements(playlistNS + "link")
						where item.Attribute("rel").Value == "holding"
					select new { LargeImageUrl = item.Attribute("href").Value };
				
				foreach(var prog2 in items2) 
				{
					this.FullsizeImageUrl = prog2.LargeImageUrl;
				}
				
				
					
				
			}
		}
		
		/// <summary>
		/// Get the streaming URL.
		/// this is mostly taken from the iPlayer downloader (iplayer-dl)
		/// 
		/// Auto accepts the Over18/Over16 dialogs.
		/// 
		/// 
		/// </summary>
		public string StreamingUrl 
		{
			get
			{
				if (String.IsNullOrEmpty(Program.StreamingUrl)) 
				{
					string pageUrl = string.Format(IplayerConst.MobileUrlTemplate, Program.ProgramId);
			
					
					
					using (CookieAwareWebClient client = new CookieAwareWebClient())
					{
						client.CookieContainer = cookieContainer;
						
						client.Headers["User-Agent"] = IplayerConst.IPHONE_UA;
						
						string page = client.DownloadString(pageUrl);
					
						if (page.Contains("isOver")) 
						{
							
							Regex overAge = new Regex(@"isOver\d+");
							
							var overAgeMatches = overAge.Matches(page);
							
							if (overAgeMatches.Count > 0) 
							{
								using (CookieAwareWebClient webclient2 = new CookieAwareWebClient())
								{
									webclient2.CookieContainer = cookieContainer;
						
									webclient2.Headers["User-Agent"] = IplayerConst.IPHONE_UA;
									
									string param = string.Format("form=guidanceprompt&{0}=1", overAgeMatches[0].Value);
									webclient2.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
									page = webclient2.UploadString(pageUrl, "POST", param);
									
								}
								
							}
						}
						
						Regex rx = new Regex("<embed[^>]*?href=\"(http://[^\"]+)");
						
						var matches = rx.Matches(page);
						
						if (matches.Count > 0)
						{
							Program.StreamingUrl = matches[0].Groups[1].Captures[0].Value;
					}
					}
				}
					
					
				return Program.StreamingUrl;
			}
		}

		
		public string OutputFilename
		{
			get
			{
				return string.Format("{0}/{0}.mp4", Program.ProgramId);
			}
		}
		
		public static string BasePath 
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
				return Program.ProgramId;
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
		
		/// <summary>
		/// Prepares a program for going into the queue
		/// 
		/// Gets the highres image, and the streaming url.
		/// Fails if we can't get the streaming url (which we then download from)
		/// </summary>
		public bool PrepareForQueue() 
		{
			
			if (!DownloadPathExists) 
			{
				Directory.CreateDirectory(Path.Combine(BasePath,DownloadFilepath));
			}
			
			
			string imageUrl = Program.ThumbnailUrl;
			if (!string.IsNullOrEmpty(FullsizeImageUrl)) imageUrl = FullsizeImageUrl;
			Program.ThumbnailPath = DownloadImageToFile(Path.Combine(BasePath,DownloadFilepath), Program.ProgramId, imageUrl);
			Program.ThumbnailPath = Program.ProgramId + "/" + Program.ThumbnailPath;
			string s = StreamingUrl;
			Program.MoviePath = OutputFilename;
			
			return !string.IsNullOrEmpty(s);
		}
		
		/// <summary>
		/// Downloads a program - does not use threads, so should be called from in a off-main-ui thread.
		/// 
		/// Called from the QueueDownloader, usually.
		/// </summary>
		/// <param name="program">
		/// A <see cref="LocalProgram"/>
		/// </param>
		public void DownloadLocalProgram(LocalProgram program) 
		{
			Program = new RemoteProgram();
			Program.ProgramId = program.ProgramId;
			Program.Name = program.Name;
			Program.Description = program.Description;
			Program.ThumbnailPath = program.ThumbnailPath;
			Program.MoviePath = program.MoviePath;
			Program.StreamingUrl = program.StreamingUrl;
			Program.ThumbnailUrl = program.ThumbnailUrl;
			
			
			if (!DownloadPathExists) 
			{
				Directory.CreateDirectory(Path.Combine(BasePath,DownloadFilepath));
			}
			
			
			string imageUrl = Program.ThumbnailUrl;
			if (!string.IsNullOrEmpty(FullsizeImageUrl)) imageUrl = FullsizeImageUrl;
			
			Program.ThumbnailPath = DownloadImageToFile(Path.Combine(BasePath,DownloadFilepath), Program.ProgramId, imageUrl);
			Program.ThumbnailPath = Program.ProgramId + "/" + Program.ThumbnailPath;
			
			Program.MoviePath = OutputFilename;
			
			if (string.IsNullOrEmpty(StreamingUrl)) 
			{
				OnDownloadFinished();
			} else {
				DownloadToFile(StreamingUrl, Path.Combine(BasePath,Program.MoviePath), OnDownloadProgress, OnDownloadFinished);
			}
		}
		
		/// <summary>
		/// Threaded download. Not used anymore.
		/// </summary>
		public void Download()
		{
			DoDownloadProgress(0);
			
			ThreadStart tsWorker = new ThreadStart(WorkerThread);  
            new Thread(tsWorker).Start();
		}
		
		[Export("WorkerThread")]
		public void WorkerThread() 
		{
			using (NSAutoreleasePool autoReleasePool = new NSAutoreleasePool()) 
			{
				if (!DownloadPathExists) 
				{
					Directory.CreateDirectory(Path.Combine(BasePath,DownloadFilepath));
				}
				
				Program.ThumbnailPath = DownloadImageToFile(Path.Combine(BasePath,DownloadFilepath), Program.ProgramId, Program.ThumbnailUrl);
				Program.ThumbnailPath = Program.ProgramId + "/" + Program.ThumbnailPath;
				
				Program.MoviePath = OutputFilename;
				
				DownloadToFile(StreamingUrl, Path.Combine(BasePath,Program.MoviePath), OnDownloadProgress, OnDownloadFinished);
			}
		}
		
		
		/// <summary>
		/// Chunked download, based on the iplayer-dl code.
		/// Downloads a megabyte between reporting progress.
		/// 
		/// Resumes downloads if they are partially down.
		/// </summary>
		private void DownloadToFile (string streamingUrl, string outputFilename, DownloadProgress OnDownloadProgress, DownloadFinished OnDownloadFinished)
		{
			
			Console.WriteLine("Downloading {0}\r\nto {1}.", streamingUrl, outputFilename);
			
			int fileSize = GetTotalFilesize(streamingUrl);
			
			
			
				
			
			int bufferSize = 1 * 1024 * 1024; //half a meg
			int bytesTransfered = 0;
			bool lastOne = false;
			
			//int debugIterations = 0;
			
			//might need to change this if/when we do pause/resume/cancel etc?
			FileStream fs;
			if (File.Exists(outputFilename)) 
			{
				Console.WriteLine("resuming download");
				fs = new FileStream(outputFilename, FileMode.Append);
				bytesTransfered = (int)fs.Length;
			} else {
				fs = new FileStream(outputFilename, FileMode.Create);
			}
			
			using (fs) 
			{
			
				while (lastOne == false && fileSize > 0) 
				{
					
					//debugIterations ++;
					
					//if (debugIterations == 14) 
					//{
					//	lastOne = true;
					//	Console.WriteLine("Exting the downloader as we are using the debug version!!");
					//}
					
				    HttpWebRequest req = new HttpWebRequest(new Uri(streamingUrl));
					
				
					req.CookieContainer = cookieContainer;
					req.UserAgent = IplayerConst.QT_UA;
				
					int endByte = bytesTransfered + bufferSize;
					if (endByte > fileSize) 
					{
						endByte = fileSize;
						lastOne = true;
					}
					
					req.AddRange("bytes", bytesTransfered, endByte);
					req.Accept = "*/*";
					
					Console.WriteLine("getting block: {0} / {1} of {2} ", bytesTransfered, endByte, fileSize);
					using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
					{
						
						byte[] buffer = null;
						using (Stream s = resp.GetResponseStream())
						{
							buffer = new byte[bufferSize];
							
							while (true)
							{
								int bytesRead = s.Read(buffer, 0, bufferSize);
								if (bytesRead == 0) break;
								
								fs.Write(buffer, 0, bytesRead);
								bytesTransfered += bytesRead; 
							}
							resp.Close();
							fs.Flush();
							
							buffer = null;
							
						}
						
						if (OnDownloadProgress != null) 
						{
							float f = (float)bytesTransfered / (float)fileSize;
							OnDownloadProgress(f);
						}
					}
					
				}
				fs.Flush();
				fs.Close();
			}
			
			if (OnDownloadFinished != null) 
			{
				OnDownloadFinished();
			}
			
			Console.WriteLine("download is done");
			
		}
		
		/// <summary>
		/// Finds the total size by asking for the range, and how big the file is.
		/// </summary>
		public int GetTotalFilesize(string streamingUrl)
		{
			
			HttpWebRequest req = new HttpWebRequest(new Uri(streamingUrl));
		
			
			req.CookieContainer = cookieContainer;
			req.UserAgent = IplayerConst.QT_UA;
		
			Console.WriteLine("getting range");
			
			req.AddRange("bytes", 0, 1);
			req.Accept = "*/*";
			
			int fileSize = 0;
			
			using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
			{
				using (StreamReader sr = new StreamReader(resp.GetResponseStream())) 
				{
					string rangeString = resp.Headers["content-range"];
					try {
					string[] ranges = rangeString.Split('/');
					fileSize = Int32.Parse(ranges[1]);	
					resp.Close();
					} catch {
						Console.WriteLine("range threw and error: " + rangeString);
						fileSize = 0;
					}
				}
			}
				
			return fileSize;
			
		}

		public static string DownloadImageToFile (string downloadFilepath, string id, string thumbnailUrl)
		{
			string thumbfilename = "";
			thumbfilename = id + ".jpg";
			
			using (WebClient client = new WebClient()) 
			{
			
				Console.WriteLine("Getting thumbnail: {0} to {1}", thumbnailUrl, Path.Combine(downloadFilepath, thumbfilename));
				client.DownloadFile(thumbnailUrl, Path.Combine(downloadFilepath, thumbfilename));
			}
		
			return thumbfilename;
		}
	}
	
	public delegate void DownloadProgress(float amountDone);
	public delegate void DownloadFinished();
}
