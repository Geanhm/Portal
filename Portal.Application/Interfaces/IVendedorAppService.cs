using Portal.Application.DTO;

namespace Portal.Application.Interfaces
{
    public interface IVendedorAppService
    {
        Task<IEnumerable<VendedorReadDto>> GetAllAsync();
        Task<VendedorReadDto?> GetByIdAsync(Guid id);
        Task<VendedorReadDto> CreateAsync(VendedorCreateDto dto);
        Task UpdateAsync(Guid id, VendedorUpdateDto dto);
        Task DeleteAsync(Guid id);
        //Task<bool> UpdateStatusAsync(Guid id);
    }
}