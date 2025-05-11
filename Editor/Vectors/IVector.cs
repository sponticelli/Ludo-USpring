namespace USpring.Vectors
{
	public interface IVector
	{
		int GetSize();

		float this[int index]
		{
			get;
			set;
		}
	}
}