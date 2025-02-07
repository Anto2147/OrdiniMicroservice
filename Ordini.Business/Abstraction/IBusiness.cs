using Ordini.Repository.Model;
using Ordini.Shared;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ordini.Business.Abstraction
{
    public interface IBusiness
    {
        // Metodi per Cliente
        Task CreateClienteAsync(ClienteDTO ClienteDTO, CancellationToken cancellationToken = default);
        Task<ClienteDTO?> ReadClienteAsync(int id, CancellationToken cancellationToken = default);
        Task<List<ClienteDTO>?> GetAllClientiAsync(CancellationToken cancellationToken = default);
        Task DeleteClienteAsync(int id, CancellationToken cancellationToken = default);

        // Metodi per Ordine
        Task CreateOrdineAsync(OrdineDTO ordineDTO, CancellationToken cancellationToken = default);
        Task<OrdineDTO?> ReadOrdineAsync(int id, CancellationToken cancellationToken = default);
        Task<List<OrdineDTO>?> GetAllOrdiniByClienteAsync(int idCliente, CancellationToken cancellationToken = default);
        Task DeleteOrdineAsync(int id, CancellationToken cancellationToken = default);

        // Metodi per AssociaProdottoOrdine
        Task CreateAssociaProdottoOrdineAsync(AssociaProdottoOrdineDTO associaProdottoOrdineDTO, CancellationToken cancellationToken = default);
        Task<AssociaProdottoOrdineDTO?> ReadAssociaProdottoOrdineAsync(int id, CancellationToken cancellationToken = default);
        Task DeleteAssociaProdottoOrdineAsync(int id, CancellationToken cancellationToken = default);
        
        //Utilities
        Task AggiornaStatoPagamentoAsync(int idOrdine,CancellationToken cancellationToken = default);
        Task <List<AssociaProdottoOrdineDTO>?> GetProdottiAssociatiByOrdineAsync(int idOrdine, CancellationToken cancellationtoken = default);
        Task<List<OrdineDTO>?> GetAllOrdiniAsync(CancellationToken cancellationToken = default);
    }
}
