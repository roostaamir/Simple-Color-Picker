using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ColorPicker
{
    public partial class MainFrom : Form
    {
        private IKeyboardMouseEvents _keyboardMouseEvents;
        private bool _isControlPressed = false;
        private Color _pixelColor;
        private Color _redOnly;
        private Color _blueOnly;
        private Color _greenOnly;

        public MainFrom()
        {
            InitializeComponent();
            SubscribeMouseEvents(Hook.GlobalEvents());
        }

        private void SubscribeMouseEvents(IKeyboardMouseEvents events)
        {
            _keyboardMouseEvents = events;
            _keyboardMouseEvents.MouseMove += OnMouseMove;
            _keyboardMouseEvents.KeyDown += OnKeyDown;
            _keyboardMouseEvents.KeyUp += OnKeyUp;
        }

        private void UnSubscribeMouseEvents()
        {
            if (_keyboardMouseEvents == null) return;
            _keyboardMouseEvents.MouseMove -= OnMouseMove; 
            _keyboardMouseEvents.KeyDown -= OnKeyDown;
            _keyboardMouseEvents.KeyUp -= OnKeyUp;
            _keyboardMouseEvents.Dispose();
            _keyboardMouseEvents = null;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            _pixelColor = ColorPicker.GetPixelColor(e.X, e.Y);
            pictureBox.BackColor = _pixelColor;
            pictureBoxRed.BackColor = Color.FromArgb(_pixelColor.R, 0, 0);
            pictureBoxGreen.BackColor = Color.FromArgb(0, _pixelColor.G, 0);
            pictureBoxBlue.BackColor = Color.FromArgb(0, 0, _pixelColor.B);

            lblRedValue.Text = _pixelColor.R.ToString();
            lblGreenValue.Text = _pixelColor.G.ToString();
            lblBlueValue.Text = _pixelColor.B.ToString();
            lblHexValue.Text = RGBToHex(_pixelColor);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.LControlKey:
                case Keys.RControlKey:
                    _isControlPressed = true;
                    break;
                case Keys.G:
                    if (_isControlPressed) Clipboard.SetText(RGBToHex(_pixelColor));
                    break;
                default:
                    break;
            }
                
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.LControlKey:
                case Keys.RControlKey:
                    _isControlPressed = true;
                    break;
                default:
                    break;
            }
        }

        private void MainFrom_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnSubscribeMouseEvents();
        }

        private static String RGBToHex(Color color)
        {
            return "#" + color.R.ToString("x2") + color.G.ToString("x2") + color.B.ToString("x2");
        }

        private static String ARGBToHex(Color color)
        {
            return "#" + color.A.ToString("x2") + color.R.ToString("x2") + color.G.ToString("x2") + color.B.ToString("X2");
        }
    }
}
