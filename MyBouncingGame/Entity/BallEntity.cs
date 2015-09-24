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
			//YVelocity += seconds * - mData.getGravity(0);

			this.PositionX += XVelocity * seconds;
			this.PositionY += YVelocity * seconds;
		}

		public void HandleCollisionWithPaddle(bool isCorner)
		{
			YVelocity *= -1;

			if (!isCorner) {

				if (XVelocity > 0) {
					XVelocity = CCRandom.GetRandomFloat (0, XVelocity + 200.0f);
				} else {
					XVelocity = CCRandom.GetRandomFloat (XVelocity - 200.0f, 0);
				}
		
			} else {
				if (XVelocity > 0) {
					XVelocity = CCRandom.GetRandomFloat (0, XVelocity + 400.0f);
				} else {
					XVelocity = CCRandom.GetRandomFloat (XVelocity - 400.0f, 0);
				}
			}

			CCSimpleAudioEngine.SharedEngine.PlayEffect ("BallCollideHigh.wav");
		}


	}
}

