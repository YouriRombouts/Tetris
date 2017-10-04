using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class Block
    {
        int m_Height = 50;
        int m_Width = 50;
        float m_Vel = 0;
        Vector2 m_Pos = new Vector2(0, 0);
        public Block(Vector2 Pos) { m_Pos = Pos; }
        public int GetHeight() { return m_Height; }
        public int GetWidth() { return m_Width; }
        public float GetVel() { return m_Vel; }
        public Vector2 GetPos() { return m_Pos; }
        public float GetPosY() { return m_Pos.Y; }
        public void MoveHorizontal(int distance) { m_Pos.X += distance; }
        public void SetPos(float NewPos) { m_Pos.Y = NewPos; }
        public void SetVel(float Vel) { m_Vel = Vel; }
    }
}
