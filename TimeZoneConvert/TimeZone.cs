using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeZoneConvert
{
    class TimeZone
    {
        private long iGroup;
        private long iId;
        private string Title;
        private long ValueX10;
        public TimeZone( long iGroup, long iId, string Title, long ValueX10)
        {
            this.iGroup = iGroup;
            this.iId = iId;
            this.Title = Title;
            this.ValueX10 = ValueX10;
        }
        public long GetiGroup() { return iGroup; }
        public void SetiGroup(long iGroup) { this.iGroup = iGroup; }
        public long GetiID() { return iId; }
        public void SetiID(long iId) { this.iId = iId; }
        public string GetTitle() { return Title; }
        public void SetTitle(string Title) { this.Title = Title; }
        public long GetValueX10() { return ValueX10; }
        public void SetValueX10 (long ValueX10) { this.ValueX10 = ValueX10; }
    }
}
