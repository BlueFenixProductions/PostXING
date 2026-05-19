namespace PostXING.Components;

public interface IEditorForm
{
	Blog CurrentBlog { get; }

	BlogPost CurrentPost { get; }

	string UserAgent { get; }

	bool IsEditing { get; set; }

	void StartProgress(string message, params object[] args);

	void StopProgress();

	void ResetMessage(string statusBarMessage);

	void ResetMessage(string statusBarMessage, bool preserveCurrentPost);

	void ShowHistory();

	void LoadToolbarButtons();
}
