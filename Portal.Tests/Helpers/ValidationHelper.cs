using Portal.Domain.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Portal.Tests.Helpers
{
    internal static class ValidationHelper
    {
        public static IList<ValidationResult> Validate(object instance)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(instance);
            Validator.TryValidateObject(instance, context, results, validateAllProperties: true);
            return results;
        }

        public static Vendedor NovoVendedorValido()
        {
            return new Vendedor(
                nomeCompleto: "Nome Valido",
                cpf: "529.982.247-25",
                email: "email@example.com",
                telefone: "5511999999999",
                percentualComissao: 5m);
        }
    }
}