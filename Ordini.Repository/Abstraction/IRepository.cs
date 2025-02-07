using Ordini.Repository.Model;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Ordini.Shared;

namespace Ordini.Repository.Abstraction
{
    public interface IRepository
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        // Cliente
        Task CreateClienteAsync(string nome, string cognome, string email, string telefono, CancellationToken cancellationToken = default);
        Task<Cliente?> ReadClienteAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Cliente>> GetAllClientiAsync(CancellationToken cancellationToken = default);
        Task DeleteClienteAsync(int id, CancellationToken cancellationToken = default);

        // Ordine
        Task CreateOrdineAsync(int idCliente, CancellationToken cancellationToken = default);
        Task<Ordine?> ReadOrdineAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Ordine>> GetAllOrdiniByClienteAsync(int idCliente, CancellationToken cancellationToken = default);
        Task DeleteOrdineAsync(int id, CancellationToken cancellationToken = default);

        // AssociaProdottoOrdine
        Task CreateAssociaProdottoOrdineAsync(AssociaProdottoOrdineDTO associaProdottoOrdineDTO, CancellationToken cancellationToken = default);
        Task<AssociaProdottoOrdine?> ReadAssociaProdottoOrdineAsync(int id, CancellationToken cancellationToken = default);
        Task DeleteAssociaProdottoOrdineAsync(int id, CancellationToken cancellationToken = default);

        public Task<List<AssociaProdottoOrdine>> GetProdottiAssociatiByOrdineAsync(int idOrdine, CancellationToken cancellationtoken = default);
        public Task<List<Ordine>> GetAllOrdiniAsync(CancellationToken cancellationToken = default);
    }
}
