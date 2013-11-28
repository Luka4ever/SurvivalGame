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
        private Queue<KeyEvent> inputQueue;
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
            this.FormClosing += Form1_Closing;
            this.inputQueue = new Queue<KeyEvent>();
            this.game = new Game(new Random().Next(), this, options);
            this.game.Run(BufferedGraphicsManager.Current.Allocate(CreateGraphics(), this.ClientRectangle), this.inputQueue);
            options.Save(properties);
            System.Environment.Exit(0);
        }

        private void Init()
        {
            Item.Init();
            Tile.Init();
            Entity.Init();
            ImageManager.Close();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            this.inputQueue.Enqueue(new KeyEvent(e, true));
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            this.inputQueue.Enqueue(new KeyEvent(e, false));
        }

        private void Form1_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.game.Exit();
        }
    }
}
