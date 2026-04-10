using System;
using System.Linq;
using Portal.Domain.Entities;
using Portal.Tests.Helpers;
using Xunit;

namespace Portal.Tests
{
    public class InvoiceTests
    {
        [Fact]
        public void NewInvoice_Should_GenerateUniqueNumber_And_DefaultValues()
        {
            var inv1 = new Invoice
            {
                Cliente = "Cliente A",
                ClienteDocumento = "529.982.247-25",
                ValorTotal = 100m,
                VendedorId = Guid.NewGuid()
            };

            var inv2 = new Invoice
            {
                Cliente = "Cliente B",
                ClienteDocumento = "04.252.011/0001-10", // sample valid CNPJ
                ValorTotal = 200m,
                VendedorId = Guid.NewGuid()
            };

            Assert.False(string.IsNullOrWhiteSpace(inv1.Number));
            Assert.False(string.IsNullOrWhiteSpace(inv2.Number));
            Assert.NotEqual(inv1.Number, inv2.Number);
            Assert.NotEqual(default, inv1.IssueDate);
            Assert.Equal(InvoiceStatus.Pendente, inv1.Status);
        }

        [Fact]
        public void InvalidClienteDocumento_Should_FailValidation()
        {
            var invoice = new Invoice
            {
                Cliente = "Cliente",
                ClienteDocumento = "1234",
                ValorTotal = 10m,
                VendedorId = Guid.NewGuid()
            };

            var results = ValidationHelper.Validate(invoice);
            Assert.Contains(results, r => r.ErrorMessage?.ToLower().Contains("cnpj") == true || r.MemberNames.Contains(nameof(Invoice.ClienteDocumento)));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public void ValorTotal_NonPositive_Should_FailValidation(decimal total)
        {
            var invoice = new Invoice
            {
                Cliente = "Cliente",
                ClienteDocumento = "52998224725",
                ValorTotal = total,
                VendedorId = Guid.NewGuid()
            };

            var results = ValidationHelper.Validate(invoice);
            Assert.Contains(results, r => r.ErrorMessage?.ToLower().Contains("valor total") == true || r.MemberNames.Contains(nameof(Invoice.ValorTotal)));
        }
    }
}