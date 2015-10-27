using System;
using MyBouncingGame.Data;
using CocosSharp;
using CocosDenshion;
using System.Threading;

namespace MyBouncingGame.Entity
{
	public class BallEntity : PhysicsEntity
	{
		public float XVelocity;
		public float YVelocity;

		public bool AccelerateState=false;

		private CCRect rotatedBallRect;
		private CCRect flatPaddleRect;
		private float rotatedX;
		private float rotatedY;
		CCDrawNode draw=new CCDrawNode();
		CCLayer gamelayer;

		float VelocityAngle;
		float Xtemp;
		float Ytemp;

		ConstantData mData;

		public BallEntity (CCLayer gameLayer)
		{
			gamelayer = gameLayer;
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

		public void HandleCollisionWithPaddle(bool isCorner, float VelocityAngle)
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
			if (XVelocity == 0) {
				XVelocity = 30;
			}

			Xtemp = XVelocity;
			Ytemp = YVelocity;

			XVelocity = (float)Math.Cos (VelocityAngle) * Xtemp - (float)Math.Sin (VelocityAngle) * Ytemp;
			YVelocity = (float)Math.Sin (VelocityAngle) * Xtemp + (float)Math.Cos (VelocityAngle) * Ytemp;

			if (YVelocity < 350) {
				YVelocity = 350;
			} 

			CCSimpleAudioEngine.SharedEngine.PlayEffect ("BallCollideHigh.wav");
		}

		public void ReactToLevelCollision()
		{
			YVelocity = -YVelocity;
			if (XVelocity == 0) {
				XVelocity = 30;
			}
		}

		public bool isCollideWithRotatedPaddle(PaddleEntity mPaddle, float angle)
		{
			bool collide=false;
			if (Math.Abs (angle) > 45) {
				VelocityAngle = -angle / 3;
			} else {
				VelocityAngle = -angle/2;
			}

			rotatedX = (float)Math.Cos (angle) * (this.PositionX-mPaddle.PositionX) - (float)Math.Sin (angle) * (this.PositionY-mPaddle.PositionY)+mPaddle.PositionX;
			rotatedY = (float)Math.Sin (angle) * (this.PositionX-mPaddle.PositionX) + (float)Math.Cos (angle) * (this.PositionY-mPaddle.PositionY)+mPaddle.PositionY;

			rotatedBallRect = new CCRect (rotatedX-this.BoundingWidth/2, rotatedY-this.BoundingHeight/2, this.BoundingWidth, this.BoundingHeight);
			flatPaddleRect = new CCRect (mPaddle.PositionX-mPaddle.flatBoundingBoxWidth/2, 50f-mPaddle.flatBoundingBoxHeight/2, mPaddle.flatBoundingBoxWidth, mPaddle.flatBoundingBoxHeight);
	
			collide = BallPaddleRectIntersects (rotatedBallRect, flatPaddleRect);


			if (rotatedBallRect.Center.Y < flatPaddleRect.Center.Y) 
			{
				collide = false;
			}
			if (rotatedBallRect.Center.X < (flatPaddleRect.Center.X - mPaddle.flatBoundingBoxWidth / 2)) 
			{
				collide = false;
			}
			if (this.YVelocity > 0) {
				collide = false;
			}
				


			if (collide) {

//				Console.WriteLine("X: "+(mPaddle.PositionX-mPaddle.flatBoundingBoxWidth/2).ToString());
//				Console.WriteLine("Y: "+(50f-mPaddle.flatBoundingBoxHeight/2).ToString());
//				Console.WriteLine("width: "+mPaddle.flatBoundingBoxWidth.ToString());
//				Console.WriteLine("height: "+mPaddle.flatBoundingBoxHeight.ToString());
				Console.WriteLine("ball: "+rotatedBallRect.MinY.ToString());
				Console.WriteLine ("paddle: "+flatPaddleRect.MaxY.ToString ());

				Console.WriteLine ("-------------------------------------");

				gamelayer.RemoveChild (draw);
				draw = new CCDrawNode ();
				draw.DrawRect (rotatedBallRect,new CCColor4B(0,0,0));
				draw.DrawRect (flatPaddleRect, new CCColor4B(0,0,0));

				gamelayer.AddChild (draw);

				//Thread.Sleep (1000);
			}

			return collide;
		}

	}
}

