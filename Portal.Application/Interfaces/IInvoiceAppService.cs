using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Portal.Application.DTO;

namespace Portal.Application.Interfaces
{
    public interface IInvoiceAppService
    {
        Task<IEnumerable<InvoiceReadDto>> GetAllAsync();
        Task<InvoiceReadDto?> GetByIdAsync(Guid id);
        Task<InvoiceReadDto> CreateAsync(InvoiceCreateDto dto);
        Task<bool> UpdateAsync(Guid id, InvoiceUpdateDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}