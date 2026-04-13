using Portal.Domain.Entities.Enums;

namespace Portal.Domain.Entities
{
    public class Vendedor : BaseEntity
    {
        protected Vendedor()
        {
        }

        public Vendedor(string nomeCompleto, string cpf, string email, string? telefone, decimal percentualComissao)
        {
            NomeCompleto = nomeCompleto;
            Cpf = cpf;
            Email = email;
            Telefone = telefone;
            PercentualComissao = percentualComissao;
            Status = StatusAtivoInativo.Ativo;
        }


        public string NomeCompleto { get; private set; } = null!;
        public string Cpf { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public string? Telefone { get; private set; }
        public decimal PercentualComissao { get; private set; }
        public StatusAtivoInativo Status { get; private set; }

        public void UpdateVendedor(string? nomeCompleto, string? cpf, string? email, string? telefone, decimal? percentualComissao, StatusAtivoInativo? status)
        {
            if (!string.IsNullOrWhiteSpace(nomeCompleto)) NomeCompleto = nomeCompleto!;
            if (!string.IsNullOrWhiteSpace(cpf)) Cpf = cpf!;
            if (!string.IsNullOrWhiteSpace(email)) Email = email!;
            if (telefone != null)
            {
                Telefone = string.IsNullOrWhiteSpace(telefone) ? null : telefone;
            }
            if (percentualComissao.HasValue) PercentualComissao = percentualComissao.Value;

            if(status.HasValue) Status = status.Value;
        }

        public void Inativar() => Status = StatusAtivoInativo.Inativo;
        public void Ativar() => Status = StatusAtivoInativo.Ativo;
    }
}
