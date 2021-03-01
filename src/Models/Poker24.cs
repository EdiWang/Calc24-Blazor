using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Calc24Blazor.Models
{
    public class Poker24
    {
        [Range(1, 13, ErrorMessage = "数值1：请输入1-13之间的数字")]
        public int Num1 { get; set; }

        [Range(1, 13, ErrorMessage = "数值2：请输入1-13之间的数字")]
        public int Num2 { get; set; }

        [Range(1, 13, ErrorMessage = "数值3：请输入1-13之间的数字")]
        public int Num3 { get; set; }

        [Range(1, 13, ErrorMessage = "数值4：请输入1-13之间的数字")]
        public int Num4 { get; set; }
    }
}
