using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data {
    public class Info {
        public readonly Rarity Rarity;
        public readonly string Name;
        public readonly List<InfoDetail> Contexts;
        public Dictionary<string, object> Params { get; set; } = null;
        public Info(string pName, List<InfoDetail> pContext, Dictionary<string, object> pParams = null, Rarity pRarity = Rarity.Etc) =>
            (Name, Contexts, Params, Rarity) = (pName, pContext, pParams, pRarity);
    }

    [Serializable]
    public struct InfoDetail {
        public string Category;
        [FormerlySerializedAs("Format")] public string Context;

        public InfoDetail(string pCategory, string pFormat) =>
            (Category, Context) = (pCategory, pFormat);
    }
}