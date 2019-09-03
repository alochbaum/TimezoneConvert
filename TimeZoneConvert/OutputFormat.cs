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
            this.SetPrefix(Prefix);
            this.SetTimeFormat(TimeFormat);
            this.SetSuffix(Suffix);
        }

        public string GetTitle(){ return Title;}
        public void SetTitle(string value){this.Title = value;}

        public string GetPrefix() { return Prefix; }
        public void SetPrefix(string value) { this.Prefix = value; }

        public string GetTimeFormat() { return TimeFormat; }
        public void SetTimeFormat(string value) { this.TimeFormat = value; }

        public string GetSuffix() { return Suffix; }
        public void SetSuffix(string value) { this.Suffix = value; }

    }
}
