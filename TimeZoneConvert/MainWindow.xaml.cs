using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
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
            // Getting config data
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            int iStatusTime = 5;
            int.TryParse(config.AppSettings.Settings["StatusTime"].Value,out iStatusTime);
            // I'm using a dispatch timer
            // https://docs.microsoft.com/en-us/dotnet/api/system.windows.threading.dispatchertimer?redirectedfrom=MSDN&view=netframework-4.8
            // https://www.wpf-tutorial.com/misc/dispatchertimer/
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(iStatusTime);
            timer.Tick += timer_Tick;
            _timer = timer;
            // Adding the version number to the title
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
                    int iIndex = 0;
                    if(config.AppSettings.Settings["Selected"]!=null)
                    int.TryParse(config.AppSettings.Settings["Selected"].Value, out iIndex);
                    cbSelect.SelectedIndex = iIndex;
                }
                if(loadTZGroups())
                {
                    int iIndex = 0;
                    if (config.AppSettings.Settings["TZgroup"] != null)
                        int.TryParse(config.AppSettings.Settings["TZgroup"].Value, out iIndex);
                    cbTZGroup.SelectedIndex = iIndex;
                }
                string strdb = myReadSQLite.GetVersion();
                MainWin.Title += " DB version " + strdb;
                // main window opening on dropdown select group
                //btnCopyFormattedTime.Focus();

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
            lbStatus.Visibility = Visibility.Visible;
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
                // Second compute the times
                DateTime DT = (DateTime)dtpInput.Value;
                tbReformatedTime.Text = tbPrefix.Text + DT.ToString(tbTimeFormat.Text) + tbSuffix.Text;
                // Dates are immutable objects so can't use just one variable for all 4 or dates will
                // not change, I revised line here DateTime tempDT = new DateTime();
                DateTime tempDT1 = DT.AddHours((double)(Convert.ToDouble(lTimeZones[0].GetValueX10()) / 10.0));
                tbOut1.Text = tbPrefix.Text + tempDT1.ToString(tbTimeFormat.Text) + tbSuffix.Text;
                DateTime tempDT2 = DT.AddHours((double)(Convert.ToDouble(lTimeZones[1].GetValueX10()) / 10.0));
                tbOut2.Text = tbPrefix.Text + tempDT2.ToString(tbTimeFormat.Text) + tbSuffix.Text;
                DateTime tempDT3 = DT.AddHours((double)(Convert.ToDouble(lTimeZones[2].GetValueX10()) / 10.0));
                tbOut3.Text = tbPrefix.Text + tempDT3.ToString(tbTimeFormat.Text) + tbSuffix.Text;
                DateTime tempDT4 = DT.AddHours((double)(Convert.ToDouble(lTimeZones[3].GetValueX10()) / 10.0));
                tbOut4.Text = tbPrefix.Text + tempDT4.ToString(tbTimeFormat.Text) + tbSuffix.Text;
                // Third display 100ths
                tbHundreths.Text = DT.ToString("fff");
            }
        }

        private void DtpInput_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            UpdateTimezones();
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

        /// <summary>
        /// Copy buttons have btn and the rest of textbox after tb in there name
        /// So getting the button name allows us to compute the textbox to copy
        /// </summary>
        /// <param name="sender">This can only be called by buttons</param>
        /// <param name="e"></param>
        private void BtnCopy_Click(object sender, RoutedEventArgs e)
        {
            string strSender = ((Button)sender).Name;
            string strComputedObj = "tb" + strSender.Substring(3);
            string t = TZgrid.Children.OfType<TextBox>().Where(x => x.Name == strComputedObj).FirstOrDefault().Text;
            Clipboard.SetText(t);
            setStatus("Copied " + t);
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
            if (strIn.Length <= 0)
            {
                setStatus($"Couldn't Find numbers in '{Clipboard.GetText()}'");
                return;
            }
            // now stripping exta characters at the end of the time string
            while (strIn.Length > 0 && !(char.IsDigit(strIn[strIn.Length-1])))
                strIn = strIn.Substring(0, strIn.Length - 1);
            // locate if space in string, to determine if date is yyyy-mm-dd
            int iSpace = strIn.IndexOf(' ');
            if (iSpace>0 && iSpace != 10)
            {
                setStatus($"Space after yyyy-mm-dd not in '{Clipboard.GetText()}'");
                return;
            }

            DateTime DTtemp;
            CultureInfo enUS = new CultureInfo("en-US");
            if (DateTime.TryParse(strIn, out DTtemp))
            {
                dtpInput.Value = DTtemp;
                setStatus($"Pasted '{DTtemp.ToString("yyyy-mm-dd hh:MM:ss")}'");
                return;
            }

            setStatus($"Not expected format '{Clipboard.GetText()}'");
     

        }

        private void TbHundreths_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            winPick100ths _winPick = new winPick100ths(int.Parse(tbHundreths.Text));
            if(_winPick.ShowDialog()==true)
            {
                int iChange = _winPick.GetValue();
                int iOriginal = int.Parse(tbHundreths.Text);
                int iDiff = 0;
                if (iOriginal <= iChange)
                {
                    iDiff = iChange - iOriginal;
                }
                else
                {
                    iDiff = -1 * (iOriginal - iChange);
                    // subtract the difference as 100th of a second
                }
                DateTime DTtemp = ((DateTime)dtpInput.Value).AddMilliseconds(iDiff);
                dtpInput.Value = DTtemp;
                setStatus($"Modified millisconds by {iDiff}");
            }
            
        }

        private void MainWin_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Load appsettings
            Configuration config = ConfigurationManager.OpenExeConfiguration(
                                    System.Reflection.Assembly.GetExecutingAssembly().Location);
            //Check if key exists in the settings
            if (config.AppSettings.Settings["TZgroup"] != null)
            {
                //If key exists, delete it
                config.AppSettings.Settings.Remove("TZgroup");
            }
            //Add new key-value pair
            config.AppSettings.Settings.Add("TZgroup", cbTZGroup.SelectedIndex.ToString());
            //Save the selected output mode
            if (config.AppSettings.Settings["Selected"] != null)
            {
                config.AppSettings.Settings.Remove("Selected");
            }
            config.AppSettings.Settings.Add("Selected", cbSelect.SelectedIndex.ToString());
            //Save the changed settings
            config.Save(ConfigurationSaveMode.Modified);
        }
    }
}
