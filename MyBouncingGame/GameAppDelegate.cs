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
			float desiredWidth = 1024.0f;


			CCScene.SetDefaultDesignResolution (desiredWidth, desiredHeight, CCSceneResolutionPolicy.ShowAll);

			mainWindow.AddSceneDirector (director);

			var scene = new SplashScene (mainWindow);
			director.RunWithScene (scene);

			scene.PerformSplash ();
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

		public static void GoToGameStartPage()
		{
			var scene = new GameStartScene (mainWindow);
			director.ReplaceScene (new CCTransitionFade(1.5f,scene));
		}
			
		public static void GoToGameScene()
		{
			var scene = new GamePlayScene (mainWindow);
			director.ReplaceScene (new CCTransitionFade(1.5f, scene));
		}
	}
}