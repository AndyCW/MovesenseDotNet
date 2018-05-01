using MdsLibrary.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleApp.Models
{
    public class Data
    {
        private AngularVelocity mAngularVelocity;
        private MagneticField mMagneticField;
        private AccData mLinearAcceleration;

        public Data(AngularVelocity angularVelocity)
        {
            mAngularVelocity = angularVelocity;
        }

        public Data(MagneticField magneticField)
        {
            mMagneticField = magneticField;
        }

        public Data(AccData linearAcceleration)
        {
            mLinearAcceleration = linearAcceleration;
        }

        public long GetTimestamp()
        {
            if (mAngularVelocity != null)
            {
                return mAngularVelocity.body.timestamp;
            }
            if (mLinearAcceleration != null)
            {
                return mLinearAcceleration.body.timestamp;
            }
            if (mMagneticField != null)
            {
                return mMagneticField.body.timestamp;
            }
            return 0;
        }

        public double[] GetData()
        {
            double[] value = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 }; // size  = 9

            if (mAngularVelocity != null)
            {
                value[0] = mAngularVelocity.body.array[0].x;
                value[1] = mAngularVelocity.body.array[0].y;
                value[2] = mAngularVelocity.body.array[0].z;
            }
            if (mLinearAcceleration != null)
            {
                value[3] = mLinearAcceleration.body.array[0].x;
                value[4] = mLinearAcceleration.body.array[0].y;
                value[5] = mLinearAcceleration.body.array[0].z;
            }
            if (mMagneticField != null)
            {
                value[6] = mMagneticField.body.array[0].x;
                value[7] = mMagneticField.body.array[0].y;
                value[8] = mMagneticField.body.array[0].z;
            }
            return value;
        }
    }

}
