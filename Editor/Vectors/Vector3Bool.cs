namespace USpring.Vectors
{
	public struct Vector3Bool : IVectorBool
	{
		public static readonly Vector3Bool AllTrue = new Vector3Bool(true, true, true);
		public static readonly Vector3Bool AllFalse = new Vector3Bool(false, false, false);

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
				}
			}
		}

		public bool X;
		public bool Y;
		public bool Z;

		public Vector3Bool(bool x, bool y, bool z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		public int GetSize()
		{
			return 3;
		}

		public Vector3Bool And(Vector3Bool vectorBoolToCompare)
		{
			Vector3Bool res = new Vector3Bool(vectorBoolToCompare.X && X, vectorBoolToCompare.Y && Y, vectorBoolToCompare.Z && Z);
			return res;
		}

		public static Vector3Bool operator !(Vector3Bool a)
		{
			Vector3Bool res = new Vector3Bool(!a.X, !a.Y, !a.Z);
			return res;
		}
	}
}