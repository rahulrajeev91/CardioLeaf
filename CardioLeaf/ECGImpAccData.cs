using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardioLeaf
{
    class ECGImpAccData
    {
        int[] points = new int[12];             //12 points in the data
        double[] acc = new double[3];           //0-x, 1-y, 2-z
        double acc_magnitude;
        int[] impedence = new int[2];           //0-Resistive, 1-Capacitive
        //const double HIGHPASS_DEGREE = 0.5;   //TODO : add high pass filter 

        private static double smoothenedMagnitude;
        private static double[] gravity = { 0, 0, 0 };

        private const double GRAVITY_LOW_PASS_MULTIPLIER = 0.8;
        private const double MAGNITUDE_SMOOTHENING = 0.95; 

        //public ECGImpAccData()
        //{
        //    //default contstructor
        //}

        public void updateData(int[]inputData, int[] raw_acc, int[]imp)
        {
            addToPoints(inputData);
            addToAcc(raw_acc);
            impedence = imp;

        }

        private void addToAcc(int[] raw_acc)
        {
            for (int i = 0; i < 3; i++)
                acc[i] = raw_acc[i];        //use the conversion factor
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
        }

        public int[] getEcgData()
        {
            return points;
        }

        public double[] getAccData()
        {
            return acc;
        }

        public double getSmoothenActivityVal()
        {
            return smoothenedMagnitude;
        }


        # region "accelerometer"
        
        private double GetSmoothenedAccMagnitude(double[] acc)
        {
            for (int i = 0; i < 3; i++)
            {
                gravity[i] = GRAVITY_LOW_PASS_MULTIPLIER * gravity[i] + (1 - GRAVITY_LOW_PASS_MULTIPLIER) * acc[i];
                acc[i] -= gravity[i];
            }

            double magnitude = Math.Pow((Math.Pow(acc[0], 2) + Math.Pow(acc[1], 2) + Math.Pow(acc[2], 2)), 0.5);
            smoothenedMagnitude = MAGNITUDE_SMOOTHENING * smoothenedMagnitude + (1 - MAGNITUDE_SMOOTHENING) * magnitude;

            return smoothenedMagnitude;
        }
        #endregion

    }


}
