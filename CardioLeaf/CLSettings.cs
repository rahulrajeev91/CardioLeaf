using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardioLeaf
{
    public static class CLSettings
    {
        private static int _dataRate = 256;
        private static int _graphWidth = 300;
        private static int _tempWidth = 50;
        private static int _HRWidth = 100;
        private static int _HRLead = 4;         //select shich lead to use for HR calculation




        public static int DataRate
        {
            get { return _dataRate; }
            set
            {
                if (value > 0 && value < 500)
                    _dataRate = value;
            }
        }
        
        
        public static int GraphWidth
        {
            get { return _graphWidth; }
            set { 
                if (value>0 && value<2000)
                    _graphWidth = value; 
            }
        }


        public static int tempGraphWidth
        {
            get { return _tempWidth; }
            set
            {
                if (value > 0 && value < 1000)
                    _tempWidth = value;
            }
        }

        public static int HRGraphWidth
        {
            get { return _HRWidth; }
            set
            {
                if (value > 0 && value < 1000)
                    _HRWidth = value;
            }
        }

        public static int HRLead
        {
            get { return _HRLead; }
            set
            {
                if (value > 0 && value < 12)
                    _HRLead = value;
            }
        }

        public static int PPGLead { get; set; }

        public static int RRLead { get; set; }
    }
}
