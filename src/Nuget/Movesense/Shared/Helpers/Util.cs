using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MdsLibrary.Helpers
{
    /// <summary>
    /// Contains utility methods
    /// </summary>
    public class Util
    {
        /// <summary>
        /// Utility method to extract the device serial number from the full device name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetVisibleSerial(string name)
        {
            string result = "";                                  //NON-NLS
            Regex rgx;
            //if (BuildConfig.DEBUG) {
            // Everything after last whitespace and possible '#'
            rgx = new Regex(@"([^\s#]*)$"); // NON-NLS
            /*
            } else {
                // Digits at the end of the string, must be 8-14 digits.
                // The first digit can be replaced with P in prototype devices.
                pattern = Pattern.compile("(P?[0-9]{7,14}$)"); //NON-NLS
            }
            */

            MatchCollection matches = rgx.Matches(name);
            if (matches.Count > 0)
            {
                result = matches[0].Value;                        // Returns the input subsequence matched by the previous match.
            }
            return result;
        }
    }
}
