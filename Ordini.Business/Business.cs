using Ordini.Business.Abstraction;
using Ordini.Repository.Abstraction;
using Ordini.Repository.Model;
using Ordini.Business.Abstraction;
using Ordini.Repository.Abstraction;
using Ordini.Shared;
using Microsoft.Extensions.Logging;
using Magazzino.Shared;
using Magazzino.ClientHttp.Abstraction;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace Ordini.Business
{
    public class Business(IRepository repository, IClientMagazzino clientMagazzino, ILogger<Business> logger) : IBusiness
    {
        // Cliente 
        public async Task CreateClienteAsync(ClienteDTO clienteDTO, CancellationToken cancellationToken = default)
        {
            await repository.CreateClienteAsync(clienteDTO.Nome, clienteDTO.Cognome, clienteDTO.Email, clienteDTO.Telefono, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);
        }

        public async Task<ClienteDTO?> ReadClienteAsync(int id, CancellationToken cancellationToken = default)
        {
            var cliente = await repository.ReadClienteAsync(id, cancellationToken);

            if (cliente == null)
                return null;

            return new ClienteDTO
            {
                Id = cliente.Id,
                Nome = cliente.Nome,
                Cognome = cliente.Cognome,
                Email = cliente.Email,
                Telefono = cliente.Telefono
            };
        }

        public async Task<List<ClienteDTO>?> GetAllClientiAsync(CancellationToken cancellationToken = default)
        {
            var clienti = await repository.GetAllClientiAsync(cancellationToken);

            if (clienti == null)
                return new List<ClienteDTO>();

            return clienti.Select(cliente => new ClienteDTO
            {
                Id = cliente.Id,
                Nome = cliente.Nome,
                Cognome = cliente.Cognome,
                Email = cliente.Email,
                Telefono = cliente.Telefono
            }).ToList();
        }

        public async Task DeleteClienteAsync(int id, CancellationToken cancellationToken = default)
        {
            await repository.DeleteClienteAsync(id, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);
        }

        // Ordine
        public async Task CreateOrdineAsync(OrdineDTO ordineDTO, CancellationToken cancellationToken = default)
        {
            await repository.CreateOrdineAsync(ordineDTO.IdCliente, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);
        }

        public async Task<OrdineDTO?> ReadOrdineAsync(int id, CancellationToken cancellationToken = default)
        {
            var ordine = await repository.ReadOrdineAsync(id, cancellationToken);

            if (ordine == null)
                return null;

            return new OrdineDTO
            {
                Id = ordine.Id,
                IdCliente = ordine.IdCliente,
                StatoPagamento = ordine.StatoPagamento,
                TotaleDaPagare = ordine.TotaleDaPagare
            };
        }

        public async Task<List<OrdineDTO>?> GetAllOrdiniByClienteAsync(int idCliente, CancellationToken cancellationToken = default)
        {
            var ordini = await repository.GetAllOrdiniByClienteAsync(idCliente, cancellationToken);

            if (ordini == null)
                return new List<OrdineDTO>();

            return ordini.Select(ordine => new OrdineDTO
            {
                Id = ordine.Id,
                IdCliente = ordine.IdCliente,
                StatoPagamento = ordine.StatoPagamento,
                TotaleDaPagare = ordine.TotaleDaPagare
            }).ToList();
        }

        public async Task DeleteOrdineAsync(int id, CancellationToken cancellationToken = default)
        {
            await repository.DeleteOrdineAsync(id, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);
        }

        // AssociaProdottoOrdine
        public async Task CreateAssociaProdottoOrdineAsync(AssociaProdottoOrdineDTO associaProdottoOrdineDTO, CancellationToken cancellationToken = default)
        {
            if (await clientMagazzino.IsDisponibileAsync(associaProdottoOrdineDTO.NomeProdotto, associaProdottoOrdineDTO.QuantitaOrdinata, cancellationToken) == true)
            {
                var ordine = await repository.ReadOrdineAsync(associaProdottoOrdineDTO.IdOrdine, cancellationToken); //provare con metodo business

                if (ordine == null)
                {
                    throw new InvalidOperationException($"Ordine con ID {associaProdottoOrdineDTO.IdOrdine} non trovato.");
                }
                await repository.CreateAssociaProdottoOrdineAsync(associaProdottoOrdineDTO, cancellationToken);

                //aggiornamento quantita disponibile in Magazzino
                Console.WriteLine($"START richiesta di prenotazione di {associaProdottoOrdineDTO.QuantitaOrdinata} prodotti al magazzino {associaProdottoOrdineDTO.NomeProdotto}");
                await clientMagazzino.PrenotazioneCarrelloAsync(associaProdottoOrdineDTO.NomeProdotto, associaProdottoOrdineDTO.QuantitaOrdinata, cancellationToken);

                await repository.SaveChangesAsync(cancellationToken);

                // Aggiorno il totale dell'ordine
                var prodotto = await clientMagazzino.ReadProdottoAsync(associaProdottoOrdineDTO.NomeProdotto, cancellationToken);
                if (prodotto == null)
                {
                    throw new InvalidOperationException($"Prodotto '{associaProdottoOrdineDTO.NomeProdotto}' non trovato.");
                }

                Console.WriteLine($"START richiesta di aggiornamento Totale da Pagare ");
                ordine.TotaleDaPagare += associaProdottoOrdineDTO.QuantitaOrdinata * prodotto.PrezzoUnitario; // Aggiorna totale da pagare
                Console.WriteLine($"END richiesta di aggiornamento Totale da Pagare");

                await repository.SaveChangesAsync(cancellationToken);

            }
            else { throw new InvalidOperationException($"Il prodottonon è disponibile nella quantità richiesta."); }
        }

        public async Task<AssociaProdottoOrdineDTO?> ReadAssociaProdottoOrdineAsync(int id, CancellationToken cancellationToken = default)
        {
            var associaProdotto = await repository.ReadAssociaProdottoOrdineAsync(id, cancellationToken);

            if (associaProdotto == null)
                return null;

            return new AssociaProdottoOrdineDTO
            {
                IdOrdine = associaProdotto.IdOrdine,
                NomeProdotto = associaProdotto.NomeProdotto,
                QuantitaOrdinata = associaProdotto.QuantitaOrdinata
            };
        }

        public async Task DeleteAssociaProdottoOrdineAsync(int id, CancellationToken cancellationToken = default)
        {
            AssociaProdottoOrdineDTO? associaProdottoOrdineDTO = await ReadAssociaProdottoOrdineAsync(id, cancellationToken);
            if (associaProdottoOrdineDTO == null)
            {
                throw new InvalidOperationException($"Non trovata l'aggiunta al carrello");
            }
            var ordine = await repository.ReadOrdineAsync(associaProdottoOrdineDTO.IdOrdine, cancellationToken);

            if (ordine == null)
            {
                throw new InvalidOperationException($"Ordine con ID {associaProdottoOrdineDTO.IdOrdine} non trovato.");
            }
            var prodotto = await clientMagazzino.ReadProdottoAsync(associaProdottoOrdineDTO.NomeProdotto, cancellationToken);

            if (prodotto == null)
            {
                throw new InvalidOperationException($"Prodotto '{associaProdottoOrdineDTO.NomeProdotto}' non trovato.");
            }
            await clientMagazzino.DeletePrenotazioneCarrelloAsync(associaProdottoOrdineDTO.NomeProdotto, associaProdottoOrdineDTO.QuantitaOrdinata, cancellationToken);
            // Aggiorna il totale dell'ordine
            ordine.TotaleDaPagare -= associaProdottoOrdineDTO.QuantitaOrdinata * prodotto.PrezzoUnitario;

            await repository.DeleteAssociaProdottoOrdineAsync(id, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);
        }

        public async Task AggiornaStatoPagamentoAsync(int idOrdine, CancellationToken cancellationToken = default)
        {
            var ordine = await repository.ReadOrdineAsync(idOrdine, cancellationToken);
            if (ordine == null)
            {
                throw new InvalidOperationException($"Ordine non trovato");
            }

            ordine.StatoPagamento = true;
            await repository.SaveChangesAsync(cancellationToken);
            Console.WriteLine($"salvato ordine {idOrdine}-[{ordine}]");
        }
        public async Task<List<AssociaProdottoOrdineDTO>?> GetProdottiAssociatiByOrdineAsync(int idOrdine, CancellationToken cancellationtoken)
        {
            var associati = await repository.GetProdottiAssociatiByOrdineAsync(idOrdine, cancellationtoken);

            if (associati == null)
                return new List<AssociaProdottoOrdineDTO>();

            return associati.Select(associati => new AssociaProdottoOrdineDTO
            {
                NomeProdotto=associati.NomeProdotto,
                QuantitaOrdinata = associati.QuantitaOrdinata,
                IdOrdine=associati.IdOrdine
            }).ToList();

        }

        public async Task<List<OrdineDTO>?> GetAllOrdiniAsync(CancellationToken cancellationToken = default)
        {
            var ordini = await repository.GetAllOrdiniAsync(cancellationToken);

            if (ordini == null || !ordini.Any())
            {
                return new List<OrdineDTO>();
            }

            return ordini.Select(ordine => new OrdineDTO
            {
                Id = ordine.Id,
                IdCliente = ordine.IdCliente,
                StatoPagamento = ordine.StatoPagamento,
                TotaleDaPagare = ordine.TotaleDaPagare
            }).ToList();
        }
    }
}
