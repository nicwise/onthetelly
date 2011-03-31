
using System;
using BigTed.OnTheTelly;
using BigTed.OnTheTelly.Database;
using System.Collections.Generic;
using System.Threading;


namespace BigTed.OnTheTelly.Net
{


	/// <summary>
	/// Threaded downloader for the queue.
	/// Spin up a new one, and call start after hooking up the events.
	/// Any event will be called from within the target thread, so InvokeOnMainThread is needed.
	/// </summary>
	public class QueueDownloader
	{

		public List<LocalProgram> Queue { get; set; }
		public QueueDownloader ()
		{
			Queue = new List<LocalProgram>();
		}
		
		public event QueueDownloadStart OnStart;
		public event QueueDownloadProgress OnProgress;
		public event QueueDownloadEnd OnEnd;
		public event QueueDownloadNoMoreItems OnNoMoreItems;
		
	
		
		public void Start() 
		{
			
			ThreadStart tsWorker = new ThreadStart(WorkerThread);  
            new Thread(tsWorker).Start();
		}
		
		public void WorkerThread()
		{
			int processed = 1;
			foreach(LocalProgram program in Queue) 
			{
				
				OnStart(program, Queue.Count - processed);
				
				RemoteProgramHelper remoteProgramHelper = new RemoteProgramHelper(null);
				remoteProgramHelper.OnDownloadProgress += delegate(float amountDone) {
					OnProgress(program, amountDone);
				};
				
				remoteProgramHelper.OnDownloadFinished += delegate() {
					OnEnd(program);
				};
				
				remoteProgramHelper.DownloadLocalProgram(program);
			
				processed ++;
				
			}
			
			OnNoMoreItems();
		}
	}
	
	public delegate void QueueDownloadStart(LocalProgram startedProgram, int queueSize);
	public delegate void QueueDownloadProgress(LocalProgram currentProgram, float progress);
	public delegate void QueueDownloadEnd(LocalProgram finishedProgram);
	public delegate void QueueDownloadNoMoreItems();
}
