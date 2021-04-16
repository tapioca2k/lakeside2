using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lakeside2.Serialization
{
    class NPCConverter : JsonConverter<NPC>
    {
        void skipPropName(ref Utf8JsonReader reader)
        {
            reader.Read();
            reader.Read();
        }

        public override NPC Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // TODO there's just no way I'm doing this right.
            skipPropName(ref reader);
            string filename = reader.GetString();
            skipPropName(ref reader);
            float x = reader.GetSingle();
            skipPropName(ref reader);
            float y = reader.GetSingle();
            skipPropName(ref reader);
            string scriptname = reader.GetString();
            skipPropName(ref reader);
            bool locked = reader.GetBoolean();
            skipPropName(ref reader);
            string entityName = reader.GetString();
            reader.Read(); // } (this is important)
            NPC n = new NPC(filename, scriptname, locked, entityName);
            n.setTileLocation(new Vector2(x, y));
            return n;
        }

        public override void Write(Utf8JsonWriter writer, NPC value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("filename", value.filename);
            writer.WriteNumber("x", value.getTileLocation().X);
            writer.WriteNumber("y", value.getTileLocation().Y);
            writer.WriteString("script", value.script.filename);
            writer.WriteBoolean("locked", value.locked);
            writer.WriteString("name", value.name);
            writer.WriteEndObject();
        }
    }
}
