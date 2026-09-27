namespace Museum.Tests;

using Museum.Domain;

/// <summary>
/// Общие тестовые данные
/// </summary>
public class MuseumFixture
{
    /// <summary>
    /// Список выставок
    /// </summary>
    public List<Exhibition> Exhibitions { get; }

    /// <summary>
    /// Список посетителей
    /// </summary>
    public List<Visitor> Visitors { get; }

    /// <summary>
    /// Список экскурсий
    /// </summary>
    public List<Tour> Tours { get; }

    /// <summary>
    /// Список билетов
    /// </summary>
    public List<Ticket> Tickets { get; }

    public MuseumFixture()
    {
        Exhibitions = DataSeeder.Exhibitions;
        Visitors = DataSeeder.Visitors;
        Tours = DataSeeder.Tours;
        Tickets = DataSeeder.Tickets;
    }
}