using System;
using MonoBrickFirmware.Native;
using MonoBrickFirmware.Graphics;

namespace MonoBrickFirmware.IO
{	
	public class Lcd
	{
		public const int Width = 178;
		public const int Height = 128;
		const int bytesPrLine = (Width+7)/8;
		const int bufferSize = ((Width+7)/8)*Height;
		const int hwBufferLineSize = 60;
		const int hwBufferSize = hwBufferLineSize*Height;			
		private UnixDevice device;
		private MemoryArea memory;

		byte[] displayBuf = new byte[bufferSize];
		public void SetPixel(int x, int y, bool color)
		{
			int index = (x/8)+y*23;
			int bit = x & 0x7;
			if (color)
				displayBuf[index] |= (byte)(1 << bit);
			else
				displayBuf[index] &= (byte)~(1 << bit);
					
		}	
		
		byte[] hwBuffer = new byte[hwBufferSize];
		
		public Lcd()
		{
			device = new UnixDevice("/dev/fb0");
			memory =  device.MMap(hwBufferSize, 0);	
		}
		
		static byte[] convert = 
		    {
		    0x00, // 000 00000000
		    0xE0, // 001 11100000
		    0x1C, // 010 00011100
		    0xFC, // 011 11111100
		    0x03, // 100 00000011
		    0xE3, // 101 11100011
		    0x1F, // 110 00011111
		    0xFF // 111 11111111
		    }; 
		
		public void Update()
		{
			int inOffset = 0;
		    int outOffset = 0;
		    for(int row = 0; row < Height; row++)
		    {
		        int pixels;
		        for(int i = 0; i < 7; i++)
		        {
		            pixels = displayBuf[inOffset++] & 0xff;
		            pixels |= (displayBuf[inOffset++] & 0xff) << 8;
		            pixels |= (displayBuf[inOffset++] & 0xff) << 16;
		            hwBuffer[outOffset++] = convert[pixels & 0x7];
		            pixels >>= 3;
		            hwBuffer[outOffset++] = convert[pixels & 0x7];
		            pixels >>= 3;
		            hwBuffer[outOffset++] = convert[pixels & 0x7];
		            pixels >>= 3;
		            hwBuffer[outOffset++] = convert[pixels & 0x7];
		            pixels >>= 3;
		            hwBuffer[outOffset++] = convert[pixels & 0x7];
		            pixels >>= 3;
		            hwBuffer[outOffset++] = convert[pixels & 0x7];
		            pixels >>= 3;
		            hwBuffer[outOffset++] = convert[pixels & 0x7];
		            pixels >>= 3;
		            hwBuffer[outOffset++] = convert[pixels & 0x7];
		        }   
		        pixels = displayBuf[inOffset++] & 0xff;
		        pixels |= (displayBuf[inOffset++] & 0xff) << 8;
		        hwBuffer[outOffset++] = convert[pixels & 0x7];
		        pixels >>= 3;
		        hwBuffer[outOffset++] = convert[pixels & 0x7];
		        pixels >>= 3;
		        hwBuffer[outOffset++] = convert[pixels & 0x7];
		        pixels >>= 3;
		        hwBuffer[outOffset++] = convert[pixels & 0x7];
		    } 
			memory.Write(0,hwBuffer);
		}
		
		public void ShowPicture(byte[] picture)
		{
			Array.Copy(picture, displayBuf, picture.Length);
			Update();
		}
		
		public void ClearLines(int y, int count)
		{			
			Array.Clear(displayBuf, bytesPrLine*y, count*bytesPrLine);
		}
		
		public void DrawHLine(Point startPoint, int length, bool setOrClear)
		{
			int bytePos = bytesPrLine*startPoint.y + startPoint.x/8;
			int bitPos = startPoint.x & 0x7;
			int bitsInFirstByte = Math.Min(8 - bitPos, length);			
			byte bitMask = (byte)((0xff >> (8-bitsInFirstByte)) << bitPos);
			
			// Set/clear bits in first byte
			if (setOrClear)
				displayBuf[bytePos] |= bitMask;
			else
				displayBuf[bytePos] &= (byte)~bitMask;
			length -= bitsInFirstByte;
			bytePos++;
			while (length >= 8) // Set/Clear all byte full bytes
			{
				displayBuf[bytePos] = setOrClear ? (byte)0xff : (byte)0;
				bytePos++;
				length -= 8;
			}
			// Set/clear bits in last byte
			if (length > 0)
			{
				bitMask = (byte)(0xff >> (8-length));
				if (setOrClear)
					displayBuf[bytePos] |= bitMask;
				else
					displayBuf[bytePos] &= (byte)~bitMask;				
			}
		}
		
		public void DrawBox(Rect r, bool setOrClear)
		{
			int length = r.p2.x - r.p1.x;
			for (int y = r.p1.y; y <= r.p2.y; ++y)
				DrawHLine(new Point(r.p1.x, y), length, setOrClear);
		}
		
		public void DrawBitmap(BitStreamer bs, Point p, uint xsize, uint ysize, bool color)
		{
			for (int yPos = p.y; yPos != p.y+ysize; yPos++)
			{
				int BufPos = bytesPrLine*yPos+p.x/8;
				uint xBitsLeft = xsize;
				int xPos = p.x;
				
				while (xBitsLeft > 0)
				{
					int bitPos = xPos & 0x7;					
					uint bitsToWrite = Math.Min(xBitsLeft, (uint)(8-bitPos));
					if (color)
						displayBuf[BufPos] |= (byte)(bs.GetBits(bitsToWrite) << bitPos);
					else
						displayBuf[BufPos] &= (byte)~(bs.GetBits(bitsToWrite) << bitPos);
					xBitsLeft -= bitsToWrite;
					xPos += (int)bitsToWrite;
					BufPos++;
				}
				
			}
		}
		
		public void WriteText(Font f, Point p, string text, bool color)
		{			
			foreach(char c in text)
			{
				CharStreamer cs = f.getChar(c);
				DrawBitmap(cs, p, cs.width, cs.height, color);		
				p.x += (int)cs.width;				
			}
		}
		
		public void WriteTextBox(Font f, Rect r, string text, bool color)
		{
			DrawBox(r, !color); // Clear background
			WriteText(f, r.p1, text, color);
		}
	}
}
