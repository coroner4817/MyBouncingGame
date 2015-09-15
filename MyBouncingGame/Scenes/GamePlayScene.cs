using System;
using CocosSharp;
using System.Collections.Generic;
using MyBouncingGame.Data;

namespace MyBouncingGame.Scenes
{
	public class GamePlayScene : CCScene
	{
		int score=0;
		float ballXVelocity;
		float ballYVelocity;

		CCLayer gameplayLayer;
		CCLayer hudLayer;

		CCSprite backgroundImage;
		CCLabel scoreLabel;
		CCSprite ballSprite;
		CCSprite paddleSprite;
		CCLabel deathRestart;

		ConstantData mData;

		int delayCount=0;
		bool deathFlag=false;
		bool AlreadyAccelerate=false;

		public GamePlayScene (CCWindow mainWindow) : base(mainWindow)
		{
			CreateLayers ();

			CreateHudAndBackground ();

			AddEntity ();

			AddTouchListener ();

			Schedule(PerformActivity);
		}

		private void CreateLayers()
		{
			hudLayer = new CCLayer ();
			this.AddChild (hudLayer);

			gameplayLayer = new CCLayer ();
			this.AddChild (gameplayLayer);

			mData = new ConstantData (gameplayLayer);
		}

		private void CreateHudAndBackground()
		{
			//add backgroung image
			backgroundImage = new CCSprite ("background.png");
			backgroundImage.Scale = 2.0f;
			backgroundImage.PositionX = ContentSize.Center.X;
			backgroundImage.PositionY = ContentSize.Center.Y;
			backgroundImage.IsAntialiased = false;
			hudLayer.AddChild (backgroundImage);

			//add score Label
			scoreLabel = new CCLabel ("Score: 0", "arial", 22, CCLabelFormat.SpriteFont);
			scoreLabel.PositionX = gameplayLayer.VisibleBoundsWorldspace.UpperRight.X-200.0f;
			scoreLabel.PositionY = gameplayLayer.VisibleBoundsWorldspace.UpperRight.Y-50.0f;
			scoreLabel.AnchorPoint = CCPoint.AnchorUpperLeft;
			hudLayer.AddChild (scoreLabel);
		}

		private void AddEntity()
		{
			paddleSprite = new CCSprite ("paddle");
			paddleSprite.PositionX = gameplayLayer.VisibleBoundsWorldspace.Center.X;
			paddleSprite.PositionY = 50;
			gameplayLayer.AddChild (paddleSprite);

			ballSprite = new CCSprite ("ball");
			ballSprite.Position = gameplayLayer.VisibleBoundsWorldspace.Center;

			gameplayLayer.AddChild  (ballSprite);
		}

		private void PerformActivity(float frameTimeInSeconds)
		{
			if (!deathFlag) {
				
				ballYVelocity += frameTimeInSeconds * - mData.getGravity(0);

				ballSprite.PositionX += ballXVelocity * frameTimeInSeconds;
				ballSprite.PositionY += ballYVelocity * frameTimeInSeconds;

				bool doesBallOverlapPaddle = 
					ballSprite.BoundingBoxTransformedToParent.IntersectsRect (paddleSprite.BoundingBoxTransformedToParent);
				
				bool isMovingDownward = ballYVelocity < 0;

				if (doesBallOverlapPaddle && isMovingDownward) {
					// First let's invert the velocity:
					ballYVelocity *= -1;

					float minXVelocity = ballXVelocity-200.0f;
					float maxXVelocity = ballXVelocity+200.0f;
					ballXVelocity = CCRandom.GetRandomFloat (minXVelocity, maxXVelocity);

					score++;
					scoreLabel.Text = "Score: " + score.ToString ();
				}

				float ballRight = ballSprite.BoundingBoxTransformedToParent.MaxX;
				float ballLeft = ballSprite.BoundingBoxTransformedToParent.MinX;
				float ballTop = ballSprite.BoundingBoxTransformedToParent.MaxY;
				float ballBottom = ballSprite.BoundingBoxTransformedToParent.MinY;

				// Check if the ball is either too far to the right or left:    
				bool shouldReflectXVelocity = 
					(ballRight > mData.screenRightX && ballXVelocity > 0) ||
					(ballLeft < mData.screenLeftX && ballXVelocity < 0);
				
				if (shouldReflectXVelocity) {
					ballXVelocity *= -1;
				}

				bool shouldReflectYVelocity = (ballTop >mData.screenTopY && ballYVelocity > 0);

				if (shouldReflectYVelocity) {
					ballYVelocity *= -1;
				}

				if ((score % 11 == 10) && (score != 0))
				{
					AlreadyAccelerate = false;
				}

				if ((score % 11 == 0)&&(score!=0)&&(!AlreadyAccelerate)) 
				{
					ballXVelocity *= mData.getSpeedCoeff(0);
					ballYVelocity *= mData.getSpeedCoeff(0);
					AlreadyAccelerate = true;
				}

				if (ballSprite.PositionY < (mData.screenBottomY-10)) {
					deathFlag = true;
				}

			} else {
				delayCount++;

				if (delayCount == 1)
				{
					deathRestart = new CCLabel("得分："+score.ToString(), "fonts/alphbeta.ttf", 75, CCLabelFormat.SystemFont);
					deathRestart.Position = gameplayLayer.VisibleBoundsWorldspace.Center;
					//backCount.IsAntialiased = false;
					hudLayer.AddChild (deathRestart);
				}

				if (delayCount == 100) 
				{
					hudLayer.RemoveChild(deathRestart);
					deathRestart = new CCLabel("重新开始", "fonts/alphbeta.ttf", 75, CCLabelFormat.SystemFont);
					deathRestart.Position = gameplayLayer.VisibleBoundsWorldspace.Center;
					//backCount.IsAntialiased = false;
					hudLayer.AddChild (deathRestart);
				}

				if (delayCount == 200) 
				{
					hudLayer.RemoveChild(deathRestart);
					deathRestart = new CCLabel("3", "fonts/alphbeta.ttf", 75, CCLabelFormat.SystemFont);
					deathRestart.Position = gameplayLayer.VisibleBoundsWorldspace.Center;
					//backCount.IsAntialiased = false;
					hudLayer.AddChild (deathRestart);

				}
				if (delayCount == 300) 
				{
					hudLayer.RemoveChild(deathRestart);
					deathRestart = new CCLabel("2", "fonts/alphbeta.ttf", 75, CCLabelFormat.SystemFont);
					deathRestart.Position = gameplayLayer.VisibleBoundsWorldspace.Center;
					//backCount.IsAntialiased = false;
					hudLayer.AddChild (deathRestart);

				}
				if (delayCount == 400) 
				{
					hudLayer.RemoveChild(deathRestart);
					deathRestart = new CCLabel("1", "fonts/alphbeta.ttf", 75, CCLabelFormat.SystemFont);
					deathRestart.Position = gameplayLayer.VisibleBoundsWorldspace.Center;
					//backCount.IsAntialiased = false;
					hudLayer.AddChild (deathRestart);
				}


				if (delayCount == 500) 
				{
					hudLayer.RemoveChild(deathRestart);

					deathFlag = false;
					delayCount = 0;

					ballSprite.Position = gameplayLayer.VisibleBoundsWorldspace.Center;

					ballYVelocity = 0;
					ballXVelocity = 0;

					scoreLabel.Text = "";
					score = 0;
				}

			
			}
			
		}

		private void AddTouchListener()
		{
			var touchListener = new CCEventListenerTouchAllAtOnce ();
			touchListener.OnTouchesEnded = OnTouchesEnded;
			// new code:
			touchListener.OnTouchesMoved = HandleTouchesMoved;
			gameplayLayer.AddEventListener (touchListener);
		}

		void HandleTouchesMoved (System.Collections.Generic.List<CCTouch> touches, CCEvent touchEvent)
		{
			// we only care about the first touch:
			var locationOnScreen = touches [0].Location;

			if (locationOnScreen.X < gameplayLayer.VisibleBoundsWorldspace.LowerLeft.X)
			{
				locationOnScreen.X = gameplayLayer.VisibleBoundsWorldspace.LowerLeft.X+50.0f;
			}
			if (locationOnScreen.X > gameplayLayer.VisibleBoundsWorldspace.UpperRight.X)
			{
				locationOnScreen.X = gameplayLayer.VisibleBoundsWorldspace.UpperRight.X-50.0f;
			}

			paddleSprite.PositionX = locationOnScreen.X;

		}

		void OnTouchesEnded (List<CCTouch> touches, CCEvent touchEvent)
		{
			if (touches.Count > 0)
			{
				// Perform touch handling here
			}
		}
	}
}

