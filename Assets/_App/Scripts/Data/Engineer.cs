using SimpleJSON;
using System;

namespace App.Data
{
    [System.Serializable]
    public class Engineer
    {
        public string Name { get; private set; }
        public string Code { get; private set; }
        public string[] Cisco { get; private set; }
        public string[] Fortinet { get; private set; }
        public string ImageUrl { get; private set; }

        public Engineer(string name, string code, string[] cisco, string[] fortinet, string imageUrl)
        {
            Name = name;
            Code = code;
            Cisco = cisco;
            Fortinet = fortinet;
            ImageUrl = imageUrl;
        }

        public static Engineer CreateEngineerFromJson(string json)
        {
            JSONNode node = JSON.Parse(json);
            return CreateEngineerFromJson(node);
        }

        public static Engineer CreateEngineerFromJson(JSONNode node)
        {
            return new Engineer(node["name"], node["code"], node["cisco"], node["fortinet"], node["imageUrl"]);
        }
    }
}