using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardioLeaf
{
    class ECGImpAccSpo2Data
    {
        int[] points = new int[12];             //12 points in the data
        double[] acc = new double[3];           //0-x, 1-y, 2-z
        double acc_magnitude;
        int[] impedence = new int[2];           //0-Resistive, 1-Capacitive
        int[] ppg = new int[2];

        int HR_Threshhold;
        //const double HIGHPASS_DEGREE = 0.5;   //TODO : add high pass filter 

        HeartRateHelper HRHelper = HeartRateHelper.Instance;

        private const double GRAVITY_LOW_PASS_MULTIPLIER = 0.8;
        private const double MAGNITUDE_SMOOTHENING = 0.999; 

        //public ECGImpAccData()
        //{
        //    //default contstructor
        //}

        public void updateData(int[]inputData, int[] raw_acc, int[]imp, int[] ppg_in)
        {
            addToPoints(inputData);
            addToAcc(raw_acc);
            impedence = imp;
            ppg = ppg_in;

        }

        private void addToAcc(int[] raw_acc)
        {
            for (int i = 0; i < 3; i++)
                acc[i] = raw_acc[i];
            acc_magnitude = GetSmoothenedAccMagnitude(acc);

        }

        private void addToPoints(int[] inputData)
        {
            for (int i = 0; i < 8; i++)
            {
                points[i] = inputData[i];
            }
            points[8] = points[1] + points[0];          //lead 2
            points[9] = points[1] - points[8]/2;        //aVL
            points[10] = points[1]/2 + points[0];       //aVF
            points[11] = -(points[1] + points[0]/2);    //aV

            //get the other new leads from byte array to int 16

            HRHelper.UpdateHeartRate(points[CLSettings.HRLead]);
            HR_Threshhold = (int)HRHelper.getGraph();
        }
        
        internal int[] getSummaryData()
        {
            int[] summaryData = new int[3];

            summaryData[0] = points[CLSettings.HRLead];
            summaryData[1] = impedence[CLSettings.RRLead];
            summaryData[2] = ppg[CLSettings.PPGLead];

            return summaryData;
        }

        public int[] getEcgData()
        {
            return points;
        }

        public int getHR()
        {
            return HRHelper.getHeartRate();
        }

        public double[] getAccData()
        {
            return acc;
        }

        public int[] getImpData()
        {
            return impedence;
        }

        public int[] getPpgData()
        {
            return ppg;
        }

        public int getHRMetadata()
        {
            return HR_Threshhold;
        }

        public double getSmoothenActivityVal()
        {
            return DSPStaticVariables.smoothenedMagnitude;
        }


        # region "accelerometer"
        
        private double GetSmoothenedAccMagnitude(double[] acc)
        {

            double[] tempAcc = new double[3];
            for (int i = 0; i < 3; i++)
            {
                tempAcc[i] = acc[i];
                DSPStaticVariables.gravity[i] = GRAVITY_LOW_PASS_MULTIPLIER * DSPStaticVariables.gravity[i] + (1 - GRAVITY_LOW_PASS_MULTIPLIER) * tempAcc[i];
                tempAcc[i] -= DSPStaticVariables.gravity[i];
            }

            double magnitude = Math.Pow((Math.Pow(tempAcc[0], 2) + Math.Pow(tempAcc[1], 2) + Math.Pow(tempAcc[2], 2)), 0.5);
            DSPStaticVariables.smoothenedMagnitude = MAGNITUDE_SMOOTHENING * DSPStaticVariables.smoothenedMagnitude + (1 - MAGNITUDE_SMOOTHENING) * magnitude;

            return DSPStaticVariables.smoothenedMagnitude;
        }
        #endregion


        
    }


}
