using Microsoft.Win32;

namespace PostXING.Components;

public class MediaPlayerInfo
{
	public static string GetCurrentlyPlayingMedia(BlogOptions options)
	{
		RegistryKey currentUser = Registry.CurrentUser;
		currentUser = currentUser.OpenSubKey("Software\\Microsoft\\MediaPlayer\\CurrentMetadata");
		string newValue = "";
		string newValue2 = "";
		string newValue3 = "";
		string newValue4 = "";
		string currentlyPlayingTemplate = options.CurrentlyPlayingTemplate;
		bool flag = false;
		string text = "";
		try
		{
			text = currentUser.GetValue("Title").ToString();
			if (text.Length != 0)
			{
				flag = true;
				newValue = text;
			}
		}
		catch
		{
			try
			{
				text = "";
				text = currentUser.GetValue("Name").ToString();
				if (text.Length != 0)
				{
					flag = true;
					newValue = text;
				}
			}
			catch
			{
			}
		}
		try
		{
			text = "";
			text = currentUser.GetValue("DurationString").ToString();
			if (text.Length != 0)
			{
				flag = true;
				newValue4 = text;
			}
		}
		catch
		{
		}
		try
		{
			text = "";
			text = currentUser.GetValue("Author").ToString();
			if (text.Length != 0)
			{
				flag = true;
				newValue2 = text;
			}
		}
		catch
		{
		}
		try
		{
			text = "";
			text = currentUser.GetValue("Album").ToString();
			if (text.Length != 0)
			{
				flag = true;
				newValue3 = text;
			}
		}
		catch
		{
		}
		currentlyPlayingTemplate = options.HtmlTag + currentlyPlayingTemplate;
		if (flag)
		{
			currentlyPlayingTemplate = currentlyPlayingTemplate.Replace("{title}", newValue);
			currentlyPlayingTemplate = currentlyPlayingTemplate.Replace("{artist}", newValue2);
			currentlyPlayingTemplate = currentlyPlayingTemplate.Replace("{album}", newValue3);
			currentlyPlayingTemplate = currentlyPlayingTemplate.Replace("{duration}", newValue4);
		}
		else
		{
			currentlyPlayingTemplate = options.NothingPlayingTemplate;
		}
		return currentlyPlayingTemplate + _closingTag(options.HtmlTag);
	}

	private static string _closingTag(string openingTag)
	{
		string text = "</";
		foreach (char c in openingTag)
		{
			switch (c)
			{
			default:
				text += c;
				continue;
			case '<':
				continue;
			case ' ':
			case '>':
				break;
			}
			break;
		}
		return text + ">";
	}
}
