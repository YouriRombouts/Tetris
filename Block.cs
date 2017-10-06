using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    class Block
    {
        int m_Height = 50;
        int m_Width = 50;
        Vector2 m_Pos = new Vector2(0, 0);
        public Block(Vector2 Pos) { m_Pos = Pos; }
        public int GetHeight() { return m_Height; }
        public int GetWidth() { return m_Width; }
        public Vector2 GetPos() { return m_Pos; }
        public float GetPosY() { return m_Pos.Y + 40 ; }
        public Vector2 GetOriginPos() { return new Vector2(m_Pos.X + m_Width/2, m_Pos.Y + 23); }
        public void MoveHorizontal(int distance) { m_Pos.X += distance; }
        public void SetPos(float NewPos) { m_Pos.Y = NewPos; }
        public void Fall(int GameTime) { m_Pos.Y += GameTime; }
    }

    class Shape1x4 : Block
    {
        public Shape1x4(Vector2 Pos) : base(Pos) {}
        bool IsShape1x4 = true;
        public override int GetOriginPos()
        {
            if (IsShape1x4 == true) 
            {
                return new Vector2(m_Pos.X + m_Width / 2, m_Pos.Y + 23);
            }
        }
    }
}
