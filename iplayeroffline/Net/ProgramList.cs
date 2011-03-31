
using System;
using System.Net;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

namespace BigTed.OnTheTelly.Net
{

	/*

	public class ProgramList
	{
		
		private string atomUrl;

		public ProgramList (string atomUrl)
		{
			this.atomUrl = atomUrl;
		}
		
		public List<Program> GetPrograms() 
		{
			List<Program> list = new List<Program>();
			
			WebClient client = new WebClient();
			
			string atomContent = client.DownloadString(atomUrl);
			
			
			XElement root = XElement.Parse(atomContent);
			
			XNamespace atomNS = "http://www.w3.org/2005/Atom";
			XNamespace mediaNS = "http://search.yahoo.com/mrss/";
			
			var items = from item in root.Descendants(atomNS + "entry")
				
				select new Program 
			{
				Id = item.Element(atomNS + "id").Value,
				Name = item.Element(atomNS + "title").Value,
				Updated = DateTime.Parse(item.Element(atomNS + "updated").Value),
				Thumbnail = item.Element(atomNS + "link").Element(mediaNS + "content").Element(mediaNS + "thumbnail").Attribute("url").Value
			};
			
			var shortList = (from item in items orderby item.Updated descending select item).Take(20);
			
			
			list.AddRange(shortList);
			
			shortList = null;
			items = null;
			
			return list;
		}
		
		public static void PopulateProgram(Program program) 
		{
			CookieAwareWebClient client = new CookieAwareWebClient();
			//client.CookieContainer = cookieContainer;
			
			//client.Headers["User-Agent"] = IPHONE_UA;
		
			
			string url = string.Format("http://www.bbc.co.uk/iplayer/playlist/{0}", program.Id);
			string atomContent = client.DownloadString(url);
			
			XElement root = XElement.Parse(atomContent);
			
			XNamespace playlistNS = "http://bbc.co.uk/2008/emp/playlist";
			
			
			var items = from item in root.Elements(playlistNS + "summary")
				select new
				{
					Summary = item.Value
				};
			
			foreach(var prog in items) 
			{
				program.Summary = prog.Summary;
			}
		}
		
		
		
		static CookieContainer cookieContainer = new CookieContainer(); 
		
		public static string GetStreamingUrl(string id)
		{
			
				
			
		}
		
		private static string[] allUrls = 
		{
			@"lifestyle_and_leisure",
			@"drama",
			@"entertainment",
			@"factual",
			@"films",
			@"news"
			
		};
		
		private static string template = "http://feeds.bbc.co.uk/iplayer/categories/{0}/tv/list";
		public static string LifestyleUrl = "http://feeds.bbc.co.uk/iplayer/categories/lifestyle_and_leisure/tv/list";
		public static string NewsUrl = "http://feeds.bbc.co.uk/iplayer/categories/news/tv/list";
		
		public static List<Program> GetAll()
		{
			Dictionary<string, Program> bigList = new Dictionary<string, Program>();
			foreach(string atomUrl in allUrls)
			{
				Console.WriteLine("getting " + atomUrl);
				
				ProgramList pl = new ProgramList(string.Format(template, atomUrl));
				
				foreach(Program prog in pl.GetPrograms()) 
				{
					if (!bigList.ContainsKey(prog.Id))
					{
						bigList.Add(prog.Id, prog);
					}
				}
			}
			
			return new List<Program>(bigList.Values);
			
		}
		
		static string[] termList = 
		{
			@"lark rise",
			@"delicious miss dahl",
			@"drow your own drugs",
			@"bang goes the theory",
			@"museum of life",
			@"panorama",
			@"top gear",
			@"doctor who",
			@"torchwood"
		};

		public static List<Program> Filter (List<Program> programs)
		{
			Dictionary<string, Program> bigList = new Dictionary<string, Program>();
			foreach(string term in termList)
			{
				var items = from item in programs where item.Name.ToLower().Contains(term)  select item;
				
				foreach(Program prog in items) 
				{
					if (!bigList.ContainsKey(prog.Id))
					{
						bigList.Add(prog.Id, prog);
					}
				}
			}
			
			var sortedList = from item in bigList.Values orderby item.Updated descending select item;
			
			return new List<Program>(sortedList);
			
			
		}
		
		private static void SetDefaultHeaders(WebHeaderCollection headers) 
		{
			headers["Accept"] = "* / *";
			headers["Accept-Language"] = "en";
			//client.Headers["Connection"] = "keep-alive";
			headers["Pragma"] = "no-cache";
			
		}


		
	}
	*/
}
