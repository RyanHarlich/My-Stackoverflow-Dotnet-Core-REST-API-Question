using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Client.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Routing;
using System.ComponentModel.DataAnnotations.Schema;
using Client.Data;
using Library.Models.HyperMediaControls;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Client.Models.HyperMediaControls
{
    public abstract class API : APIModelExample
    {

        [NotMapped]
        public sealed class InheritedMasterNode : MasterNode
        {
            //[JsonPropertyName("infoInherited")]
            //[JsonPropertyAttribute("infoInherited")]
            [JsonProperty(PropertyName = "infoInherited")]
            public override List<HyperMediaLink> Info { get; set; } = null;
            public override List<HyperMediaLink> Level1 { get; set; } = null;


            public override MasterNode Add(Model model, HyperMediaLink link, MasterNode masterNode, HyperMediaLink nestInThisLink)
            {
                InheritedMasterNode curNode = base.Add(model, link, masterNode, nestInThisLink) as InheritedMasterNode;
                switch (model)
                {
                    //case Model.Info:
                    //    if (curNode.Info == null)
                    //        curNode.Info = new List<HyperMediaLink>();
                    //    curNode.Info.Add(link);
                    //    break;
                    case Model.Level1:
                        if (curNode.Level1 == null)
                            curNode.Level1 = new List<HyperMediaLink>();
                        curNode.Level1.Add(link);
                        break;
 
                }
                return null;
            }
        }

    }
}
