using Avalonia;
using Avalonia.Controls;
using Demo2.Context;
using Demo2.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Demo2;

public partial class HistoryWindow : Window
{
    List<Product> products = Helper.DataBase.Products.Include(x => x.Productsales).ToList();
    Product CurrentProduct;
   
    public HistoryWindow()
    {
        InitializeComponent();
    }

    public HistoryWindow(Product product)
    {
        InitializeComponent();
        CurrentProduct = product;
        ProductPanel.DataContext = CurrentProduct;
        ProductComboBox.ItemsSource = products;
        ProductComboBox.SelectedItem = CurrentProduct;
        SaleListBox.ItemsSource = CurrentProduct.Productsales.OrderByDescending(x => x.Saledate);
    }

    private void ComboBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        CurrentProduct = (Product)ProductComboBox.SelectedItem!;
        ProductPanel.DataContext = CurrentProduct;
        SaleListBox.ItemsSource = CurrentProduct.Productsales.OrderByDescending(x => x.Saledate);
    }

    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        Close();
    }
}