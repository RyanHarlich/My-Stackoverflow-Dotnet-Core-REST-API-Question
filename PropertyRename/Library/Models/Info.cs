using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Library.Models
{
    public class Info : HyperMediaControls.APIModelExample
    {
        [Key]
        public int Id { get; set; }
        public String Title { get; set; }
    }

    public class SerializeInfo : Info
    {
        private static String InfoName { get; set; }
        private static String Level1Name { get; set; }

        public static void Serialize(Utf8JsonWriter writer, Info value, String level1Name = "", String infoName = "info")
        {
            InfoName = infoName;
            Level1Name = level1Name;

            writer.WriteStartObject();

            writer.WriteNumber("id", value.Id);
            writer.WriteString("title", value.Title);

            if (value.Links != null)
                WriteLinks(writer, value.Links);

            writer.WriteEndObject();
        }

        private static void WriteLinks(Utf8JsonWriter writer, MasterNode value)
        {
            writer.WriteStartObject("Links");
            if (value.Info != null)
                WriteArray(writer, InfoName, value.Info);
            if (value.Level1 != null)
                WriteArray(writer, Level1Name, value.Level1);

            writer.WriteEndObject();
        }

        private static void WriteArray(Utf8JsonWriter writer, String arrayName, List<Library.Models.HyperMediaControls.HyperMediaLink> value)
        {
            writer.WriteStartArray(arrayName);
            writer.WriteStartObject();
            foreach (var item in value)
            {
                writer.WriteString("href", item.Href);
                if (item.InnerLinks != null)
                    WriteLinks(writer, item.InnerLinks);
            }
            writer.WriteEndObject();
            writer.WriteEndArray();
        }
    }
}
