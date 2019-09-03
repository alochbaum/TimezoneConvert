using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeZoneConvert
{
    class OutputFormat
    {
        private string Title;
        private string Prefix;
        private string TimeFormat;
        private string Suffix;
        public OutputFormat(string Title,string Prefix,string TimeFormat,string Suffix)
        {
            this.SetTitle(Title);
            this.Prefix = Prefix;
            this.TimeFormat = TimeFormat;
            this.Suffix = Suffix;
        }

        public string GetTitle()
        {
            return Title;
        }

        public void SetTitle(string value)
        {
            this.Title = value;
        }

    }
}
