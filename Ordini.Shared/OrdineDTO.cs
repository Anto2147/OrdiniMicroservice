namespace Ordini.Shared;

public class OrdineDTO
{   
    public int Id { get; set; }
    public int IdCliente { get; set; }
    public bool StatoPagamento { get; set; }
    public decimal TotaleDaPagare { get; set; }
}
