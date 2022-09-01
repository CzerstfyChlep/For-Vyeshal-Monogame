using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace ForVyeshal___Monogame
{
    class KeyboardControl
    {
        public int KeyDelay = 0;
        public Keys LastKey;
        public List<Keys> TextKeys = new List<Keys>{ Keys.Q, Keys.W, Keys.E, Keys.R, Keys.T, Keys.Y, Keys.U, Keys.I, Keys.O, Keys.P, Keys.A, Keys.S, Keys.D, Keys.F, Keys.G
        ,Keys.H, Keys.J, Keys.K, Keys.L, Keys.Z, Keys.X, Keys.C, Keys.V,Keys.B, Keys.N, Keys.M, Keys.Space, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7
        , Keys.D8, Keys.D9, Keys.D0, Keys.OemMinus};
        public List<Keys> ControlKeys = new List<Keys> { Keys.Back, Keys.OemTilde, Keys.A, Keys.S, Keys.D, Keys.W, Keys.Enter, Keys.Tab, Keys.Up, Keys.Down, Keys.Left, Keys.Right};
        public string GetTextKey()
        {
            KeyboardState ks = Keyboard.GetState();
            foreach (Keys key in TextKeys)
            {
                if (ks.IsKeyDown(key) && (LastKey != key || KeyDelay == 0))
                {
                    KeyDelay = 8;
                    LastKey = key;             
                    if(key.ToString().Length == 1 && (ks.IsKeyDown(Keys.LeftShift) || ks.IsKeyDown(Keys.RightShift)))
                        return key.ToString();
                    else if(key.ToString().Length == 1)
                        return key.ToString().ToLower();
                    else
                    {
                        if (key == Keys.Space)
                            return " ";
                        else if (key == Keys.D1)
                            return "1";
                        else if (key == Keys.D2)
                            return "2";
                        else if (key == Keys.D3)
                            return "3";
                        else if (key == Keys.D4)
                            return "4";
                        else if (key == Keys.D5)
                            return "5";
                        else if (key == Keys.D6)
                            return "6";
                        else if (key == Keys.D7)
                            return "7";
                        else if (key == Keys.D8)
                            return "8";
                        else if (key == Keys.D9)
                            return "9";
                        else if (key == Keys.D0)
                            return "0";
                        else if (key == Keys.OemMinus)
                            return "-";

                    }
                }
            }
            return "";
        }
        public Keys GetControlKey()
        {
            KeyboardState ks = Keyboard.GetState();
            foreach (Keys key in ControlKeys)
            {
                if (ks.IsKeyDown(key) && (LastKey != key || KeyDelay == 0))
                {
                    KeyDelay = 8;
                    LastKey = key;
                    return key;
                }
            }
            return Keys.A;
        }
    }
}
