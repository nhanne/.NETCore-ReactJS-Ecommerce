namespace Clothings_Store.Models.Database;
public partial class Promotion
{
    public int PromotionId { get; set; }
    public string PromotionName { get; set; } = null!;
    public string? Description { get; set; }
    public decimal DiscountPercentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
