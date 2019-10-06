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

namespace TimeZoneConvert
{
    /// <summary>
    /// Interaction logic for winPick100ths.xaml
    /// </summary>
    public partial class winPick100ths : Window
    {
        private int _iValue;
        public winPick100ths(int iValue)
        {
            InitializeComponent();
            this.Owner = App.Current.MainWindow;
            updateValue(iValue);
        }
        public int GetValue()
        {
            return _iValue;
        }
        private void updateValue(int iUpdate)
        {
            _iValue = iUpdate;
            // checking range
            if (_iValue < 0) _iValue = 0;
            if (_iValue > 999) _iValue = 999;
            lbvalue.Content = _iValue.ToString();
            sldValue.Value = _iValue;
        }

        private void SldValue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            updateValue((int)sldValue.Value);
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Btn0_Click(object sender, RoutedEventArgs e)
        {
            updateValue(0);
        }
    }
}
