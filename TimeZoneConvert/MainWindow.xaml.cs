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
        private ReadSQLite myReadSQLite;
        private List<OutputFormat> lOutformat;
        private List<TimeZone> lTimeZone;
        public MainWindow()
        {
            InitializeComponent();
            // Adding the version number to the title
            MainWin.Title = "Timezone Conert version: " + Assembly.GetExecutingAssembly().GetName().Version;
            dtpInput.Value = DateTime.Now;
            SetTimes("MM/dd/yy H:mm:ss");
            myReadSQLite = new ReadSQLite();
            if (!myReadSQLite.GotDB())
                MessageBox.Show("Didn't get database");
            else loadFormats();
        }
        private void loadFormats()
        {
            lOutformat = myReadSQLite.GetOutputFormats();
            foreach (OutputFormat ouf in lOutformat)
            {
                cbSelect.Items.Add(ouf.GetTitle());
            }
        }
        private void SetTimes(string strFormat)
        {
            DateTime dt = (DateTime)dtpInput.Value;
            dt = dt.AddHours(-4.0);
            rtbOut1.Text = dt.ToString(strFormat);
            dt = dt.AddHours(-1.0);
            rtbOut2.Text = dt.ToString(strFormat);
            dt = dt.AddHours(-1.0);
            rtbOut3.Text = dt.ToString(strFormat);
            dt = dt.AddHours(-1.0);
            rtbOut4.Text = dt.ToString(strFormat);
            dt = dt.AddHours(-1.0);
        }

        private void DtpInput_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SetTimes("MM/dd/yy H:mm:ss");
        }

        private void GetOutFormats()
        {
            lOutformat = myReadSQLite.GetOutputFormats();

        }
        private void BtnCopy1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CbSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tbTimeFormat.Text= lOutformat[cbSelect.SelectedIndex].GetTimeFormat();
            tbPrefix.Text = lOutformat[cbSelect.SelectedIndex].GetPrefix();
            tbSuffix.Text = lOutformat[cbSelect.SelectedIndex].GetSuffix();
        }
        // note: the code for the original program is located in 
        // \\192.168.3.98\NextBurn\VS2010C#_Projects\TimeZoneConvert
    }
}
