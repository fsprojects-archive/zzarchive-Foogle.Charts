Foogle Charts
=============

Readme is coming soon!

### Maintainer(s)

- We are seeking a primary active maintainer for this stable repo. Please record your interest by [adding an admin issue](https://github.com/fsprojects/FsProjectsAdmin/issues)

The default maintainer account for projects under "fsprojects" is [@fsgit](https://github.com/fsgit) - F# Community Project Incubation Space (repo management)


Troubleshooting
---------------

On a windows machine, you might encounter problems if you have McAfee anti-virus running.  Specifically "McAfee Agent Activity Log" might be shown when you have McAfee installed.

If you get the following error when trying to display a graph:

	> System.Net.HttpListenerException (0x80004005): The process cannot access the file because it is being used by another process
	   at Microsoft.FSharp.Control.CancellationTokenOps.Start@1234-1.Invoke(Exception e)
	   at <StartupCode$FSharp-Core>.$Control.loop@435-40(Trampoline this, FSharpFunc`2 action)
	   at Microsoft.FSharp.Control.Trampoline.ExecuteAction(FSharpFunc`2 firstAction)
	   at Microsoft.FSharp.Control.TrampolineHolder.Protect(FSharpFunc`2 firstAction)
	   at <StartupCode$FSharp-Core>.$Control.-ctor@520-1.Invoke(Object state)
	   at System.Threading.QueueUserWorkItemCallback.WaitCallback_Context(Object state)
	   at System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
	   at System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
	   at System.Threading.QueueUserWorkItemCallback.System.Threading.IThreadPoolWorkItem.ExecuteWorkItem()
	   at System.Threading.ThreadPoolWorkQueue.Dispatch()
	   at System.Threading._ThreadPoolWaitCallback.PerformWaitCallback()
	Stopped due to error

Open a web browser and navigate to

	http://localhost:8081

If something shows up in the browser window, then there is something running on that port and you must change the port used in the Foogle.Charts code.  The easiest way to do this is:

1. Search for the following line in the code and, replace "8081" with different port

	let serverPath = "http://localhost:8081/" // replace 8081 with different non-used port

2. Rebuild so that all scripts get updated in the bin folder properly

3. (In Visual Studio) reset interactive Session