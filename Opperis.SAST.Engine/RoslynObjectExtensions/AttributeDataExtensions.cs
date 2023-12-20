using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.Engine.RoslynObjectExtensions
{
    internal static class AttributeDataExtensions
    {
        internal static bool InheritsFrom(this AttributeData attribute, string name)
        {
            if (attribute.AttributeClass.ToString() == name)
                return true;

            var attributeClass = attribute.AttributeClass.BaseType;

            while (attributeClass != null) 
            {
                if (attributeClass.ToString() == name)
                    return true;

                attributeClass = attributeClass.BaseType;
            }

            return false;
        }
    }
}
