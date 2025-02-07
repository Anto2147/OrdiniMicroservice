using Microsoft.AspNetCore.Http;
using Ordini.Shared;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Json;
using Ordini.ClientHttp.Abstraction;

namespace Ordini.ClientHttp;

public class OrdiniClient(HttpClient httpclient) : IOrdiniClient
{
    public async Task<OrdineDTO?> ReadOrdineAsync(int idOrdine, CancellationToken cancellationToken = default)
    {
        var queryString = QueryString.Create(new Dictionary<string, string>(){
        { "id", idOrdine.ToString(CultureInfo.InvariantCulture) }
        });

        var response = await httpclient.GetAsync($"/Ordini/ReadOrdine{queryString}", cancellationToken);
        return await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<OrdineDTO?>(cancellationToken: cancellationToken);
    }
    
    public async Task<string> AggiornaStatoPagamentoAsync(int IdOrdine, CancellationToken cancellationToken = default)
    {
        //var payload = new { idOrdine=IdOrdine };
        var queryString = QueryString.Create(new Dictionary<string, string>(){
        { "id", IdOrdine.ToString(CultureInfo.InvariantCulture) }
        });
        var url = $"/Ordini/AggiornaStatoPagamento{queryString}";
        Console.WriteLine($"RIchiesta PUT per {url}");
        var response = await httpclient.PutAsync(url,null, cancellationToken);

        return await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync(cancellationToken: cancellationToken);
        //return await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<string>(cancellationToken: cancellationToken);
    }

    public async Task<List<AssociaProdottoOrdineDTO>?> GetProdottiAssociatiByOrdineAsync(int idOrdine, CancellationToken cancellationToken = default)
    {
        var queryString = QueryString.Create(new Dictionary<string, string>
        {
            { "idOrdine", idOrdine.ToString(CultureInfo.InvariantCulture) }
        });

        var response = await httpclient.GetAsync($"/Ordini/GetProdottiAssociatiByOrdine{queryString}", cancellationToken);
        return await response.EnsureSuccessStatusCode()
                             .Content.ReadFromJsonAsync<List<AssociaProdottoOrdineDTO>>(cancellationToken: cancellationToken);
    }

    public async Task<List<OrdineDTO>?> GetAllOrdiniAsync(CancellationToken cancellationToken = default)
    {
        var response = await httpclient.GetAsync("/Ordini/GetAllOrdini", cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            // Log dell'errore se necessario
            return new List<OrdineDTO>();
        }

        return await response.Content.ReadFromJsonAsync<List<OrdineDTO>>(cancellationToken: cancellationToken);
    }

}