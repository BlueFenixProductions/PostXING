namespace PostXING.ViewModels;

public interface IPendingPostBox
{
    OpenedPost? Take();
    void Put(OpenedPost post);
}
