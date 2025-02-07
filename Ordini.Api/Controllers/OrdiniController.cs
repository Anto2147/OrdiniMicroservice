using Ordini.Business.Abstraction;
using Ordini.Shared;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc;
namespace Ordini.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class OrdiniController : ControllerBase
{
    private readonly IBusiness _business;
    private readonly ILogger<OrdiniController> _logger;

    public OrdiniController(IBusiness business, ILogger<OrdiniController> logger)
    {
        _business = business;
        _logger = logger;
    }


    [HttpPost(Name = "CreateCliente")]
    public async Task<ActionResult> CreateClienteAsync(ClienteDTO clienteDTO)
    {
        await _business.CreateClienteAsync(clienteDTO);
        return Ok("Cliente creato con successo.");
    }

    [HttpGet(Name = "ReadCliente")]
    public async Task<ActionResult<ClienteDTO?>> ReadClienteAsync(int id)
    {
        var cliente = await _business.ReadClienteAsync(id);
        return new JsonResult(cliente);
    }

    [HttpGet(Name = "GetAllClienti")]
    public async Task<ActionResult<List<ClienteDTO>?>> GetAllClientiAsync()
    {
        var clienti = await _business.GetAllClientiAsync();
        return new JsonResult(clienti);
    }

    [HttpPost(Name = "CreateOrdine")]
    public async Task<ActionResult> CreateOrdineAsync(OrdineDTO ordineDTO)
    {
        await _business.CreateOrdineAsync(ordineDTO);
        return Ok("Ordine creato con successo.");
    }

    [HttpGet(Name = "ReadOrdine")]
    public async Task<ActionResult<OrdineDTO?>> ReadOrdineAsync(int id)
    {
        var ordine = await _business.ReadOrdineAsync(id);
        return new JsonResult(ordine);
    }

    [HttpGet(Name = "GetAllOrdiniByCliente")]
    public async Task<ActionResult<List<OrdineDTO>?>> GetAllOrdiniByClienteAsync(int idCliente)
    {
        var ordini = await _business.GetAllOrdiniByClienteAsync(idCliente);
        return new JsonResult(ordini);
    }

    // Associazione Prodotto-Ordine Endpoints
    [HttpPost(Name = "CreateAssociaProdottoOrdine")]  //metodo principale che aggiunge prodotto a carrello
    public async Task<IActionResult> CreateAssociaProdottoOrdineAsync([FromBody] AssociaProdottoOrdineDTO associaProdottoOrdineDTO)
    {
        await _business.CreateAssociaProdottoOrdineAsync(associaProdottoOrdineDTO);
        return Ok("Associazione Prodotto-Ordine creata con successo.");
    }

    [HttpGet(Name = "ReadAssociaProdottoOrdine")]
    public async Task<ActionResult<AssociaProdottoOrdineDTO?>> ReadAssociaProdottoOrdineAsync(int id)
    {
        var associazione = await _business.ReadAssociaProdottoOrdineAsync(id);
        return new JsonResult(associazione);
    }


    [HttpPut(Name = "AggiornaStatoPagamento")]
    public async Task<ActionResult> AggiornaStatoPagamento(int id)
    {
        Console.WriteLine($"Richiesto aggiornamento stato per ordine{id}");
        await _business.AggiornaStatoPagamentoAsync(id);
        Console.WriteLine($"END Richiesto aggiornamento stato per ordine{id}");
        return Ok();
    }

    [HttpGet(Name = "GetProdottiAssociatiByOrdine")]
    public async Task<ActionResult<List<AssociaProdottoOrdineDTO>?>> GetProdottiAssociatiByOrdineAsync(int idOrdine, CancellationToken cancellationToken = default)
    {
        var prodottiAssociati = await _business.GetProdottiAssociatiByOrdineAsync(idOrdine, cancellationToken);

        if (prodottiAssociati == null || !prodottiAssociati.Any())
        {
            return NotFound("Nessun prodotto associato a questo ordine.");
        }

        return Ok(prodottiAssociati);
    }

    [HttpGet(Name = "GetAllOrdini")]
    public async Task<ActionResult<List<AssociaProdottoOrdineDTO>?>> GetAllOrdiniAsync(CancellationToken cancellationToken = default)
    {
        var ordini = await _business.GetAllOrdiniAsync(cancellationToken);

        if (ordini == null || !ordini.Any())
        {
            return NotFound("Nessun ordine trovato.");
        }

        return Ok(ordini);
    }

    #region Deletion

    [HttpDelete(Name = "DeleteAssociaProdottoOrdine")]
    public async Task<ActionResult> DeleteAssociaProdottoOrdineAsync(int id)
    {
        await _business.DeleteAssociaProdottoOrdineAsync(id);
        return Ok("Associazione Prodotto-Ordine eliminata con successo.");
    }

    [HttpDelete(Name = "DeleteCliente")]
    public async Task<ActionResult> DeleteClienteAsync(int id)
    {
        await _business.DeleteClienteAsync(id);
        return Ok("Cliente eliminato con successo.");
    }

    [HttpDelete(Name = "DeleteOrdine")]
    public async Task<ActionResult> DeleteOrdineAsync(int id)
    {
        await _business.DeleteOrdineAsync(id);
        return Ok("Ordine eliminato con successo.");
    }
    #endregion
}

