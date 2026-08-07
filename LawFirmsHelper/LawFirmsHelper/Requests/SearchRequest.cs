namespace LawFirmsHelper.Requests;

public class SearchRequest
{
    private int _limit { get; set; }
    private int _offset { get; set; }

    public int Offset
    {
        get => _offset;
        set => _offset = Math.Max(0, value);
    }

    public int Limit
    {
        get => _limit;
        set => _limit = Math.Clamp(value, 1, 100);
    }
}