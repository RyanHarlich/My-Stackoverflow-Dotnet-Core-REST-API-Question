using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using JsonProperty = Newtonsoft.Json.Serialization.JsonProperty;
using Library.Data;

namespace Library.Models.HyperMediaControls
{
    public abstract class APIModelExample
    {
        private HyperMediaLink Add(APIModelExample.Model model, LinkGenerator generator, MasterNode masterNode, HyperMediaLink nestInThisLink = null)
        {
            nestInThisLink = AddModel(model, generator, masterNode, nestInThisLink);
            return nestInThisLink;
        }


        public void AddInfo(LinkGenerator linkGenerator, IUnitOfWork unitOfWork)
        {
            HyperMediaLink nestInThisInfoLink = Add(APIModelExample.Model.Info, linkGenerator, unitOfWork.StartMasterNode);
            Add(APIModelExample.Model.Level1, linkGenerator, unitOfWork.StartMasterNode, nestInThisInfoLink);
        }


        public enum Model { Info, Level1 }
        public MasterNode Links { get; set; }

        protected HyperMediaLink AddModel(Model model, LinkGenerator generator, MasterNode masterNode, HyperMediaLink nestInThisLink = null)
        {
            String href = "hello, world";
            var link = new HyperMediaLink()
            {
                Href = href == null ? "null" : href,
            };

            Add(model, link, masterNode, nestInThisLink);
            nestInThisLink = link;
            return nestInThisLink;
        }

        private void Add(Model model, HyperMediaLink link, MasterNode masterNode, HyperMediaLink nestInThisLink = null)
        {
            if (Links == null)
                Links = masterNode;
            Links.Add(model, link, masterNode, nestInThisLink);
        }



        [NotMapped]
        public abstract class MasterNode
        {
            //[JsonPropertyName("hello")]
            public abstract List<HyperMediaLink> Info { get; set; }
            public abstract List<HyperMediaLink> Level1 { get; set; }


            public virtual MasterNode Add(Model model, HyperMediaLink link, MasterNode masterNode, HyperMediaLink nestInThisLink) {

                MasterNode curNode = this;
                if (nestInThisLink != null)
                {
                    if (nestInThisLink.InnerLinks == null)
                    {
                        curNode = nestInThisLink.InnerLinks = masterNode.MemberwiseClone() as MasterNode;
                        curNode.Info = null;
                    }
                    else
                        curNode = nestInThisLink.InnerLinks;
                }

                switch (model)
                {
                    case Model.Info:
                        if (curNode.Info == null)
                            curNode.Info = new List<HyperMediaLink>();
                        curNode.Info.Add(link);
                        break;
                }
                return curNode;
            }
        }
    }



    [NotMapped]
    public class HyperMediaLink
    {
        public string Href { get; set; }
        public APIModelExample.MasterNode InnerLinks { get; set; } = null;
    }
}
