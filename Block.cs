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
        public int m_Height = 30;
        public int m_Width = 30;
        public Vector2 GridPos = new Vector2(0, 0);
        public Vector2 m_Pos = new Vector2(0, 0);
        public Block(Vector2 Pos) { m_Pos = Pos; }
        public int GetHeight() { return m_Height; }
        public int GetWidth() { return m_Width; }
        public Vector2 GetPos() { return m_Pos; }
        public Vector2 GetGridPos() { return GridPos; }
        public float GetPosY() { return m_Pos.Y + 30 ; }
        public virtual Vector2 GetOriginPos() { return new Vector2(m_Pos.X + m_Width/2, m_Pos.Y + 23); }
        public void MoveHorizontal(int distance) { m_Pos.X += distance; }
        public void SetPos(float NewPos) { m_Pos.Y = NewPos; }
        public void Fall(double GameTime)
        {
            if(GameTime % 1 == 0)
            {
                m_Pos.Y += 0;
            }
            else
            {
                //wait
            }            
        }
    }

    class Shape1x4 : Block
    {
        public Shape1x4(Vector2 Pos) : base(Pos) {}
        bool IsShape1x4 = true;
        public override Vector2 GetOriginPos()
        {
            if (IsShape1x4 == true) 
            {
                return new Vector2(m_Pos.X + 2 * m_Width, m_Pos.Y + m_Height / 2);
            }
            else
            {
                return base.GetOriginPos();
            }
        }
    }
}
