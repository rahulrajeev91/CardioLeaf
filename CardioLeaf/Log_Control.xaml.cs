using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
//using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CardioLeaf
{
    /// <summary>
    /// Interaction logic for Log_Control.xaml
    /// </summary>
    public partial class Log_Control : UserControl
    {

        datatableControl dtController = new datatableControl();
        public Log_Control()
        {
            InitializeComponent();
            datagridFormHost.Child = dtController;
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of the open file dialog box.
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();

            // Process input if the user clicked OK.
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "CardioLeaf Log files (*.cll)|*.cll";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filepath = openFileDialog1.FileName;
                dtController.importCsv(filepath);
                //// Open the selected file to read.
                //System.IO.Stream fileStream = openFileDialog1.OpenFile();

                //using (System.IO.StreamReader reader = new System.IO.StreamReader(fileStream))
                //{
                //    // Read the first line from the file and write it the textbox.
                //    tbFileContents.Text = reader.ReadToEnd();
                //}
                //fileStream.Close();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            System.IO.StreamWriter fs = null;
            System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();

            saveFileDialog1.Filter = "CardioLeaf Log files (*.cll)|*.cll";
            saveFileDialog1.FilterIndex = 0;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                string filepath = saveFileDialog1.FileName;
                dtController.ExportCsv(filepath);
                //try
                //{
                //    fs = new System.IO.StreamWriter(saveFileDialog1.OpenFile());
                //    fs.Write(tbFileContents.Text);
                //    fs.Close();
                //}
                //catch (Exception)
                //{
                //    throw;
                //}

            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            dtController.ClearDT();
        }
    }
}
