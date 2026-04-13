using Microsoft.EntityFrameworkCore;
using Portal.Application.DTO;
using Portal.Application.Interfaces;
using Portal.Domain.Entities;
using Portal.Domain.Validators;
using Portal.Infra.Data.Repository;
using Portal.Domain.Extensions;

namespace Portal.Application.AppServices
{
    public class VendedorAppService : IVendedorAppService
    {
        private readonly PortalDbContext _db;

        public VendedorAppService(PortalDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<VendedorReadDto>> GetAllAsync()
        {
            return await _db.Vendedores
                .AsNoTracking()
                .Select(v => new VendedorReadDto
                {
                    Id = v.Id,
                    NomeCompleto = v.NomeCompleto,
                    Cpf = v.Cpf.FormatarComoCpfCnpj(),
                    Email = v.Email,
                    Telefone = v.Telefone,
                    PercentualComissao = v.PercentualComissao,
                    Status = v.Status.ToString()
                })
                .ToListAsync();
        }

        public async Task<VendedorReadDto?> GetByIdAsync(Guid id)
        {
            var v = await _db.Vendedores.FindAsync(id);
            if (v == null) return null;
            return new VendedorReadDto
            {
                Id = v.Id,
                NomeCompleto = v.NomeCompleto,
                Cpf = v.Cpf.FormatarComoCpfCnpj(),
                Email = v.Email,
                Telefone = v.Telefone,
                PercentualComissao = v.PercentualComissao,
                Status = v.Status.ToString()
            };
        }

        public async Task<VendedorReadDto> CreateAsync(VendedorCreateDto dto)
        {
            var cpfLimpo = dto.Cpf.SomenteNumeros();
            if (await _db.ExisteCpf(cpfLimpo))
                throw new BusinessException("CPF já cadastrado");

            if (await _db.ExisteEmail(dto.Email))
                throw new BusinessException("Email já cadastrado");

            var entity = new Vendedor(
                dto.NomeCompleto,
                cpfLimpo,
                dto.Email,
                dto.Telefone,
                dto.PercentualComissao
            );

            _db.Vendedores.Add(entity);
            await _db.SaveChangesAsync();

            return new VendedorReadDto
            {
                Id = entity.Id,
                NomeCompleto = entity.NomeCompleto,
                Cpf = entity.Cpf.FormatarComoCpfCnpj(),
                Email = entity.Email,
                Telefone = entity.Telefone,
                PercentualComissao = entity.PercentualComissao,
                Status = entity.Status.ToString()
            };
        }

        public async Task UpdateAsync(Guid id, VendedorUpdateDto dto)
        {
            var entity = await _db.Vendedores.FindAsync(id);
            var cpfLimpo = dto.Cpf.SomenteNumeros();

            if (entity == null)
                throw new BusinessException("Vendedor não encontrado para atualização.");

            if (!string.IsNullOrWhiteSpace(cpfLimpo) && cpfLimpo != entity.Cpf)
            {
                if (await _db.Vendedores.AnyAsync(v => v.Cpf == cpfLimpo))
                    throw new BusinessException("Este CPF já está sendo usado por outro vendedor.");
            }

            if (!string.IsNullOrWhiteSpace(dto.Email) && dto.Email != entity.Email)
            {
                if (await _db.Vendedores.AnyAsync(v => v.Email == dto.Email))
                    throw new BusinessException("Este email já está sendo usado por outro vendedor.");
            }

            entity.UpdateVendedor(
                dto.NomeCompleto,
                cpfLimpo,
                dto.Email,
                dto.Telefone,
                dto.PercentualComissao,
                dto.Status
            );

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new BusinessException("O registro foi alterado por outro usuário. Recarregue a página.");
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var vendedorInfo = await _db.Vendedores
                .Where(v => v.Id == id)
                .Select(v => new {
                    Existe = true,
                    TemComissoes = _db.Comissoes.Any(c => c.Invoice != null && c.Invoice.VendedorId == id)
                })
                .FirstOrDefaultAsync();

            if (vendedorInfo == null)
                throw new BusinessException("Vendedor não encontrado.");

            if (vendedorInfo.TemComissoes)
                throw new BusinessException("Não é possível excluir um vendedor com comissões. Favor inativar o vendedor.");

            await _db.Vendedores.Where(x => x.Id == id).ExecuteDeleteAsync();
        }
    }
}