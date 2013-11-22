using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace SurvivalGame.src
{
    class Options
    {
        [JsonProperty("MaxFPS")]
        public int MaxFPS = 60;
        [JsonProperty("LimitFPS")]
        public bool LimitFPS = true;

        public static Options Load(string path)
        {
            if (File.Exists(@path))
            {
                return JsonConvert.DeserializeObject<Options>(File.ReadAllText(@path));
            }
            return new Options();
        }

        public void Save(string path)
        {
            System.IO.File.WriteAllText(@path, JsonConvert.SerializeObject(this));
            /*
            UnicodeEncoding uniencoding = new UnicodeEncoding();
            byte[] buffer = uniencoding.GetBytes(JsonConvert.SerializeObject(this));
            
            using (FileStream SourceStream = File.Open(path, FileMode.OpenOrCreate))
            {
                SourceStream.Seek(0, 0);
                SourceStream.Write(buffer, 0, buffer.Length);
            }
             * */
        }
    }
}
