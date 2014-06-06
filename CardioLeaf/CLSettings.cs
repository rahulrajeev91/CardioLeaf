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
        public static int GraphWidth
        {
            get { return _graphWidth; }
            set { 
                if (value>100 && value<2000)
                    _graphWidth = value; 
            }
        }
        
    }
}
