using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lakeside2.Serialization
{
    class Vector2Converter : JsonConverter<Vector2>
    {
        void skipPropName(ref Utf8JsonReader reader)
        {
            reader.Read();
            reader.Read();
        }

        public override Vector2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // TODO there's just no way I'm doing this right.
            skipPropName(ref reader);
            float x = reader.GetSingle();
            skipPropName(ref reader);
            float y = reader.GetSingle();
            reader.Read(); // } (this is important)
            return new Vector2(x, y);
        }

        public override void Write(Utf8JsonWriter writer, Vector2 value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber("x", value.X);
            writer.WriteNumber("y", value.Y);
            writer.WriteEndObject();
        }
    }
}
