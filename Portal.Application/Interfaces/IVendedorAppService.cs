using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Portal.Application.DTO;

namespace Portal.Application.Interfaces
{
    public interface IVendedorAppService
    {
        Task<IEnumerable<VendedorReadDto>> GetAllAsync();
        Task<VendedorReadDto?> GetByIdAsync(Guid id);
        Task<VendedorReadDto> CreateAsync(VendedorCreateDto dto);
        Task<bool> UpdateAsync(Guid id, VendedorUpdateDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}