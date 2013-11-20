using SurvivalGame.src;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SurvivalGame
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Init();
            string properties = "properties.json";
            Options options = new Options(properties);
            this.Visible = true;
            Game game = new Game(new Random().Next(), this, options);
            game.Run(BufferedGraphicsManager.Current.Allocate(CreateGraphics(), this.ClientRectangle));
            options.Save(properties);
        }

        private void Init()
        {
            Item.Init();
            Tile.Init();
            ImageManager.Close();
        }
    }
}
