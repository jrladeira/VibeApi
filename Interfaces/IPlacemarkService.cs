using VibeApi.Models;

namespace VibeApi.Interfaces;

public interface IPlacemarkService
{
    Task<byte[]> ExportarPlacemarksAsync(FiltroModel filtro);
    Task<List<PlacemarkModel>> BuscarPlacemarksAsync(FiltroModel filtro);
    Task<ElementoModel> ListarElementoAsync();
    Task<ErroModel> ValidarFiltroAsync(FiltroModel filtro);
}
