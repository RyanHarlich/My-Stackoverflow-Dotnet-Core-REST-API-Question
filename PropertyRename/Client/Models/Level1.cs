using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Client.Models
{
    public class Level1 : HyperMediaControls.API
    {
        public int Level1Id { get; set; }

        /*** Values ***/
        [Required]
        public String Name { get; set; }
    }

  
}
