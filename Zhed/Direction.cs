using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Zhed
{
    public enum Direction
    {
        [Display(Name = "Up")]
        Up,
        [Display(Name = "Down")]
        Down,
        [Display(Name = "Left")]
        Left,
        [Display(Name = "Right")]
        Right
    }
}
