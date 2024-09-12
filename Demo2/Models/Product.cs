using Avalonia.Media;
using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Demo2.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public decimal Cost { get; set; }

    public string? Description { get; set; }

    public string? Mainimagepath { get; set; }

    public bool Isactive { get; set; }

    public string IsEnable => Isactive ? "В наличии" : "Отсуствует";

    public int? Manufacturerid { get; set; }

    public Bitmap Image => File.Exists($"./Assets/{Mainimagepath}") ? new Bitmap($"./Assets/{Mainimagepath}") : null!;

    public SolidColorBrush Color => Isactive ? new SolidColorBrush(Colors.Gray) : new SolidColorBrush(Colors.White);

    public int AttachedCount => Attachedproducts.Count; 

    public virtual Manufacturer? Manufacturer { get; set; }

    public virtual ICollection<Productphoto> Productphotos { get; set; } = new List<Productphoto>();

    public virtual ICollection<Productsale> Productsales { get; set; } = new List<Productsale>();

    public virtual ICollection<Product> Attachedproducts { get; set; } = new List<Product>();

    public virtual ICollection<Product> Mainproducts { get; set; } = new List<Product>();
}
