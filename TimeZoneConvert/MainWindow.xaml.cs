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
using System.Windows.Threading;

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
        private static DispatcherTimer _timer;
        public MainWindow()
        {
            InitializeComponent();
            // Setting up the time for the static label
            // I need to replace this with a dispatch timer
            // https://docs.microsoft.com/en-us/dotnet/api/system.windows.threading.dispatchertimer?redirectedfrom=MSDN&view=netframework-4.8
            // https://www.wpf-tutorial.com/misc/dispatchertimer/
            //var timer = new Timer(9000);
            //timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
            //timer.Enabled = false;
            //_timer = timer;
            // Adding the version number to the title
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            _timer = timer;
            MainWin.Title = "Timezone Conert version: " + Assembly.GetExecutingAssembly().GetName().Version;
            dtpInput.Value = DateTime.Now;
            setStatus("Starting to load database");
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
        private void setStatus(string strIn)
        {
            lbStatus.Content = strIn;
            _timer.Start();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            if (lbStatus.IsVisible == true)
            {
                lbStatus.Visibility = Visibility.Hidden;
            }
            _timer.Stop();
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
        /// <summary>
        /// This function takes text from the clipboard which might have characters in front of time
        /// and time in formats from iTX like extra : and the frame number or .fff where f is 100ths 
        /// of a second and puts in to the time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtPaste_Click(object sender, RoutedEventArgs e)
        {
            string strIn = Clipboard.GetText();
            // stripping extra characters at the beginning of the time string
            while (strIn.Length > 0 && !(char.IsDigit(strIn[0])))
                strIn = strIn.Substring(1);
            if (strIn.Length < 0) Xceed.Wpf.Toolkit.MessageBox.Show("Couldn't Find numbers in "
                + Clipboard.GetText(), "Time Parsing Error ", MessageBoxButton.OK, MessageBoxImage.Error);
            // now stripping exta characters at the end of the time string
            while (strIn.Length > 0 && !(char.IsDigit(strIn[strIn.Length-1])))
                strIn = strIn.Substring(0, strIn.Length - 1);
            // locate if space in string, to determine if date is yyyy-mm-dd
            int iSpace = strIn.IndexOf(' ');
            if(iSpace!=10)Xceed.Wpf.Toolkit.MessageBox.Show("Expected yyyy-mm-dd at the beginning of copied time",
                   "Time Parsing Error" + Clipboard.GetText(), MessageBoxButton.OK, MessageBoxImage.Error);
            Xceed.Wpf.Toolkit.MessageBox.Show(strIn + " from " + Clipboard.GetText(), 
                "Final string parsed", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            //Xceed.Wpf.Toolkit.MessageBox.Show("Failed to parse time",
            //       "Time Parsing Error" + Clipboard.GetText(), MessageBoxButton.OK, MessageBoxImage.Error);

        }
    }
}
