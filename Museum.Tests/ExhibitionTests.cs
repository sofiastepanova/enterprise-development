namespace Museum.Tests;

using Museum.Domain;

/// <summary>
/// Тесты LINQ-запросов над тестовыми данными музея
/// </summary>
public class ExhibitionTests : IClassFixture<MuseumFixture>
{
    private readonly MuseumFixture _fixture;

    public ExhibitionTests(MuseumFixture fixture)
    {
        _fixture = fixture;
    }

    /// <summary>
    /// Результат не должен превышать 5 выставок и должен быть отсортирован по убыванию посетителей
    /// </summary>
    [Fact]
    public void GetTopExhibitions_ByVisitorCount_ReturnsTopFive()
    {
        var result = _fixture.Tickets
            .SelectMany(t => t.Tour.Exhibitions.Select(e => new { Exhibition = e, Ticket = t }))
            .GroupBy(x => x.Exhibition)
            .Select(g => new { Exhibition = g.Key, VisitorCount = g.Count() })
            .OrderByDescending(x => x.VisitorCount)
            .Take(5)
            .ToList();

        Assert.True(result.Count <= 5);
        Assert.True(result.Count > 0);

        var visitorCounts = result.Select(x => x.VisitorCount).ToList();
        var sortedDescending = visitorCounts.OrderByDescending(x => x).ToList();
        Assert.Equal(sortedDescending, visitorCounts);
    }

    /// <summary>
    /// Все экскурсии в выборке должны иметь одинаковое число участников, равное минимальному
    /// </summary>
    [Fact]
    public void GetTours_WithMinVisitors_ReturnsLowestCount()
    {
        var tourVisitorCounts = _fixture.Tickets
            .GroupBy(t => t.Tour)
            .Select(g => new { Tour = g.Key, VisitorCount = g.Count() })
            .ToList();

        var minCount = tourVisitorCounts.Min(x => x.VisitorCount);

        var result = tourVisitorCounts
            .Where(x => x.VisitorCount == minCount)
            .Select(x => x.Tour)
            .ToList();

        Assert.NotEmpty(result);
        Assert.All(result, tour =>
        {
            var actualCount = _fixture.Tickets.Count(t => t.Tour.Id == tour.Id);
            Assert.Equal(minCount, actualCount);
        });
    }

    /// <summary>
    /// Для каждой тематики среднее число посетителей в день должно быть не больше максимального и не меньше минимального
    /// </summary>
    [Fact]
    public void GetAttendanceSummary_ByTheme_ReturnsGroupedStats()
    {
        var startDate = new DateTime(2025, 1, 1);
        var endDate = new DateTime(2025, 12, 31);

        var ticketsInPeriod = _fixture.Tickets
            .Where(t => t.Tour.Date >= startDate && t.Tour.Date <= endDate)
            .ToList();

        var result = ticketsInPeriod
            .SelectMany(t => t.Tour.Exhibitions.Select(e => new { Theme = e.Theme, Ticket = t }))
            .GroupBy(x => x.Theme)
            .Select(g => new
            {
                Theme = g.Key,
                TotalVisitors = g.Count(),
                TotalPrice = g.Sum(x => x.Ticket.Price),
                DailyVisitorCounts = g
                    .GroupBy(x => x.Ticket.Tour.Date.Date)
                    .Select(dayGroup => dayGroup.Count())
                    .ToList()
            })
            .Select(x => new
            {
                x.Theme,
                x.TotalVisitors,
                x.TotalPrice,
                AverageVisitorsPerDay = x.DailyVisitorCounts.Average(),
                MinVisitorsPerDay = x.DailyVisitorCounts.Min(),
                MaxVisitorsPerDay = x.DailyVisitorCounts.Max()
            })
            .ToList();

        Assert.NotEmpty(result);
        Assert.All(result, theme =>
        {
            Assert.True(theme.AverageVisitorsPerDay >= theme.MinVisitorsPerDay);
            Assert.True(theme.AverageVisitorsPerDay <= theme.MaxVisitorsPerDay);
            Assert.True(theme.TotalPrice > 0);
        });
    }

    /// <summary>
    /// У каждой экскурсии в выборке должна быть хотя бы одна выставка из нужного зала
    /// </summary>
    [Fact]
    public void GetTours_InHall_ReturnsMatchingTours()
    {
        var hallNumber = 1;
        var startDate = new DateTime(2025, 1, 1);
        var endDate = new DateTime(2025, 12, 31);

        var result = _fixture.Tours
            .Where(t => t.Date >= startDate && t.Date <= endDate)
            .Where(t => t.Exhibitions.Any(e => e.HallNumber == hallNumber))
            .ToList();

        Assert.NotEmpty(result);
        Assert.All(result, tour => Assert.Contains(tour.Exhibitions, e => e.HallNumber == hallNumber));
    }

    /// <summary>
    /// Список посетителей должен быть отсортирован по ФИО по возрастанию
    /// </summary>
    [Fact]
    public void GetVisitors_ForTour_ReturnsSortedByName()
    {
        var selectedTourId = _fixture.Tours.First().Id;

        var result = _fixture.Tickets
            .Where(t => t.Tour.Id == selectedTourId)
            .Select(t => t.Visitor)
            .OrderBy(v => v.FullName)
            .ToList();

        Assert.NotEmpty(result);

        var names = result.Select(v => v.FullName).ToList();
        var sortedNames = names.OrderBy(n => n).ToList();
        Assert.Equal(sortedNames, names);
    }
}