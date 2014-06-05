using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardioLeaf
{
    class datagridSelectedData
    {
        public List<int> selectedPoints;
        public bool isBeginning,isEnd,isDataCollection;

        public datagridSelectedData()
        {
            isDataCollection = false;
            selectedPoints = new List<int>();
            isBeginning = false;
            isEnd = false;

        }
    }
}
