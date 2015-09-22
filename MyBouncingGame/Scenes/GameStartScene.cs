using System;
using CocosSharp;
using MyBouncingGame.Views;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace MyBouncingGame.Scenes
{
	public class GameStartScene : CCScene
	{
		CCLayer gameStartLayer;

		CCLabel gameStartTittle;
		CCSprite gameStartBackground;

		Button goToLevelSceneBtn;
		Button goToSettingBtn;

		public GameStartScene (CCWindow mainWindow) : base(mainWindow)
		{
			CreateLayer ();

			addBackgroundLabel ();

			CreateButton ();
		}

		private void CreateLayer()
		{
			gameStartLayer = new CCLayer ();
			this.AddChild (gameStartLayer);
		}

		private void addBackgroundLabel()
		{
			gameStartBackground = new CCSprite ("images/GameStartSceneBackground.png");
			gameStartBackground.PositionX = ContentSize.Center.X;
			gameStartBackground.PositionY = ContentSize.Center.Y + 150.0f;
			gameStartBackground.IsAntialiased = false;
			gameStartLayer.AddChild (gameStartBackground);

			gameStartTittle = new CCLabel ("Bouncing King", "fonts/go3v2.ttf", 80, CCLabelFormat.SystemFont);
			gameStartTittle.Color = new CCColor3B (0, 0, 0);
			gameStartTittle.PositionX = gameStartLayer.VisibleBoundsWorldspace.Center.X;
			gameStartTittle.PositionY = gameStartLayer.VisibleBoundsWorldspace.Center.Y + 120.0f;
			gameStartLayer.AddChild (gameStartTittle);
		}

		private void CreateButton()
		{
			goToLevelSceneBtn = new Button (gameStartLayer);
			goToLevelSceneBtn.ButtonStyle = ButtonStyle.confirmBtn1;
			goToLevelSceneBtn.btnText = "Start";
			goToLevelSceneBtn.PositionX = gameStartLayer.VisibleBoundsWorldspace.UpperRight.X - 200.0f;
			goToLevelSceneBtn.PositionY = gameStartLayer.VisibleBoundsWorldspace.LowerLeft.Y + 230.0f;

			//goToLevelSceneBtn.Position = gameStartLayer.VisibleBoundsWorldspace.Center;
			goToLevelSceneBtn.Clicked += goToLevelSelectScene;
			gameStartLayer.AddChild(goToLevelSceneBtn);

			goToSettingBtn = new Button (gameStartLayer);
			goToSettingBtn.ButtonStyle = ButtonStyle.confirmBtn2;
			goToSettingBtn.btnText="Setting";
			goToSettingBtn.PositionX = gameStartLayer.VisibleBoundsWorldspace.UpperRight.X - 200.0f;
			goToSettingBtn.PositionY = gameStartLayer.VisibleBoundsWorldspace.LowerLeft.Y + 100.0f;

			//goToSettingBtn.Position = gameStartLayer.VisibleBoundsWorldspace.Center;
			goToSettingBtn.Clicked += goToSettingScene;
			gameStartLayer.AddChild(goToSettingBtn);
		}

		private void goToLevelSelectScene(object sender, EventArgs args)
		{
			gameStartLayer.RemoveAllChildren (true);

			goToLevelSceneBtn.RemoveBtnTouchListener ();
			goToSettingBtn.RemoveBtnTouchListener ();

			gameStartLayer.RunAction (new CCFadeOut (1.5f));

			Unschedule (HandleBackKeyPress);
			GameAppDelegate.GoToGameScene ();
		}

		private void goToSettingScene(object sender, EventArgs args)
		{
			gameStartLayer.RemoveAllChildren (true);

			goToLevelSceneBtn.RemoveBtnTouchListener ();
			goToSettingBtn.RemoveBtnTouchListener ();

			//GameAppDelegate.GoToGameScene ();
		}

		public void CheckIfBackPressed()
		{
			Schedule (HandleBackKeyPress);
		}

		private void HandleBackKeyPress(float seconds)
		{
			Console.WriteLine ("GameStart Schedule Running!!!!!!!!!");
			if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
			{
				Unschedule (HandleBackKeyPress);
				//GameAppDelegate.EndGame ();
			}
		}
			
	}
}

