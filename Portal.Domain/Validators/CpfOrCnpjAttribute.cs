using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Portal.Domain.Validators
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class CpfAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return true;
            }

            if (value is not string cpf) return false;

            cpf = Regex.Replace(cpf, @"\D", "");

            if (cpf.Length != 11) return false;
            if (cpf.Distinct().Count() == 1) return false;

            int[] numbers = cpf.Select(c => c - '0').ToArray();

            int sum = 0;
            for (int i = 0; i < 9; i++)
                sum += numbers[i] * (10 - i);

            int remainder = sum % 11;
            int firstCheck = remainder < 2 ? 0 : 11 - remainder;
            if (numbers[9] != firstCheck) return false;

            sum = 0;
            for (int i = 0; i < 10; i++)
                sum += numbers[i] * (11 - i);

            remainder = sum % 11;
            int secondCheck = remainder < 2 ? 0 : 11 - remainder;
            return numbers[10] == secondCheck;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class CnpjAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return true;
            }

            if (value is not string cnpj) return false;

            cnpj = Regex.Replace(cnpj, @"\D", "");

            if (cnpj.Length != 14) return false;
            if (cnpj.Distinct().Count() == 1) return false;

            int[] numbers = cnpj.Select(c => c - '0').ToArray();

            int[] firstWeights = new int[12] {5,4,3,2,9,8,7,6,5,4,3,2};
            int[] secondWeights = new int[13] {6,5,4,3,2,9,8,7,6,5,4,3,2};

            int sum = 0;
            for (int i = 0; i < 12; i++)
                sum += numbers[i] * firstWeights[i];

            int remainder = sum % 11;
            int firstCheck = remainder < 2 ? 0 : 11 - remainder;
            if (numbers[12] != firstCheck) return false;

            sum = 0;
            for (int i = 0; i < 13; i++)
                sum += numbers[i] * secondWeights[i];

            remainder = sum % 11;
            int secondCheck = remainder < 2 ? 0 : 11 - remainder;
            return numbers[13] == secondCheck;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class CpfOrCnpjAttribute : ValidationAttribute
    {
        private readonly CpfAttribute _cpf = new();
        private readonly CnpjAttribute _cnpj = new();

        public override bool IsValid(object? value)
        {
            if (value is not string s) return false;
            var digits = Regex.Replace(s, @"\D", "");
            return digits.Length switch
            {
                11 => _cpf.IsValid(s),
                14 => _cnpj.IsValid(s),
                _ => false
            };
        }
    }
}