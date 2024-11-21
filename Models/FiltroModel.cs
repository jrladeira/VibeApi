namespace VibeApi.Models;

public class FiltroModel
{
    public List<string> Cliente { get; set; } = [];
    public List<string> Situacao { get; set; } = [];
    public List<string> Bairro { get; set; } = [];
    public string Referencia { get; set; } = string.Empty;
    public string RuaCruzamento { get; set; } = string.Empty;
}
