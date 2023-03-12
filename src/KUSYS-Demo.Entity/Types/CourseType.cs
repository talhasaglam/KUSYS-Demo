using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KUSYS_Demo.Entity.Types
{
    public enum CourseType
    {
        [Description("MAT")]
        Maths=0,

        [Description("CSI")]
        ComputerScience = 1,

        [Description("PHY")]
        Physics =2
    }

    public static class CourseTypeExtensions
    {
        public static string ToDescriptionString(this CourseType val)
        {
            var attributes = (DescriptionAttribute[])val
                .GetType()
                .GetField(val.ToString())!
                .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}
