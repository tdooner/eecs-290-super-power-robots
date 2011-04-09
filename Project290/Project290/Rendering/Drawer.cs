//////__     __               _                 _     _               _   
//////\ \   / /              | |               | |   | |             | |  
////// \ \_/ /__  _   _   ___| |__   ___  _   _| | __| |  _ __   ___ | |_ 
//////  \   / _ \| | | | / __| '_ \ / _ \| | | | |/ _` | | '_ \ / _ \| __|
//////   | | (_) | |_| | \__ \ | | | (_) | |_| | | (_| | | | | | (_) | |_ 
//////   |_|\___/ \__,_| |___/_| |_|\___/ \__,_|_|\__,_| |_| |_|\___/ \__|
//////      _                              _   _     _     _ 
//////     | |                            | | | |   (_)   | |
//////  ___| |__   __ _ _ __   __ _  ___  | |_| |__  _ ___| |
////// / __| '_ \ / _` | '_ \ / _` |/ _ \ | __| '_ \| / __| |
//////| (__| | | | (_| | | | | (_| |  __/ | |_| | | | \__ \_|
////// \___|_| |_|\__,_|_| |_|\__, |\___|  \__|_| |_|_|___(_)
//////                         __/ |                         
//////                        |___/                          

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Project290.GameElements;
using Project290.Rendering;
using Project290.Mathematics;
using Project290.Screens;
using Project290.Inputs;
using Microsoft.Xna.Framework.Input;

namespace Project290.Rendering
{
    /// <summary>
    /// This static public class is used for all rendering in the game. It can be called from 
    /// anywhere, but do not attempt to use it until the Initiallize() method has been
    /// called (this only needs to happen once ever).
    /// </summary>
    static public class Drawer
    {
        /// <summary>
        /// A temp variable for the draw line method.
        /// </summary>
        private static Vector2 drawLineDiff = new Vector2();

        /// <summary>
        /// A temp variable for the draw line method.
        /// </summary>
        private static Vector2 drawLineScale = new Vector2();

        /// <summary>
        /// A temp variable for the draw line method.
        /// </summary>
        private static Vector2 drawLinePosition = new Vector2();

        /// <summary>
        /// Gets a circle buffer, corresponding to 18 points around a unit circle,
        /// for rendering outlined font.
        /// </summary>
        private static List<Vector2> circleBuffer;

        /// <summary>
        /// Gets the rectangle {0, 0, 1920, 1080}.
        /// </summary>
        /// <value>The full screen rectangle.</value>
        public static Rectangle FullScreenRectangle { get; private set; }

        /// <summary>
        /// Initiallizes the static variables used by this static public class..
        /// </summary>
        public static void Initiallize()
        {
            FullScreenRectangle = new Rectangle(0, 0, 1920, 1080);

            circleBuffer = new List<Vector2>();
            for (float angle = 0; angle < MathHelper.TwoPi; angle += MathHelper.TwoPi / 16f)
            {
                circleBuffer.Add(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)));
            }
        }

        /// <summary>
        /// Formats the number. For example, 12345 returns "12,345".
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A formatted number.</returns>
        public static string FormatNumber(uint value)
        {
            string toReturn = string.Empty;
            while (value >= 1000)
            {
                toReturn = "," + (value % 1000).ToString().PadLeft(3, '0') + toReturn;
                value /= 1000;
            }

            return value.ToString() + toReturn;
        }

        /// <summary>
        /// This returns the ratio of the screen height to 1080, in the event that
        /// the TV is not at a 1080i or 1080p resolution. All scales and positions should
        /// be multiplied by this in order to correctly adjust to any screen resolution,
        /// while always assuming that the screen is 1920 * 1080.
        /// </summary>
        /// <returns>The ratio of the screen height to 1080.</returns>
        public static float GetRatio()
        {
            return GameWorld.graphics.Viewport.Width / 1920f;
        }

        /// <summary>
        /// Gets the layer depth scale (so screens will be drawn on top of each other without having to worry about layer depths).
        /// </summary>
        /// <param name="layerDepth">The layer depth.</param>
        /// <returns>The layer depth scale (so screens will be drawn on top of each other without having to worry about layer depths).</returns>
        private static float GetLayerDepthScale(float layerDepth)
        {
            return (layerDepth * (ScreenContainer.LayerDepthEnd - ScreenContainer.LayerDepthStart)) + ScreenContainer.LayerDepthStart;
        }

        /// <summary>
        /// Draws the rectangle.
        /// </summary>
        /// <param name="toDraw">The rectangle to draw.</param>
        /// <param name="width">The width.</param>
        /// <param name="layerDepth">The layer depth.</param>
        /// <param name="color">The color.</param>
        public static void DrawRectangle(Rectangle toDraw, float width, float layerDepth, Color color)
        {
            Vector2 tl = new Vector2(toDraw.Left, toDraw.Top);
            Vector2 tr = new Vector2(toDraw.Right, toDraw.Top);
            Vector2 bl = new Vector2(toDraw.Left, toDraw.Bottom);
            Vector2 br = new Vector2(toDraw.Right, toDraw.Bottom);

            DrawLine(tl, tr, width, layerDepth, color);
            DrawLine(tr, br, width, layerDepth, color);
            DrawLine(br, bl, width, layerDepth, color);
            DrawLine(bl, tl, width, layerDepth, color);
        }

        /// <summary>
        /// Draws a black line from point A to point B.
        /// </summary>
        /// <param name="pointA">The point A.</param>
        /// <param name="pointB">The point B.</param>
        /// <param name="width">The width.</param>
        /// <param name="layerDepth">The layer depth.</param>
        /// <param name="color">The color.</param>
        public static void DrawLine(Vector2 pointA, Vector2 pointB, float width, float layerDepth, Color color)
        {
            // Check that all points are valid.
            if (pointA == null || pointB == null ||
                float.IsNaN(pointA.X) || float.IsNaN(pointA.Y) ||
                float.IsNaN(pointB.X) || float.IsNaN(pointB.Y))
            {
                return;
            }

            drawLineDiff.X = pointA.X - pointB.X;
            drawLineDiff.Y = pointA.Y - pointB.Y;
            drawLineScale.X = drawLineDiff.Length() / 20f;
            drawLineScale.Y = width * 0.05f;
            drawLinePosition.X = (pointA.X + pointB.X) / 2f;
            drawLinePosition.Y = (pointA.Y + pointB.Y) / 2f;
            float angle = (RotationHelper.Vector2ToAngle(drawLineDiff.X, drawLineDiff.Y) + MathHelper.PiOver2) % MathHelper.TwoPi;

            // If the slope is steeper than 1, we need to mirror the angle
            // around the verticle due to the 0-2pi singularity of the Vector2ToAngle funciton.
            if (pointA.X > pointB.X)
            {
                angle = MathHelper.TwoPi - angle;
            }

            float ratio = GetRatio();
            GameWorld.spriteBatch.Draw(
                TextureStatic.Get("blank"),
                drawLinePosition * ratio,
                null,
                color,
                angle,
                TextureStatic.GetOrigin("blank"),
                drawLineScale * ratio,
                SpriteEffects.None,
                GetLayerDepthScale(layerDepth));
        }

        /// <summary>
        /// Draws the specified texture2D, assuming that it was drawn assuming the screen is
        /// exactly 1920 * 1080. Be sure to call spriteBatch.Begin() before this and spriteBatch.End()
        /// at the end of the series of Drawing calls.
        /// </summary>
        /// <param name="texture2D">The texture2D.</param>
        /// <param name="position">The position.</param>
        /// <param name="sourceRectangle">The source rectangle.</param>
        /// <param name="color">The color.</param>
        /// <param name="rotation">The rotation.</param>
        /// <param name="origin">The origin.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="effects">The effects.</param>
        /// <param name="layerDepth">The layer depth.</param>
        public static void Draw(
            Texture2D texture2D,
            Vector2 position,
            Rectangle? sourceRectangle,
            Color color,
            float rotation,
            Vector2 origin,
            Vector2 scale,
            SpriteEffects effects,
            float layerDepth)
        {
            float ratio = GetRatio();
            GameWorld.spriteBatch.Draw(
                texture2D,
                position * ratio,
                sourceRectangle,
                color,
                rotation,
                origin,
                scale * ratio,
                effects,
                GetLayerDepthScale(layerDepth));
        }

        /// <summary>
        /// Draws the specified texture2D, assuming that it was drawn assuming the screen is
        /// exactly 1920 * 1080. Be sure to call spriteBatch.Begin() before this and spriteBatch.End()
        /// at the end of the series of Drawing calls.
        /// </summary>
        /// <param name="texture2D">The texture2 D.</param>
        /// <param name="position">The position.</param>
        /// <param name="sourceRectangle">The source rectangle.</param>
        /// <param name="color">The color.</param>
        /// <param name="rotation">The rotation.</param>
        /// <param name="origin">The origin.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="effects">The effects.</param>
        /// <param name="layerDepth">The layer depth.</param>
        public static void Draw(
            Texture2D texture2D,
            Vector2 position,
            Rectangle? sourceRectangle,
            Color color,
            float rotation,
            Vector2 origin,
            float scale,
            SpriteEffects effects,
            float layerDepth)
        {
            float ratio = GetRatio();
            GameWorld.spriteBatch.Draw(
                texture2D,
                position * ratio,
                sourceRectangle,
                color,
                rotation,
                origin,
                scale * ratio,
                effects,
                GetLayerDepthScale(layerDepth));
        }

        /// <summary>
        /// Draws the specified texture2D, assuming that it was drawn assuming the screen is
        /// exactly 1920 * 1080. Be sure to call spriteBatch.Begin() before this and spriteBatch.End()
        /// at the end of the series of Drawing calls.
        /// </summary>
        /// <param name="texture2D">The texture2 D.</param>
        /// <param name="destinationRectangle">The destination rectangle.</param>
        /// <param name="sourceRectangle">The source rectangle.</param>
        /// <param name="color">The color.</param>
        /// <param name="rotation">The rotation.</param>
        /// <param name="origin">The origin.</param>
        /// <param name="effects">The effects.</param>
        /// <param name="layerDepth">The layer depth.</param>
        public static void Draw(
            Texture2D texture2D,
            Rectangle destinationRectangle,
            Rectangle? sourceRectangle,
            Color color,
            float rotation,
            Vector2 origin,
            SpriteEffects effects,
            float layerDepth)
        {
            float ratio = GetRatio();
            destinationRectangle.X = (int)Math.Round(destinationRectangle.X * ratio);
            destinationRectangle.Y = (int)Math.Round(destinationRectangle.Y * ratio);
            destinationRectangle.Width = (int)Math.Round(destinationRectangle.Width * ratio);
            destinationRectangle.Height = (int)Math.Round(destinationRectangle.Height * ratio);
            GameWorld.spriteBatch.Draw(
                texture2D,
                destinationRectangle,
                sourceRectangle,
                color,
                rotation,
                origin,
                effects,
                GetLayerDepthScale(layerDepth));
        }

        /// <summary>
        /// Draws the specified string in the specified font, assuming that it was drawn assuming the screen is
        /// exactly 1920 * 1080. Be sure to call spriteBatch.Begin() before this and spriteBatch.End()
        /// at the end of the series of Drawing calls.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <param name="text">The text to write.</param>
        /// <param name="position">The position.</param>
        /// <param name="color">The color of the string.</param>
        /// <param name="rotation">The rotation.</param>
        /// <param name="origin">The origin.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="spriteEffects">The sprite effects.</param>
        /// <param name="layerDepth">The layer depth.</param>
        public static void DrawString(
            SpriteFont font,
            string text,
            Vector2 position,
            Color color,
            float rotation,
            Vector2 origin,
            float scale,
            SpriteEffects spriteEffects,
            float layerDepth)
        {
            float ratio = GetRatio();
            GameWorld.spriteBatch.DrawString(
                font,
                text,
                position * ratio,
                color,
                rotation,
                origin,
                scale * ratio,
                spriteEffects,
                GetLayerDepthScale(layerDepth));
        }

        /// <summary>
        /// Draws the specified string in the specified font, assuming that it was drawn assuming the screen is
        /// exactly 1920 * 1080. Be sure to call spriteBatch.Begin() before this and spriteBatch.End()
        /// at the end of the series of Drawing calls.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <param name="text">The text to write.</param>
        /// <param name="position">The position.</param>
        /// <param name="color">The color of the string.</param>
        /// <param name="rotation">The rotation.</param>
        /// <param name="origin">The origin.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="spriteEffects">The sprite effects.</param>
        /// <param name="layerDepth">The layer depth.</param>
        public static void DrawString(
            SpriteFont font,
            string text,
            Vector2 position,
            Color color,
            float rotation,
            Vector2 origin,
            Vector2 scale,
            SpriteEffects spriteEffects,
            float layerDepth)
        {
            float ratio = GetRatio();
            GameWorld.spriteBatch.DrawString(
                font,
                text,
                position * ratio,
                color,
                rotation,
                origin,
                scale * ratio,
                spriteEffects,
                GetLayerDepthScale(layerDepth));
        }

        /// <summary>
        /// Draws the specified string in the specified font, assuming that it was drawn assuming the screen is
        /// exactly 1920 * 1080. Be sure to call spriteBatch.Begin() before this and spriteBatch.End()
        /// at the end of the series of Drawing calls. This creates a black outline around the string.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <param name="text">The text to write.</param>
        /// <param name="position">The position.</param>
        /// <param name="color">The color of the string.</param>
        /// <param name="rotation">The rotation.</param>
        /// <param name="origin">The origin.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="spriteEffects">The sprite effects.</param>
        /// <param name="layerDepth">The layer depth.</param>
        public static void DrawOutlinedString(
            SpriteFont font,
            string text,
            Vector2 position,
            Color color,
            float rotation,
            Vector2 origin,
            float scale,
            SpriteEffects spriteEffects,
            float layerDepth)
        {
            float ratio = GetRatio();
            Color temp = Color.Black;
            temp.A = color.A;
            foreach (Vector2 offset in circleBuffer)
            {
                GameWorld.spriteBatch.DrawString(
                    font,
                    text,
                    (position + offset * scale * 5f) * ratio,
                    temp,
                    rotation,
                    origin,
                    scale * ratio,
                    spriteEffects,
                    GetLayerDepthScale(layerDepth));
            }

            GameWorld.spriteBatch.DrawString(
                font,
                text,
                position * ratio,
                color,
                rotation,
                origin,
                scale * ratio,
                spriteEffects,
                GetLayerDepthScale(layerDepth + 0.001f));
        }

        /// <summary>
        /// Draws the specified controller symbol, assuming that it was drawn assuming the screen is
        /// exactly 1920 * 1080. Be sure to call spriteBatch.Begin() before this and spriteBatch.End()
        /// at the end of the series of Drawing calls.
        /// </summary>
        /// <param name="button">The button to draw.</param>
        /// <param name="position">The position.</param>
        /// <param name="color">The color of the string.</param>
        /// <param name="rotation">The rotation.</param>
        /// <param name="origin">The origin.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="spriteEffects">The sprite effects.</param>
        /// <param name="layerDepth">The layer depth.</param>
        public static void DrawControllerSymbol(
            Buttons button,
            Vector2 position,
            Color color,
            float rotation,
            Vector2 origin,
            float scale,
            SpriteEffects spriteEffects,
            float layerDepth)
        {
            string text = string.Empty;
            switch (button)
            {
                case Buttons.A:
                    text = "'";
                    break;
                case Buttons.B:
                    text = ")";
                    break;
                case Buttons.Back:
                    text = "#";
                    break;
                case Buttons.BigButton:
                    text = "$";
                    break;
                case Buttons.DPadDown:
                case Buttons.DPadLeft:
                case Buttons.DPadRight:
                case Buttons.DPadUp:
                    text = "!";
                    break;
                case Buttons.LeftShoulder:
                    text = "-";
                    break;
                case Buttons.LeftStick:
                case Buttons.LeftThumbstickDown:
                case Buttons.LeftThumbstickLeft:
                case Buttons.LeftThumbstickRight:
                case Buttons.LeftThumbstickUp:
                    text = " ";
                    break;
                case Buttons.LeftTrigger:
                    text = ",";
                    break;
                case Buttons.RightShoulder:
                    text = "*";
                    break;
                case Buttons.RightStick:
                case Buttons.RightThumbstickDown:
                case Buttons.RightThumbstickLeft:
                case Buttons.RightThumbstickRight:
                case Buttons.RightThumbstickUp:
                    text = "\"";
                    break;
                case Buttons.RightTrigger:
                    text = "+";
                    break;
                case Buttons.Start:
                    text = "%";
                    break;
                case Buttons.X:
                    text = "&";
                    break;
                case Buttons.Y:
                    text = "(";
                    break;
            }

            float ratio = GetRatio();
            GameWorld.spriteBatch.DrawString(
                FontStatic.Get("controllerFont"),
                text,
                position * ratio,
                color,
                rotation,
                origin,
                scale * ratio,
                spriteEffects,
                GetLayerDepthScale(layerDepth));
        }
        
        /// <summary>
        /// Gets the controller symbol mapping.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static char GetControllerSymbolMap(char input)
        {
            switch (input)
            {
                case 'A':
                case 'a':
                    return '\'';                    
                case 'B':
                case 'b':
                    return ')';                    
                case '<':
                    return '#';                    
                case '$':
                    return '$';                    
                case 'D':
                    return '!';                    
                case 'L':
                case 'l':
                    return '-';                    
                case 'T':
                case 't':
                    return ' ';                    
                case 'R':
                case 'r':
                    return ',';                    
                case 'S':
                case 's':
                    return '*';                    
                case 'W':
                case 'w':
                    return '\"';                    
                case 'U':
                case 'u':
                    return '+';                    
                case '+':
                    return '%';                    
                case 'X':
                case 'x':
                    return '&';                    
                case 'Y':
                case 'y':
                    return '(';                    
            }

            return input;
        }
    }
}
