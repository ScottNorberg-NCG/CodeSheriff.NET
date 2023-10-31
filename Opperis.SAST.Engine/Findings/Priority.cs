using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.Findings
{
    internal class Priority
    {
        internal float Sort { get; private set; }
        internal string Text { get; private set; }

        internal static Priority VeryHigh
        {
            get { return new Priority() { Sort = 1f, Text = "Very High" }; }
        }

        internal static Priority High
        {
            get { return new Priority() { Sort = 2f, Text = "High" }; }
        }

        internal static Priority Medium
        {
            get { return new Priority() { Sort = 3f, Text = "Medium" }; }
        }

        internal static Priority Low
        {
            get { return new Priority() { Sort = 4f, Text = "Low" }; }
        }

        internal static Priority Info
        {
            get { return new Priority() { Sort = 5f, Text = "Information" }; }
        }
    }
}
