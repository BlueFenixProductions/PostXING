using PostXING.ViewModels;

namespace PostXING.App.Services;

/// <summary>Single-slot handoff of the editor's markdown to the preview page (mirrors PendingPostBox).</summary>
public sealed class PreviewBox : IPreviewBox
{
    private string? _markdown;

    public void Put(string markdown) => _markdown = markdown;

    public string? Take()
    {
        var m = _markdown;
        _markdown = null;
        return m;
    }
}
