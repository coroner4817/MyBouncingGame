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


	}
}

