
using Ordini.Repository.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Ordini.Repository.Abstraction;
using Ordini.Shared;

namespace Ordini.Repository
{
    public class Repository(OrdiniDbContext ordiniDbContext) : IRepository
    {
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await ordiniDbContext.SaveChangesAsync(cancellationToken);
        }

        // Cliente
        public async Task CreateClienteAsync(string nome, string cognome, string email, string telefono, CancellationToken cancellationToken = default)
        {
            Cliente cliente = new() { Nome = nome, Cognome = cognome, Email = email, Telefono = telefono };
            await ordiniDbContext.Clienti.AddAsync(cliente, cancellationToken);
        }

        public async Task<Cliente?> ReadClienteAsync(int id, CancellationToken cancellationToken = default)
        {
            return await ordiniDbContext.Clienti.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<List<Cliente>> GetAllClientiAsync(CancellationToken cancellationToken = default)
        {
            return await ordiniDbContext.Clienti.ToListAsync(cancellationToken);
        }

        public async Task DeleteClienteAsync(int id, CancellationToken cancellationToken = default)
        {
            var cliente = await ReadClienteAsync(id, cancellationToken);
            if (cliente != null)
            {
                ordiniDbContext.Clienti.Remove(cliente);
            }
        }

        // Ordini
        public async Task CreateOrdineAsync(int idCliente, CancellationToken cancellationToken = default)
        {
            Ordine ordine = new () { IdCliente = idCliente, StatoPagamento = false, TotaleDaPagare = 0 };
            await ordiniDbContext.Ordini.AddAsync(ordine, cancellationToken);
        }

        public async Task<Ordine?> ReadOrdineAsync(int id, CancellationToken cancellationToken = default)
        {
            return await ordiniDbContext.Ordini.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        public async Task<List<Ordine>> GetAllOrdiniByClienteAsync(int idCliente, CancellationToken cancellationToken = default)
        {
            return await ordiniDbContext.Ordini.Where(o => o.IdCliente == idCliente).ToListAsync(cancellationToken);
        }

        public async Task DeleteOrdineAsync(int id, CancellationToken cancellationToken = default)
        {
            var ordine = await ReadOrdineAsync(id, cancellationToken);
            if (ordine != null)
            {
                ordiniDbContext.Ordini.Remove(ordine);
            }
        }

        // AssociaProdottoOrdine
        public async Task CreateAssociaProdottoOrdineAsync(AssociaProdottoOrdineDTO associaProdottoOrdineDTO, CancellationToken cancellationToken = default)
        {
            var ordine = await ReadOrdineAsync(associaProdottoOrdineDTO.IdOrdine, cancellationToken);
            if (ordine == null) throw new KeyNotFoundException("Ordine non trovato.");

            AssociaProdottoOrdine associazione = new AssociaProdottoOrdine()
            {
                IdOrdine = associaProdottoOrdineDTO.IdOrdine,
                NomeProdotto = associaProdottoOrdineDTO.NomeProdotto,
                QuantitaOrdinata = associaProdottoOrdineDTO.QuantitaOrdinata,
            };
            await ordiniDbContext.AssociaProdottiOrdine.AddAsync(associazione, cancellationToken);
        }

        public async Task<AssociaProdottoOrdine?> ReadAssociaProdottoOrdineAsync(int id, CancellationToken cancellationToken = default)
        {
            return await ordiniDbContext.AssociaProdottiOrdine.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }

        public async Task DeleteAssociaProdottoOrdineAsync(int id, CancellationToken cancellationToken = default)
        {
            var associa = await ReadAssociaProdottoOrdineAsync(id, cancellationToken);
            if (associa == null){throw new InvalidOperationException($"Non trovata l'aggiunta al carrello");}

            ordiniDbContext.AssociaProdottiOrdine.Remove(associa);
          
        }

        public async Task<List<AssociaProdottoOrdine>> GetProdottiAssociatiByOrdineAsync(int idOrdine, CancellationToken cancellationtoken = default)
        {
            return await ordiniDbContext.AssociaProdottiOrdine.Where(a => a.IdOrdine == idOrdine).ToListAsync(); 
        }
        public async Task<List<Ordine>> GetAllOrdiniAsync(CancellationToken cancellationToken = default)
        {
            return await ordiniDbContext.Ordini.ToListAsync(); 
        }
    }
}
