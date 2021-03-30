using System;

namespace RouteMatcher
{
	internal static class Extensions
	{
		public static void GetBits(this Span<bool> res, byte b)
		{
			res[0] = Convert.ToBoolean(b >> 7 & 1);
			res[1] = Convert.ToBoolean(b >> 6 & 1);
			res[2] = Convert.ToBoolean(b >> 5 & 1);
			res[3] = Convert.ToBoolean(b >> 4 & 1);
			res[4] = Convert.ToBoolean(b >> 3 & 1);
			res[5] = Convert.ToBoolean(b >> 2 & 1);
			res[6] = Convert.ToBoolean(b >> 1 & 1);
			res[7] = Convert.ToBoolean(b & 1);
		}

		public static void GetBits(this Span<byte> res, byte b)
		{
			res[0] = (byte)(b >> 7 & 1);
			res[1] = (byte)(b >> 6 & 1);
			res[2] = (byte)(b >> 5 & 1);
			res[3] = (byte)(b >> 4 & 1);
			res[4] = (byte)(b >> 3 & 1);
			res[5] = (byte)(b >> 2 & 1);
			res[6] = (byte)(b >> 1 & 1);
			res[7] = (byte)(b & 1);
		}
	}
}
