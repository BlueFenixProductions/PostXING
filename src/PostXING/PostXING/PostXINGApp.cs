using System;
using System.Windows.Forms;
using Genghis.Windows.Forms;
using PostXING.Components;
using PostXING.Dialogs;
using PostXING.Forms;

namespace PostXING;

public sealed class PostXINGApp
{
	[STAThread]
	private static void Main(string[] args)
	{
		Application.EnableVisualStyles();
		PostXINGPreferences postXINGPreferences = AppManager.LoadPreferences();
		SplashScreen splashScreen = null;
		if (!postXINGPreferences.SuppressSplashPage)
		{
			splashScreen = new SplashScreen(typeof(PostXINGSplashForm));
		}
		if (args.Length == 1 && args[0] != null)
		{
			AppManager.TempFileLocation = args[0];
		}
		EditorForm editorForm = (EditorForm)(AppManager.EditorForm = new EditorForm());
		OptionsDialog optionsDialog = new OptionsDialog();
		AppManager.OptionsDialog = optionsDialog;
		AppManager.Start(postXINGPreferences);
		if (!postXINGPreferences.SuppressSplashPage)
		{
			splashScreen.Close(editorForm);
		}
		Application.Run(editorForm);
	}
}
