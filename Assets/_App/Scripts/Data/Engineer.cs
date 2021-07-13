using SimpleJSON;
using System;

namespace App.Data
{
    [Serializable]
    public class Engineer
    {
        public string Name { get; private set; }
        public string ImageUrl { get; private set; }

        public Engineer(string name, string imageUrl)
        {
            Name = name;
            ImageUrl = imageUrl;
        }

        public static Engineer CreateEngineerFromJson(string json)
        {
            JSONNode node = JSON.Parse(json);
            return CreateEngineerFromJson(node);
        }

        public static Engineer CreateEngineerFromJson(JSONNode node)
        {
            return new Engineer(
                node["name"],
                node["imageUrl"]);
        }
    }
}