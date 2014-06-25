using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CardioLeaf
{
    class LoggingHelper
    {
        FileStream F;
        public void StartLogging()
        {
            CLSettings.fileName = "CardioLeafLog"+DateTime.Now.ToString("_yyyyMMdd_HHmmss");
            if (CLSettings.logFilePath == "")
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\clLog";
                System.IO.Directory.CreateDirectory(path);
                CLSettings.logFilePath = path;
            }
            try
            {
                F = new FileStream(CLSettings.logFilePath + "\\" + CLSettings.fileName + ".CLL", FileMode.Create);
                F.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void addToFile(ECGImpAccSpo2Data datapoint)
        {
            String entry = string.Join(",", datapoint.getEcgData()) + "," +datapoint.getHR().ToString() + ","+ string.Join(",", datapoint.getAccData()) + "," + string.Join(",", datapoint.getImpData()) + "," + string.Join(",", datapoint.getPpgData()) + "," + DSPStaticVariables.Temperature.ToString();
           
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@""+CLSettings.logFilePath + "\\" + CLSettings.fileName + ".CLL", true))
            {
                file.WriteLine(entry);
                file.Close();
            }
        }
    }
}
