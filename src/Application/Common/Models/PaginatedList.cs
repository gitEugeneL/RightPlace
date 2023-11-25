namespace Application.Common.Models;

public class PaginatedList<T>
{
    public IReadOnlyCollection<T> Items { get; }
    public int PageNumber { get; }
    public int TotalPages { get; }
    public int TotalItemsCount { get; }

    public PaginatedList(IReadOnlyCollection<T> items, int totalItemsCount, int pageNumber, int pageSize)
    {
        Items = items;
        PageNumber = pageNumber;
        TotalItemsCount = totalItemsCount;
        TotalPages = (int) Math.Ceiling(totalItemsCount / (double) pageSize);
    }
}