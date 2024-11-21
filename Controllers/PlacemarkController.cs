using Microsoft.AspNetCore.Mvc;
using VibeApi.Interfaces;
using VibeApi.Models;

namespace VibeApi.Controllers;

[ApiController]
[Route("api/placemarks")]
public class PlacemarkController(IPlacemarkService placemarkService) : ControllerBase
{
    private readonly IPlacemarkService _placemarkService = placemarkService;

    [HttpPost("export")]
    public async Task<IActionResult> ExportarPlacemarks([FromBody] FiltroModel filtro)
    {
        var erro = await _placemarkService.ValidarFiltroAsync(filtro);

        if (erro.Status)
        {
            return BadRequest(erro);
        }

        var conteudo = await _placemarkService.ExportarPlacemarksAsync(filtro);

        return File(conteudo, "application/vnd.google-earth.kml+xml", "Export.kml");
    }

    [HttpGet]
    public async Task<IActionResult> BuscarPlacemarks([FromQuery] FiltroModel filtro)
    {
        var erro = await _placemarkService.ValidarFiltroAsync(filtro);

        if (erro.Status)
        {
            return BadRequest(erro);
        }

        var placemarks = await _placemarkService.BuscarPlacemarksAsync(filtro);

        return Ok(placemarks);
    }

    [HttpGet("filters")]
    public async Task<IActionResult> ListarElemento()
    {
        var elemento = await _placemarkService.ListarElementoAsync();

        return Ok(elemento);
    }
}
