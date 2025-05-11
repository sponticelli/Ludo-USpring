using UnityEngine;

namespace USpring.Vectors
{
	public struct CustomVector2 : IVector
	{
		public Vector2 Vector2;

		public CustomVector2(Vector2 vector2)
		{
			this.Vector2 = vector2;
		}

		public float this[int index]
		{
			get
			{
				return Vector2[index];
			}
			set
			{
				Vector2[index] = value;
			}
		}
		public int GetSize()
		{
			return 2;
		}
	}
}