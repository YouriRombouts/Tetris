﻿using System;
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
        public Vector2 GridPos = new Vector2(0, 0);
        public Vector2 m_Pos = new Vector2(0, 0);
        public Block(Vector2 Pos) { m_Pos = Pos; }
        public int GetHeight() { return m_Height; }
        public int GetWidth() { return m_Width; }
        public Vector2 GetPos() { return m_Pos; }
        public float GetPosX() { return m_Pos.X; }
        public float GetPosY() { return m_Pos.Y; }
        public string GetColor() { return m_Color; }
        public void SetPosX(int NewPosX) { m_Pos.X = NewPosX; }
        public Vector2 GetGridPos() { return GridPos; }
        public int GetGridPosX() { return (int)GridPos.X; }
        public int GetGridPosY() { return (int)GridPos.Y; }
        public virtual float GetMaxPosY() { return m_Pos.Y + m_Height; }
        public virtual float GetMaxPosX() { return m_Pos.X + m_Width; }
        //GBIS = Get Back In Screen
        public void GBISY() { m_Pos.Y -= GetMaxPosY() - 500; }
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
        public virtual Vector2 GetNextPos(int BlockNumber)
        {
            return m_Pos;
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

    class Shape1x4 : Block
    {
        public Shape1x4(Vector2 Pos) : base(Pos) { }
        bool IsShape1x4 = true;
        public override void Draw(SpriteBatch spriteBatch, Vector2 Scale, Texture2D SpriteColor)
        {
            m_Color = "LegoBaby";
            if (IsShape1x4 == true)
            {
                spriteBatch.Draw(SpriteColor, GetPos(), scale: Scale);
                spriteBatch.Draw(SpriteColor, GetNextPos(1), scale: Scale);
                spriteBatch.Draw(SpriteColor, GetNextPos(2), scale: Scale);
                spriteBatch.Draw(SpriteColor, GetNextPos(3), scale: Scale);
            }
            else
            {
                spriteBatch.Draw(SpriteColor, this.GetPos(), scale: Scale);
            }
        }
        public override float GetMaxPosX()
        {
            if (IsShape1x4 == true)
            {
                if (m_Rotation == 1 || m_Rotation == 3)
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
        public override Vector2 GetNextPos(int BlockNumber)
        {
            if (m_Rotation == 0 || m_Rotation == 2)
            {
                return new Vector2(m_Pos.X, m_Pos.Y - (float)(m_Height * BlockNumber));
            }
            else if (m_Rotation == 1 || m_Rotation == 3)
            {
                return new Vector2(m_Pos.X + (float)(m_Width * BlockNumber), m_Pos.Y);
            }
            else { return new Vector2(0, 0); }
        }
    }
}

