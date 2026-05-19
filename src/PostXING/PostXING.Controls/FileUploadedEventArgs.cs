using System;

namespace PostXING.Controls;

public class FileUploadedEventArgs : EventArgs
{
	private FTPControl.UploadedFileInfo m_fileInfo;

	private string m_html;

	public FTPControl.UploadedFileInfo FileInfo => m_fileInfo;

	public string Html => m_html;

	public FileUploadedEventArgs(FTPControl.UploadedFileInfo fileInfo, string html)
	{
		m_fileInfo = fileInfo;
		m_html = html;
	}
}
