using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TimeZoneConvert
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Adding the version number to the title
            MainWin.Title = "Timezone Conert version: " + Assembly.GetExecutingAssembly().GetName().Version;
            dtpInput.Value = DateTime.Now;
            SetTimes();
        }
        private void SetTimes()
        {
            rtbOut1.Text = dtpInput.Value.ToString();
        }
        // note: the code for the original program is located in 
        // \\192.168.3.98\NextBurn\VS2010C#_Projects\TimeZoneConvert
    }
}
