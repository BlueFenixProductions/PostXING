#define TRACE
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;

namespace PostXING.Controls;

[ToolboxBitmap(typeof(BackgroundWorker), "bw.bmp")]
public class BackgroundWorker : Component
{
	private bool _cancelPending;

	private bool _reportsProgress;

	private bool _supportsCancellation;

	public bool WorkerSupportsCancellation
	{
		get
		{
			lock (this)
			{
				return _supportsCancellation;
			}
		}
		set
		{
			lock (this)
			{
				_supportsCancellation = value;
			}
		}
	}

	public bool WorkerReportsProgress
	{
		get
		{
			lock (this)
			{
				return _reportsProgress;
			}
		}
		set
		{
			lock (this)
			{
				_reportsProgress = value;
			}
		}
	}

	public bool CancellationPending
	{
		get
		{
			lock (this)
			{
				return _cancelPending;
			}
		}
	}

	public event DoWorkEventHandler DoWork;

	public event ProgressChangedEventHandler ProgressChanged;

	public event RunWorkerCompletedEventHandler RunWorkerCompleted;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ProcessDelegate(Delegate del, params object[] args)
	{
		if ((object)del != null)
		{
			Delegate[] invocationList = del.GetInvocationList();
			Delegate[] array = invocationList;
			foreach (Delegate del2 in array)
			{
				InvokeDelegate(del2, args);
			}
		}
	}

	private void InvokeDelegate(Delegate del, params object[] args)
	{
		ISynchronizeInvoke synchronizeInvoke = del.Target as ISynchronizeInvoke;
		if (!synchronizeInvoke.InvokeRequired)
		{
			try
			{
				del.DynamicInvoke(args);
				return;
			}
			catch (Exception ex)
			{
				Trace.WriteLine(ex.Message);
				return;
			}
		}
		try
		{
			synchronizeInvoke.Invoke(del, args);
		}
		catch (Exception ex2)
		{
			Trace.WriteLine(ex2.Message);
		}
	}

	private void ReportCompletion(IAsyncResult asyncResult)
	{
		AsyncResult asyncResult2 = (AsyncResult)asyncResult;
		DoWorkEventHandler doWorkEventHandler = (DoWorkEventHandler)asyncResult2.AsyncDelegate;
		DoWorkEventArgs e = (DoWorkEventArgs)asyncResult.AsyncState;
		object workArgument = null;
		object result = null;
		Exception error = null;
		try
		{
			doWorkEventHandler.EndInvoke(asyncResult);
			result = e.Result;
			workArgument = e.Argument;
		}
		catch (Exception ex)
		{
			error = ex;
		}
		RunWorkerCompletedEventArgs e2 = new RunWorkerCompletedEventArgs(workArgument, result, error, e.Cancel);
		ProcessDelegate(this.RunWorkerCompleted, this, e2);
	}

	public void RunWorkerAsync()
	{
		RunWorkerAsync(null);
	}

	public void RunWorkerAsync(object argument)
	{
		RunWorkerAsync(this.DoWork, argument);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RunWorkerAsync(DoWorkEventHandler doWork, object argument)
	{
		_cancelPending = false;
		if (doWork != null)
		{
			DoWorkEventArgs e = new DoWorkEventArgs(argument);
			AsyncCallback callback = ReportCompletion;
			doWork.BeginInvoke(this, e, callback, e);
		}
	}

	public void ReportProgress(int percent)
	{
		ProgressChangedEventArgs e = new ProgressChangedEventArgs(percent);
		ProcessDelegate(this.ProgressChanged, this, e);
	}

	public void CancelAsync()
	{
		lock (this)
		{
			_cancelPending = true;
		}
	}
}
