using SharpKml.Base;
using SharpKml.Dom;
using VibeApi.Interfaces;
using VibeApi.Models;

namespace VibeApi.Services;

internal class PlacemarkService(IKmlService kmlService) : IPlacemarkService
{
    private readonly IKmlService _kmlService = kmlService;

    public Task<byte[]> ExportarPlacemarksAsync(FiltroModel filtro)
    {
        var placemarks = BuscarPlacemarksAsync(filtro).Result;

        var document = new Document();

        foreach (var placemarkModel in placemarks)
        {
            var placemark = new Placemark
            {
                Name = placemarkModel.Name,
                Description = new Description { Text = placemarkModel.Description },
                ExtendedData = new ExtendedData(),
                Geometry = new Point
                {
                    Coordinate = new Vector(placemarkModel.Latitude, placemarkModel.Longitude)
                }
            };

            placemark.ExtendedData.AddData(new Data { Name = "RUA/CRUZAMENTO", Value = placemarkModel.RuaCruzamento });
            placemark.ExtendedData.AddData(new Data { Name = "REFERENCIA", Value = placemarkModel.Referencia });
            placemark.ExtendedData.AddData(new Data { Name = "BAIRRO", Value = placemarkModel.Bairro });
            placemark.ExtendedData.AddData(new Data { Name = "SITUAÇÃO", Value = placemarkModel.Situacao });
            placemark.ExtendedData.AddData(new Data { Name = "CLIENTE", Value = placemarkModel.Cliente });
            placemark.ExtendedData.AddData(new Data { Name = "DATA", Value = placemarkModel.Data });
            placemark.ExtendedData.AddData(new Data { Name = "COORDENADAS", Value = placemarkModel.Coordenadas });
            placemark.ExtendedData.AddData(new Data { Name = "gx_media_links", Value = placemarkModel.GxMediaLinks });

            document.AddFeature(placemark);
        }

        var kml = new Kml { Feature = document };
        var serializer = new Serializer();

        serializer.Serialize(kml);

        var conteudo = System.Text.Encoding.UTF8.GetBytes(serializer.Xml);

        return Task.FromResult(conteudo);
    }

    public Task<List<PlacemarkModel>> BuscarPlacemarksAsync(FiltroModel filtro)
    {
        var placemarks = _kmlService.ListarPlacemarks();

        if(filtro.Cliente.Count > 0)
        {
            placemarks = placemarks.Where(p => filtro.Cliente.Contains(p.Cliente)).ToList();
        }

        if(filtro.Situacao.Count > 0)
        {
            placemarks = placemarks.Where(p => filtro.Situacao.Contains(p.Situacao)).ToList();
        }

        if(filtro.Bairro.Count > 0)
        {
            placemarks = placemarks.Where(p => filtro.Bairro.Contains(p.Bairro)).ToList();
        }

        if (filtro.Referencia != string.Empty)
        {
            placemarks = placemarks.Where(p => p.Referencia.Contains(filtro.Referencia, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        if (filtro.RuaCruzamento != string.Empty)
        {
            placemarks = placemarks.Where(p => p.RuaCruzamento.Contains(filtro.RuaCruzamento, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        return Task.FromResult(placemarks);
    }

    public Task<ElementoModel> ListarElementoAsync()
    {
        var placemarks = _kmlService.ListarPlacemarks();

        var elemento = new ElementoModel
        {
            Cliente = placemarks.Select(d => d.Cliente).Distinct().Order().ToList(),
            Situacao = placemarks.Select(d => d.Situacao).Distinct().Order().ToList(),
            Bairro = placemarks.Select(d => d.Bairro).Distinct().Order().ToList()
        };

        return Task.FromResult(elemento);
    }

    public Task<ErroModel> ValidarFiltroAsync(FiltroModel filtro)
    {
        var erro = new ErroModel();

        var elemento = ListarElementoAsync().Result;

        if (filtro.Cliente.Count > 0 && filtro.Cliente.Except(elemento.Cliente).Count() > 0)
        {
            erro.Status = true;
            erro.Mensagens.Add("Cliente informado não existe");
        }

        if (filtro.Situacao.Count > 0 && filtro.Situacao.Except(elemento.Situacao).Count() > 0)
        {
            erro.Status = true;
            erro.Mensagens.Add("Situação informada não existe");
        }

        if (filtro.Bairro.Count > 0 && filtro.Bairro.Except(elemento.Bairro).Count() > 0)
        {
            erro.Status = true;
            erro.Mensagens.Add("Bairro informado não existe");
        }

        if (filtro.Referencia != string.Empty && filtro.Referencia.Length < 3)
        {
            erro.Status = true;
            erro.Mensagens.Add("Referência menor que 3 caracteres");
        }

        if (filtro.RuaCruzamento != string.Empty && filtro.RuaCruzamento.Length < 3)
        {
            erro.Status = true;
            erro.Mensagens.Add("Rua/Cruzamento menor que 3 caracteres");
        }

        return Task.FromResult(erro);
    }
}
