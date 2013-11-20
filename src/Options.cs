using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.src
{
    class Options
    {
        [JsonProperty("MaxFPS")]
        public int MaxFPS = 60;
        [JsonProperty("LimitFPS")]
        public bool LimitFPS = true;

        public Options(string path)
        {
            if (File.Exists(@path))
            {
                JsonConvert.DeserializeObject<Options>(File.ReadAllText(@path));
            }
        }

        public void Save(string path)
        {
            FileStream file = File.OpenWrite(@path);
            byte[] buffer = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(this));
            file.WriteAsync(buffer, 0, buffer.Length);
            file.Close();
        }
    }
}
