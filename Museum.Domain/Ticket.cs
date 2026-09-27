namespace Museum.Domain;

/// <summary>
/// Билет на экскурсию
/// </summary>
public class Ticket
{
    /// <summary>
    /// Идентификатор 
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Экскурсия, на которую куплен билет
    /// </summary>
    public Tour Tour { get; set; } = null!;

    /// <summary>
    /// Посетитель
    /// </summary>
    public Visitor Visitor { get; set; } = null!;

    /// <summary>
    /// Тип билета (взрослый или льготный)
    /// </summary>
    public TicketType Type { get; set; }

    /// <summary>
    /// Цена 
    /// </summary>
    public decimal Price { get; set; }
}