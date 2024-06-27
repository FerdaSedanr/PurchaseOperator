using PurchaseOperator.Domain.Models.PurchaseOrderModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PurchaseOperator.Domain.Models.PurchaseDispatchTransactionModels;

public class DispatchItem : INotifyPropertyChanged
{
    public DispatchItem()
    {
        Lines = new();
    }

    public int ReferenceId { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
    public int UnıtsetReferenceId { get; set; }
    public string UnitsetCode { get; set; } = string.Empty;
    public int SubUnitsetReferenceId { get; set; }
    public string SubUnitsetCode { get; set; } = string.Empty;
    public double DemandQuantity { get; set; } = default;

    public double SupplyChainQuantity { get; set; } = default;

    public double? TotalQuantity { get; set; } = default;

    public double? TotalWaitingQuantity { get; set; } = default;

    public double? TotalShippedQuantity { get; set; } = default;//Shipped (Satış Olduğunda Sevkedilenn, Satınalma  olduğunda Kabul edilendir

    private double customQuantity;
    public double CustomQuantity
    {
        get { return customQuantity; }
        set
        {
            customQuantity = value;
            if (customQuantity < 1)
                customQuantity = 1;

            OnPropertyChanged(nameof(UnderCountQuantity));
            OnPropertyChanged(nameof(OverOrderQuantity));
        }
    }

    private double countAmount;
    public double CountAmount
    {
        get { return countAmount; }
        set
        {
            countAmount = value;
            if(countAmount < 1)
                countAmount = 1;

            if (countAmount > customQuantity)            
                countAmount = customQuantity;
            
            OnPropertyChanged(nameof(UnderCountQuantity));
            OnPropertyChanged(nameof(OverOrderQuantity));
        }
    }

    public string ManufactureCode { get; set; } = string.Empty;

    public int LineNumber { get; set; }
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Sayım Eksiği Miktarı
    /// </summary>
    public double UnderCountQuantity => CountAmount == 0 ? 0 : CustomQuantity - CountAmount < 0 ? 0 : CustomQuantity - CountAmount;

    /// <summary>
    /// Sipariş Fazlası Miktarı
    /// </summary>
    public double OverOrderQuantity => Convert.ToDouble(CountAmount - TotalWaitingQuantity) < 0 ? 0 : Convert.ToDouble(CountAmount - TotalWaitingQuantity);

    public List<PurchaseOrderLine> Lines { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string name = null!)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        switch (name)
        {
            case nameof(CountAmount):
            case nameof(CustomQuantity):
                //UnderCountQuantity = CustomQuantity - CountAmount;
                //OverOrderQuantity = Convert.ToDouble(CountAmount - TotalWaitingQuantity);
                break;

            default:
                break;
        }
    }
}