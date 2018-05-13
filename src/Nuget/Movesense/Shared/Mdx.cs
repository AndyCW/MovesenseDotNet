using Com.Movesense.Mds;
using System;

namespace MdsLibrary
{
    public sealed class Mdx
    {
        private static Mds instance = null;
        private static MdsConnectionListener mdsConnectionListener = new MdsConnectionListener();
        private static readonly object padlock = new object();

        public static string SCHEME_PREFIX = "suunto://";
        public static Android.App.Activity Activity = null;

        public static MdsConnectionListener MdsConnectionListener
        {
            get
            {
                return mdsConnectionListener;
            }
        }

        Mdx()
        {
        }

        public static Mds MdsInstance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        if (Activity == null)
                        {
                            throw new InvalidOperationException("Set Activity property to current Android.App.Activity before calling MdsInstance");
                        }

                        instance = new Mds.Builder().Build(Activity);
                    }
                    return instance;
                }
            }
        }

    }
}