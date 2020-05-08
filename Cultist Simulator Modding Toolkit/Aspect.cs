﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace Cultist_Simulator_Modding_Toolkit
{
    public class Aspect : Element
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? isAspect, isHidden, noartneeded;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Induces> induces;


        [JsonConstructor]
        public Aspect(string id, string label, string description,
                      bool? isHidden, bool? noartneeded, string icon = null, JArray induces = null,
                      bool? isAspect = true, string comments = null, JToken aspects = null)
        {
            // necessary
            this.id = id;
            // necessary
            this.label = label;
            // necessary
            this.description = description;
            // optional
            this.isAspect = isAspect;
            // optional
            if (icon != null) this.icon = icon;
            else this.icon = id;
            // isHidden is true iff isAspect is true
            // optional
            if (isHidden == true) this.isHidden = true;
            // optional
            if (induces != null) this.induces = induces.ToObject<List<Induces>>();
            // optional
            this.noartneeded = noartneeded;
            // optional
            this.comments = comments;
            // optional, didn't even know it was possible tbqh
            if (aspects != null) this.aspects = aspects.ToObject<Dictionary<string, int>>();
        }

        public Aspect(string id, string label, string description,
                      string icon = null, List<Induces> induces = null,
                      bool isHidden = false, bool noartneeded = false,
                      bool isAspect = true, string comments = null)
        {
            // id is what is used to reference the aspect
            this.id = id;
            // label is what it gets called on the pop-up
            this.label = label;
            // description is the flavor text shown in the pop-up
            this.description = description;
            // isAspect is always true for aspects, it's what differentiates them from Elements
            this.isAspect = isAspect;
            // icon is probably the id of the card or the id of the picture (icon.png minus the extension)
            if (icon != null)
            {
                this.icon = icon;
            }
            else
            {   // Usually ignored, but can simply be identical to the Aspect ID
                this.icon = id;
            }
            // Determines whether the Aspect is shown on Elements, such as uniqueness identifiers
            if (isHidden == true)
            {
                this.isHidden = true;
            }
            // induces is an array containing objects (Induces) containing a recipe ID and a chance out of 0-100 of triggering the recipe
            if (induces != null)
            {
                this.induces = induces;
            }
            // Determines whether the aspect needs an icon, true iff isHidden
            if (noartneeded == true)
            {
                this.noartneeded = true;
            }
            // comments are just comments, they're stored and can be edited, but are never displayed ingame
            this.comments = comments;
        }

        public Aspect()
        {
            isAspect = true;
        }

        public string toString()
        {
            return JsonConvert.SerializeObject(this);
        }
        
        public class Induces
        {
            public string id;
            public int chance;
            public bool? additional;

            [JsonConstructor]
            public Induces(string id, int chance, bool? additional)
            {
                this.id = id;
                this.chance = chance;
                this.additional = additional;
            }
        }
    }
}