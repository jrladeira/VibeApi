
namespace VibeApi.Models;

public class PlacemarkModel
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string RuaCruzamento { get; set; } = string.Empty;
    public string Referencia { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Situacao { get; set; } = string.Empty;
    public string Cliente { get; set; } = string.Empty;
    public string Data { get; set; } = string.Empty;
    public string Coordenadas { get; set; } = string.Empty;
    public string GxMediaLinks { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
