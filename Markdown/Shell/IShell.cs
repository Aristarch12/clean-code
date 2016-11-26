namespace Markdown.Shell
{
    public interface IShell
    {
        bool Contains(IShell shell);
        bool TryMatch(string text, int startPosition, out MatchObject matchObject);
        char GetStopSymbol();
    }
}
