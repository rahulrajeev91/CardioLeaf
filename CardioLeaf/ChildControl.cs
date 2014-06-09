using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardioLeaf
{
    interface ChildControl
    {
        void Reset();
    }
}

// A simple interface to be added to all the child classes that are added to the main window. 
// This is to make sure that all the essential functions are implemented in all of them
