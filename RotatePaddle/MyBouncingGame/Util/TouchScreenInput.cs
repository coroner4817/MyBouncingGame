using System;
using CocosSharp;
using System.Collections.Generic;
using MyBouncingGame.Entity;

using MyBouncingGame.Scenes;

namespace MyBouncingGame.Util
{
	public class TouchScreenInput : IDisposable
	{
		CCEventListenerTouchAllAtOnce touchListener;

		CCLayer owner;
		PaddleEntity mControledEntity;

		CCPoint beganLocation;
		CCPoint locationOnScreen;
		CCPoint endLocation;
		CCPoint LoactionTemp;

		float rotateBeganThreshold;
		float verticalMovement;
		float rotatedAngle;
		float rotateSum;
		bool isRotated=false;
		bool touchAtLeft;

		public TouchScreenInput (CCLayer Owner,PaddleEntity controledEntity)
		{
			this.owner = Owner;
			mControledEntity = controledEntity;
	
			touchListener = new CCEventListenerTouchAllAtOnce ();
			touchListener.OnTouchesBegan = OnTouchesBegan;
			touchListener.OnTouchesMoved = HandleTouchesMoved;
			touchListener.OnTouchesEnded = OnTouchesEnded;
			owner.AddEventListener (touchListener);
		}

		void OnTouchesBegan (List<CCTouch> touches, CCEvent touchEvent)
		{
			if (touches.Count > 0)
			{
				beganLocation = touches [0].Location;
				LoactionTemp = beganLocation;
				rotateSum = 0;
				isRotated = false;
				if (beganLocation.X < mControledEntity.BoundingBoxTransformedToWorld.Center.X) {
					touchAtLeft = true;
				} else {
					touchAtLeft = false;
				}

				if (Math.Abs (LoactionTemp.X - mControledEntity.PositionX) > 100) {
					var moveby = new CCMoveTo ((float)0.1, new CCPoint (LoactionTemp.X, 50.0f));
					mControledEntity.RunAction (moveby);
				} 
			}
		}

		void HandleTouchesMoved (System.Collections.Generic.List<CCTouch> touches, CCEvent touchEvent)
		{
			locationOnScreen = touches [0].Location;

			verticalMovement = locationOnScreen.Y - LoactionTemp.Y;
			rotateBeganThreshold = locationOnScreen.Y - beganLocation.Y;

			if (touchAtLeft) 
			{
				if (rotateBeganThreshold > 50)
				{
					if (verticalMovement > 0) 
					{
						//do turn
						if (verticalMovement > 200) {
							verticalMovement = 200;
						}
						rotatedAngle = (float)(0.08 * (verticalMovement));
						rotateSum += rotatedAngle;
						mControledEntity.Rotation = rotateSum;
						isRotated = true;
					}
					if (verticalMovement < 0) 
					{
						if (verticalMovement < -200) {
							verticalMovement = -200;
						}
						rotatedAngle = (float)(0.08 * (verticalMovement));
						rotateSum += rotatedAngle;
						mControledEntity.Rotation = rotateSum;
			
						isRotated = true;
					}
				}		
			}


			if (!touchAtLeft) 
			{
				if (rotateBeganThreshold > 50)
				{
					if (verticalMovement > 0) 
					{
						//do turn
						if (verticalMovement > 200) {
							verticalMovement = 200;
						}
						rotatedAngle = (float)(0.08 * (verticalMovement));
						rotateSum -= rotatedAngle;
						mControledEntity.Rotation = rotateSum;
						isRotated = true;
					}
					if (verticalMovement < 0) 
					{
						if (verticalMovement < -200) {
							verticalMovement = -200;
						}
						rotatedAngle = (float)(0.08 * (verticalMovement));
						rotateSum -= rotatedAngle;
						mControledEntity.Rotation = rotateSum;
						isRotated = true;
					}
				}		
			}

			mControledEntity.Angle = rotateSum;
			//Console.WriteLine ("angle = "+mControledEntity.Angle.ToString ());

//			if ((isRotated==true)&&((rotateSum -0)<3))
//			{
//				if (locationOnScreen.X < mControledEntity.BoundingBoxTransformedToWorld.Center.X) {
//					touchAtLeft = true;
//				} else {
//					touchAtLeft = false;
//				}
//			}

			if (locationOnScreen.X < owner.VisibleBoundsWorldspace.LowerLeft.X)
			{
				locationOnScreen.X = owner.VisibleBoundsWorldspace.LowerLeft.X + (mControledEntity.RightX- mControledEntity.LeftX)/2;
			}
			if (locationOnScreen.X > owner.VisibleBoundsWorldspace.UpperRight.X)
			{
				locationOnScreen.X = owner.VisibleBoundsWorldspace.UpperRight.X - (mControledEntity.RightX - mControledEntity.LeftX)/2;
			}
			mControledEntity.PositionX = locationOnScreen.X;
		

			LoactionTemp = locationOnScreen;
		}

		void OnTouchesEnded (List<CCTouch> touches, CCEvent touchEvent)
		{
			if (touches.Count > 0)
			{
				endLocation=touches [0].Location;

				if (isRotated) 
				{
					var rotateBack=new CCRotateBy(0.2f,-rotateSum);
					mControledEntity.RunAction (rotateBack);

					rotateSum = 0;
					mControledEntity.Angle = rotateSum;
				}
			}
		}

		public void Dispose()
		{
			owner.RemoveEventListener (touchListener);
		}
	}
}

