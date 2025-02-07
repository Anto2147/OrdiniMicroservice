using Microsoft.EntityFrameworkCore;
using Ordini.Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordini.Repository;

public class OrdiniDbContext(DbContextOptions<OrdiniDbContext> dbContextOptions) : DbContext(dbContextOptions)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurazione per la tabella Cliente
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(c => c.Id);  // Chiave primaria
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
            entity.Property(c => c.Nome).IsRequired().HasMaxLength(50);
            entity.Property(c => c.Cognome).IsRequired().HasMaxLength(50);
            entity.Property(c => c.Email).HasMaxLength(100);
            entity.Property(c => c.Telefono).HasMaxLength(20);

            // Relazione uno a molti tra Cliente e Ordine
            entity.HasMany(c => c.Ordini)    // Un Cliente può avere molti Ordini
                  .WithOne(o => o.Cliente)   // Un Ordine ha un solo Cliente
                  .HasForeignKey(o => o.IdCliente)
                  .OnDelete(DeleteBehavior.Cascade);  // Eliminando un Cliente elimino anche gli ordini associati
        });

        // Configurazione per la tabella Ordine
        modelBuilder.Entity<Ordine>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(o => o.Id).ValueGeneratedOnAdd();// Chiave primaria dell'Ordine
            entity.Property(o => o.StatoPagamento).IsRequired();
            entity.Property(o => o.TotaleDaPagare).IsRequired();

            // Relazione con Cliente
            entity.HasOne(o => o.Cliente)               // Un Ordine ha un solo Cliente
                  .WithMany(c => c.Ordini)             // Un Cliente può avere più Ordini
                  .HasForeignKey(o => o.IdCliente)    // La chiave esterna è 'IdCliente'
                  .OnDelete(DeleteBehavior.Cascade);  

            // Relazione con AssociaProdottoOrdine
            entity.HasMany(o => o.AssociaProdottiOrdine)    // Un Ordine può avere più Associazioni Prodotti
                  .WithOne(a => a.Ordine)                 // Un AssociaProdottoOrdine è associato a un solo Ordine
                  .HasForeignKey(a => a.IdOrdine)         // La chiave esterna è 'IdOrdine'
                  .OnDelete(DeleteBehavior.Cascade);      
        });

        // Configurazione per la tabella AssociaProdottoOrdine
        modelBuilder.Entity<AssociaProdottoOrdine>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Id).ValueGeneratedOnAdd();// Chiave primaria 
            entity.Property(a => a.NomeProdotto).IsRequired().HasMaxLength(100);
            entity.Property(a => a.QuantitaOrdinata).IsRequired();

            // Relazione con Ordine
            entity.HasOne(a => a.Ordine)               // Un AssociaProdottoOrdine è associato a un solo Ordine
                  .WithMany(o => o.AssociaProdottiOrdine)  // Un Ordine può avere più Associazioni Prodotti
                  .HasForeignKey(a => a.IdOrdine)        // La chiave esterna è 'IdOrdine'
                  .OnDelete(DeleteBehavior.Cascade);     
        });
    }
    public DbSet<Cliente> Clienti { get; set; }
    public DbSet<Ordine> Ordini { get; set; }
    public DbSet<AssociaProdottoOrdine> AssociaProdottiOrdine { get; set; }

}
