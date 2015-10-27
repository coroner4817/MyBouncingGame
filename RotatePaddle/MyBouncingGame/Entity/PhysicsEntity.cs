using System;
using CocosSharp;
using MyBouncingGame.Data;

namespace MyBouncingGame.Entity
{
	public class PhysicsEntity : CCNode
	{
		CCSprite sprite;

		public PhysicsEntity ()
		{
			
		}
			

		public float LeftX
		{
			get
			{
				return sprite.BoundingBoxTransformedToWorld.MinX;
			}
		}
		public float RightX
		{
			get
			{
				return sprite.BoundingBoxTransformedToWorld.MaxX;
			}
		}
		public float TopY
		{
			get
			{
				return sprite.BoundingBoxTransformedToWorld.MaxY;
			}
		}
		public float BottomY
		{
			get
			{
				return sprite.BoundingBoxTransformedToWorld.MinY;
			}
		}

		public float BoundingWidth
		{
			get
			{
				return this.RightX - this.LeftX;
			}
		}

		public float BoundingHeight
		{
			get
			{
				return this.TopY - this.BottomY;
			}
		}
			

		public void InitialPhysicsEntity(string fileName)
		{
			this.sprite = new CCSprite (fileName);
			this.sprite.IsAntialiased = false;
			this.AddChild (sprite);
		}

		public bool Intersects(PhysicsEntity other)
		{
			return this.sprite.BoundingBoxTransformedToWorld.IntersectsRect (other.sprite.BoundingBoxTransformedToWorld);
		}

		public bool BallPaddleRectIntersects(CCRect r1, CCRect r2)
		{
			if (r1.MinY < r2.MaxY) {
				return true;
			} else {
				return false;
			}
		}

	}
}

