namespace USpring.Vectors
{
	public struct Vector4Bool : IVectorBool
	{
		public static readonly Vector4Bool AllTrue = new Vector4Bool(true, true, true, true);
		public static readonly Vector4Bool AllFalse = new Vector4Bool(false, false, false, false);

		public bool this[int index]
		{
			get
			{
				bool res = false;
				switch (index)
				{
					case 0:
						res = X;
						break;
					case 1:
						res = Y;
						break;
					case 2:
						res = Z;
						break;
					case 3:
						res = W;
						break;
				}

				return res;
			}
			set
			{
				switch (index)
				{
					case 0:
						X = value;
						break;
					case 1:
						Y = value;
						break;
					case 2:
						Z = value;
						break;
					case 3:
						W = value;
						break;
				}
			}
		}

		public bool X;
		public bool Y;
		public bool Z;
		public bool W;

		public Vector4Bool(bool x, bool y, bool z, bool w)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
			this.W = w;
		}

		public int GetSize()
		{
			return 4;
		}

		public static Vector4Bool operator !(Vector4Bool a)
		{
			Vector4Bool res = new Vector4Bool(!a.X, !a.Y, !a.Z, !a.W);
			return res;
		}

		public Vector4Bool And(Vector4Bool vectorBoolToCompare)
		{
			Vector4Bool res = new Vector4Bool(vectorBoolToCompare.X && X, vectorBoolToCompare.Y && Y, vectorBoolToCompare.Z && Z, vectorBoolToCompare.W && W);
			return res;
		}
	}
}