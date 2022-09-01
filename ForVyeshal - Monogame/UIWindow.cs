using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ForVyeshal___Monogame
{
    public class UIWindow : Control
    {
        public string Title = "";        
        public bool Closable = false;
        public bool PermaClose = false;
        public bool Movable = false;
        public string Type;       
        public List<ExtraLine> ExtraLines = new List<ExtraLine>();       
        public bool Visible = true;

    }

    public class ExtraLine : Control
    {
        public Vector2 SecondPosition = new Vector2();
        public int LineWidth = 6;
        public Color Color;
        public ExtraLine(Vector2 pos1, Vector2 pos2, Color c, int size = 6)
        {
            Position = pos1;
            SecondPosition = pos2;
            Color = c;
            LineWidth = size;
        }
        public Vector2 GetAbsoluteSecondPosition()
        {
            Vector2 RelativePosition = SecondPosition;
            if (Parent != null)
                RelativePosition += Parent.GetAbsolutePosition();
            return RelativePosition;
        }
    }

    public class TextInput : Control
    {
        public string Text = "";
        public bool Focus = false;
        public SpriteFont Font;
        public int Width = 0;
        public TextInput(Vector2 position, SpriteFont font, int width)
        {
            Position = position;
            Font = font;
            Width = width;
        }
    }

    public class ImageBox : Control
    {

    }

    public class Scroll : Control
    {
        public int ScrollPosition = 0;
        public int MaxScroll = 10;
        public int ScrollAmmount = 5;
        public int ScrollShown = 0;
    }

    public class Container : Control
    {

        
        public Container(string name, Vector2 position, Vector2 size)
        {
            Name = name;
            Position = position;
            Size = size;
        }       
    }

    public class Button : Control
    {
        public string Text;
        public Color Color;
        public Texture2D Texture = null;
        public bool Clicked = false;
        public Action<string> Method = null;
        public bool Shadow = false;
        public string Argument = "";
        public SpriteFont Font = null;
        public Button(string name, string text, Color color, Vector2 position, SpriteFont font, Vector2 size, Action<string> method, string arg = "")
        {
            Name = name;
            Text = text;
            Color = color;
            Position = position;
            Size = size;
            Method = method;
            Argument = arg;
            Font = font;
        }
    }

    public class Label : Control
    {
        public string Text;
        public Color Color;
        public SpriteFont Font;
        public AlignmentClass Alignment = TextAlignemt.Center;
        public Label(string name, string text, Color color, Vector2 position, SpriteFont font, bool allowclipping = false)
        {
            Name = name;
            Text = text;
            Color = color;
            Position = position;
            Font = font;
            AllowClipping = allowclipping;
        }
        public Label(string name, string text, Color color, Vector2 position, Vector2 size, SpriteFont font, bool allowclipping = false)
        {
            Name = name;
            Text = text;
            Color = color;
            Position = position;
            Size = size;
            Font = font;
            AllowClipping = allowclipping;
        }
        public class AlignmentClass
        {
            public string Name = "";
            public AlignmentClass(string n)
            {
                Name = n;
            }
        }
        
    }

    static class TextAlignemt
    {
        public static Label.AlignmentClass TopLeft = new Label.AlignmentClass("TopLeft");
        public static Label.AlignmentClass Top = new Label.AlignmentClass("Top");
        public static Label.AlignmentClass TopRight = new Label.AlignmentClass("TopRight");
        public static Label.AlignmentClass MiddleLeft = new Label.AlignmentClass("MiddleLeft");
        public static Label.AlignmentClass Center = new Label.AlignmentClass("Center");
        public static Label.AlignmentClass MiddleRight = new Label.AlignmentClass("MiddleRight");
        public static Label.AlignmentClass BottomLeft = new Label.AlignmentClass("BottomLeft");
        public static Label.AlignmentClass Bottom = new Label.AlignmentClass("Bottom");
        public static Label.AlignmentClass BottomRight = new Label.AlignmentClass("BottomRight");
    }


    public class Control
    {
        public string Name;
        public Vector2 Position;
        public Vector2 Size = new Vector2(0, 0);
        public Color Backcolor = Color.White;
        public Control Parent = null;
        public List<Control> Controls = new List<Control>();
        public bool Scrolable = false;
        public bool AllowClipping = false;
        public Scroll Scroll = null;
        public Control GetControl(string ControlName)
        {
            return Controls.Find(x => x.Name == ControlName);
        } 
        public void AddControl(Control c)
        {
            Controls.Add(c);
            c.Parent = this;
        }
        public void AttachScroll(int maxscroll = 10, int scrollammount = 5)
        {
            Scroll s = new Scroll();
            Scroll = s;
            AddControl(s);
            s.Position = new Vector2(Size.X - 30, 10);
            s.Size = new Vector2(20, Size.Y - 20);
            s.ScrollAmmount = scrollammount;
            s.MaxScroll = maxscroll;
            //RenewScroll();
            s.ScrollShown = (int)Math.Floor(Size.Y / s.ScrollAmmount);
        }
        public void DetachScroll()
        {
            Controls.Remove(Scroll);
            Scroll = null;            
        }
        public void RenewScroll()
        {
            float lowest = 0;
            int ScrollPos = Scroll.ScrollPosition;
            ScrollHandle(-1000);
            foreach(Control c in Controls)
            {
                if(c.Position.Y + c.Size.Y > Size.Y && c.Position.Y + c.Size.Y > lowest)
                {
                    lowest = c.Size.Y + c.Position.Y;
                }
            }
            if(lowest > 0)
            {
                float needed = lowest - Size.Y;
                float toadd = needed / Scroll.ScrollAmmount;
                toadd = (int)Math.Ceiling(toadd);
                Scroll.MaxScroll = (int)toadd;
            }
            else
            {
                Scroll.MaxScroll = 0;
            }
            ScrollHandle(ScrollPos);
        }
        public void ScrollHandle(int ammount)
        {
            int amm = 0;
            if (ammount > 0)
            {
                if (ammount + Scroll.ScrollPosition <= Scroll.MaxScroll)
                {
                    amm = Scroll.ScrollAmmount * -ammount;
                    Scroll.ScrollPosition += ammount;
                }
                else
                {
                    amm = Scroll.ScrollAmmount * -(Scroll.MaxScroll - Scroll.ScrollPosition);
                    Scroll.ScrollPosition = Scroll.MaxScroll;
                }
            }
            else
            {
                if (ammount * -1 <= Scroll.ScrollPosition)
                {
                    amm = Scroll.ScrollAmmount * -ammount;
                    Scroll.ScrollPosition += ammount;
                }
                else
                {
                    amm = Scroll.ScrollAmmount * (Scroll.ScrollPosition * 1);
                    Scroll.ScrollPosition = 0;
                }
            }
            foreach (Control c in Controls)
            {                
                if (!(c is Scroll) && c.Scrolable)
                {
                    c.Position.Y += amm;
                }
            }
            
        }
       

        /// <summary>
        /// Returns absolute position, that is, position acording to game window, not it's parent.
        /// </summary>
        /// <returns>Absolute position</returns>
        public Vector2 GetAbsolutePosition()
        {           
            Vector2 RelativePosition = Position;
            if (Parent != null)
                RelativePosition += Parent.GetAbsolutePosition();
            return RelativePosition;
        }
        public Vector2 GetAbsolutePosition(Vector2 point)
        {
            Vector2 RelativePosition = point;
            if (Parent != null)
                RelativePosition += Parent.GetAbsolutePosition();
            return RelativePosition;
        }
        public List<Control> GetAllChildren()
        {
            List<Control> controls = new List<Control>();
            foreach(Control c in Controls)
            {
                controls.Add(c);
                controls.AddRange(c.GetAllChildren());
            }
            return controls;
        }     
        public List<Control> GetAllParents()
        {
            List<Control> Parents = new List<Control>();
            if (Parent != null && Parent != this)
            {
                Parents.AddRange(Parent.GetAllParents());
                Parents.Add(Parent);
                return Parents;
            }
            else
            {                
                return Parents;
            }
            
        }
    }
}
