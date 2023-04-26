using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Lakeside2.Serialization
{
    internal class PointConverter : JsonConverter<Point>
    {
        void skipPropName(ref Utf8JsonReader reader)
        {
            reader.Read();
            reader.Read();
        }

        public override Point Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // TODO there's just no way I'm doing this right.
            skipPropName(ref reader);
            int x = reader.GetInt32();
            skipPropName(ref reader);
            int y = reader.GetInt32();
            reader.Read(); // } (this is important)
            return new Point(x, y);
        }

        public override void Write(Utf8JsonWriter writer, Point value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber("x", value.X);
            writer.WriteNumber("y", value.Y);
            writer.WriteEndObject();
        }
    }
}
