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

		CCLabel debugLabel1;
		CCLabel debugLabel2;

		int delayCount=0;
		int beginCount=0;

		bool deathFlag=false;
		bool beginCountFinishFlag=false;

		List<BrickEntity> mBrickList;
		List<float> brickLevelPositionX;
		Random rnd = new Random ();
		int scrollCount=0;
		BrickEntity brick;

		TouchScreenInput input;


		public GamePlayScene (CCWindow mainWindow) : base(mainWindow)
		{
			CreateLayers ();

			CreateHudAndBackground ();

			AddEntity ();

			AddTouchListener ();

			InitBricks ();

			Schedule(PerformActivity);
		}

		void InitBricks()
		{
			//初始化地图
			CreateInitMap();
		}

		void CreateInitMap()
		{
			mBrickList = new List<BrickEntity> ();

			int i;
			int level=0;
			int levelBrickCount;

			levelBrickCount = randomBrickPositionGenerator ();

			for (i = 0; i < 20; i++)
			{
				if (levelBrickCount == 0) {
					if (level == 2) {
						break;
					}
					levelBrickCount = randomBrickPositionGenerator ();
					level++;
				}

				brick = new BrickEntity (randomBrickGenerator());
				mBrickList.Add (brick);
				brick.indexInBrickList = mBrickList.IndexOf (brick);

				brick.brickX = brickLevelPositionX[levelBrickCount-1];
				brick.brickY = 480 + 8 + level * 96;
				brick.PositionX = brick.brickX;
				brick.PositionY = brick.brickY;

				gameplayLayer.AddChild (brick);
				levelBrickCount--;
			}
				
		}

		BrickType randomBrickGenerator()
		{
			switch (rnd.Next (1, 4)) 
			{
			case 1:
				return BrickType.blue;

			case 2:
				return BrickType.green;

			case 3:
				return BrickType.purple;

			case 4:
				return BrickType.red;

			default:
				return BrickType.blue;
			}
		}

		int randomBrickPositionGenerator()
		{
			brickLevelPositionX = new List<float> ();

			int brickCount = rnd.Next (3, 5);

			int startX = rnd.Next(160,200+(5-brickCount)*48);
			brickLevelPositionX.Add (startX);
			int i,newx;
			for (i = 0; i < brickCount-1; i++) 
			{
				newx = startX + rnd.Next (144, 176+(5-brickCount)*96);
				brickLevelPositionX.Add (newx);
				startX = newx;
			}

			return brickCount;
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

			//add debug Label
			debugLabel1 = new CCLabel ("Y velocity: ", "fonts/arial-22", 20, CCLabelFormat.SystemFont);
			debugLabel1.PositionX = gameplayLayer.VisibleBoundsWorldspace.LowerLeft.X+50.0f;
			debugLabel1.PositionY = gameplayLayer.VisibleBoundsWorldspace.LowerLeft.Y+50.0f;
			debugLabel1.AnchorPoint = CCPoint.AnchorUpperLeft;
			hudLayer.AddChild (debugLabel1);

			debugLabel2 = new CCLabel ("X velocity: ", "fonts/arial-22", 20, CCLabelFormat.SystemFont);
			debugLabel2.PositionX = gameplayLayer.VisibleBoundsWorldspace.LowerLeft.X+50.0f;
			debugLabel2.PositionY = gameplayLayer.VisibleBoundsWorldspace.LowerLeft.Y+80.0f;
			debugLabel2.AnchorPoint = CCPoint.AnchorUpperLeft;
			hudLayer.AddChild (debugLabel2);

		}

		private void AddEntity()
		{
			mPaddle = new PaddleEntity ();
			mPaddle.PositionX = gameplayLayer.VisibleBoundsWorldspace.Center.X;
			mPaddle.PositionY = 50;
			gameplayLayer.AddChild (mPaddle);

			mBall = new BallEntity (gameplayLayer);
			mBall.Position = gameplayLayer.VisibleBoundsWorldspace.Center;
			mBall.Visible = false;
			mBall.YVelocity = -350;
			gameplayLayer.AddChild  (mBall);
		}

		private void PerformActivity(float frameTimeInSeconds)
		{
			//Console.WriteLine ("GamePlay Schedule Running!!!!!!!!!");
			beginCount++;

			PerformDebug ();

			if (beginCountFinishFlag == false) {
			
				HandleBegin ();

			}
			else {
				
				if (!deathFlag) {

					mBall.PerformActivity (frameTimeInSeconds);

					PerformCollision ();

					PerformAccelerate ();

					PerformTiledMapScrollDown ();

					TestIfDeath ();

				} else {

					HandleDeath ();
				}
			}

			HandleBackKeyPress ();

		}

		void PerformDebug()
		{
			debugLabel1.Text = "Y velocity: " + mBall.YVelocity.ToString ();
			debugLabel2.Text = "X velocity: " + mBall.XVelocity.ToString ();

		}

		private void PerformAccelerate()
		{
//			if ((score % 10 == 9) && (score != 0))
//			{
//				mBall.AccelerateState = false;
//			}
//
//			if ((score % 10 == 0)&&(score!=0)&&(!mBall.AccelerateState)) 
//			{
//				mBall.XVelocity *= mData.getSpeedCoeff(1);
//				mBall.YVelocity *= mData.getSpeedCoeff(1);
//				mBall.AccelerateState = true;
//			}
		}

		private void PerformCollision()
		{
			PerformEnvironmentCollision ();

			PerformPaddleCollision ();

			PerformBricksCollision ();

		}

		void PerformEnvironmentCollision()
		{
			bool ifCollisionLeftRight = 
				(mBall.RightX > mData.screenRightX && mBall.XVelocity > 0) ||(mBall.LeftX < mData.screenLeftX && mBall.XVelocity < 0);

			if (ifCollisionLeftRight) {
				mBall.XVelocity *= -1;
				CCSimpleAudioEngine.SharedEngine.PlayEffect ("BallCollideLow.wav");
				if (mBall.XVelocity == 0) {
					mBall.XVelocity = 30;
				}
			}

			bool ifCollisionTop = (mBall.TopY >mData.screenTopY && mBall.YVelocity > 0);

			if (ifCollisionTop) {
				mBall.YVelocity *= -1;
				CCSimpleAudioEngine.SharedEngine.PlayEffect ("BallCollideLow.wav");
				if (mBall.XVelocity == 0) {
					mBall.XVelocity = 30;
				}
			}
				
		}

		void PerformPaddleCollision()
		{
			//和paddle
			bool doesBallOverlapPaddle;

			if (Math.Sqrt((mBall.PositionX-mPaddle.PositionX)*(mBall.PositionX-mPaddle.PositionX)+(mBall.PositionY-mPaddle.PositionY)*(mBall.PositionY-mPaddle.PositionY))<(mPaddle.flatBoundingBoxWidth/2+10)) 
			{
				if (Math.Abs (mPaddle.Angle) > 5) {
					doesBallOverlapPaddle = mBall.isCollideWithRotatedPaddle (mPaddle, mPaddle.Angle);
				} else {
					doesBallOverlapPaddle = mBall.Intersects (mPaddle);
				}

				bool isMovingDownward = mBall.YVelocity < 0;

				if (doesBallOverlapPaddle && isMovingDownward) {

//					if ((mBall.RightX < (mPaddle.LeftX + mData.paddleCornerDefine)) || (mBall.LeftX > (mPaddle.RightX - mData.paddleCornerDefine))) {
//						mBall.HandleCollisionWithPaddle (true,mPaddle.Angle);
//					} else {
//						mBall.HandleCollisionWithPaddle (false,mPaddle.Angle);
//					}
					mBall.HandleCollisionWithPaddle (false,mPaddle.Angle);
				}
			}

		}

		void PerformBricksCollision()
		{
			bool doesBallCollideBricks = false;

			int i;
			for (i = 0; i < mBrickList.Count; i++) 
			{
				doesBallCollideBricks = mBall.Intersects (mBrickList [i]);
				if (doesBallCollideBricks == true) {
					mBall.ReactToLevelCollision ();

					score += mBrickList [i].BrickScore;
					scoreLabel.Text = "Score: " + score.ToString ();
					CCSimpleAudioEngine.SharedEngine.PlayEffect ("BallCollideLow.wav");

					gameplayLayer.RemoveChild (mBrickList [i]);
					mBrickList.RemoveAt (i);
				}
			}
		}

		void PerformTiledMapScrollDown()
		{
			if ((beginCount % 100 == 0)&&(beginCount!=0)) {
			
				if (scrollCount % 5 == 0) {
					AddNewLevelAtTop ();
				}

				foreach (var brick in mBrickList) {
					brick.handleBrickScrollDown ();
					if (brick.PositionY < -5) {
						deathFlag = true;
					}
				}
				scrollCount++;
			}
		}

		void AddNewLevelAtTop()
		{
			int levelCount=randomBrickPositionGenerator ();

			for (int i = 0; i < levelCount; i++) {
				brick = new BrickEntity (randomBrickGenerator());
				mBrickList.Add (brick);
				brick.indexInBrickList = mBrickList.IndexOf (brick);

				brick.brickX = brickLevelPositionX[levelCount-i-1];
				brick.brickY = 768 + 8;
				brick.PositionX = brick.brickX;
				brick.PositionY = brick.brickY;

				gameplayLayer.AddChild (brick);
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
			gameplayLayer.RemoveChild (mBall);

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

