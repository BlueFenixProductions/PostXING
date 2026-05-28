using PostXING.ViewModels;

namespace PostXING.App.Services;

public sealed class PendingPostBox : IPendingPostBox
{
    private OpenedPost? _current;
    public OpenedPost? Take() { var c = _current; _current = null; return c; }
    public void Put(OpenedPost post) => _current = post;
}
