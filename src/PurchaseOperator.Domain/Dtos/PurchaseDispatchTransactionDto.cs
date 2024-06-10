namespace PurchaseOperator.Domain.Dtos
{
    public class PurchaseDispatchTransactionDto
    {
        public PurchaseDispatchTransactionDto()
        {
            Lines = new List<PurchaseDispatchTransactionLineDto>();
        }

        public IList<PurchaseDispatchTransactionLineDto> Lines { get; set; }
        public Guid Owner { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now;
        public string? Code { get; set; } = string.Empty;
        public short? GroupType { get; set; } = 3;
        public short? IOType { get; set; } = 0;
        public short? TransactionType { get; set; } = 0;
        public int? WarehouseNumber { get; set; }
        public int? CurrentReferenceId { get; set; }
        public string CurrentCode { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public double Total { get; set; } = default;
        public double TotalVat { get; set; } = default;
        public double NetTotal { get; set; } = default;
        public string? SpeCode { get; set; } = string.Empty;
        public short? IsEDispatch { get; set; } = default;
        public short? IsEInvoice { get; set; } = default;
        public int? EDispatchProfileId { get; set; }
        public int? EInvoiceProfileId { get; set; }
        public string? DoCode { get; set; } = string.Empty;
        public string? DocTrackingNumber { get; set; } = string.Empty;
    }
}