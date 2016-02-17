using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

// Extensions for the otter tilemap class

namespace NotSonic
{
    class TilemapExt
    {
        public static Color ReadTilemapPixel(Tilemap tilemap, int X, int Y)
        {
            Color outCol = new Color();

            tilemap.UsePositions = false;

            // First, get the tile from the tilemap.
            int TileX, TileY;
            TileX = X / tilemap.TileWidth;
            TileY = Y / tilemap.TileHeight;

            // Get the tile 
            TileInfo tile = tilemap.GetTile(TileX, TileY, "base");

            tilemap.UsePositions = true;

            if (tile == null)
            {
                return new Color();
            }

            // Get the fraction of how far into this tile we are
            int TileTraversalX = X - TileX * tilemap.TileWidth;
            int TileTraversalY = Y - TileY * tilemap.TileHeight;

            // Get the texture coordinates
            int TexX = TileTraversalX + tile.TX;
            if(TexX < 0)
            {
                TexX += tilemap.TileWidth;
            }

            int TexY = TileTraversalX + tile.TY;
            if (TexY < 0)
            {
                TexY += tilemap.TileHeight;
            }


            outCol = tilemap.Texture.GetPixel(TexX, TexY);
            

            return outCol;
        }
    }
}
