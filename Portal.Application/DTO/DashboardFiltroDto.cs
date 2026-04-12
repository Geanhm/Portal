using Portal.Domain.Entities.Enums;

namespace Portal.Application.DTO
{
    public class DashboardFiltroDto
    {
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public InvoiceStatus? StatusInvoice { get; set; }
        public Guid? VendedorId { get; set; }   
    }
}