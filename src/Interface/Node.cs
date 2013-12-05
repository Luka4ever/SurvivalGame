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
        private bool hover = false;
        private Func<Node, int, int, bool> onClick = null;
        private Func<Node, int, int, bool> onRelease = null;
        private Action<Node> onMouseOver = null;
        private Action<Node> onMouseOut = null;
        private Node computed = null;

        public Node()
        {

        }

        public bool Click(Point cursor)
        {
            bool consumed = false;
            if (this.visible)
            {
                bool clicked = new Rectangle(this.computed.x, this.computed.y, this.computed.width, this.computed.height).Contains(cursor);
                if (clicked && this.onClick != null)
                {
                    consumed = this.onClick(this, cursor.X, cursor.Y);
                }
                if (clicked && !consumed)
                {
                    foreach (Node node in this.children)
                    {
                        if (!consumed && node.Click(cursor))
                        {
                            consumed = true;
                        }
                    }
                }
            }
            return consumed;
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

        public virtual void Reflow(int sx, int sy)
        {
            this.computed = new Node();
            this.computed.x = sx + this.x;
            this.computed.y = sy + this.y;
            this.computed.width = this.width;
            this.computed.height = this.height;
            this.computed.backgroundImage = this.backgroundImage;
            this.computed.image = this.image;
            this.computed.text = this.text;
            this.computed.marginTop = this.marginTop;
            this.computed.marginBottom = this.marginBottom;
            this.computed.marginLeft = this.marginLeft;
            this.computed.marginRight = this.marginRight;
            this.computed.paddingTop = this.paddingTop;
            this.computed.paddingBottom = this.paddingBottom;
            this.computed.paddingLeft = this.paddingLeft;
            this.computed.paddingRight = this.paddingRight;
            this.computed.parent = this.parent;
            this.computed.children = this.children;
            this.computed.visible = this.visible;
            this.computed.id = this.id;
            this.computed.hover = this.hover;
            int ssx = sx;
            bool newLine = true;
            Node last = null;
            foreach (Node node in this.children)
            {
                if (sx + node.width > ssx + this.width)
                {
                    if (sx != ssx)
                    {
                        sy += node.height + node.paddingTop + node.paddingBottom;
                        sx = ssx;
                        newLine = true;
                    }
                }
                node.Reflow(sx + this.x + (newLine ? this.paddingLeft : Math.Max(node.paddingLeft, last.paddingRight)), sy + this.y + this.paddingTop);
                sx += node.width + (newLine ? node.paddingLeft : Math.Max(node.paddingLeft, last.paddingRight));
                last = node;
                newLine = false;
            }
        }

        public virtual void Draw(Graphics g)
        {
            if (this.visible)
            {
                if (this.backgroundImage != -1)
                {
                    g.DrawImage(ImageManager.GetImage(this.backgroundImage), this.computed.x, this.computed.y, this.computed.width, this.computed.height);
                }
                if (this.image != -1)
                {
                    g.DrawImage(ImageManager.GetImage(this.image), this.computed.x + this.computed.marginLeft, this.computed.y + this.computed.marginTop, this.computed.width - this.computed.marginLeft - this.computed.marginRight, this.computed.height - this.computed.marginTop - this.computed.marginBottom);
                }
                foreach (Node node in this.children)
                {
                    node.Draw(g);
                }
            }
        }

        public void CheckHover(Point cursor)
        {
            this.hover = new Rectangle(this.computed.x, this.computed.y, this.computed.width, this.computed.height).Contains(cursor);
            if (this.hover != this.computed.hover)
            {
                this.computed.hover = this.hover;
                if (this.hover)
                {
                    if (this.onMouseOver != null)
                    {
                        this.onMouseOver(this);
                    }
                }
                else
                {
                    if (this.onMouseOut != null)
                    {
                        this.onMouseOut(this);
                    }
                }
            }
            foreach (Node node in this.children)
            {
                node.CheckHover(cursor);
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

        public bool Hover
        {
            get { return this.hover; }
            set { this.hover = value; }
        }

        public Node Computed
        {
            get { return this.computed; }
        }

        public Func<Node, int, int, bool> OnClick
        {
            set { this.onClick = value; }
        }

        public Func<Node, int, int, bool> OnRelease
        {
            set { this.onRelease = value; }
        }

        public Action<Node> OnMouseOver
        {
            set { this.onMouseOver = value; }
        }

        public Action<Node> OnMouseOut
        {
            set { this.onMouseOut = value; }
        }
    }
}
