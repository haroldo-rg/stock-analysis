/// <summary>
/// Representa as configurações de um filtro por expressão regular
/// </summary>
public class RegexFilterConfig
{
    public string Property { get; set; }
    public string Pattern { get; set; }
    public bool Enabled { get; set; } 

    public RegexFilterConfig(string property, string pattern, bool enabled)
    {
        Property = property;
        Pattern = pattern;
        Enabled = enabled;
    }
}