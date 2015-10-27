using System;
using CocosSharp;

namespace MyBouncingGame.Entity
{
	public class PaddleEntity : PhysicsEntity
	{
		public float YVelocity;
		public float Angle;

		public float flatBoundingBoxWidth;
		public float flatBoundingBoxHeight;

		public CCRect rotatedBackBoundingBox
		{
			get
			{
				return new CCRect (this.PositionX, 50f, flatBoundingBoxWidth, flatBoundingBoxHeight);
			}
		}

		public PaddleEntity ()
		{
			InitialPhysicsEntity ("paddle");

			flatBoundingBoxWidth = this.BoundingWidth;
			flatBoundingBoxHeight = this.BoundingHeight;
		}

	}
}

