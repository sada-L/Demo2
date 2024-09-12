using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Demo2.Context;
using Demo2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Demo2;

public partial class ProductWindow : Window
{
    private Product CurrentProduct;
    List<Product> products = Helper.DataBase.Products.ToList();
    public ProductWindow()
    {
        InitializeComponent();

        CurrentProduct = new Product();

        ManufComboBox.ItemsSource = Helper.DataBase.Manufacturers.ToList();
        ManufComboBox.SelectedIndex = 0;
        ProductPanel.DataContext = CurrentProduct;
        AttachedListBox.ItemsSource = products;
    }
    public ProductWindow(Product product)
    {
        InitializeComponent();

        CurrentProduct = product;

        ManufComboBox.ItemsSource = Helper.DataBase.Manufacturers.ToList();
        ProductPanel.DataContext = CurrentProduct;
        AttachedListBox.ItemsSource = products;
    }

    private async void Button_Click_AddPhoto(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog();
        var result = await openFileDialog.ShowAsync(this);

        if (result?.Length > 0)
        {
            var filePath = result[0];
            var appDir = AppContext.BaseDirectory;
            var fileName = Path.GetFileName(filePath);
            var destinationPath = Path.Combine(appDir, "Assets", fileName);
            Directory.CreateDirectory(Path.Combine(appDir, "Assets"));
            if (CurrentProduct.Mainimagepath != null)
            {
                try
                {
                    File.Delete(Path.Combine(appDir, "Assets", CurrentProduct.Mainimagepath));
                }
                catch (Exception error)
                {
                    Console.WriteLine(error);
                    throw;
                }
            }
            File.Copy(filePath, destinationPath, true);

            CurrentProduct.Mainimagepath = fileName;
            this.Image.Source = new Bitmap(destinationPath);
        }
    }
}