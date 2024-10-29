/// <summary>
/// Representa as configurações de um filtro por range de valores em uma propriedade
/// </summary>
public class RangeFilterConfig
{
    public string Property { get; set; }
    public decimal Min { get; set; }
    public decimal Max { get; set; }
    public bool Enabled { get; set; } 

    public RangeFilterConfig(string property, decimal min, decimal max, bool enabled)
    {
        Property = property;
        Min = min;
        Max = max;
        Enabled = enabled;
    }
}