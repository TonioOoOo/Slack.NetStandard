﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Slack.NetStandard.Objects.Stars;
using System;

namespace Slack.NetStandard.JsonConverters
{
    public class StarredItemConverter:JsonConverter<StarredItem>
    {
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, StarredItem value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override StarredItem ReadJson(JsonReader reader, Type objectType, StarredItem existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (objectType != typeof(StarredItem))
            {
                var known = Activator.CreateInstance(objectType);
                serializer.Populate(reader, known);
                return known as StarredItem;
            }
            var jObject = JObject.Load(reader);

            var target = GetItemType(jObject.Value<string>("type"));

            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }

        private StarredItem GetItemType(string value)
        {
            return value switch
            {
                ItemTypes.Message => new StarredMessageItem(),
                ItemTypes.File => new StarredFileItem(),
                ItemTypes.Channel => new StarredChannelItem(),
                _ => new StarredItem()
            };
        }
    }
}
