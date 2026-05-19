using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using EnterpriseDT.Net.Ftp;
using PostXING.Components;

namespace PostXING.Controls;

public class FTPControl : UserControl
{
	private enum ProgressProperty
	{
		Minimum,
		Maximum,
		Value
	}

	private delegate void updateTransferOpDelegate(string text);

	private delegate void updateProgressDelegate(ProgressProperty prop, int val);

	private delegate void setProgressEnableDelegate(bool enabled);

	public class UploadedFileInfo
	{
		public enum UploadMode
		{
			None,
			Anchor,
			Image
		}

		private const UploadMode m_defaultMode = UploadMode.Anchor;

		private static string[] m_imageExtensions = ".jpg;.jpeg;.gif;.png;.tiff;.bmp".Split(';');

		private string m_filePath;

		private UploadMode m_mode;

		public string FilePath => m_filePath;

		public UploadMode Mode
		{
			get
			{
				return m_mode;
			}
			set
			{
				m_mode = value;
			}
		}

		public UploadedFileInfo(string path)
		{
			m_filePath = path;
			m_mode = ((Array.IndexOf(m_imageExtensions, Path.GetExtension(path).ToLower()) == -1) ? UploadMode.Anchor : UploadMode.Image);
		}

		public override string ToString()
		{
			return Path.GetFileName(m_filePath);
		}
	}

	private IContainer components;

	private GroupBox groupBox2;

	private TextBox txtTransferOp;

	private ProgressBar prgTransfer;

	private Thread m_worker;

	private UploadedFileInfo[] m_files;

	private FTPSite m_site;

	public event FileUploadedEventHandler FileUploaded;

	public event TransferFailedEventHandler TransferFailed;

	public event EventHandler TransferComplete;

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.txtTransferOp = new System.Windows.Forms.TextBox();
		this.prgTransfer = new System.Windows.Forms.ProgressBar();
		this.groupBox2.SuspendLayout();
		base.SuspendLayout();
		this.groupBox2.Controls.Add(this.txtTransferOp);
		this.groupBox2.Controls.Add(this.prgTransfer);
		this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.groupBox2.Location = new System.Drawing.Point(0, 0);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(205, 78);
		this.groupBox2.TabIndex = 12;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Progress";
		this.txtTransferOp.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtTransferOp.Enabled = false;
		this.txtTransferOp.Location = new System.Drawing.Point(9, 45);
		this.txtTransferOp.Name = "txtTransferOp";
		this.txtTransferOp.Size = new System.Drawing.Size(190, 20);
		this.txtTransferOp.TabIndex = 1;
		this.txtTransferOp.Text = "Idle";
		this.txtTransferOp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.prgTransfer.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.prgTransfer.Enabled = false;
		this.prgTransfer.Location = new System.Drawing.Point(9, 20);
		this.prgTransfer.Name = "prgTransfer";
		this.prgTransfer.Size = new System.Drawing.Size(190, 19);
		this.prgTransfer.TabIndex = 0;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.groupBox2);
		base.Name = "FTPControl";
		base.Size = new System.Drawing.Size(205, 78);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		base.ResumeLayout(false);
	}

	public FTPControl()
	{
		InitializeComponent();
	}

	public void Transfer(UploadedFileInfo[] files, FTPSite site)
	{
		if (m_worker != null && m_worker.IsAlive)
		{
			throw new InvalidOperationException("Upload operation already in progress!");
		}
		m_worker = new Thread(workerThread);
		m_worker.IsBackground = true;
		m_files = files;
		m_site = site;
		m_worker.Start();
	}

	private void setProgressEnable(bool enabled)
	{
		if (base.InvokeRequired)
		{
			Invoke(new setProgressEnableDelegate(setProgressEnable), enabled);
		}
		else
		{
			TextBox textBox = txtTransferOp;
			bool enabled2 = (prgTransfer.Enabled = enabled);
			textBox.Enabled = enabled2;
		}
	}

	private void updateTransferOp(string text)
	{
		if (base.InvokeRequired)
		{
			Invoke(new updateTransferOpDelegate(updateTransferOp), text);
		}
		else
		{
			txtTransferOp.Text = text;
		}
	}

	private void updateProgress(ProgressProperty prop, int val)
	{
		if (base.InvokeRequired)
		{
			Invoke(new updateProgressDelegate(updateProgress), prop, val);
			return;
		}
		switch (prop)
		{
		case ProgressProperty.Minimum:
			prgTransfer.Minimum = val;
			break;
		case ProgressProperty.Maximum:
			prgTransfer.Maximum = val;
			break;
		case ProgressProperty.Value:
			prgTransfer.Value = val;
			break;
		}
	}

	private void OnTransferFailed(string caption, Exception e)
	{
		if (this.TransferFailed != null)
		{
			this.TransferFailed(this, new TransferFailedEventArgs(caption, e));
		}
	}

	private void workerThread()
	{
		try
		{
			FTPClient fTPClient;
			try
			{
				setProgressEnable(enabled: true);
				updateTransferOp("Logging on");
				fTPClient = new FTPClient(m_site.Target.Host, m_site.Target.Port);
				fTPClient.Login(m_site.UserName, m_site.Password);
			}
			catch (FTPException e)
			{
				OnTransferFailed("Login to FTP server failed", e);
				return;
			}
			try
			{
				updateTransferOp("Changing directory");
				fTPClient.ChDir(m_site.Target.AbsolutePath.TrimStart('/'));
			}
			catch (FTPException e2)
			{
				OnTransferFailed("Can't change directory", e2);
				return;
			}
			updateTransferOp("Setting up transfer");
			fTPClient.TransferType = FTPTransferType.BINARY;
			fTPClient.ConnectMode = ((!m_site.Passive) ? FTPConnectMode.ACTIVE : FTPConnectMode.PASV);
			fTPClient.BytesTransferred += ftpClient_BytesTransferred;
			UploadedFileInfo[] files = m_files;
			foreach (UploadedFileInfo uploadedFileInfo in files)
			{
				FileInfo fileInfo = new FileInfo(uploadedFileInfo.FilePath);
				updateProgress(ProgressProperty.Maximum, (int)fileInfo.Length);
				updateProgress(ProgressProperty.Value, 0);
				fTPClient.TransferNotifyInterval = fileInfo.Length / 1000 + 1;
				updateTransferOp("Uploading " + fileInfo.Name + "...");
				try
				{
					fTPClient.Put(fileInfo.FullName, fileInfo.Name);
					if (this.FileUploaded != null)
					{
						string arg = Path.Combine(m_site.BaseUrl, fileInfo.Name);
						string html = string.Empty;
						switch (uploadedFileInfo.Mode)
						{
						case UploadedFileInfo.UploadMode.Image:
							html = $"<img src=\"{arg}\">";
							break;
						case UploadedFileInfo.UploadMode.Anchor:
							html = $"<a href=\"{arg}\">{fileInfo.Name}</a>";
							break;
						default:
							throw new InvalidOperationException("Unexpected file upload mode!");
						case UploadedFileInfo.UploadMode.None:
							break;
						}
						this.FileUploaded(this, new FileUploadedEventArgs(uploadedFileInfo, html));
					}
				}
				catch (FTPException e3)
				{
					OnTransferFailed("Can't upload file " + fileInfo.Name, e3);
				}
			}
			updateTransferOp("Done");
			if (this.TransferComplete != null)
			{
				this.TransferComplete(this, new EventArgs());
			}
		}
		catch (Exception e4)
		{
			OnTransferFailed("Unexpected error", e4);
		}
		finally
		{
			setProgressEnable(enabled: false);
		}
	}

	private void ftpClient_BytesTransferred(object sender, BytesTransferredEventArgs bytesTransferred)
	{
		updateProgress(ProgressProperty.Value, (int)bytesTransferred.ByteCount);
	}
}
