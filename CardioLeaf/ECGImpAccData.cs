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
        int[] acc = new int[3];                 //0-x, 1-y, 2-z
        int[] impedence = new int[2];           //0-Resistive, 1-Capacitive
        //const double HIGHPASS_DEGREE = 0.5;   //TODO : add high pass filter 

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
    }
}
