using System;
using CocosSharp;
using MyBouncingGame.Scenes;


namespace MyBouncingGame
{
	public class GameAppDelegate : CCApplicationDelegate
	{
		static CCDirector director;
		static CCWindow mainWindow;

		public override void ApplicationDidFinishLaunching (CCApplication application, CCWindow mainWindow)
		{
			GameAppDelegate.mainWindow = mainWindow;

			application.PreferMultiSampling = false;

			application.ContentRootDirectory = "Content";
			application.ContentSearchPaths.Add ("fonts");
			application.ContentSearchPaths.Add ("images");
	
			director = new CCDirector ();

			CCSize windowSize = mainWindow.WindowSizeInPixels;
	
			float desiredHeight = 768.0f;
			float desiredWidth = 768.0f;


			CCScene.SetDefaultDesignResolution (desiredWidth, desiredHeight, CCSceneResolutionPolicy.ShowAll);

			mainWindow.AddSceneDirector (director);

			var scene = new GamePlaySceneTest (mainWindow);
			director.RunWithScene (scene);

		}

		//返回键把游戏最小化
		public override void ApplicationDidEnterBackground (CCApplication application)
		{
			application.Paused = true;
		}

		public override void ApplicationWillEnterForeground (CCApplication application)
		{
			application.Paused = false;
		}
			
		public static void GoToGameScene()
		{
			var scene = new GamePlaySceneTest (mainWindow);
			director.ReplaceScene (scene);
		}
	}
}