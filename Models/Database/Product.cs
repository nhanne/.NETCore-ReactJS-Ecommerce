using System.ComponentModel.DataAnnotations;

namespace Clothings_Store.Models.Database;

public partial class Product
{
    public int Id { get; set; }

    public int? CategoryId { get; set; }

    public string Picture { get; set; }

    [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
    public string Name { get; set; }

    public string Code { get; set; }

    public double CostPrice { get; set; }

    public double UnitPrice { get; set; }

    public int? Sold { get; set; }

    public int? Sale { get; set; }

    public DateTime? StockInDate { get; set; }

    public virtual Category Category { get; set; }

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
