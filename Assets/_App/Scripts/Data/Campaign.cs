using SimpleJSON;
using System;

namespace App.Data
{
    [Serializable]
    public class Campaign
    {
        public string Id { get; private set; }
        public string Title { get; private set; }
        public string Subtitle { get; private set; }
        public string Description { get; private set; }
        public string Instruction { get; private set; }
        public string ButtonText { get; private set; }
        public string Help { get; private set; }
        public string ThumbnailUrl { get; private set; }
        public string DetailImageUrl { get; private set; }
        public TrackableData[] Trackables { get; private set; }
        public MarkerDataSet Dataset { get; private set; }
        public ExtrasData[] Extras { get; private set; }
        public int Timestamp { get; private set; }
        public bool IsAR { get; private set; }

        public Campaign(string id, string title, string subtitle, string description, string instruction, string buttonText, string help, string thumbnailUrl, string detailImageUrl, TrackableData[] trackables, MarkerDataSet dataset, ExtrasData[] extras, int timestamp, bool isAR)
        {
            Id = id;
            Title = title;
            Subtitle = subtitle;
            Description = description;
            Instruction = instruction;
            ButtonText = buttonText;
            Help = help;
            ThumbnailUrl = thumbnailUrl;
            DetailImageUrl = detailImageUrl;
            Trackables = trackables;
            Extras = extras;
            Dataset = dataset;
            Timestamp = timestamp;
            IsAR = isAR;
        }

        public static Campaign CreateCampaignFromJson(string json)
        {
            JSONNode node = JSON.Parse(json);
            return CreateCampaignFromJson(node);
        }

        public static Campaign CreateCampaignFromJson(JSONNode node)
        {
            JSONArray trackablesNode = node["content"]["trackables"].AsArray;

            TrackableData[] trackables = new TrackableData[trackablesNode.Count];

            for(int i = 0; i < trackables.Length; i++)
            {
#if UNITY_ANDROID
                trackables[i] = new TrackableData() 
                { 
                    type = trackablesNode[i]["type"],
                    markerName = trackablesNode[i]["name"], 
                    prefabName = trackablesNode[i]["asset"]["prefabName"], 
                    assetUrl = trackablesNode[i]["asset"]["android"] 
                };
#elif UNITY_IOS
                trackables[i] = new TrackableData() 
                {
                    type = trackablesNode[i]["type"],
                    markerName = trackablesNode[i]["name"], 
                    prefabName = trackablesNode[i]["asset"]["prefabName"], 
                    assetUrl = trackablesNode[i]["asset"]["ios"] 
                };
#endif
            }

            JSONArray extrasNode = node["content"]["extras"].AsArray;
            ExtrasData[] extras = new ExtrasData[extrasNode.Count];

            for (int i = 0; i < extras.Length; i++)           
                extras[i] = new ExtrasData() { assetName = extrasNode[i]["name"], type = extrasNode[i]["type"], assetUrl = extrasNode[i]["source"] };

            return new Campaign(
                node["id"],
                node["title"],
                node["subtitle"],
                node["description"],
                node["instruction"],
                node["buttonText"],
                node["help"],
                node["thumbnailUrl"],
                node["detailImageUrl"],
                trackables,
                new MarkerDataSet() 
                {
                    type = node["content"]["dataset"]["type"],
                    dat = node["content"]["dataset"]["dat"], 
                    xml = node["content"]["dataset"]["xml"] 
                },
                extras,
                node["timestamp"],
                node["isAR"].AsBool
                );
        }

        [Serializable]
        public struct MarkerDataSet
        {
            public string type;
            public string xml;
            public string dat;
        }

        [Serializable]
        public struct TrackableData
        {
            public string type;
            public string markerName;
            public string prefabName;
            public string assetUrl;
        }

        [Serializable]
        public struct ExtrasData
        {
            public string type;
            public string assetName;
            public string assetUrl;
        }

    }
}