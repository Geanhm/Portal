using Portal.Domain.Entities;
using Portal.Domain.Entities.Enums;
using Portal.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace Portal.Tests.Domain
{
    public class VendedorTests
    {
        [Fact]
        public void UpdateVendedor_TelefoneVazio_DeveLimparParaNulo()
        {
            var vendedor = new Vendedor("Nome", "123", "e@e.com", "9999", 10);

            vendedor.UpdateVendedor(null, null, null, "", null, null);

            Assert.Null(vendedor.Telefone);
        }
    }
}
