using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.Findings
{
    internal class Priority
    {
        internal int Sort { get; private set; }
        internal string Text { get; private set; }

        internal static Priority VeryHigh
        {
            get { return new Priority() { Sort = 1, Text = "Very High" }; }
        }

        internal static Priority High
        {
            get { return new Priority() { Sort = 2, Text = "High" }; }
        }

        internal static Priority Medium
        {
            get { return new Priority() { Sort = 3, Text = "Medium" }; }
        }

        internal static Priority MediumLow
        {
            get { return new Priority() { Sort = 4, Text = "Medium/Low" }; }
        }

        internal static Priority Low
        {
            get { return new Priority() { Sort = 5, Text = "Low" }; }
        }

        internal static Priority VeryLow
        {
            get { return new Priority() { Sort = 6, Text = "Very Low" }; }
        }

        internal static Priority Info
        {
            get { return new Priority() { Sort = 7, Text = "Information" }; }
        }
    }
}
