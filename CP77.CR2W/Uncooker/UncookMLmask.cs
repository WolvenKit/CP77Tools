using CP77.Common.Image;
using CP77.CR2W.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using WolvenKit.Common.Tools.DDS;
using System.Runtime.InteropServices;

namespace CP77.CR2W.Uncooker
{
    public static class Mlmask
    {
        private static uint CountBits(uint v)
        {
            uint t = v;
            uint count = 0;
            for (uint i = 0; i < 32; i++)
            {
                if ((t & 1) == 1)
                {
                    count++;
                }
                t >>= 1;
            }
            return count;
        }

        private static uint DivCeil(uint l, uint r)
        {
            return (l + r - 1) / r;
        }

        public static bool UncookMultilayer(CR2WFile cr2w, List<byte[]> buffers, FileInfo outfile, EUncookExtension uncookext)
        {
            //We need 2 buffers one for atlas one for tile data
            if (buffers.Count < 2)
                return false;

            if (!(cr2w.Chunks.FirstOrDefault()?.data is Multilayer_Mask mlmask) ||
                !(cr2w.Chunks[1]?.data is rendRenderMultilayerMaskBlobPC blob))
                return false;

            string filename = Path.GetFileNameWithoutExtension(outfile.FullName);
            string path = outfile.Directory.FullName;

            uint atlasWidth = blob.Header.AtlasWidth.val;
            uint atlasHeight = blob.Header.AtlasHeight.val;

            uint maskWidth = blob.Header.MaskWidth.val;
            uint maskHeight = blob.Header.MaskHeight.val;

            uint maskWidthLow = blob.Header.MaskWidthLow.val;
            uint maskHeightLow = blob.Header.MaskHeightLow.val;

            uint maskTileSize = blob.Header.MaskTileSize.val;

            var atlasData = blob.AtlasData;
            var tilesData = blob.TilesData;

            byte[] atlas = buffers[atlasData.Buffer.GetBytes()[0] - 1];
            uint[] tiles;

            //Read tilesdata buffer into appropriate variable type
            var b = buffers[tilesData.Buffer.GetBytes()[0] - 1];
            tiles = new uint[b.Length / 4];
            for (int i = 0, j = 0; i < b.Length; i += 4, j++)
            {
                tiles[j] = BitConverter.ToUInt32(b, i);
            }
            byte[] atlasRaw = new byte[atlasWidth * atlasHeight];

            //Decode compressed data into single channel uncompressed
            //Mlmask always BC4?
            if (!BlockCompression.DecodeBC(atlas, ref atlasRaw, atlasWidth, atlasHeight, BlockCompression.BlockCompressionType.BC4))
            {
                return false;
            }
            byte[] maskData = new byte[maskWidth * maskHeight];
            

            Directory.CreateDirectory(path);
            for (uint i = 0; i < maskTileSize; i++)
            {
                var mFilename = filename + $"_{i}.dds";
                var newpath = Path.Combine(path, mFilename);

                //Clear instead of allocate new is faster?
                //Mandatory cause decode does not always right to every pixel
                Array.Clear(maskData, 0, maskData.Length);
                Decode(ref maskData, maskWidth, maskHeight, maskWidthLow, maskHeightLow, atlasRaw, atlasWidth, atlasHeight, tiles, maskTileSize, i);

                using (var ddsStream = new FileStream($"{newpath}", FileMode.Create, FileAccess.Write))
                {
                    DDSUtils.GenerateAndWriteHeader(ddsStream, new DDSMetadata(maskWidth, maskHeight, 0, EFormat.R8_UNORM, 8, false, 0, false));

                    ddsStream.Write(maskData);
                }

                //convert texture if neccessary
                if (uncookext != EUncookExtension.dds && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    var di = new FileInfo(outfile.FullName).Directory;
                    TexconvWrapper.Convert(di.FullName, $"{newpath}", uncookext);
                }
            }
            return true;
        }

        private static void Decode(ref byte[] maskData, uint maskWidth, uint maskHeight, uint mWidthLow, uint mHeightLow, byte[] atlasData, uint atlasWidth, uint atlasHeight, uint[] tileData, uint maskTileSize, uint maskIndex)
        {
            uint widthInTiles0 = DivCeil(maskWidth, maskTileSize);
            uint heightInTiles0 = DivCeil(maskHeight, maskTileSize);
            uint smallOffset = (widthInTiles0 * heightInTiles0);

            uint smallScale = maskWidth / mWidthLow;

            for (uint x = 0; x < maskWidth; x++)
            {
                for (uint y = 0; y < maskWidth; y++)
                {
                    DecodeSingle(ref maskData, maskWidth, maskHeight, atlasData, atlasWidth, atlasHeight, x, y, tileData, maskTileSize, maskIndex, smallOffset, smallScale);
                    DecodeSingle(ref maskData, maskWidth, maskHeight, atlasData, atlasWidth, atlasHeight, x, y, tileData, maskTileSize, maskIndex, 0, 1);
                }
            }

        }
        
        private static void DecodeSingle(ref byte[] maskData, uint maskWidth, uint maskHeight, byte[] atlasData, uint atlasWidth, uint atlasHeight, uint x, uint y, uint[] tilesData, uint maskTileSize, uint maskIndex, uint tilesOffset, uint smallScale)
        {
            uint widthInTiles = DivCeil(maskWidth / smallScale, maskTileSize);

            uint xTile = x / maskTileSize / smallScale;
            uint yTile = y / maskTileSize / smallScale;

            uint tileIndex = widthInTiles * yTile + xTile + tilesOffset;

            uint paramOffset = tilesData[tileIndex * 2];
            uint paramBits = tilesData[tileIndex * 2 + 1];

            if ((paramBits & (1 << ((int)maskIndex))) == 0)
                return;

            uint extraAdd = CountBits((uint)(paramBits & ((1 << ((int)maskIndex)) - 1)));

            uint tileDecl = tilesData[paramOffset + extraAdd];

            uint dx = tileDecl & 0x3ff;
            uint dy = (tileDecl >> 10) & 0x3ff;
            uint sx = (tileDecl >> 20) & 0xf;
            uint sy = (tileDecl >> 24) & 0xf;

            uint atlasTileSize = maskTileSize + 2;

            uint x1 = (uint)(1 << (int)sx) - 1;
            uint xi = x & x1;
            uint y1 = (uint)(1 << (int)sy) - 1;
            uint yi = y & y1;

            uint ux = (x >> (int)sx) % maskTileSize + 1 + dx * atlasTileSize;
            uint uy = (y >> (int)sy) % maskTileSize + 1 + dy * atlasTileSize;

            byte q00 = atlasData[(ux + 0) + (uy + 0) * atlasWidth];
            byte q10 = atlasData[(ux + 1) + (uy + 0) * atlasWidth];
            byte q01 = atlasData[(ux + 0) + (uy + 1) * atlasWidth];
            byte q11 = atlasData[(ux + 1) + (uy + 1) * atlasWidth];

            byte p = BilinearInterpolation(q00, q10, q01, q11, xi, x1, yi, y1);

            maskData[x + y * maskWidth] = p;

        }

        private static byte BilinearInterpolation(byte q00, byte q10, byte q01, byte q11, uint x, uint x1, uint y, uint y1)
        {
            uint sc = 256;

            if(x1 == 0 || y1 == 0)
            {
                return q00;
            }

            uint q00s = q00 * sc;
            uint q10s = q10 * sc;
            uint q01s = q01 * sc;
            uint q11s = q11 * sc;

            uint a0 = q00s;
            uint a1 = (q10s - q00s) * x / x1;
            uint a2 = (q01s - q00s) * y / y1;
            uint a3 = (q00s - q01s - q10s + q11s) * x * y / x1 / y1;

            uint f = a0 + a1 + a2 + a3;

            uint r = f / sc;
            if (r > 255)
                r = 255;

            return (byte)r;
        }
    }
}
