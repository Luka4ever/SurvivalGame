using SurvivalGame.src.Biomes;
using SurvivalGame.src.Entities;
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
        private Form window;
        private Entity player;
        private int line;
        private List<Node> children;
        private bool dragging = false;
        private int draggingItem = -1;
        private bool debug = false;

        public UI(Entity player, Form window)
        {
            this.window = window;
            this.player = player;
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
            Node bars = new Node();
            bars.ID = "bars";
            bars.Height = 40;
            bars.Width = 96;
            Node health = new Node();
            health.ID = "health";
            health.Width = 96;
            health.Height = 18;
            health.MarginBottom = 2;
            health.Image = ImageManager.RegisterImage(@"res/Interface/HealthBar.png");
            Node healthMeter = new Node();
            healthMeter.ID = "healthMeter";
            healthMeter.Height = 16;
            healthMeter.MarginLeft = 15;
            healthMeter.MarginTop = 6;
            healthMeter.MarginBottom = 6;
            healthMeter.Image = ImageManager.RegisterImage(@"res/Interface/Bar.png");
            health.AddNode(healthMeter);
            bars.AddNode(health);
            Node hunger = new Node();
            hunger.ID = "hunger";
            hunger.Width = 96;
            hunger.Height = 18;
            hunger.MarginBottom = 2;
            hunger.Image = ImageManager.RegisterImage(@"res/Interface/HungerBar.png");
            Node hungerMeter = new Node();
            hungerMeter.ID = "hungerMeter";
            hungerMeter.Height = 16;
            hungerMeter.Width = 40;
            hungerMeter.MarginLeft = 15;
            hungerMeter.MarginTop = 6;
            hungerMeter.MarginBottom = 6;
            hungerMeter.Image = ImageManager.RegisterImage(@"res/Interface/Bar.png");
            hunger.AddNode(hungerMeter);
            bars.AddNode(hunger);
            this.AddNode(bars);
            Node dead = new Node();
            dead.ID = "dead";
            dead.Text = "You have died";
            dead.Visible = false;
            this.AddNode(dead);
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
            drag.Computed.X = Cursor.Position.X - this.window.DesktopLocation.X - drag.X - 8;
            drag.Computed.Y = Cursor.Position.Y - this.window.DesktopLocation.Y - drag.Y - 32;
            drag.Image = (this.draggingItem != -1 ? Item.GetItem(this.draggingItem).Icon : -1);
            Node bars = this.GetNodeByID("bars");
            Node hungerMeter = bars.GetNodeByID("hungerMeter");
            Node healthMeter = bars.GetNodeByID("healthMeter");
            hungerMeter.Width = (int)Math.Round(80d * (((Human)this.player).Hunger / 100d) + 16d);
            healthMeter.Width = Math.Min((int)Math.Round(80d * (((Human)this.player).Health / 20d) + 16d), 96);
            bars.Reflow(8, this.window.ClientSize.Height - 50);
            if (((Human)this.player).Health <= 0)
            {
                Node dead = this.GetNodeByID("dead");
                dead.Visible = true;
                dead.Reflow(this.window.ClientSize.Width / 2 - 50, this.window.ClientSize.Height / 2 - 10);
            }
            for (int i = 0; i < 3; i++)
            {
                int item = ((Unit)this.player).Inventory.GetEquipmentAt(i);
                this.GetNodeByID("col0row" + (i + 1)).Image = (item == -1 ? -1 : Item.GetItem(item).Icon);
            }
            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    int item = ((Unit)this.player).Inventory.GetItemAt(r * 4 + c);
                    this.GetNodeByID("col" + (c + 1) + "row" + r).Image = (item == -1 ? -1 : Item.GetItem(item).Icon);
                }
            }
            for (int i = 0; i < 4; i++)
            {
                int item = ((Unit)this.player).Inventory.GetCraftingItemAt(i);
                this.GetNodeByID("col5row" + i).Image = (item == -1 ? -1 : Item.GetItem(item).Icon);
            }
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
                if (this.dragging)
                {
                    bool valid = false;
                    if (slot == 0 && Item.GetItem(this.draggingItem).GetType() == typeof(Items.StandardHelmet))
                    {
                        valid = true;
                    }
                    else if (slot == 1 && Item.GetItem(this.draggingItem).GetType() == typeof(Items.StandardArmor))
                    {
                        valid = true;
                    }
                    else if (slot == 2 && Item.GetItem(this.draggingItem).GetType() == typeof(Items.StandardWeapon))
                    {
                        valid = true;
                    }
                    if (valid)
                    {
                        ((Unit)this.player).Inventory.SetEquipmentAt(slot, this.draggingItem);
                        this.draggingItem = item;
                        this.dragging = this.draggingItem != -1;
                    }
                }
                else if (item != -1)
                {
                    this.dragging = true;
                    this.draggingItem = item;
                    ((Unit)this.player).Inventory.SetEquipmentAt(slot, -1);
                    Node drag = this.GetNodeByID("drag");
                    drag.X = x - node.Computed.X;
                    drag.Y = y - node.Computed.Y;
                    drag.Visible = this.dragging;
                }
            }
            else if (col == 5)
            {
                if (row == 3)
                {
                    int item = ((Unit)this.player).Inventory.GetCraftingItemAt(row);
                    if (!this.dragging && item != -1)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            ((Unit)this.player).Inventory.SetCraftingItemAt(i, -1);
                        }
                        this.dragging = true;
                        this.draggingItem = item;
                        Node drag = this.GetNodeByID("drag");
                        drag.X = x - node.Computed.X;
                        drag.Y = y - node.Computed.Y;
                        drag.Visible = this.dragging;
                    }
                }
                else
                {
                    int item = ((Unit)this.player).Inventory.GetCraftingItemAt(row);
                    ((Unit)this.player).Inventory.SetCraftingItemAt(row, this.draggingItem);
                    this.draggingItem = item;
                    this.dragging = this.draggingItem != -1;
                    Node drag = this.GetNodeByID("drag");
                    drag.X = x - node.Computed.X;
                    drag.Y = y - node.Computed.Y;
                    drag.Visible = this.dragging;
                }
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
