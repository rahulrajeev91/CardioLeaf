using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardioLeaf
{
    public static class CLSettings
    {

        private static int _graphWidth = 300;
        private static int _tempWidth = 50;
        private static int _HRWidth = 200;
        public static int GraphWidth
        {
            get { return _graphWidth; }
            set { 
                if (value>100 && value<2000)
                    _graphWidth = value; 
            }
        }


        public static int tempGraphWidth
        {
            get { return _tempWidth; }
            set
            {
                if (value > 10 && value < 1000)
                    _tempWidth = value;
            }
        }

        public static int HRGraphWidth
        {
            get { return _HRWidth; }
            set
            {
                if (value > 10 && value < 1000)
                    _HRWidth = value;
            }
        }
    }
}
