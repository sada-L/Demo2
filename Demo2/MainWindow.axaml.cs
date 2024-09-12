using Avalonia.Controls;
using Demo2.Context;
using Demo2.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Demo2
{
    public partial class MainWindow : Window
    {
        private List<Product> products = Helper.DataBase.Products.Include(x => x.Attachedproducts).ToList();
        private List<Manufacturer> manufacturers = Helper.DataBase.Manufacturers.ToList();
        public MainWindow()
        {
            InitializeComponent();

            SetFilterBox();
            InitList();

            AllTextBlock.Text = products.Count.ToString();
        }

        private void InitList()
        {
            if (SearchTextBox == null || FilterComboBox == null || SortComboBox == null) return;

            var list = products;

            list = FilterComboBox.SelectedIndex switch
            {
                0 => list,
                _ => list.Where(x => x.Manufacturerid == ((Manufacturer)FilterComboBox.SelectedItem!).Id).ToList(),
            };

            list = SortComboBox.SelectedIndex switch
            {
                1 => list.OrderByDescending(x => x.Cost).ToList(),
                2 => list.OrderBy(x => x.Cost).ToList(),
                _ => list,
            };

            if (!string.IsNullOrEmpty(SearchTextBox.Text))
            {
                string[] searchTerms = SearchTextBox.Text.ToLower().Split(' ', System.StringSplitOptions.RemoveEmptyEntries); 

                list = list.Where(product =>
                {
                    string[] productFilds =
                    [
                        product.Title.ToLower(),
                        product.Description!.ToLower()
                    ];

                    return searchTerms.Any(term => productFilds.Any(field => field.Contains(term)));
                }).ToList();
            }
            CurrentTextBlock.Text = list.Count.ToString();
            ProductsListBox.ItemsSource = null;
            ProductsListBox.ItemsSource = list;
        }

        private void SetFilterBox()
        {
            var list = new List<Manufacturer>() { new Manufacturer() { Id = 0, Name = "Все производители"} };
            list.AddRange(manufacturers);

            FilterComboBox.ItemsSource = list;
            FilterComboBox.SelectedIndex = 0;
            FilterComboBox.SelectedItem = list[0];
        }

        private void TextBox_TextChanged(object? sender, Avalonia.Controls.TextChangedEventArgs e) => InitList();

        private void ComboBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e) => InitList();

        private void Button_Click_History(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            HistoryWindow historyWindow = new HistoryWindow((Product)ProductsListBox.SelectedItem!);
            historyWindow.Show();
            Close();
        }

        private void Button_Click_Add(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            ProductWindow productWindow = new ProductWindow();  
            productWindow.Show();
            Close();
        }

        private void Button_Click_Edit(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            ProductWindow productWindow = new ProductWindow((Product)ProductsListBox.SelectedItem!);
            productWindow.Show();
            Close();
        }
    }
}