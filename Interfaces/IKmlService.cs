using VibeApi.Models;

namespace VibeApi.Interfaces;

public interface IKmlService
{
    List<PlacemarkModel> ListarPlacemarks();
}
