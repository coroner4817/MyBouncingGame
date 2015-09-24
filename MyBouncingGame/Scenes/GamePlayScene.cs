using System;
using CocosSharp;
using System.Collections.Generic;
using MyBouncingGame.Data;
using MyBouncingGame.Entity;
using MyBouncingGame.Util;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using CocosDenshion;

namespace MyBouncingGame.Scenes
{
	public class GamePlayScene : CCScene
	{
		int score=0;
		ConstantData mData;

		CCLayer gameplayLayer;
		CCLayer hudLayer;


		BallEntity mBall;
		PaddleEntity mPaddle;

		CCSprite backgroundImage;
		CCLabel scoreLabel;
		CCLabel deathRestartLabel;

		int delayCount=0;
		int beginCount=0;

		bool deathFlag=false;
		bool beginCountFinishFlag=false;

		TouchScreenInput input;

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
			backgroundImage = new CCSprite ("GamePlayBackground.png");
			backgroundImage.PositionX = ContentSize.Center.X;
			backgroundImage.PositionY = ContentSize.Center.Y+500.0f;
			backgroundImage.IsAntialiased = false;
			hudLayer.AddChild (backgroundImage);

			//add score Label
			scoreLabel = new CCLabel ("Score: 0", "fonts/Russian.ttf", 35, CCLabelFormat.SystemFont);
			scoreLabel.PositionX = gameplayLayer.VisibleBoundsWorldspace.UpperRight.X-200.0f;
			scoreLabel.PositionY = gameplayLayer.VisibleBoundsWorldspace.UpperRight.Y-50.0f;
			scoreLabel.AnchorPoint = CCPoint.AnchorUpperLeft;
			hudLayer.AddChild (scoreLabel);

		}

		private void AddEntity()
		{
			mPaddle = new PaddleEntity ();
			mPaddle.PositionX = gameplayLayer.VisibleBoundsWorldspace.Center.X;
			mPaddle.PositionY = 50;
			gameplayLayer.AddChild (mPaddle);

			mBall = new BallEntity ();
			mBall.Position = gameplayLayer.VisibleBoundsWorldspace.Center;
			mBall.Visible = false;
			mBall.YVelocity = -350;
			gameplayLayer.AddChild  (mBall);
		}

		private void PerformActivity(float frameTimeInSeconds)
		{
			Console.WriteLine ("GamePlay Schedule Running!!!!!!!!!");
			//Console.WriteLine(mBall.XVelocity.ToString());
			//Console.WriteLine (mBall.YVelocity.ToString ());

			beginCount++;

			if (beginCountFinishFlag == false) {
			
				HandleBegin ();

			}
			else {
				
				if (!deathFlag) {

					mBall.PerformActivity (frameTimeInSeconds);

					PerformCollision ();

					PerformAccelerate ();

					TestIfDeath ();

				} else {

					HandleDeath ();
				}
			}

			HandleBackKeyPress ();

		}

		private void PerformAccelerate()
		{
			if ((score % 10 == 9) && (score != 0))
			{
				mBall.AccelerateState = false;
			}

			if ((score % 10 == 0)&&(score!=0)&&(!mBall.AccelerateState)) 
			{
				mBall.XVelocity *= mData.getSpeedCoeff(1);
				mBall.YVelocity *= mData.getSpeedCoeff(1);
				mBall.AccelerateState = true;
			}
		}

		private void PerformCollision()
		{

			bool ifCollisionLeftRight = 
				(mBall.RightX > mData.screenRightX && mBall.XVelocity > 0) ||(mBall.LeftX < mData.screenLeftX && mBall.XVelocity < 0);

			if (ifCollisionLeftRight) {
				mBall.XVelocity *= -1;
				CCSimpleAudioEngine.SharedEngine.PlayEffect ("BallCollideLow.wav");
			}
	
			bool ifCollisionTop = (mBall.TopY >mData.screenTopY && mBall.YVelocity > 0);

			if (ifCollisionTop) {
				mBall.YVelocity *= -1;
				CCSimpleAudioEngine.SharedEngine.PlayEffect ("BallCollideLow.wav");
			}


			//和paddle
			bool doesBallOverlapPaddle = mBall.Intersects (mPaddle);
			bool isMovingDownward = mBall.YVelocity < 0;

			if (doesBallOverlapPaddle && isMovingDownward) {

				if ((mBall.RightX < (mPaddle.LeftX + mData.paddleCornerDefine)) || (mBall.LeftX > (mPaddle.RightX - mData.paddleCornerDefine))) {
					mBall.HandleCollisionWithPaddle (true);
				} else {
					mBall.HandleCollisionWithPaddle (false);
				}

				score++;
				scoreLabel.Text = "Score: " + score.ToString ();
			}

		}

		private void TestIfDeath()
		{
			if (mBall.TopY < (mData.screenBottomY-20)) 
			{
				deathFlag = true;
			}
		}

		private void HandleBegin()
		{
			if (beginCount == 100) {
				hudLayer.RemoveChild (deathRestartLabel);
				deathRestartLabel = new CCLabel ("3", "fonts/Gimme Danger.ttf", 75, CCLabelFormat.SystemFont);
				deathRestartLabel.Color = new CCColor3B (255, 0, 0);
				deathRestartLabel.Position = hudLayer.VisibleBoundsWorldspace.Center;
				hudLayer.AddChild (deathRestartLabel);

			} else if (beginCount == 200) {
				hudLayer.RemoveChild (deathRestartLabel);
				deathRestartLabel = new CCLabel ("2", "fonts/Gimme Danger.ttf", 75, CCLabelFormat.SystemFont);
				deathRestartLabel.Color = new CCColor3B (255, 0, 0);
				deathRestartLabel.Position = hudLayer.VisibleBoundsWorldspace.Center;
				hudLayer.AddChild (deathRestartLabel);

			} else if (beginCount == 300) {
				hudLayer.RemoveChild (deathRestartLabel);
				deathRestartLabel = new CCLabel ("1", "fonts/Gimme Danger.ttf", 75, CCLabelFormat.SystemFont);
				deathRestartLabel.Color = new CCColor3B (255, 0, 0);
				deathRestartLabel.Position = hudLayer.VisibleBoundsWorldspace.Center;
				hudLayer.AddChild (deathRestartLabel);

			} else if (beginCount == 400) {
				hudLayer.RemoveChild (deathRestartLabel);
				beginCountFinishFlag = true;
				mBall.Visible = true;
			}
			
		}


		private void HandleDeath()
		{
			delayCount++;

			if (delayCount == 1)
			{

				hudLayer.RemoveChild(deathRestartLabel);
				deathRestartLabel = new CCLabel("Death!", "fonts/Gimme Danger.ttf", 75, CCLabelFormat.SystemFont);
				deathRestartLabel.Color = new CCColor3B (255, 0, 0);
				deathRestartLabel.Position = hudLayer.VisibleBoundsWorldspace.Center;
				hudLayer.AddChild (deathRestartLabel);
			}

			if (delayCount == 100) 
			{


				hudLayer.RemoveChild(deathRestartLabel);
				deathRestartLabel = new CCLabel("Restart", "fonts/Gimme Danger.ttf", 75, CCLabelFormat.SystemFont);
				deathRestartLabel.Color = new CCColor3B (255, 0, 0);
				deathRestartLabel.Position = hudLayer.VisibleBoundsWorldspace.Center;
				hudLayer.AddChild (deathRestartLabel);
			}

			if (delayCount == 200) 
			{


				hudLayer.RemoveChild(deathRestartLabel);
				deathRestartLabel = new CCLabel("3", "fonts/Gimme Danger.ttf", 75, CCLabelFormat.SystemFont);
				deathRestartLabel.Color = new CCColor3B (255, 0, 0);
				deathRestartLabel.Position = hudLayer.VisibleBoundsWorldspace.Center;
				hudLayer.AddChild (deathRestartLabel);

			}
			if (delayCount == 300) 
			{
				

				hudLayer.RemoveChild(deathRestartLabel);
				deathRestartLabel = new CCLabel("2", "fonts/Gimme Danger.ttf", 75, CCLabelFormat.SystemFont);
				deathRestartLabel.Color = new CCColor3B (255, 0, 0);
				deathRestartLabel.Position = hudLayer.VisibleBoundsWorldspace.Center;
				hudLayer.AddChild (deathRestartLabel);

			}
			if (delayCount == 400) 
			{
				

				hudLayer.RemoveChild(deathRestartLabel);
				deathRestartLabel = new CCLabel("1", "fonts/Gimme Danger.ttf", 75, CCLabelFormat.SystemFont);
				deathRestartLabel.Color = new CCColor3B (255, 0, 0);
				deathRestartLabel.Position = hudLayer.VisibleBoundsWorldspace.Center;
				hudLayer.AddChild (deathRestartLabel);
			}
				
			if (delayCount == 500) 
			{
				hudLayer.RemoveChild(deathRestartLabel);

				deathFlag = false;
				delayCount = 0;

				mBall.Position = gameplayLayer.VisibleBoundsWorldspace.Center;

				mBall.YVelocity = -350;
				mBall.XVelocity = 0;

				scoreLabel.Text = "Score: 0";
				score = 0;
			}
		}
			
		private void AddTouchListener()
		{
			input = new TouchScreenInput (gameplayLayer,mPaddle);
		}

		private void HandleBackKeyPress()
		{
			if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
			{
				Unschedule (PerformActivity);
				GameAppDelegate.GoToGameStartPage ();
			}
		}
			
	}
}

