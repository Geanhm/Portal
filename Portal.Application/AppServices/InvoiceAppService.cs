using Microsoft.EntityFrameworkCore;
using Portal.Application.DTO;
using Portal.Application.Interfaces;
using Portal.Domain.Entities;
using Portal.Domain.Entities.Enums;
using Portal.Domain.Validators;
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
                    DataEmissao = i.DataEmissao,
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
                DataEmissao = i.DataEmissao,
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
            var vendedor = await _db.Vendedores.FindAsync(dto.VendedorId);
            if (vendedor == null)
                throw new BusinessException("Vendedor não encontrado.");

            if (vendedor.Status == StatusAtivoInativo.Inativo)
                throw new BusinessException("Não é possível criar faturas para um vendedor inativo.");

            var invoice = new Invoice(dto.VendedorId, 
                                        dto.Cliente, 
                                        dto.ClienteDocumento, 
                                        dto.ValorTotal, 
                                        dto.Observacoes, 
                                        vendedor);


            _db.Invoices.Add(invoice);
            await _db.SaveChangesAsync();

            return new InvoiceReadDto
            {
                Id = invoice.Id,
                Number = invoice.Number,
                DataEmissao = invoice.DataEmissao,
                VendedorId = invoice.VendedorId,
                Cliente = invoice.Cliente,
                ClienteDocumento = invoice.ClienteDocumento,
                ValorTotal = invoice.ValorTotal,
                Status = invoice.Status.ToString(),
                Observacoes = invoice.Observacoes
            };
        }

        public async Task UpdateAsync(Guid id, InvoiceUpdateDto dto)
        {
            var invoice = await _db.Invoices
                .Include(i => i.Comissao)
                .Include(i => i.Vendedor)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (invoice == null) throw new BusinessException("Fatura não encontrada para atualização.");

            Vendedor? novoVendedor = null;

            if (dto.VendedorId.HasValue && dto.VendedorId.Value != invoice.VendedorId)
            {
                novoVendedor = await _db.Vendedores.FindAsync(dto.VendedorId.Value);
                if (novoVendedor == null)
                    throw new BusinessException("Novo vendedor não encontrado.");

                if (novoVendedor.Status == StatusAtivoInativo.Inativo)
                    throw new BusinessException("Não é possível atualizar a fatura para um vendedor inativo.");
            }

            invoice.AtualizarDados(
                dto.Cliente,
                dto.ClienteDocumento,
                dto.ValorTotal,
                dto.Observacoes,
                novoVendedor);

            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var invoice = await _db.Invoices.FindAsync(id);
            if (invoice == null) throw new BusinessException("Fatura não encontrada para exclusão.");

            _db.Invoices.Remove(invoice);
            await _db.SaveChangesAsync();
        }

        public async Task ApproveAsync(Guid id)
        {
            var invoice = await _db.Invoices.FindAsync(id);
            if (invoice == null) throw new BusinessException("Fatura não encontrada para aprovação.");

            invoice.AprovarInvoice();
            await _db.SaveChangesAsync();
        }

        public async Task CancelAsync(Guid id)
        {
            var invoice = await _db.Invoices
                .Include(i => i.Comissao)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (invoice == null) throw new BusinessException("Fatura não encontrada para atualização.");
            invoice.CancelarInvoice();
            await _db.SaveChangesAsync();
        }
    }
}