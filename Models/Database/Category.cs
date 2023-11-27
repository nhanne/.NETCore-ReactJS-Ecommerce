using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Clothings_Store.Models.Database;

public partial class Category
{
    public int Id { get; set; }

    public string Code { get; set; }

    [Required(ErrorMessage = "Tên danh mục là bắt buộc.")]
    public string Name { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
