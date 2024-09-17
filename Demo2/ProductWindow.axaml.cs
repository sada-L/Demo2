using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Demo2.Context;
using Demo2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Demo2;

public partial class ProductWindow : Window
{
    private Product CurrentProduct;
    List<Product> products = Helper.DataBase.Products.ToList();

    ObservableCollection<Product> AttachedProductList; 
    public ProductWindow()
    {
        InitializeComponent();

        CurrentProduct = new Product();

        ManufComboBox.ItemsSource = Helper.DataBase.Manufacturers.ToList();
        ManufComboBox.SelectedIndex = 0;
        ProductPanel.DataContext = CurrentProduct;

        AttachedProductList = new ObservableCollection<Product>(CurrentProduct.Attachedproducts);
        AttachedListBox.ItemsSource = AttachedProductList;

        products.Remove(CurrentProduct);
        ProductListBox.ItemsSource = products.Where(x => x.Isactive);
    }
    public ProductWindow(Product product)
    {
        InitializeComponent();

        CurrentProduct = product;

        ManufComboBox.ItemsSource = Helper.DataBase.Manufacturers.ToList();
        ProductPanel.DataContext = CurrentProduct;

        AttachedProductList = new ObservableCollection<Product>(CurrentProduct.Attachedproducts);
        AttachedListBox.ItemsSource = AttachedProductList;

        products.Remove(CurrentProduct);
        ProductListBox.ItemsSource = products.Where(x => x.Isactive);
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

    private void Button_Click_Save(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if(CurrentProduct.Id == 0)
        {
            Helper.DataBase.Products.Add(CurrentProduct);
            Helper.DataBase.SaveChanges();
        }
        else
        {
            Helper.DataBase.Products.Update(CurrentProduct);
            Helper.DataBase.SaveChanges();
        }
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        Close();
    }

    private void Button_Click_Back(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        Close();
    }

    private void Button_Click_Add(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var product = ProductListBox.SelectedItem as Product;

        if (product != null)
        {
            CurrentProduct.Attachedproducts.Add(product);
            AttachedProductList.Add(product);
        }
    }

    private void Button_Click_Delete(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var product = ProductListBox.SelectedItem as Product;

        if (product != null)
        {
            CurrentProduct.Attachedproducts.Remove(product);
            AttachedProductList.Remove(product);
        }
    }

    private void ListBox_DoubleTapped(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        ProductWindow productWindow = new ProductWindow(AttachedListBox.SelectedItem as Product);
        productWindow.Show();
        Close();
    }

    private void ListBox_DoubleTapped1(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        ProductWindow productWindow = new ProductWindow(ProductListBox.SelectedItem as Product);
        productWindow.Show();
        Close();
    }
}