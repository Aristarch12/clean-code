namespace Markdown.Shell
{
    public interface IShell
    {
        bool Contains(IShell shell);
        MatchObject MatchText(string text, int startPosition);
        char[] GetStopSymbols();
    }
}
