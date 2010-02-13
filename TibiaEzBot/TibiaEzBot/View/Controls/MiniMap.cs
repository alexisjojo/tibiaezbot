using System;
using System.Data;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

namespace TibiaEzBot.View.Controls
{
    public partial class MiniMap : UserControl
    {
        private const int X = 18;
        private const int Y = 14;

        #region Variables Declaration
        private byte[,] mMatrix = new byte[X, Y];
        #endregion

        #region Constructors
        public MiniMap()
        {
            InitializeComponent();
            ResetMatrix();
        }
        #endregion

        #region Properties
        public byte[,] Matrix
        {
            get { return mMatrix; }
            set
            {
                mMatrix = value;
                Invalidate();
            }
        }
        #endregion

        #region Methods
        public void ResetMatrix()
        {
            Random r = new Random();
            for (int y = 0; y < Y; y++)
            {
                for (int x = 0; x < X; x++)
                {
                    mMatrix[x, y] = (byte)r.Next(255);
                }
            }
        }

        private Color GetColor(byte color)
        {
            byte b = (byte)((color % 6) / 5.0 * 255);
            byte g = (byte)(((color / 6) % 6) / 5.0 * 255);
            byte r = (byte)((color / 36.0) / 6.0 * 255);
            return Color.FromArgb(r, g, b);
        }
        #endregion

        #region Overrides
        protected override void OnSizeChanged(System.EventArgs e)
        {
            Invalidate();
            base.OnSizeChanged(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            float mGridSizeY = Height / Y;
            float mGridSizeX = Width / X;

            if (mMatrix != null)
            {
                int sy = 0;
                for (float y = 0; y < mGridSizeY * Y; y += mGridSizeY, sy++)
                {
                    int sx = 0;
                    for (float x = 0; x < mGridSizeX * X; x += mGridSizeX, sx++)
                    {
                        // Lets render the obstacules
                        Color color = GetColor(mMatrix[sx, sy]);

                        using (SolidBrush brush = new SolidBrush(color))
                            g.FillRectangle(brush, x, y, mGridSizeX, mGridSizeY);
                    }
                }
            }

            using (Pen pen = new Pen(Color.Black))
            {
                for (float y = 0; y <= mGridSizeY * Y; y += mGridSizeY)
                    g.DrawLine(pen, e.ClipRectangle.X, y, mGridSizeX * 18, y);

                for (float x = 0; x <= mGridSizeX * X; x += mGridSizeX)
                    g.DrawLine(pen, x, e.ClipRectangle.Y, x, mGridSizeY * 14);
            }

            base.OnPaint(e);
        }
        #endregion
    }
}
