﻿using Newtonsoft.Json;
using System;

namespace CarcassSpark.ObjectTypes
{
    public class Induces
    {
        [JsonIgnore]
        public Guid guid = Guid.NewGuid();
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string id;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? chance;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? additional;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Expulsion expulsion;

        [JsonConstructor]
        public Induces(string id, int? chance, bool? additional, Expulsion expulsion)
        {
            this.id = id;
            this.chance = chance;
            this.additional = additional;
            this.expulsion = expulsion;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public Induces Copy()
        {
            string serializedObject = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<Induces>(serializedObject);
        }
    }
}
