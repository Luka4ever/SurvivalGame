using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.src.Interface
{
    class Node
    {
        private int x = 0;
        private int y = 0;
        private int width = 0;
        private int height = 0;
        private int backgroundImage = -1;
        private int image = -1;
        private string text = "";
        private int marginTop = 0;
        private int marginBottom = 0;
        private int marginLeft = 0;
        private int marginRight = 0;
        private int paddingTop = 0;
        private int paddingBottom = 0;
        private int paddingLeft = 0;
        private int paddingRight = 0;
        private Node parent = null;
        private List<Node> children = new List<Node>();
        private bool visible = true;
        private string id = null;

        public Node()
        {

        }

        public void AddNode(Node node)
        {
            this.children.Add(node);
            node.Parent = this;
        }

        public Node GetNodeByID(string id)
        {
            foreach (Node node in this.children)
            {
                if (node.ID == id)
                {
                    return node;
                }
                Node n = node.GetNodeByID(id);
                if (n != null)
                {
                    return n;
                }
            }
            return null;
        }

        public virtual void Draw(Graphics g, int sx, int sy)
        {
            if (this.visible)
            {
                if (this.backgroundImage != -1)
                {
                    g.DrawImage(ImageManager.GetImage(this.backgroundImage), sx + this.x, sy + this.y, this.width, this.height);
                }
                foreach (Node node in this.children)
                {
                    node.Draw(g, sx, sy);
                }
            }
        }

        public int X
        {
            get { return this.x; }
            set { this.x = value; }
        }

        public int Y
        {
            get { return this.y; }
            set { this.y = value; }
        }

        public int Width
        {
            get { return this.width; }
            set { this.width = value; }
        }

        public int Height
        {
            get { return this.height; }
            set { this.height = value; }
        }

        public int BackgroundImage
        {
            get { return this.backgroundImage; }
            set { this.backgroundImage = value; }
        }

        public int Image {
            get { return this.image; }
            set { this.image = value; }
        }

        public string Text
        {
            get { return this.text; }
            set { this.text = value; }
        }

        public int Margin
        {
            set
            {
                this.marginTop = value;
                this.marginBottom = value;
                this.marginLeft = value;
                this.marginRight = value;
            }
        }

        public int MarginTop
        {
            get { return this.marginTop; }
            set { this.marginTop = value; }
        }

        public int MarginBottom
        {
            get { return this.marginBottom; }
            set { this.marginBottom = value; }
        }

        public int MarginLeft
        {
            get { return this.marginLeft; }
            set { this.marginLeft = value; }
        }

        public int MarginRight
        {
            get { return this.marginRight; }
            set { this.marginRight = value; }
        }

        public int Padding
        {
            set
            {
                this.paddingTop = value;
                this.paddingBottom = value;
                this.paddingLeft = value;
                this.paddingRight = value;
            }
        }

        public int PaddingTop
        {
            get { return this.paddingTop; }
            set { this.paddingTop = value; }
        }

        public int PaddingBottom
        {
            get { return this.paddingBottom; }
            set { this.paddingBottom = value; }
        }

        public int PaddingLeft
        {
            get { return this.paddingLeft; }
            set { this.paddingLeft = value; }
        }

        public int PaddingRight
        {
            get { return this.paddingRight; }
            set { this.paddingRight = value; }
        }

        public Node Parent
        {
            get { return this.parent; }
            set { this.parent = value; }
        }

        public bool Visible
        {
            get { return this.visible; }
            set { this.visible = value; }
        }

        public string ID
        {
            get { return this.id; }
            set { this.id = value; }
        }
    }
}
