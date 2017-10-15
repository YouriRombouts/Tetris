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
        public int m_Height = 25;
        public int m_Width = 30;
        public int m_Rotation = 0;
        public string m_Color;
        public Vector2 m_Pos = new Vector2(0, 0);     
        public Block(Vector2 Pos) { m_Pos = Pos; }
        public int GetHeight() { return m_Height; }
        public int GetWidth() { return m_Width; }
        public Vector2 GetPos() { return m_Pos; }
        public float GetPosX() { return m_Pos.X; }
        public float GetPosY() { return m_Pos.Y; }
        public string GetColor() { return m_Color; }
        public void SetPosX(int NewPosX) { m_Pos.X = NewPosX; }
        public int GetGridPosX() { return (int)(m_Pos.X / 30); }
        public int GetGridPosY() { return (int)(m_Pos.Y / 25); }
        public virtual float GetMaxPosY() { return m_Pos.Y + m_Height; }
        public virtual float GetMinPosY() { return m_Pos.Y; }
        public virtual float GetMaxPosX() { return m_Pos.X + m_Width; }
        //GBIS = Get Back In Screen
        public void GBISY() { m_Pos.Y -= GetMaxPosY() - 500; }
        public void GBISYT() { m_Pos.Y -= GetMinPosY(); }
        public void GBISX() { m_Pos.X -= GetMaxPosX() - 360; }
        public Vector2 GetOriginPos() { return new Vector2(m_Pos.X + m_Width / 2, m_Pos.Y + 23); }
        public void MoveHorizontal(int distance) { m_Pos.X += distance; }
        public void SetPos(float NewPos) { m_Pos.Y = NewPos; }
        public void Fall() { m_Pos.Y += 25; }
        public void AddRotation()
        {
            if (m_Rotation < 3)
            {
                m_Rotation++;
            }
            else
            {
                m_Rotation = 0;
            }
        }
        public virtual int GetNextPosX(int BlockNumber)
        {
            return (int)m_Pos.X;
        }
        public virtual int GetNextPosY(int BlockNumber)
        {
            return (int)m_Pos.Y;
        }
        public virtual int GetNextGridPosX(int BlockNumber)
        {
            return (int)GetGridPosX();
        }
        public virtual int GetNextGridPosY(int BlockNumber)
        {
            return (int)GetGridPosY();
        }
        public virtual void Draw(SpriteBatch spriteBatch, Vector2 Scale, Texture2D SpriteColor)
        {
            if (GetMaxPosX() > 0 && GetMaxPosX() < 360)
            {
                spriteBatch.Draw(SpriteColor, this.GetPos(), scale: Scale);
            }
            else
            {
                m_Pos.X -= GetMaxPosX() - 360;
            }
        }
    }

    class IShape : Block
    {
        public IShape(Vector2 Pos) : base(Pos) { }
        bool IsIShape = true;
        public override void Draw(SpriteBatch spriteBatch, Vector2 Scale, Texture2D SpriteColor)
        {
            m_Color = "LegoBaby";
            if (IsIShape == true)
            {
                spriteBatch.Draw(SpriteColor, GetPos(), scale: Scale);
                spriteBatch.Draw(SpriteColor, new Vector2(GetNextPosX(1), GetNextPosY(1)), scale: Scale);
                spriteBatch.Draw(SpriteColor, new Vector2(GetNextPosX(2), GetNextPosY(2)), scale: Scale);
                spriteBatch.Draw(SpriteColor, new Vector2(GetNextPosX(3), GetNextPosY(3)), scale: Scale);
            }
            else
            {
                spriteBatch.Draw(SpriteColor, this.GetPos(), scale: Scale);
            }
        }
        public override float GetMaxPosX()
        {
            if (IsIShape == true)
            {
                if (m_Rotation == 0 || m_Rotation == 2)
                {
                    return (m_Pos.X + m_Width * 4);
                }
                else
                {
                    return base.GetMaxPosX();
                }
            }
            else
            {
                return base.GetMaxPosX();
            }
        }
        public override int GetNextPosX(int BlockNumber)
        {
            if (m_Rotation == 1 || m_Rotation == 3)
            {
                return (int)m_Pos.X;
            }
            else if (m_Rotation == 0 || m_Rotation == 2)
            {
                return (int)(m_Pos.X + (float)(m_Width * BlockNumber));
            }
            else { return 0; }
        }
        public override int GetNextPosY(int BlockNumber)
        {
            if (m_Rotation == 1 || m_Rotation == 3)
            {
                return (int)(m_Pos.Y - (float)(m_Height * BlockNumber));
            }
            else if (m_Rotation == 0 || m_Rotation == 2)
            {
                return (int)m_Pos.Y;
            }
            else { return 0; }
        }
        public override int GetNextGridPosX(int BlockNumber)
        {
            if (m_Rotation == 1 || m_Rotation == 3)
            {
                return (int) GetGridPosX();
            }
            else if (m_Rotation == 0 || m_Rotation == 2)
            {
                return (int) GetGridPosX() + 1 * BlockNumber;
            }
            else { return 0; }
        }
        public override int GetNextGridPosY(int BlockNumber)
        {
            if (m_Rotation == 1 || m_Rotation == 3)
            {
                return GetGridPosY() - 1 * BlockNumber;
            }
            else if (m_Rotation == 0 || m_Rotation == 2)
            {
                return GetGridPosY();
            }
            else { return 0; }
        }
        public override float GetMinPosY()
        {
            if (m_Rotation == 1 || m_Rotation == 3)
            {
                return (m_Pos.Y - 3 * m_Height);
            }
            else
            {
                return base.GetMinPosY();
            }
        }
    }
    /*class LShape : Block
    {
        public LShape(Vector2 Pos) : base(Pos) {}
        bool IsLShape = true;
        public override void Draw(SpriteBatch spriteBatch, Vector2 Scale, Texture2D SpriteColor)
        {
            m_Color = "LegoBlue";
            if (IsLShape == true)
            {
                spriteBatch.Draw(SpriteColor, GetPos(), scale: Scale);
                spriteBatch.Draw(SpriteColor, new Vector2(GetNextPosX(1), GetNextPosY(1)), scale: Scale);
                spriteBatch.Draw(SpriteColor, new Vector2(GetNextPosX(2), GetNextPosY(2)), scale: Scale);
                spriteBatch.Draw(SpriteColor, new Vector2(GetNextPosX(3), GetNextPosY(3)), scale: Scale);
            }
            else
            {
                spriteBatch.Draw(SpriteColor, this.GetPos(), scale: Scale);
            }
        }
        public override float GetMaxPosX()
        {
            if (IsLShape == true)
            {
                if (m_Rotation == 0)
                {
                    return (m_Pos.X + m_Width * 3);
                }
                else if (m_Rotation == 1)
                {
                    return (m_Pos.X + m_Width * 2);
                }
                else
                {
                    return base.GetMaxPosX();
                }
            }
            else
            {
                return base.GetMaxPosX();
            }
        }
        public override int GetNextPosX(int BlockNumber)
        {
            if (m_Rotation == 0 || m_Rotation == 2)
            {
                if (BlockNumber < 3)
                {
                    return (int)(m_Pos.X * BlockNumber);
                }
                else
                {
                    return (int)m_Pos.X;
                }

            }
            else if (m_Rotation == 1)
            {
                if (BlockNumber < 2)
                {
                    return (int)(m_Pos.X * BlockNumber);
                }
                else
                {
                    return (int)m_Pos.X;
                }

            }
            else
            {
                if (BlockNumber < 2)
                {
                    return (int)(m_Pos.X * -BlockNumber);
                }
                else
                {
                    return (int)m_Pos.X;
                }

            }
        }
        public override int GetNextPosY(int BlockNumber)
        {
            if (m_Rotation == 1 || m_Rotation == 3)
            {
                return (int)(m_Pos.Y - (float)(m_Height * BlockNumber));
            }
            else if (m_Rotation == 0 || m_Rotation == 2)
            {
                return (int)m_Pos.Y;
            }
            else { return 0; }
        }
        public override int GetNextGridPosX(int BlockNumber)
        {
            if (m_Rotation == 1 || m_Rotation == 3)
            {
                return (int)GetGridPosX();
            }
            else if (m_Rotation == 0 || m_Rotation == 2)
            {
                return (int)GetGridPosX() + 1 * BlockNumber;
            }
            else { return 0; }
        }
        public override int GetNextGridPosY(int BlockNumber)
        {
            if (m_Rotation == 1 || m_Rotation == 3)
            {
                return GetGridPosY() - 1 * BlockNumber;
            }
            else if (m_Rotation == 0 || m_Rotation == 2)
            {
                return GetGridPosY();
            }
            else { return 0; }
        }
        public override float GetMinPosY()
        {
            if (m_Rotation == 1 || m_Rotation == 3)
            {
                return (m_Pos.Y - 3 * m_Height);
            }
            else
            {
                return base.GetMinPosY();
            }
        }
    }*/
}


