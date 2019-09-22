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
        private List<TimeZone> lTimeZones;
        private List<string> lTZgroups;
        public MainWindow()
        {
            InitializeComponent();
            // Adding the version number to the title
            MainWin.Title = "Timezone Conert version: " + Assembly.GetExecutingAssembly().GetName().Version;
            dtpInput.Value = DateTime.Now;
            Xceed.Wpf.Toolkit.MessageBox.Show("Starting to get database", "Hi", MessageBoxButton.OK);
            myReadSQLite = new ReadSQLite();
            if (!myReadSQLite.GotDB())
                MessageBox.Show("Didn't get database");
            else
            {
                if (loadTimezones(0))
                {
                    UpdateTimezones();
                }
                if (loadFormats())
                {
                    cbSelect.SelectedIndex = 0;
                }
                if(loadTZGroups())
                {
                    cbTZGroup.SelectedIndex = 0;
                }
                string strdb = myReadSQLite.GetVersion();
                MainWin.Title += " DB version " + strdb;

            }
        }
        private bool loadFormats()
        {
            lOutformat = myReadSQLite.GetOutputFormats();
            if (lOutformat.Count > 0)
            {
                foreach (OutputFormat ouf in lOutformat)
                {
                    cbSelect.Items.Add(ouf.GetTitle());
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// When you input a group number this gets a list of Timezones
        /// </summary>
        /// <param name="iGroupNum">Group number from 1 to max numbers (adding 1 to selections for now)</param>
        /// <returns>Return False if list doesn't have at least 4 timezones</returns>
        private bool loadTimezones(int iGroupNum)
        {
            if(lTimeZones!=null) lTimeZones.Clear();
            lTimeZones = myReadSQLite.GetTimezones(iGroupNum);
            if (lTimeZones.Count > 3) return true;
            // I'm thinking of adding this after I load the groups with the count of the groups
            return false;
        }
        private bool loadTZGroups()
        {
            lTZgroups = myReadSQLite.GetTZGroups();
            if (lTZgroups.Count > 0)
            {
                foreach(string s in lTZgroups)
                {
                    cbTZGroup.Items.Add(s);
                }
                return true;
            }
            return false;
        }
        private void UpdateTimezones()
        {
            // This function is called often in setup functions before the list of timezones is complete
            if ((lTimeZones != null)&&(lTimeZones.Count>3))
            {
                // First update labels
                lbl1.Content = lTimeZones[0].GetTitle();
                lbl2.Content = lTimeZones[1].GetTitle();
                lbl3.Content = lTimeZones[2].GetTitle();
                lbl4.Content = lTimeZones[3].GetTitle();
                // Second computer the times
                DateTime DT = (DateTime)dtpInput.Value;
                tbReformatedTime.Text = tbPrefix.Text + DT.ToString(tbTimeFormat.Text) + tbSuffix.Text;
                DateTime tempDT = new DateTime();
                tempDT = DT.AddHours((double)(Convert.ToDouble(lTimeZones[0].GetValueX10()) / 10.0));
                rtbOut1.Text = tbPrefix.Text + tempDT.ToString(tbTimeFormat.Text) + tbSuffix.Text;
                tempDT = DT.AddHours((double)(Convert.ToDouble(lTimeZones[1].GetValueX10()) / 10.0));
                rtbOut2.Text = tbPrefix.Text + tempDT.ToString(tbTimeFormat.Text) + tbSuffix.Text;
                tempDT = DT.AddHours((double)(Convert.ToDouble(lTimeZones[2].GetValueX10()) / 10.0));
                rtbOut3.Text = tbPrefix.Text + tempDT.ToString(tbTimeFormat.Text) + tbSuffix.Text;
                tempDT = DT.AddHours((double)(Convert.ToDouble(lTimeZones[3].GetValueX10()) / 10.0));
                rtbOut4.Text = tbPrefix.Text + tempDT.ToString(tbTimeFormat.Text) + tbSuffix.Text;
            }
        }

        private void DtpInput_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            UpdateTimezones();
        }

        private void GetOutFormats()
        {
            lOutformat = myReadSQLite.GetOutputFormats();

        }
        private void BtnCopy1_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(rtbOut1.Text);
        }
        private void BtnCopy2_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(rtbOut2.Text);
        }
        private void BtnCopy3_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(rtbOut3.Text);
        }
        private void BtnCopy4_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(rtbOut4.Text);
        }
        private void CbSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tbTimeFormat.Text= lOutformat[cbSelect.SelectedIndex].GetTimeFormat();
            tbPrefix.Text = lOutformat[cbSelect.SelectedIndex].GetPrefix();
            tbSuffix.Text = lOutformat[cbSelect.SelectedIndex].GetSuffix();
            UpdateTimezones();
        }

        private void CbTZGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            loadTimezones(cbTZGroup.SelectedIndex + 1);
            UpdateTimezones();

        }

        private void BtnCopyFormattedTime_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(tbReformatedTime.Text);
        }

        private void BtPaste_Click(object sender, RoutedEventArgs e)
        {
            string strIn = Clipboard.GetText();
            while (strIn.Length > 0 && !(char.IsDigit(strIn[0])))
                strIn = strIn.Substring(1);
            if (strIn.Length < 0) Xceed.Wpf.Toolkit.MessageBox.Show("Couldn't Find numbers",
                   "Time Parsing Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Xceed.Wpf.Toolkit.MessageBox.Show("Failed to parse time",
                   "Time Parsing Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
