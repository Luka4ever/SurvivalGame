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

        public Form1()
        {
            InitializeComponent();
            Init();
            this.options = new Options(properties);
            this.Visible = true;
            this.KeyPreview = true;
            this.inputQueue = new Queue<KeyEventArgs>();
            Game game = new Game(new Random().Next(), this, options);
            //this.Invalidate();
            game.Run(BufferedGraphicsManager.Current.Allocate(CreateGraphics(), this.ClientRectangle), this.inputQueue);
            options.Save(properties);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Close();
            Application.Exit();
            Application.ExitThread();
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
    }
}
