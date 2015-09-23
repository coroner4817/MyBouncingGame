using System;
using MyBouncingGame.Data;
using CocosSharp;
using CocosDenshion;

namespace MyBouncingGame.Entity
{
	public class BallEntity : PhysicsEntity
	{
		public float XVelocity;
		public float YVelocity;

		public bool AccelerateState=false;

		ConstantData mData;

		public BallEntity ()
		{
			mData = new ConstantData ();
			InitialPhysicsEntity ("ball");
			
		}

		public void PerformActivity(float seconds)
		{
			calculateSpeedUpdatePosition (seconds);
		}

		private void calculateSpeedUpdatePosition(float seconds)
		{
			YVelocity += seconds * - mData.getGravity(0);

			this.PositionX += XVelocity * seconds;
			this.PositionY += YVelocity * seconds;
		}

		public void HandleCollisionWithPaddle()
		{
			YVelocity *= -1;

			float minXVelocity = XVelocity-200.0f;
			float maxXVelocity = XVelocity+200.0f;
			XVelocity = CCRandom.GetRandomFloat (minXVelocity, maxXVelocity);

			CCSimpleAudioEngine.SharedEngine.PlayEffect ("BallCollideHigh.wav");
		}


	}
}

