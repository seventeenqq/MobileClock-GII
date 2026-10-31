using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Clock
{
    public class ClockView : GraphicsView, IDrawable
    {
        private DateTime time;

        // 1 = рисовать, 0 = пропустить.
        // Сегменты расположены:
        //    0
        //  5   1
        //    6
        //  4   2
        //    3
        private int[,] segments = new int[,]
        {
            { 1, 1, 1, 1, 1, 1, 0 }, // 0
            { 0, 1, 1, 0, 0, 0, 0 }, // 1
            { 1, 1, 0, 1, 1, 0, 1 }, // 2
            { 1, 1, 1, 1, 0, 0, 1 }, // 3
            { 0, 1, 1, 0, 0, 1, 1 }, // 4
            { 1, 0, 1, 1, 0, 1, 1 }, // 5
            { 1, 0, 1, 1, 1, 1, 1 }, // 6
            { 1, 1, 1, 0, 0, 0, 0 }, // 7
            { 1, 1, 1, 1, 1, 1, 1 }, // 8
            { 1, 1, 1, 1, 0, 1, 1 }  // 9
        };

        public ClockView()
        {
            Drawable = this;
            BackgroundColor = Colors.White;
        }

        public void SetTime(DateTime newTime)
        {
            time = newTime;
            Invalidate();
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            // Рисунок занимает 280 x 64 условных единицы.
            float scale = Math.Min(dirtyRect.Width / 280, dirtyRect.Height / 64);
            float left = (dirtyRect.Width - 280 * scale) / 2;
            float top = (dirtyRect.Height - 64 * scale) / 2;

            canvas.SaveState();
            canvas.Translate(left, top);
            canvas.Scale(scale, scale);
            canvas.FillColor = Colors.Black;

            DrawDigit(canvas, time.Hour / 10, 4);
            DrawDigit(canvas, time.Hour % 10, 44);
            DrawColon(canvas, 87);
            DrawDigit(canvas, time.Minute / 10, 104);
            DrawDigit(canvas, time.Minute % 10, 144);
            DrawColon(canvas, 187);
            DrawDigit(canvas, time.Second / 10, 204);
            DrawDigit(canvas, time.Second % 10, 244);

            canvas.RestoreState();
        }

        private void DrawDigit(ICanvas canvas, int digit, float x)
        {
            // FillRectangle(x, y, ширина, высота).
            if (segments[digit, 0] == 1)
            {
                canvas.FillRectangle(x + 6, 4, 20, 6);
            }
            if (segments[digit, 1] == 1)
            {
                canvas.FillRectangle(x + 26, 10, 6, 19);
            }
            if (segments[digit, 2] == 1)
            {
                canvas.FillRectangle(x + 26, 35, 6, 19);
            }
            if (segments[digit, 3] == 1)
            {
                canvas.FillRectangle(x + 6, 54, 20, 6);
            }
            if (segments[digit, 4] == 1)
            {
                canvas.FillRectangle(x, 35, 6, 19);
            }
            if (segments[digit, 5] == 1)
            {
                canvas.FillRectangle(x, 10, 6, 19);
            }
            if (segments[digit, 6] == 1)
            {
                canvas.FillRectangle(x + 6, 29, 20, 6);
            }
        }

        private void DrawColon(ICanvas canvas, float x)
        {
            canvas.FillRectangle(x, 19, 6, 6);
            canvas.FillRectangle(x, 39, 6, 6);
        }
    }
}
