using System;
using CocosSharp;
using System.Collections.Generic;
using MyBouncingGame.Entity;

namespace MyBouncingGame.Util
{
	public class TouchScreenInput : IDisposable
	{
		CCEventListenerTouchAllAtOnce touchListener;

		CCLayer owner;
		PhysicsEntity mControledEntity;

		public TouchScreenInput (CCLayer Owner,PhysicsEntity controledEntity)
		{
			this.owner = Owner;
			mControledEntity = controledEntity;

			touchListener = new CCEventListenerTouchAllAtOnce ();
			touchListener.OnTouchesMoved = HandleTouchesMoved;
			touchListener.OnTouchesEnded = OnTouchesEnded;
			owner.AddEventListener (touchListener);
		}

		void HandleTouchesMoved (System.Collections.Generic.List<CCTouch> touches, CCEvent touchEvent)
		{

			var locationOnScreen = touches [0].Location;

			if (locationOnScreen.X < owner.VisibleBoundsWorldspace.LowerLeft.X)
			{
				locationOnScreen.X = owner.VisibleBoundsWorldspace.LowerLeft.X+50.0f;
			}
			if (locationOnScreen.X > owner.VisibleBoundsWorldspace.UpperRight.X)
			{
				locationOnScreen.X = owner.VisibleBoundsWorldspace.UpperRight.X-50.0f;
			}

			mControledEntity.PositionX = locationOnScreen.X;

		}

		void OnTouchesEnded (List<CCTouch> touches, CCEvent touchEvent)
		{
			if (touches.Count > 0)
			{
				// Perform touch handling here
			}
		}

		public void Dispose()
		{
			owner.RemoveEventListener (touchListener);
		}
	}
}

