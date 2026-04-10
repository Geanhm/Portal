using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Portal.Application.DTO;
using Portal.Application.Interfaces;
using Portal.Domain.Entities;
using Portal.Domain.Entities.Enums;
using Portal.Infra.Data.Repository;

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
                    Cpf = v.Cpf,
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
                Cpf = v.Cpf,
                Email = v.Email,
                Telefone = v.Telefone,
                PercentualComissao = v.PercentualComissao,
                Status = v.Status.ToString()
            };
        }

        public async Task<VendedorReadDto> CreateAsync(VendedorCreateDto dto)
        {
            var entity = new Vendedor
            {
                NomeCompleto = dto.NomeCompleto,
                Cpf = dto.Cpf,
                Email = dto.Email,
                Telefone = dto.Telefone,
                PercentualComissao = dto.PercentualComissao,
                Status = Enum.TryParse<StatusAtivoInativo>(dto.Status, true, out var st) ? st : StatusAtivoInativo.Ativo
            };

            _db.Vendedores.Add(entity);
            await _db.SaveChangesAsync();

            return new VendedorReadDto
            {
                Id = entity.Id,
                NomeCompleto = entity.NomeCompleto,
                Cpf = entity.Cpf,
                Email = entity.Email,
                Telefone = entity.Telefone,
                PercentualComissao = entity.PercentualComissao,
                Status = entity.Status.ToString()
            };
        }

        public async Task<bool> UpdateAsync(Guid id, VendedorUpdateDto dto)
        {
            var entity = await _db.Vendedores.FindAsync(id);
            if (entity == null) return false;

            entity.NomeCompleto = dto.NomeCompleto;
            entity.Cpf = dto.Cpf;
            entity.Email = dto.Email;
            entity.Telefone = dto.Telefone;
            entity.PercentualComissao = dto.PercentualComissao;
            if (!string.IsNullOrWhiteSpace(dto.Status) && Enum.TryParse<StatusAtivoInativo>(dto.Status, true, out var st))
                entity.Status = st;

            _db.Vendedores.Update(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _db.Vendedores.FindAsync(id);
            if (entity == null) return false;
            _db.Vendedores.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}