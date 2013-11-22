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
        private static readonly string properties = "properties.json";
        private Queue<KeyEventArgs> inputQueue;
        private Options options;
        private Game game;

        public Form1()
        {
            InitializeComponent();
            Init();
            this.options = Options.Load(properties);
            this.Visible = true;
            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;
            this.KeyUp += Form1_KeyUp;
            this.inputQueue = new Queue<KeyEventArgs>();
            this.game = new Game(new Random().Next(), this, options);
            this.game.Run(BufferedGraphicsManager.Current.Allocate(CreateGraphics(), this.ClientRectangle), this.inputQueue);
            options.Save(properties);
            System.Environment.Exit(0);
        }

        private void Init()
        {
            Item.Init();
            Tile.Init();
            ImageManager.Close();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            this.inputQueue.Enqueue(e);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            this.inputQueue.Enqueue(e);
        }

        private void Form1_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = false;
            this.game.Exit();
        }
    }
}
