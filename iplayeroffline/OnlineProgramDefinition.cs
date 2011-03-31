
using System;

namespace BigTed.OnTheTelly
{


	/// <summary>
	/// Definition for the name, URL and image for each category.
	/// </summary>
	public class OnlineProgramDefinition
	{

		public OnlineProgramDefinition ()
		{
		}
		
		public string Title { get; set; }
		public string FeedUrl { get; set; }
		public string Image { get; set; }
		
		public static OnlineProgramDefinition Children 
		{
			get 
			{
				return new OnlineProgramDefinition() { Title = "Children's", FeedUrl = string.Format(IplayerConst.FeedTemplate, "childrens"), Image = "images/114-balloon.png" };
			}
		}
		
		public static OnlineProgramDefinition Comedy 
		{
			get 
			{
				return new OnlineProgramDefinition() { Title = "Comedy", FeedUrl = string.Format(IplayerConst.FeedTemplate, "comedy"), Image = "images/09-chat2.png" };
			}
		}
		
		public static OnlineProgramDefinition Drama 
		{
			get 
			{
				return new OnlineProgramDefinition() { Title = "Drama", FeedUrl = string.Format(IplayerConst.FeedTemplate, "drama"), Image = "images/41-picture-frame.png" };
			}
		}
		
		public static OnlineProgramDefinition News 
		{
			get 
			{
				return new OnlineProgramDefinition() { Title = "News", FeedUrl = string.Format(IplayerConst.FeedTemplate, "news"), Image = "images/124-bullhorn.png" };
			}
		}
		
		public static OnlineProgramDefinition Entertainment 
		{
			get 
			{
				return new OnlineProgramDefinition() { Title = "Entertainment", FeedUrl = string.Format(IplayerConst.FeedTemplate, "entertainment"), Image = "images/24-gift.png" };
			}
		}
		
		public static OnlineProgramDefinition Factual 
		{
			get 
			{
				return new OnlineProgramDefinition() { Title = "Factual", FeedUrl = string.Format(IplayerConst.FeedTemplate, "factual"), Image = "images/81-dashboard.png" };
			}
		}
		public static OnlineProgramDefinition Films 
		{
			get 
			{
				return new OnlineProgramDefinition() { Title = "Films", FeedUrl = string.Format(IplayerConst.FeedTemplate, "films"), Image = "images/46-movie2.png" };
			}
		}
		
		public static OnlineProgramDefinition LifestyleAndLeisure 
		{
			get 
			{
				return new OnlineProgramDefinition() { Title = "Lifestyle & Leisure", FeedUrl = string.Format(IplayerConst.FeedTemplate, "lifestyle_and_leisure"), Image = "images/113-navigation.png" };
			}
		}
		
		public static OnlineProgramDefinition Music 
		{
			get 
			{
				return new OnlineProgramDefinition() { Title = "Music", FeedUrl = string.Format(IplayerConst.FeedTemplate, "music"), Image = "images/65-note.png" };
			}
		}
		
		public static OnlineProgramDefinition ReligionAndEthics 
		{
			get 
			{
				return new OnlineProgramDefinition() { Title = "Religion & Ethics", FeedUrl = string.Format(IplayerConst.FeedTemplate, "religion_and_ethics"), Image = "images/96-book.png" };
			}
		}
		
		public static OnlineProgramDefinition Sport 
		{
			get 
			{
				return new OnlineProgramDefinition() { Title = "Sport", FeedUrl = string.Format(IplayerConst.FeedTemplate, "sport"), Image = "images/63-runner.png" };
			}
		}
		
		public static OnlineProgramDefinition SignZone 
		{
			get 
			{
				return new OnlineProgramDefinition() { Title = "Sign Zone", FeedUrl = string.Format(IplayerConst.FeedTemplate, "signed"), Image = "images/12-eye.png" };
			}
		}
		
		public static OnlineProgramDefinition AudioDescribed 
		{
			get 
			{
				return new OnlineProgramDefinition() { Title = "Audio Described", FeedUrl = string.Format(IplayerConst.FeedTemplate, "audiodescribed"), Image = "images/120-headphones.png" };
			}
		}
		
		public static OnlineProgramDefinition NorthernIreland 
		{
			get 
			{
				return new OnlineProgramDefinition() { Title = "Northern Ireland", FeedUrl = string.Format(IplayerConst.FeedTemplate, "northern_ireland"), Image = "images/121-lanscape.png" };
			}
		}
		
		public static OnlineProgramDefinition Scotland 
		{
			get 
			{
				return new OnlineProgramDefinition() { Title = "Scotland", FeedUrl = string.Format(IplayerConst.FeedTemplate, "scotland"), Image = "images/121-lanscape.png" };
			}
		}
		
		public static OnlineProgramDefinition Wales 
		{
			get 
			{
				return new OnlineProgramDefinition() { Title = "Wales", FeedUrl = string.Format(IplayerConst.FeedTemplate, "wales"), Image = "images/121-lanscape.png" };
			}
		}
	}
}
