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
            if (!string.IsNullOrEmpty(SearchTextBox.Text))
            {
                string[] searchTerms = SearchTextBox.Text.ToLower().Split(' ', System.StringSplitOptions.RemoveEmptyEntries); 

                var list = products.Where(product =>
                {
                    string[] productFilds =
                    [
                        product.Tie
                    ];

                });
            }
        }
    }
}