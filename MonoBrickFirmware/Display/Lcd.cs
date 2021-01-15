using MonoBrickFirmware.Tools;

namespace MonoBrickFirmware.Display
{
	public static class Lcd
	{

		#region Public Enums

		public enum Alignment { Left, Center, Right };

		public enum ArrowOrientation { Left, Right, Down, Up }

		#endregion

		#region Public Properties

		public static int Height { get { return Instance.Height; } }

		public static int Width { get { return Instance.Width; } }

		#endregion

		#region Internal Properties

		internal static ILcd Instance { get; set; }

		#endregion

		#region Public Constructors

		static Lcd()
		{
			if (PlatFormHelper.RunningPlatform == PlatFormHelper.Platform.EV3)
			{
				Instance = new EV3Lcd();
				((EV3Lcd)Instance).Initialize();
			}
			else
			{
				Instance = null; //Not running on a EV3
			}
		}

		#endregion

		#region Public Methods

		public static void Clear()
		{
			Instance.Clear();
		}

		public static void ClearLines(int y, int count)
		{
			Instance.ClearLines(y, count);
		}

		public static void DrawArrow(Rectangle r, Lcd.ArrowOrientation orientation, bool color)
		{
			Instance.DrawArrow(r, orientation, color);
		}

		public static void DrawBitmap(Bitmap bm, Point p)
		{
			Instance.DrawBitmap(bm, p);
		}

		public static void DrawBitmap(BitStreamer bs, Point p, uint xsize, uint ysize, bool color)
		{
			Instance.DrawBitmap(bs, p, xsize, ysize, color);
		}

		public static void DrawCircle(Point center, ushort radius, bool color, bool fill)
		{
			Instance.DrawCircle(center, radius, color, fill);
		}

		public static void DrawEllipse(Point center, ushort radiusA, ushort radiusB, bool color, bool fill)
		{
			Instance.DrawEllipse(center, radiusA, radiusB, color, fill);
		}

		public static void DrawHLine(Point startPoint, int length, bool color)
		{
			Instance.DrawHLine(startPoint, length, color);
		}

		public static void DrawLine(Point start, Point end, bool color)
		{
			Instance.DrawLine(start, end, color);
		}

		public static void DrawRectangle(int x1, int y1, int x2, int y2, bool color, bool fill)
		{
			Instance.DrawRectangle(new Rectangle(x1, y1, x2, y2), color, fill);
		}

		public static void DrawRectangle(Rectangle r, bool color, bool fill)
		{
			Instance.DrawRectangle(r, color, fill);
		}

		public static void DrawVLine(Point startPoint, int height, bool color)
		{
			Instance.DrawVLine(startPoint, height, color);
		}

		public static bool IsPixelSet(int x, int y)
		{
			return Instance.IsPixelSet(x, y);
		}

		public static void LoadScreen()
		{
			Instance.LoadScreen();
		}

		public static void SaveScreen()
		{
			Instance.SaveScreen();
		}

		public static void SetPixel(int x, int y, bool color)
		{
			Instance.SetPixel(x, y, color);
		}

		public static void ShowPicture(byte[] picture)
		{
			Instance.ShowPicture(picture);
		}

		public static void TakeScreenShot()
		{
			Instance.TakeScreenShot();
		}

		public static void TakeScreenShot(string directory, string fileName)
		{
			Instance.TakeScreenShot(directory, fileName);
		}

		public static int TextWidth(Font f, string text)
		{
			return Instance.TextWidth(f, text);
		}

		public static void Update(int yOffset = 0)
		{
			Instance.Update(yOffset);
		}

		public static void WriteText(Font f, int x, int y, string text, bool color)
		{
			Instance.WriteText(f, new Point(x, y), text, color);
		}

		public static void WriteText(Font f, Point p, string text, bool color)
		{
			Instance.WriteText(f, p, text, color);
		}

		public static void WriteTextBox(Font f, Rectangle r, string text, bool color)
		{
			Instance.WriteTextBox(f, r, text, color);
		}

		public static void WriteTextBox(Font f, Rectangle r, string text, bool color, Lcd.Alignment aln)
		{
			Instance.WriteTextBox(f, r, text, color, aln);
		}

		#endregion

	}
}