using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeZoneConvert
{
    class TimeZone
    {
        private int iGroup;
        private int iId;
        private string Title;
        private int ValueX10;
        public TimeZone( int iGroup, int iId, string Title, int ValueX10)
        {
            this.iGroup = iGroup;
            this.iId = iId;
            this.Title = Title;
            this.ValueX10 = ValueX10;
        }
        public int GetiGroup() { return iGroup; }
        public void SetiGroup(int iGroup) { this.iGroup = iGroup; }
        public int GetiID() { return iId; }
        public void SetiID(int iID) { this.iId = iId; }
        public string GetTitle() { return Title; }
        public void SetTitle(string Title) { this.Title = Title; }
        public int GetValueX10() { return ValueX10; }
        public void SetValueX10 (int ValueX10) { this.ValueX10 = ValueX10; }
    }
}
