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
        public MainWindow()
        {
            InitializeComponent();

            InitList();
        }

        private void InitList()
        {
            var list = products;

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
            ProductsListBox.ItemsSource = null;
            ProductsListBox.ItemsSource = list;
        }

        private void TextBox_TextChanged(object? sender, Avalonia.Controls.TextChangedEventArgs e) => InitList();
    }
}