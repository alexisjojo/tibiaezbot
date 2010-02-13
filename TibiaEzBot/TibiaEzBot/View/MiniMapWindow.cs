using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using TibiaEzBot.Core;
using TibiaEzBot.Core.Entities;

namespace TibiaEzBot.View
{
    public partial class MiniMapWindow : Form
    {
        Thread thread;
        private bool closing;

        public MiniMapWindow()
        {
            InitializeComponent();
        }

        private void MiniMapWindow_Load(object sender, EventArgs e)
        {
            this.FormClosing += new FormClosingEventHandler(MiniMapWindow_FormClosing);
            thread = new Thread(new ThreadStart(update));
            thread.Start();
        }

        private void MiniMapWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            closing = true;
        }

        public void update()
        {
            while (!closing)
            {
                if (GlobalVariables.IsConnected())
                {
                    Position currentPos = GlobalVariables.GetPlayerPosition();
                    if (currentPos != null)
                    {
                        upDateMap((int)currentPos.X - 8, (int)currentPos.Y - 6, (int)currentPos.Z, 18, 14);
                        this.miniMap.Invalidate();
                    }
                }
                Thread.Sleep(500);
            }
        }

        private void upDateMap(int x, int y, int z, int width, int height)
        {
            Map map = Map.GetInstance();
            for (int nx = 0; nx < width; nx++)
            {
                for (int ny = 0; ny < height; ny++)
                {
                    Position pos = new Position((uint)(x + nx), (uint)(y + ny), (uint)z);
                    Tile tile = map.GetTile(pos);
                    miniMap.Matrix[nx, ny] = tile != null ? tile.GetMinimapColor() : Byte.MinValue;
                }
            }
        }
    }
}
