
using System;

namespace BigTed.OnTheTelly
{


	/// <summary>
	/// iPlayer related constants.
	/// Lots of this comes from iplayer-dl or the iplayer website.
	/// iplayer-dl: http://po-ru.com/projects/iplayer-downloader/
	/// </summary>
	public static class IplayerConst
	{
		public static string PlaylistUrlTemplate = "http://www.bbc.co.uk/iplayer/playlist/{0}";
		public static string MobileUrlTemplate = "http://www.bbc.co.uk/mobile/iplayer/episode/{0}";
		public static string PlaylistNS = "http://bbc.co.uk/2008/emp/playlist";
		public static string IPHONE_UA  = "Mozilla/5.0 (iPhone; U; CPU iPhone OS 3_1_2 like Mac OS X; en-us) AppleWebKit/528.18 (KHTML, like Gecko) Version/4.0 Mobile/7D11 Safari/528.16";
		public static string QT_UA = "Apple iPhone v1.1.4 CoreMedia v1.0.0.4A102";
		
		public static string FeedTemplate = "http://feeds.bbc.co.uk/iplayer/categories/{0}/tv/list";
		public static string LifestyleFeedUrl = "http://feeds.bbc.co.uk/iplayer/categories/lifestyle_and_leisure/tv/list";
		public static string NewsFeedUrl = "http://feeds.bbc.co.uk/iplayer/categories/news/tv/list";
		public static string FactualFeedUrl = "http://feeds.bbc.co.uk/iplayer/categories/factual/tv/list";
		
		public static string[] Categories = {
			@"lifestyle_and_leisure",
			@"drama",
			@"entertainment",
			@"factual",
			@"films",
			@"news"
		};
		
		public static string HighlightsFeedUrl = "http://feeds.bbc.co.uk/iplayer/highlights/tv";
		public static string MostPopularFeedUrl = "http://feeds.bbc.co.uk/iplayer/popular/tv/list";
		
		public static float DetailTextLabelSize = 13;
		public static float TextLabelSize = 15;
		
	}
}
