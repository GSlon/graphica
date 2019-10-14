using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        short Weight = 1;

        public EditWindow()
        {
            InitializeComponent();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (Int16.TryParse(EditBox.Text, out Weight))
            {
                if (Weight <= 0)
                {
                    MessageBox.Show("Enter the integer > 0");
                }
                else
                    DialogResult = true; 
            }
            else
            {
                MessageBox.Show("Enter the integer");
            }
        }

        private void EditBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (Int16.TryParse(EditBox.Text, out Weight) || (EditBox.Text == ""))
            {
                EditBox.Background = Brushes.White;
            }
            else
                EditBox.Background = Brushes.Red;
        }

        public short GetWeight()
        {
            return Weight;            
        }
    }
}
