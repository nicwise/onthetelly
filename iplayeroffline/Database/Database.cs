using System;
using System.Collections.Generic;
using System.Linq;
using Path = System.IO.Path;
using SQLite;

/*
 * Reuse of the SqLite stuff as used in MonoSpace2
 * http://conceptdev.blogspot.com/2009/10/monotouch-for-monospace.html
 */

namespace BigTed.OnTheTelly.Database
{
	[MonoTouch.Foundation.Preserve(AllMembers=true)]
	public class LocalProgram
	{
		[PrimaryKey, AutoIncrement]
		public int Id {get; set; }
		public string ProgramId {get;set;}
		public string Name {get;set;}
		public string Description { get; set; }
		public string ThumbnailPath {get;set;}
		public string MoviePath {get;set;}
		public string State { get; set; }
		public DateTime Downloaded { get; set; }
		public DateTime Expires { get; set; }
		public string StreamingUrl { get; set; }
		public string ThumbnailUrl { get; set; }
	}
	
	[MonoTouch.Foundation.Preserve(AllMembers=true)]
	public class RemoteProgram
	{
		[PrimaryKey, AutoIncrement]
		public int Id {get; set; }
		public string ProgramId {get;set;}
		public string Name {get;set;}
		public string ThumbnailPath {get;set;}
		public string MoviePath { get; set; }
		public string Description { get; set; }
		public DateTime Updated { get; set; }
		public string StreamingUrl { get; set; }
		public string ThumbnailUrl { get; set; }
	}

	
	
	
	
	public class Database : SQLiteConnection
	{
		public int LastSeenDownloadCount { get; set; }
		public Database (string path) : base (path)
		{
			CreateTable<LocalProgram>();
			CreateTable<RemoteProgram>();
		}
		public List<LocalProgram> GetLocalDownloadedPrograms ()
        {
            List<LocalProgram> progs = Query<LocalProgram> ("select * from LocalProgram where State = 'D'").ToList();
			LastSeenDownloadCount = progs.Count;
			return progs;
        }
		
		public List<LocalProgram> GetLocalQueuedPrograms ()
        {
                return Query<LocalProgram> ("select * from LocalProgram where State = 'Q'").ToList();
        }
		
		public void DeleteLocalProgramById(int id) 
		{
			LastSeenDownloadCount--;
			Execute("delete from LocalProgram where Id = ?", id);
		}
		
		public void DeleteRemoteProgramById(int id) 
		{
			Execute("delete from RemoteProgram where Id = ?", id);
		}
		
		public void MarkProgramAsDownloaded(int id) 
		{
			Execute("update LocalProgram set State = 'D' where Id = ?", id);
		}
		
		public LocalProgram AddLocalProgram(LocalProgram program) 
		{
			int newid = Insert(program);	
			program.Id = newid;
			return program;
		}
		
		
	}
}
