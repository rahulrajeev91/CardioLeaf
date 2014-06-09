using System;
using System.Collections.Generic;
using System.Linq;
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

namespace CardioLeaf
{
    /// <summary>
    /// Interaction logic for Temperature_Control.xaml
    /// </summary>
    public partial class Temperature_Control : UserControl, ChildControl
    {
        public Temperature_Control()
        {
            InitializeComponent();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
