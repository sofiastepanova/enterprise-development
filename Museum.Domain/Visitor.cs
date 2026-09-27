namespace Museum.Domain;

/// <summary>
/// Посетитель 
/// </summary>
public class Visitor
{
    /// <summary>
    /// Идентификатор 
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// фио
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Номерочек 
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Др посетителя
    /// </summary>
    public DateTime BirthDate { get; set; }
}