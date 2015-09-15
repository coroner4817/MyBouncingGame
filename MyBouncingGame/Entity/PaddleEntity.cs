using System;
using CocosSharp;

namespace MyBouncingGame.Entity
{
	public class PaddleEntity : PhysicsEntity
	{
		public PaddleEntity ()
		{
			InitialPhysicsEntity ("paddle");
		}

	}
}

