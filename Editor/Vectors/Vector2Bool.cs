namespace USpring.Vectors
{
	public struct Vector2Bool: IVectorBool
	{
		public static readonly Vector2Bool AllTrue = new Vector2Bool(true, true);
		public static readonly Vector2Bool AllFalse = new Vector2Bool(false, false);

		public bool X;
		public bool Y;

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
				}
			}
		}

		public Vector2Bool(bool x, bool y)
		{
			this.X = x;
			this.Y = y;
		}

		public int GetSize()
		{
			return 2;
		}

		public void SetValue(int index, bool value)
		{
			this[index] = value;
		}

		public static Vector2Bool operator !(Vector2Bool a)
		{
			Vector2Bool res = new Vector2Bool(!a.X, !a.Y);
			return res;
		}

		public Vector2Bool And(Vector2Bool vectorBoolToCompare)
		{
			Vector2Bool res = new Vector2Bool(vectorBoolToCompare.X && X, vectorBoolToCompare.Y && Y);
			return res;
		}
	}
}