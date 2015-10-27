using System;
using CocosSharp;

namespace MyBouncingGame.Entity
{
	public enum BrickType{
		blue,
		green,
		red,
		purple
	}

	public class BrickEntity : PhysicsEntity
	{
		private int score;
		private int index;

		public int indexInBrickList
		{
			get
			{
				return index;
			}
			set
			{
				index = value;
			}
		}

		public float brickX;
		public float brickY;

		public int BrickScore{
			get
			{ 
				return score;			
			}
		}

		public BrickEntity (BrickType typeOfBrick)
		{
			switch (typeOfBrick) 
			{
			case BrickType.blue:
				InitialPhysicsEntity ("BlueBrick.png");
				score = 10;
				break;
			case BrickType.green:
				InitialPhysicsEntity ("GreenBrick.png");
				score = 20;
				break;
			case BrickType.purple:
				InitialPhysicsEntity ("PurpleBrick.png");
				score = 30;
				break;
			case BrickType.red:
				InitialPhysicsEntity ("RedBrick.png");
				score = 40;
				break;
			}
				
		}

		public void handleBrickScrollDown()
		{
			this.PositionY -= 16;
		}
	}
}

