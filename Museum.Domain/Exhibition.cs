namespace Museum.Domain;

/// <summary>
/// Выставка музея
/// </summary>
public class Exhibition
{
    /// <summary>
    /// Идентификатор выставки
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название 
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Тематика 
    /// </summary>
    public ExhibitionTheme Theme { get; set; }

    /// <summary>
    /// Номер зала, где выставка
    /// </summary>
    public int HallNumber { get; set; }

    /// <summary>
    /// Дата начала 
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Дата окончания 
    /// </summary>
    public DateTime EndDate { get; set; }
}