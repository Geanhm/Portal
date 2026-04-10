using Microsoft.EntityFrameworkCore;
using Portal.Application.DTO;
using Portal.Application.Interfaces;
using Portal.Domain.Entities;
using Portal.Domain.Entities.Enums;
using Portal.Infra.Data.Repository;

namespace Portal.Application.AppServices
{
    public class InvoiceAppService : IInvoiceAppService
    {
        private readonly PortalDbContext _db;

        public InvoiceAppService(PortalDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<InvoiceReadDto>> GetAllAsync()
        {
            return await _db.Invoices
                .AsNoTracking()
                .Select(i => new InvoiceReadDto
                {
                    Id = i.Id,
                    Number = i.Number,
                    IssueDate = i.IssueDate,
                    VendedorId = i.VendedorId,
                    Cliente = i.Cliente,
                    ClienteDocumento = i.ClienteDocumento,
                    ValorTotal = i.ValorTotal,
                    Status = i.Status.ToString(),
                    Observacoes = i.Observacoes
                })
                .ToListAsync();
        }

        public async Task<InvoiceReadDto?> GetByIdAsync(Guid id)
        {
            var i = await _db.Invoices.FindAsync(id);
            if (i == null) return null;
            return new InvoiceReadDto
            {
                Id = i.Id,
                Number = i.Number,
                IssueDate = i.IssueDate,
                VendedorId = i.VendedorId,
                Cliente = i.Cliente,
                ClienteDocumento = i.ClienteDocumento,
                ValorTotal = i.ValorTotal,
                Status = i.Status.ToString(),
                Observacoes = i.Observacoes
            };
        }

        public async Task<InvoiceReadDto> CreateAsync(InvoiceCreateDto dto)
        {
            // ensure vendedor exists
            var vendedor = await _db.Vendedores.FindAsync(dto.VendedorId);
            if (vendedor == null) throw new InvalidOperationException("Vendedor not found.");

            var invoice = new Invoice
            {
                VendedorId = dto.VendedorId,
                Cliente = dto.Cliente,
                ClienteDocumento = dto.ClienteDocumento,
                ValorTotal = dto.ValorTotal,
                Observacoes = dto.Observacoes
            };

            _db.Invoices.Add(invoice);
            await _db.SaveChangesAsync();

            return new InvoiceReadDto
            {
                Id = invoice.Id,
                Number = invoice.Number,
                IssueDate = invoice.IssueDate,
                VendedorId = invoice.VendedorId,
                Cliente = invoice.Cliente,
                ClienteDocumento = invoice.ClienteDocumento,
                ValorTotal = invoice.ValorTotal,
                Status = invoice.Status.ToString(),
                Observacoes = invoice.Observacoes
            };
        }

        public async Task<bool> UpdateAsync(Guid id, InvoiceUpdateDto dto)
        {
            var invoice = await _db.Invoices.FindAsync(id);
            if (invoice == null) return false;

            if (!string.IsNullOrWhiteSpace(dto.Cliente)) invoice.Cliente = dto.Cliente;
            if (!string.IsNullOrWhiteSpace(dto.ClienteDocumento)) invoice.ClienteDocumento = dto.ClienteDocumento;
            if (dto.ValorTotal.HasValue) invoice.ValorTotal = dto.ValorTotal.Value;
            if (!string.IsNullOrWhiteSpace(dto.Observacoes)) invoice.Observacoes = dto.Observacoes;
            if (!string.IsNullOrWhiteSpace(dto.Status) && Enum.TryParse<InvoiceStatus>(dto.Status, true, out var st))
                invoice.Status = st;

            _db.Invoices.Update(invoice);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var invoice = await _db.Invoices.FindAsync(id);
            if (invoice == null) return false;

            _db.Invoices.Remove(invoice);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}