using Ordini.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordini.ClientHttp.Abstraction;

public interface IOrdiniClient
{
    public Task<OrdineDTO?> ReadOrdineAsync(int idOrdine, CancellationToken cancellationToken = default);
    public Task<string> AggiornaStatoPagamentoAsync(int idOrdine, CancellationToken cancellationToken = default);
    public Task<List<AssociaProdottoOrdineDTO>?> GetProdottiAssociatiByOrdineAsync(int idOrdine, CancellationToken cancellationToken = default);
    public Task<List<OrdineDTO>?> GetAllOrdiniAsync(CancellationToken cancellationToken = default);
}
