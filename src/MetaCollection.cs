using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SurvivalGame.src
{
    class MetaCollection
    {
        private Dictionary<int, string> data;
        
        public string Get(int x, int y)
        {
            string data = "";
            this.data.TryGetValue(this.ToIndex(x, y), out data);
            return data;
        }

        public void Remove(int x, int y)
        {
            this.data.Remove(this.ToIndex(x, y));
        }

        public void Set(int x, int y, string data)
        {
            this.data.Add(this.ToIndex(x, y), data);
        }

        private int ToIndex(int x, int y)
        {
            return y * Chunk.size + x;
        }
    }
}
