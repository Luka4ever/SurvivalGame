using SurvivalGame.src.Biomes;
using SurvivalGame.src.Interface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SurvivalGame.src
{
    class UI : Node
    {
        private Entity player;
        private int line;
        private Font font;
        private Brush brush;
        private List<Node> children;
        private bool dragging = false;
        private int draggingItem = -1;
        private bool debug = false;

        public UI(Entity player)
        {
            this.player = player;
            this.font = new Font("Lucida Console", 14, FontStyle.Bold);
            this.brush = new SolidBrush(Color.White);
            this.children = new List<Node>();
            this.InitGameInterface();
        }

        private void InitGameInterface()
        {
            Node inventory = new Node();
            inventory.ID = "inventory";
            inventory.X = 10;
            inventory.Y = 78;
            inventory.Width = 288;
            inventory.Height = 228;
            inventory.BackgroundImage = ImageManager.RegisterImage(@"res/Interface/CharacterInventory.png");
            inventory.Visible = false;
            Node equipment = new Node();
            equipment.ID = "col0";
            equipment.Y = 93;
            equipment.Width = 32;
            equipment.Height = 135;
            equipment.PaddingLeft = 9;
            equipment.PaddingRight = 9;
            inventory.AddNode(equipment);
            for (int i = 1; i <= 3; i++)
            {
                Node cell = new Node();
                cell.ID = equipment.ID + "row" + i;
                cell.Width = 32;
                cell.Height = 32;
                cell.PaddingBottom = 16;
                cell.Margin = 1;
                cell.Image = i + 1;
                cell.OnClick = this.Click;
                equipment.AddNode(cell);
            }
            for (int c = 1; c <= 4; c++)
            {
                Node col = new Node();
                col.ID = "col" + c;
                col.Y = 44;
                col.Width = 32;
                col.Height = 184;
                col.PaddingLeft = 6;
                col.PaddingRight = 16;
                for (int i = 0; i <= 3; i++)
                {
                    Node cell = new Node();
                    cell.ID = col.ID + "row" + i;
                    cell.Width = 32;
                    cell.Height = 32;
                    cell.PaddingBottom = 16;
                    cell.Margin = 1;
                    cell.Image = i + c;
                    cell.OnClick = this.Click;
                    col.AddNode(cell);
                }
                inventory.AddNode(col);
            }
            Node crafting = new Node();
            crafting.ID = "col5";
            crafting.Y = 44;
            crafting.Width = 32;
            crafting.Height = 184;
            crafting.PaddingLeft = 6;
            for (int i = 0; i <= 3; i++)
            {
                Node cell = new Node();
                cell.ID = "col5row" + i;
                cell.Width = 32;
                cell.Height = 32;
                cell.PaddingBottom = 16;
                cell.Margin = 1;
                cell.Image = i + 1;
                cell.OnClick = this.Click;
                crafting.AddNode(cell);
            }
            inventory.AddNode(crafting);
            this.AddNode(inventory);
            Node drag = new Node();
            drag.ID = "drag";
            drag.Width = 30;
            drag.Height = 30;
            this.AddNode(drag);
        }

        private void Reset()
        {
            this.line = 0;
        }

        private void DrawLine(Graphics g, String line)
        {
            g.DrawString(line, font, brush, 10, 10 + (this.line++) * 20);
        }

        public void Draw(Graphics g, World world, Game game)
        {
            if (this.debug)
            {
                Reset();
                DrawLine(g, "X: " + this.player.X + ", Y: " + this.player.Y + ", FPS: " + game.FPS);
                DrawLine(g, "S: " + world.Seed + ", L: " + Chunk.GetLocalSeed(world.Seed) + ", B: " + Chunk.GetBiomeSeed(world.Seed) + ", C: " + Chunk.GetContinentSeed(world.Seed));
                Biome C = Biome.GetContinentFromNoise(Chunk.GetContinentNoise(this.player.X, this.player.Y, Chunk.GetContinentSeed(world.Seed)));
                Biome B = C.GetBiomeFromNoise(Chunk.GetBiomeNoise(this.player.X, this.player.Y, Chunk.GetBiomeSeed(world.Seed)));
                DrawLine(g, "C: " + C.Name + ", B: " + B.Name);
                float CN = Chunk.GetContinentNoise(this.player.X, this.player.Y, Chunk.GetContinentSeed(world.Seed));
                float BN = Chunk.GetBiomeNoise(this.player.X, this.player.Y, Chunk.GetBiomeSeed(world.Seed));
                float LN = Chunk.GetLocalNoise(this.player.X, this.player.Y, Chunk.GetLocalSeed(world.Seed));
                DrawLine(g, "CN: " + CN + ", BN: " + BN + ", LN: " + LN);
            }
            Node drag = this.GetNodeByID("drag");
            drag.Computed.X = Cursor.Position.X + drag.X;
            drag.Computed.Y = Cursor.Position.Y + drag.Y;
            drag.Computed.Image = 1;
            this.Draw(g);
        }

        public bool Click(Node node, int x, int y)
        {
            int row = int.Parse(node.ID.Substring(7, 1));
            int col = int.Parse(node.ID.Substring(3, 1));
            if (col == 0)
            {
                int slot = row - 1;
                int item = ((Unit)this.player).Inventory.GetEquipmentAt(slot);
            }
            else if (col == 5)
            {

            }
            else
            {
                int slot = row * 4 + col - 1;
                int item = ((Unit)this.player).Inventory.GetItemAt(slot);
                ((Unit)this.player).Inventory.SetItemAt(slot, this.draggingItem);
                this.draggingItem = item;
                Node drag = this.GetNodeByID("drag");
                drag.X = x - node.Computed.X;
                drag.Y = y - node.Computed.Y;
                if (!this.dragging && item != -1)
                {
                    this.dragging = true;
                }
                else if (item == -1)
                {
                    this.dragging = false;
                }
                drag.Visible = this.dragging;
            }
            return true;
        }

        public void Reflow(View view)
        {
            this.X = 0;
            this.Y = 0;
            this.Width = view.GetWidth();
            this.Height = view.GetHeight();
            this.Reflow(0, 0);
        }
        
        public void Input(Form window)
        {
            Point cursor = Cursor.Position;
            cursor.X -= window.DesktopLocation.X;
            cursor.Y -= window.DesktopLocation.Y;
            this.CheckHover(cursor);
            
        }

        public bool Debug
        {
            get { return this.debug; }
            set { this.debug = value; }
        }

        public void Click(object sender, EventArgs e)
        {
            Point cursor = new Point(((MouseEventArgs)e).X, ((MouseEventArgs) e).Y);
            this.Click(cursor);
        }
    }
}
