using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
namespace PersonalFinanceMng.Domain.Entities
{
    public class PaymentMethod
    {
        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string NormalizedName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool? IsActive { get; set; }
    
        public int? UserId { get; set; }

        public PaymentMethod(int? id, string name, string? description, int? userId, bool isActive = true)
        {
            if(string.IsNullOrWhiteSpace(name)) throw new ArgumentException("O Nome do método deve ser fornecido: ", nameof(name));

            Id = id;
            SetName(name);
            Description = description;
            UserId = userId;
            IsActive = isActive;
        }


        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) { throw new ArgumentException("O Nome do método deve ser fornecido: ", nameof(name)); }
            
            Name = name.Trim();


            NormalizedName = Regex.Replace(Name.ToLowerInvariant(), @"\s+", " ");
        }

        public override bool Equals(object? obj)
        {
            if (obj is not PaymentMethod other)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (Id.HasValue && other.Id.HasValue)
                return Id.Value == other.Id.Value;

            return false;
        }

        public override int GetHashCode()
        {
            return Id.HasValue ? Id.Value.GetHashCode() : base.GetHashCode();
        }
    }
}
