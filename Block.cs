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
        public string m_Color = "";
        public Vector2 m_Pos = new Vector2(0, 0);
        public Block(Vector2 Pos) { m_Pos = Pos; }
        public int GetHeight() { return m_Height; }
        public int GetWidth() { return m_Width; }
        public Vector2 GetPos() { return m_Pos; }
        public float GetPosX() { return m_Pos.X; }
        public float GetPosY() { return m_Pos.Y; }
        public virtual string GetColor() { return m_Color; }
        public int GetRotation() { return m_Rotation; }
        public int GetNextRotation()
        {
            if (m_Rotation == 3)
            {
                return 0;
            }
            else
            {
                return m_Rotation + 1;
            }
        }
        public int GetPrevRotation()
        {
            if (m_Rotation == 0)
            {
                return 3;
            }
            else
            {
                return m_Rotation - 1;
            }
        }
        public void SetPosX(int NewPosX) { m_Pos.X = NewPosX; }
        public int GetGridPosX() { return (int)(m_Pos.X / 30); }
        public int GetGridPosY() { return (int)(m_Pos.Y / 25); }
        public virtual float GetMaxPosY() { return m_Pos.Y + m_Height; }
        public virtual float GetMinPosY() { return m_Pos.Y; }
        public virtual float GetMaxPosX() { return m_Pos.X + m_Width; }
        public virtual float GetMinPosX() { return m_Pos.X; }
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
        public void SubRotation()
        {
            if (m_Rotation > 0)
            {
                m_Rotation--;
            }
            else
            {
                m_Rotation = 3;
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
        public virtual int GetNextGridPosX(int BlockNumber, int rotation)
        {
            return (int)GetGridPosX();
        }
        public virtual int GetNextGridPosY(int BlockNumber, int rotation)
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
        public Vector2 GetRRotatedGridPos(int BlockNumber)
        {
            return (new Vector2(GetNextGridPosX(BlockNumber, GetNextRotation()), GetNextGridPosY(BlockNumber, GetNextRotation())));
        }
        public Vector2 GetLRotatedGridPos(int BlockNumber)
        {
            return (new Vector2(GetNextGridPosX(BlockNumber, GetPrevRotation()), GetNextGridPosY(BlockNumber, GetPrevRotation())));
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
                spriteBatch.Draw(SpriteColor, new Vector2(GetNextPosX(0), GetNextPosY(0)), scale: Scale);
                spriteBatch.Draw(SpriteColor, new Vector2(GetNextPosX(1), GetNextPosY(1)), scale: Scale);
                spriteBatch.Draw(SpriteColor, new Vector2(GetNextPosX(2), GetNextPosY(2)), scale: Scale);
                spriteBatch.Draw(SpriteColor, new Vector2(GetNextPosX(3), GetNextPosY(3)), scale: Scale);
            }
            else
            {
                spriteBatch.Draw(SpriteColor, this.GetPos(), scale: Scale);
            }
        }
        public override string GetColor() { return "legobaby"; }
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
            if (m_Rotation == 0)
            {
                return (int)(m_Pos.X + (float)(m_Width * BlockNumber));
            }
            else if (m_Rotation == 1 || m_Rotation == 3)
            {
                return (int)m_Pos.X;
            }
            else if (m_Rotation == 2)
            {
                return (int)(m_Pos.X - (float)(m_Width * BlockNumber));
            }
            else { return 0; }
        }
        public override int GetNextPosY(int BlockNumber)
        {
            if (m_Rotation == 1)
            {
                return (int)(m_Pos.Y - (float)(m_Height * BlockNumber));
            }
            else if (m_Rotation == 0 || m_Rotation == 2)
            {
                return (int)m_Pos.Y;
            }
            else if (m_Rotation == 3)
            {
                return (int)(m_Pos.Y + m_Height * 3 - (float)(m_Height * BlockNumber));
            }
            else { return 0; }
        }
        public override int GetNextGridPosX(int BlockNumber, int Rotation)
        {
            if (Rotation == 1 || Rotation == 3)
            {
                return GetGridPosX();
            }
            else if (Rotation == 0 || Rotation == 2)
            {
                return GetGridPosX() + 1 * BlockNumber;
            }
            else { return 0; }
        }
        public override int GetNextGridPosY(int BlockNumber, int Rotation)
        {
            if (Rotation == 1)
            {
                return GetGridPosY() - 1 * BlockNumber;
            }
            else if (Rotation == 3)
            {
                return GetGridPosY() + 1 * BlockNumber;
            }
            else if (Rotation == 0 || Rotation == 2)
            {
                return GetGridPosY();
            }
            else { return 0; }
        }
        public override float GetMinPosY()
        {
            if (m_Rotation == 0 || m_Rotation == 2 || m_Rotation == 1)
            {
                return m_Pos.Y;
            }
            else
            {
                return m_Pos.Y + m_Height * 3;
            }

        }
        public override float GetMaxPosY()
        {
            if (m_Rotation == 0 || m_Rotation == 2 || m_Rotation == 1)
            {
                return m_Pos.Y + m_Height;
            }
            else if (m_Rotation == 3)
            {
                return m_Pos.Y + m_Height * 4;
            }
            else
            {
                return m_Pos.Y;
            }
        }
    }
    class ZShape : Block
    {
        public ZShape(Vector2 Pos) : base(Pos) { }
        bool IsZShape = true;
        public override void Draw(SpriteBatch spriteBatch, Vector2 Scale, Texture2D SpriteColor)
        {
            if (IsZShape == true)
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
        public override string GetColor() { return "legored"; }
        public override float GetMaxPosX()
        {
            if (IsZShape == true)
            {
                if (m_Rotation == 0 || m_Rotation == 2 || m_Rotation == 1 || m_Rotation == 3)
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

        public override float GetMinPosX()
        {
            if (m_Rotation == 0 || m_Rotation == 2)
            {
                return m_Pos.X - m_Width;
            }
            else return m_Pos.X;
        }
        public override int GetNextPosX(int BlockNumber)
        {
            if (m_Rotation == 0 || m_Rotation == 2)
            {
                if (BlockNumber == 2)
                {
                    return (int)(m_Pos.X);
                }
                else if (BlockNumber == 1)
                {
                    return (int)(m_Pos.X + m_Width);
                }
                else if (BlockNumber == 3)
                {
                    return (int)(m_Pos.X - m_Width);
                }
                else
                {
                    return (int)m_Pos.X;
                }

            }
            else if (m_Rotation == 1 || m_Rotation == 3)
            {
                if (BlockNumber < 2)
                {
                    return (int)(m_Pos.X);
                }
                else ///blocknummers 2 en 3 
                {
                    return (int)(m_Pos.X + m_Width);
                }
            }
            else return 0;
        }
        public override int GetNextPosY(int BlockNumber)
        {
            if (m_Rotation == 0 || m_Rotation == 2)
            {
                if (BlockNumber == 1) { return (int)m_Pos.Y; }
                else { return (int)(m_Pos.Y - m_Height); }
            }
            else if (m_Rotation == 1 || m_Rotation == 3)
            {
                if (BlockNumber == 3)
                {
                    return (int)(m_Pos.Y - m_Height * 2);
                }
                else if (BlockNumber == 1 || BlockNumber == 2)
                {
                    return (int)(m_Pos.Y - m_Height);
                }
                else
                {
                    return (int)m_Pos.Y;
                }
            }
            else { return 0; }
        }
        public override int GetNextGridPosX(int BlockNumber, int Rotation)
        {
            if (m_Rotation == 0 || m_Rotation == 2)
            {
                if (BlockNumber == 2)
                {
                    return (int)(GetGridPosX());
                }
                else if (BlockNumber == 1)
                {
                    return (int)(GetGridPosX() + 1);
                }
                else if (BlockNumber == 3)
                {
                    return (int)(GetGridPosX() - 1);
                }
                else if (BlockNumber == 0)
                {
                    return GetGridPosX();
                }
                else { return 0; }
            }
            else if (m_Rotation == 1 || m_Rotation == 3)
            {
                if (BlockNumber < 2)
                {
                    return (int)GetGridPosX();
                }
                else
                {
                    return (int)GetGridPosX() + 1;
                }
            }
            else { return 0; }
        }
        public override int GetNextGridPosY(int BlockNumber, int Rotation)
        {
            if (m_Rotation == 0 || m_Rotation == 2)
            {
                if (BlockNumber < 2)
                {
                    return (int)GetGridPosY();
                }
                else
                {
                    return (int)(GetGridPosY() - 1);
                }
            }
            else if (m_Rotation == 1 || m_Rotation == 3)
            {
                if (BlockNumber == 3)
                {
                    return (int)(GetGridPosY() - 2);
                }
                else if (BlockNumber == 1 || BlockNumber == 2)
                {
                    return (int)(GetGridPosY() - 1);
                }
                else return GetGridPosY();
            }
            else { return 0; }
        }
    }

    class SShape : Block
    {
        public SShape(Vector2 Pos) : base(Pos) { }
        bool IsSShape = true;
        public override void Draw(SpriteBatch spriteBatch, Vector2 Scale, Texture2D SpriteColor)
        {
            if (IsSShape == true)
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
        public override string GetColor() { return "legogreen"; }
        public override float GetMaxPosX()
        {
            if (IsSShape == true)
            {
                if (m_Rotation == 0 || m_Rotation == 2)
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

        public override float GetMinPosX()
        {
            return m_Pos.X - m_Width;
        }
        public override int GetNextPosX(int BlockNumber)
        {
            if (m_Rotation == 0 || m_Rotation == 2)
            {
                if (BlockNumber == 2)
                {
                    return (int)(m_Pos.X);
                }
                else if (BlockNumber == 3)
                {
                    return (int)(m_Pos.X + m_Width);
                }
                else if (BlockNumber == 1)
                {
                    return (int)(m_Pos.X - m_Width);
                }
                else
                {
                    return (int)m_Pos.X;
                }

            }
            else if (m_Rotation == 1 || m_Rotation == 3)
            {
                if (BlockNumber < 2)
                {
                    return (int)(m_Pos.X);
                }
                else ///blocknummers 2 en 3 
                {
                    return (int)(m_Pos.X - m_Width);
                }
            }
            else return 0;
        }
        public override int GetNextPosY(int BlockNumber)
        {
            if (m_Rotation == 0 || m_Rotation == 2)
            {
                if (BlockNumber == 1) { return (int)m_Pos.Y; }
                else { return (int)(m_Pos.Y - m_Height); }
            }
            else if (m_Rotation == 1 || m_Rotation == 3)
            {
                if (BlockNumber == 3)
                {
                    return (int)(m_Pos.Y - m_Height * 2);
                }
                else if (BlockNumber == 1 || BlockNumber == 2)
                {
                    return (int)(m_Pos.Y - m_Height);
                }
                else
                {
                    return (int)m_Pos.Y;
                }
            }
            else { return 0; }
        }
        public override int GetNextGridPosX(int BlockNumber, int Rotation)
        {
            if (m_Rotation == 0 || m_Rotation == 2)
            {
                if (BlockNumber == 2)
                {
                    return (GetGridPosX());
                }
                else if (BlockNumber == 3)
                {
                    return (GetGridPosX() + 1);
                }
                else if (BlockNumber == 1)
                {
                    return (GetGridPosX() - 1);
                }
                else if (BlockNumber == 0)
                {
                    return GetGridPosX();
                }
                else { return 0; }
            }
            else if (m_Rotation == 1 || m_Rotation == 3)
            {
                if (BlockNumber < 2)
                {
                    return (int)GetGridPosX();
                }
                else
                {
                    return (int)GetGridPosX() - 1;
                }
            }
            else { return 0; }
        }
        public override int GetNextGridPosY(int BlockNumber, int Rotation)
        {
            if (m_Rotation == 0 || m_Rotation == 2)
            {
                if (BlockNumber < 2)
                {
                    return (int)GetGridPosY();
                }
                else
                {
                    return (int)(GetGridPosY() - 1);
                }
            }
            else if (m_Rotation == 1 || m_Rotation == 3)
            {
                if (BlockNumber == 3)
                {
                    return (int)(GetGridPosY() - 2);
                }
                else if (BlockNumber == 1 || BlockNumber == 2)
                {
                    return (int)(GetGridPosY() - 1);
                }
                else return GetGridPosY();
            }
            else { return 0; }
        }
    }
    class OShape : Block
    {
        public OShape(Vector2 Pos) : base(Pos) { }
        bool IsOShape = true;
        public override void Draw(SpriteBatch spriteBatch, Vector2 Scale, Texture2D SpriteColor)
        {
            m_Color = "Legoyellow";
            if (IsOShape == true)
            {
                spriteBatch.Draw(SpriteColor, new Vector2(GetNextPosX(0), GetNextPosY(0)), scale: Scale);
                spriteBatch.Draw(SpriteColor, new Vector2(GetNextPosX(1), GetNextPosY(1)), scale: Scale);
                spriteBatch.Draw(SpriteColor, new Vector2(GetNextPosX(2), GetNextPosY(2)), scale: Scale);
                spriteBatch.Draw(SpriteColor, new Vector2(GetNextPosX(3), GetNextPosY(3)), scale: Scale);
            }
            else
            {
                spriteBatch.Draw(SpriteColor, this.GetPos(), scale: Scale);
            }
        }
        public override string GetColor() { return "legoyellow"; }
        public override float GetMaxPosX()
        {
            if (IsOShape == true)
            {
                return m_Pos.X + GetWidth() * 2;
            }
            else
            {
                return m_Pos.X;
            }
        }
        public override int GetNextPosX(int BlockNumber)
        {
            if (BlockNumber < 2)
            {
                return (int)m_Pos.X;
            }
            else
            {
                return (int)m_Pos.X + GetWidth();
            }
        }
        public override int GetNextPosY(int BlockNumber)
        {
            if (BlockNumber == 1 || BlockNumber == 3)
            {
                return (int)m_Pos.Y - GetHeight();
            }
            else if (BlockNumber == 2 || BlockNumber == 0)
            {
                return (int)m_Pos.Y;
            }
            else return 0;
        }
        public override int GetNextGridPosX(int BlockNumber, int Rotation)
        {
            if (BlockNumber < 2)
            {
                return (int)GetGridPosX();
            }
            else
            {
                return (int)GetGridPosX() + 1;
            }
        }
        public override int GetNextGridPosY(int BlockNumber, int Rotation)
        {
            if (BlockNumber == 1 || BlockNumber == 3)
            {
                return GetGridPosY() - 1;
            }
            else
            {
                return GetGridPosY();
            }
        }
        /*public override Vector2 GetRRotatedGridPos(int BlockNumber)
        {
            return (new Vector2(GetNextGridPosX(BlockNumber, m_Rotation + 1), GetNextGridPosY(BlockNumber, m_Rotation + 1)));
        }
        public override Vector2 GetLRotatedGridPos(int BlockNumber)
        {
            return (new Vector2(GetNextGridPosX(BlockNumber, m_Rotation - 1), GetNextGridPosY(BlockNumber, m_Rotation - 1)));
        }*/
        public override float GetMinPosY()
        {
            return m_Pos.Y - GetHeight() * 2;
        }
        public override float GetMaxPosY()
        {
            return m_Pos.Y + m_Height;
        }
    }
    class TShape : Block
    {
        public TShape(Vector2 Pos) : base(Pos) { }
        bool IsTShape = true;
        public override void Draw(SpriteBatch spriteBatch, Vector2 Scale, Texture2D SpriteColor)
        {
            if (IsTShape == true)
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
        public override string GetColor() { return "legopurple"; }
        public override float GetMaxPosX()
        {
            if (IsTShape == true)
            {
                if (m_Rotation == 0 || m_Rotation == 1 || m_Rotation == 2)
                {
                    return (m_Pos.X + 2 * m_Width);
                }
                else if (m_Rotation == 3)
                {
                    return (m_Pos.X + m_Width);
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

        public override float GetMinPosX()
        {
            if (m_Rotation == 0 || m_Rotation == 3 || m_Rotation == 2)
            {
                return (m_Pos.X - m_Width);
            }
            else return m_Pos.X;
        }
        public override int GetNextPosX(int BlockNumber)
        {
            if (m_Rotation == 0)
            {
                if (BlockNumber == 2 || BlockNumber == 0)
                {
                    return (int)(m_Pos.X);
                }
                else if (BlockNumber == 1)
                {
                    return (int)(m_Pos.X - m_Width);
                }
                else if (BlockNumber == 3)
                {
                    return (int)(m_Pos.X + m_Width);
                }
                else
                {
                    return (int)m_Pos.X;
                }

            }
            else if (m_Rotation == 1)
            {
                if (BlockNumber < 3)
                {
                    return (int)(m_Pos.X);
                }
                else
                {
                    return (int)(m_Pos.X + m_Width);
                }
            }
            else if (m_Rotation == 2)
            {
                if (BlockNumber == 0 || BlockNumber == 3)
                {
                    return (int)m_Pos.X;
                }
                if (BlockNumber == 1)
                {
                    return (int)m_Pos.X - m_Width;
                }
                if (BlockNumber == 2)
                {
                    return (int)m_Pos.X + m_Width;
                }
                else return 0;
            }
            else if (m_Rotation == 3)
            {
                if (BlockNumber < 3)
                {
                    return (int)m_Pos.X;
                }
                else return (int)m_Pos.X - m_Width;
            }
            else return 0;
        }
        public override int GetNextPosY(int BlockNumber)
        {
            if (m_Rotation == 0)
            {
                if (BlockNumber > 0)
                {
                    return (int)m_Pos.Y - m_Height;
                }
                else
                {
                    return (int)(m_Pos.Y);
                }
            }
            else if (m_Rotation == 1)
            {
                if (BlockNumber == 3 || BlockNumber == 1)
                {
                    return (int)(m_Pos.Y - m_Height);
                }
                else if (BlockNumber == 2)
                {
                    return (int)(m_Pos.Y - m_Height * 2);
                }
                else
                {
                    return (int)m_Pos.Y;
                }
            }
            else if (m_Rotation == 2)
            {
                if (BlockNumber < 3)
                {
                    return (int)m_Pos.Y;
                }
                else return (int)m_Pos.Y - m_Height;
            }
            else if (m_Rotation == 3)
            {
                if (BlockNumber == 1 || BlockNumber == 3)
                {
                    return (int)m_Pos.Y - m_Height;
                }
                else if (BlockNumber == 2)
                {
                    return (int)m_Pos.Y - 2 * m_Height;
                }
                else
                {
                    return (int)m_Pos.Y;
                }
            }
            else { return (int)m_Pos.Y; }
        }
        public override int GetNextGridPosX(int BlockNumber, int Rotation)
        {
            if (m_Rotation == 0)
            {
                if (BlockNumber == 2 || BlockNumber == 0)
                {
                    return GetGridPosX();
                }
                else if (BlockNumber == 1)
                {
                    return (GetGridPosX() - 1);
                }
                else if (BlockNumber == 3)
                {
                    return (int)(GetGridPosX() + 1);
                }
                else
                {
                    return GetGridPosX();
                }

            }
            else if (m_Rotation == 1)
            {
                if (BlockNumber < 3)
                {
                    return GetGridPosX();
                }
                else
                {
                    return (GetGridPosX() + 1);
                }
            }
            else if (m_Rotation == 2)
            {
                if (BlockNumber == 0 || BlockNumber == 3)
                {
                    return GetGridPosX();
                }
                if (BlockNumber == 1)
                {
                    return GetGridPosX() - 1;
                }
                if (BlockNumber == 2)
                {
                    return GetGridPosX() + 1;
                }
                else return 0;
            }
            else if (m_Rotation == 3)
            {
                if (BlockNumber < 3)
                {
                    return GetGridPosX();
                }
                else return GetGridPosX() - 1;
            }
            else return 0;
        }
        public override int GetNextGridPosY(int BlockNumber, int Rotation)
        {
            if (m_Rotation == 0)
            {
                if (BlockNumber > 0)
                {
                    return GetGridPosY() - 1;
                }
                else
                {
                    return GetGridPosY();
                }
            }
            else if (m_Rotation == 1)
            {
                if (BlockNumber == 3 || BlockNumber == 1)
                {
                    return GetGridPosY() - 1;
                }
                else if (BlockNumber == 2)
                {
                    return (int)(GetGridPosY() - 2);
                }
                else
                {
                    return GetGridPosY();
                }
            }
            else if (m_Rotation == 2)
            {
                if (BlockNumber < 3)
                {
                    return GetGridPosY();
                }
                else return GetGridPosY() - 1;
            }
            else if (m_Rotation == 3)
            {
                if (BlockNumber == 1 || BlockNumber == 3)
                {
                    return GetGridPosY() - 1;
                }
                else if (BlockNumber == 2)
                {
                    return GetGridPosY() - 2;
                }
                else
                {
                    return GetGridPosY();
                }
            }
            else { return GetGridPosY(); }
        }
        public override float GetMinPosY()
        {
            if (m_Rotation == 0 || m_Rotation == 2)
            {
                return m_Pos.Y - m_Height;
            }
            else
            {
                return m_Pos.Y - m_Height * 2;
            }
        }
        public override float GetMaxPosY()
        {
            return m_Pos.Y + m_Height;
        }
    }
    class LShape : Block
    {
        public LShape(Vector2 Pos) : base(Pos) { }
        bool IsLShape = true;
        public override void Draw(SpriteBatch spriteBatch, Vector2 Scale, Texture2D SpriteColor)
        {
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
        public override string GetColor() { return "legoorange"; }
        public override float GetMaxPosX()
        {
            if (IsLShape == true)
            {
                if (m_Rotation == 2 || m_Rotation == 0)
                {
                    return (m_Pos.X + 2 * m_Width);
                }
                else if (m_Rotation == 1)
                {
                    return (m_Pos.X + m_Width);
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

        public override float GetMinPosX()
        {
            if (m_Rotation == 3 || m_Rotation == 2 || m_Rotation == 0)
            {
                return (m_Pos.X);
            }
            else return m_Pos.X - m_Width;
        }
        public override int GetNextPosX(int BlockNumber)
        {
            if (m_Rotation == 3)
            {
                if (BlockNumber == 0 || BlockNumber == 2 || BlockNumber == 3)
                {
                    return (int)(m_Pos.X);
                }
                else if (BlockNumber == 1)
                {
                    return (int)(m_Pos.X + m_Width);
                }
                else
                {
                    return (int)m_Pos.X;
                }

            }
            else if (m_Rotation == 0)
            {
                if (BlockNumber > 1)
                {
                    return (int)(m_Pos.X + 2 * m_Width);
                }
                else if (BlockNumber == 1)
                {
                    return (int)(m_Pos.X + m_Width);
                }
                else return (int)m_Pos.X;
            }
            else if (m_Rotation == 1)
            {
                if (BlockNumber < 3)
                {
                    return (int)m_Pos.X;
                }
                if (BlockNumber == 3)
                {
                    return (int)m_Pos.X - m_Width;
                }
                else return 0;
            }
            else if (m_Rotation == 2)
            {
                if (BlockNumber < 2)
                {
                    return (int)m_Pos.X;
                }
                else if (BlockNumber == 2)
                {
                    return (int)m_Pos.X + m_Width;
                }
                else
                {
                    return (int)m_Pos.X + m_Width * 2;
                }
            }
            else return 0;
        }
        public override int GetNextPosY(int BlockNumber)
        {
            if (m_Rotation == 3)
            {
                if (BlockNumber < 2)
                {
                    return (int)m_Pos.Y;
                }
                else if (BlockNumber == 2)
                {
                    return (int)m_Pos.Y - m_Height;
                }
                else if (BlockNumber == 3)
                {
                    return (int)m_Pos.Y - 2 * m_Height;
                }
                else
                {
                    return (int)(m_Pos.Y);
                }
            }
            else if (m_Rotation == 0)
            {
                if (BlockNumber < 3)
                {
                    return (int)(m_Pos.Y);
                }
                else if (BlockNumber == 3)
                {
                    return (int)m_Pos.Y - m_Height;
                }
                else
                {
                    return (int)m_Pos.Y;
                }
            }
            else if (m_Rotation == 1)
            {
                if (BlockNumber > 1)
                {
                    return (int)m_Pos.Y - m_Height * 2;
                }
                else if (BlockNumber == 1)
                {
                    return (int)m_Pos.Y - m_Height;
                }
                else return (int)m_Pos.Y;
            }
            else if (m_Rotation == 2)
            {
                if (BlockNumber > 0)
                {
                    return (int)m_Pos.Y - m_Height;
                }
                else
                {
                    return (int)m_Pos.Y;
                }
            }
            else { return (int)m_Pos.Y; }
        }
        public override int GetNextGridPosX(int BlockNumber, int Rotation)
        {
            if (m_Rotation == 3)
            {
                if (BlockNumber == 0 || BlockNumber == 2 || BlockNumber == 3)
                {
                    return GetGridPosX();
                }
                else if (BlockNumber == 1)
                {
                    return GetGridPosX() + 1;
                }
                else
                {
                    return GetGridPosX();
                }

            }
            else if (m_Rotation == 0)
            {
                if (BlockNumber > 1)
                {
                    return GetGridPosX() + 2;
                }
                else if (BlockNumber == 1)
                {
                    return GetGridPosX() + 1;
                }
                else return GetGridPosX();
            }
            else if (m_Rotation == 1)
            {
                if (BlockNumber < 3)
                {
                    return GetGridPosX();
                }
                if (BlockNumber == 3)
                {
                    return GetGridPosX() - 1;
                }
                else return 0;
            }
            else if (m_Rotation == 2)
            {
                if (BlockNumber < 2)
                {
                    return GetGridPosX();
                }
                else if (BlockNumber == 2)
                {
                    return GetGridPosX();
                }
                else
                {
                    return GetGridPosX() + 2;
                }
            }
            else return 0;
        }
        public override int GetNextGridPosY(int BlockNumber, int Rotation)
        {
            if (m_Rotation == 3)
            {
                if (BlockNumber < 2)
                {
                    return GetGridPosY();
                }
                else if (BlockNumber == 2)
                {
                    return GetGridPosY() - 1;
                }
                else if (BlockNumber == 3)
                {
                    return GetGridPosY() - 2;
                }
                else
                {
                    return GetGridPosY();
                }
            }
            else if (m_Rotation == 0)
            {
                if (BlockNumber < 3)
                {
                    return GetGridPosY();
                }
                else if (BlockNumber == 3)
                {
                    return GetGridPosY() - 1;
                }
                else
                {
                    return GetGridPosY();
                }
            }
            else if (m_Rotation == 1)
            {
                if (BlockNumber > 1)
                {
                    return GetGridPosY() - 2;
                }
                else if (BlockNumber == 1)
                {
                    return GetGridPosY() - 1;
                }
                else return GetGridPosY();
            }
            else if (m_Rotation == 2)
            {
                if (BlockNumber > 0)
                {
                    return GetGridPosY() - 1;
                }
                else
                {
                    return GetGridPosY();
                }
            }
            else { return GetGridPosY(); }
        }
        public override float GetMinPosY()
        {
            if (m_Rotation == 3 || m_Rotation == 1)
            {
                return m_Pos.Y - m_Height * 2;
            }
            else
            {
                return m_Pos.Y - m_Height;
            }
        }
        public override float GetMaxPosY()
        {
            return m_Pos.Y + m_Height;
        }
    }
    class JShape : Block
    {
        public JShape(Vector2 Pos) : base(Pos) { }
        bool IsJShape = true;
        public override void Draw(SpriteBatch spriteBatch, Vector2 Scale, Texture2D SpriteColor)
        {
            if (IsJShape == true)
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
        public override string GetColor() { return "legoblue"; }
        public override float GetMaxPosX()
        {
            if (IsJShape == true)
            {
                if (m_Rotation == 2 || m_Rotation == 0)
                {
                    return (m_Pos.X + 2 * m_Width);
                }
                else if (m_Rotation == 1)
                {
                    return (m_Pos.X + m_Width);
                }
                else if (m_Rotation == 3)
                {
                    return m_Pos.X - m_Width;
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

        public override float GetMinPosX()
        {
            if (m_Rotation == 1 || m_Rotation == 2 || m_Rotation == 0)
            {
                return (m_Pos.X);
            }
            else return m_Pos.X - m_Width;
        }
        public override int GetNextPosX(int BlockNumber)
        {
            if (m_Rotation == 3)
            {
                if (BlockNumber == 0 || BlockNumber == 2 || BlockNumber == 3)
                {
                    return (int)(m_Pos.X);
                }
                else if (BlockNumber == 1)
                {
                    return (int)(m_Pos.X - m_Width);
                }
                else
                {
                    return (int)m_Pos.X;
                }

            }
            else if (m_Rotation == 0)
            {
                if (BlockNumber == 0 || BlockNumber == 1)
                {
                    return (int)m_Pos.X;
                }
                else if (BlockNumber == 2)
                {
                    return (int)(m_Pos.X - m_Width);
                }
                else return (int)m_Pos.X - 2 * m_Width;
            }
            else if (m_Rotation == 1)
            {
                if (BlockNumber < 3)
                {
                    return (int)m_Pos.X;
                }
                if (BlockNumber == 3)
                {
                    return (int)m_Pos.X + m_Width;
                }
                else return 0;
            }
            else if (m_Rotation == 2)
            {
                if (BlockNumber < 2)
                {
                    return (int)m_Pos.X;
                }
                else if (BlockNumber == 2)
                {
                    return (int)m_Pos.X + m_Width;
                }
                else
                {
                    return (int)m_Pos.X + m_Width * 2;
                }
            }
            else return 0;
        }
        public override int GetNextPosY(int BlockNumber)
        {
            if (m_Rotation == 3)
            {
                if (BlockNumber < 2)
                {
                    return (int)m_Pos.Y;
                }
                else if (BlockNumber == 2)
                {
                    return (int)m_Pos.Y - m_Height;
                }
                else if (BlockNumber == 3)
                {
                    return (int)m_Pos.Y - 2 * m_Height;
                }
                else
                {
                    return (int)(m_Pos.Y);
                }
            }
            else if (m_Rotation == 0)
            {
                if (BlockNumber > 0)
                {
                    return (int)(m_Pos.Y) - m_Height;
                }
                else if (BlockNumber == 0)
                {
                    return (int)m_Pos.Y;
                }
                else
                {
                    return (int)m_Pos.Y;
                }
            }
            else if (m_Rotation == 1)
            {
                if (BlockNumber > 1)
                {
                    return (int)m_Pos.Y - m_Height * 2;
                }
                else if (BlockNumber == 1)
                {
                    return (int)m_Pos.Y - m_Height;
                }
                else return (int)m_Pos.Y;
            }
            else if (m_Rotation == 2)
            {
                if (BlockNumber == 0 || BlockNumber == 2 || BlockNumber == 3)
                {
                    return (int)m_Pos.Y;
                }
                else
                {
                    return (int)m_Pos.Y - m_Height;
                }
            }
            else { return (int)m_Pos.Y; }
        }
        public override int GetNextGridPosX(int BlockNumber, int Rotation)
        {
            if (m_Rotation == 3)
            {
                if (BlockNumber == 0 || BlockNumber == 2 || BlockNumber == 3)
                {
                    return GetGridPosX();
                }
                else if (BlockNumber == 1)
                {
                    return GetGridPosX() - 1;
                }
                else
                {
                    return GetGridPosX();
                }

            }
            else if (m_Rotation == 0)
            {
                if (BlockNumber == 0 || BlockNumber == 1)
                {
                    return GetGridPosX();
                }
                else if (BlockNumber == 2)
                {
                    return GetGridPosX() - 1;
                }
                else return GetGridPosX() - 2;
            }
            else if (m_Rotation == 1)
            {
                if (BlockNumber < 3)
                {
                    return GetGridPosX();
                }
                if (BlockNumber == 3)
                {
                    return GetGridPosX() + 1;
                }
                else return 0;
            }
            else if (m_Rotation == 2)
            {
                if (BlockNumber < 2)
                {
                    return GetGridPosX();
                }
                else if (BlockNumber == 2)
                {
                    return GetGridPosX();
                }
                else
                {
                    return GetGridPosX() + 2;
                }
            }
            else return 0;
        }
        public override int GetNextGridPosY(int BlockNumber, int Rotation)
        {
            if (m_Rotation == 3)
            {
                if (BlockNumber < 2)
                {
                    return GetGridPosY();
                }
                else if (BlockNumber == 2)
                {
                    return GetGridPosY() - 1;
                }
                else if (BlockNumber == 3)
                {
                    return GetGridPosY() - 2;
                }
                else
                {
                    return GetGridPosY();
                }
            }
            else if (m_Rotation == 0)
            {
                if (BlockNumber > 0)
                {
                    return GetGridPosY() - 1;
                }
                else if (BlockNumber == 0)
                {
                    return GetGridPosY();
                }
                else
                {
                    return GetGridPosY();
                }
            }
            else if (m_Rotation == 1)
            {
                if (BlockNumber > 1)
                {
                    return GetGridPosY() - 2;
                }
                else if (BlockNumber == 1)
                {
                    return GetGridPosY() - 1;
                }
                else return GetGridPosY();
            }
            else if (m_Rotation == 2)
            {
                if (BlockNumber == 0 || BlockNumber == 2 || BlockNumber == 3)
                {
                    return GetGridPosY();
                }
                else
                {
                    return GetGridPosY() - 1;
                }
            }
            else { return GetGridPosY(); }
        }
        public override float GetMinPosY()
        {
            if (m_Rotation == 3 || m_Rotation == 1)
            {
                return m_Pos.Y - m_Height * 2;
            }
            else
            {
                return m_Pos.Y - m_Height;
            }
        }
        public override float GetMaxPosY()
        {
            return m_Pos.Y + m_Height;
        }
    }
}