using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SurvivalGame.src
{
    class KeyEvent : KeyEventArgs
    {
        private bool keyDown;

        public KeyEvent(KeyEventArgs e, bool keyDown) : base(e.KeyData)
        {
            this.keyDown = keyDown;
        }

        public bool KeyDown
        {
            get { return this.keyDown; }
            set { this.keyDown = value; }
        }

        public bool KeyUp
        {
            get { return !this.keyDown; }
            set { this.keyDown = !value; }
        }
    }
}
