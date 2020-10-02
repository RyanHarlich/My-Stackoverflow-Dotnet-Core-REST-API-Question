using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public class Info : HyperMediaControls.APIModelExample
    {
        [Key]
        public int Id { get; set; }
        public String Title { get; set; }
    }
}
