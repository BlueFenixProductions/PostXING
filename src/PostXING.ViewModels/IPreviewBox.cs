namespace PostXING.ViewModels;

/// <summary>
/// Single-slot handoff of the editor's current markdown to the pushed preview page, mirroring
/// <c>IPendingPostBox</c>. The editor Puts on preview; PreviewPage Takes it on appear.
/// </summary>
public interface IPreviewBox
{
    void Put(string markdown);
    string? Take();
}
