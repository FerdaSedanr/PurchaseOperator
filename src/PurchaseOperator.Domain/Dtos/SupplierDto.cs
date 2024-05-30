using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseOperator.Domain.Dtos
{
    public class SupplierDto
    {
        public SupplierDto()
        {
        }

        public Guid Oid { get; set; }
        public string? Code { get; set; }
        public string? Title { get; set; }
        public string? Address { get; set; }
        public string? OtherAddress { get; set; }
        public string? DistrictCode { get; set; }
        public string? CountyCode { get; set; }
        public string? CityCode { get; set; }
        public string? CountryCode { get; set; }
        public string? Telephone { get; set; }
        public string? TaxNumber { get; set; }
        public string? TaxOffice { get; set; }
        public string? PaymentCode { get; set; }
        public string? EMail { get; set; }
        public string? TCKN { get; set; }
    }
}