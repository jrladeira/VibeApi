using SharpKml.Base;
using SharpKml.Dom;
using SharpKml.Engine;
using VibeApi.Interfaces;
using VibeApi.Models;

namespace VibeApi.Services;

internal class KmlService : IKmlService
{
    private readonly string _arquivo = Path.Combine("Data", "DIRECIONADORES1.kml");

    public List<PlacemarkModel> ListarPlacemarks()
    {
        var placemarks = new List<PlacemarkModel>();

        using (var stream = File.OpenRead(_arquivo))
        {
            var parser = new Parser();

            parser.Parse(stream);

            var kml = (Kml)parser.Root;
            var document = (Document)kml.Feature;

            foreach (var placemark in document.Flatten().OfType<Placemark>())
            {
                var placemarkModel = new PlacemarkModel
                {
                    Name = placemark.Name,
                    Description = placemark.Description.Text,
                    Latitude = ((Point)placemark.Geometry).Coordinate.Latitude,
                    Longitude = ((Point)placemark.Geometry).Coordinate.Longitude
                };

                foreach (var data in placemark.ExtendedData.Data)
                {
                    switch (data.Name.ToUpper())
                    {
                        case "RUA/CRUZAMENTO":
                            placemarkModel.RuaCruzamento = data.Value;
                            break;
                        case "REFERENCIA":
                            placemarkModel.Referencia = data.Value;
                            break;
                        case "BAIRRO":
                            placemarkModel.Bairro = data.Value;
                            break;
                        case "SITUAÇÃO":
                            placemarkModel.Situacao = data.Value;
                            break;
                        case "CLIENTE":
                            placemarkModel.Cliente = data.Value;
                            break;
                        case "DATA":
                            placemarkModel.Data = data.Value;
                            break;
                        case "COORDENADAS":
                            placemarkModel.Coordenadas = data.Value;
                            break;
                        case "GX_MEDIA_LINKS":
                            placemarkModel.GxMediaLinks = data.Value;
                            break;
                    }
                }

                placemarks.Add(placemarkModel);
            }
        }

        return placemarks;
    }
}
