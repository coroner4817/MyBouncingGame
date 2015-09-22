using System;
using CocosSharp;
using MyBouncingGame.Scenes;
using CocosDenshion;


namespace MyBouncingGame
{
	public class GameAppDelegate : CCApplicationDelegate
	{
		static CCDirector director;
		static CCWindow mainWindow;

		public static SplashScene sceneSplash;
		public static GameStartScene sceneGameStart;
		public static GamePlayScene sceneGamePlay;

		public override void ApplicationDidFinishLaunching (CCApplication application, CCWindow mainWindow)
		{
			GameAppDelegate.mainWindow = mainWindow;

			application.PreferMultiSampling = false;

			application.ContentRootDirectory = "Content";
			application.ContentSearchPaths.Add ("Entity");
			application.ContentSearchPaths.Add ("fonts");
			application.ContentSearchPaths.Add ("images");
			application.ContentSearchPaths.Add ("Sound");
			application.ContentSearchPaths.Add ("ViewsImage");

	
			float desiredHeight = 768.0f;
			float desiredWidth = 1024.0f;
			CCScene.SetDefaultDesignResolution (desiredWidth, desiredHeight, CCSceneResolutionPolicy.ShowAll);

			CCSimpleAudioEngine.SharedEngine.PlayBackgroundMusic ("backgroundMusic.wav", true);


			director = new CCDirector ();
			mainWindow.AddSceneDirector (director);

			var scene = new SplashScene (mainWindow);
			director.RunWithScene (scene);

			scene.PerformSplash ();
		}

		//返回键把游戏最小化
		public override void ApplicationDidEnterBackground (CCApplication application)
		{
			application.Paused = true;

			CCSimpleAudioEngine.SharedEngine.PauseBackgroundMusic ();
		}

		public override void ApplicationWillEnterForeground (CCApplication application)
		{
			application.Paused = false;

			CCSimpleAudioEngine.SharedEngine.ResumeBackgroundMusic ();
		}

		public static void GoToGameStartPage()
		{
			var scene = new GameStartScene (mainWindow);
			director.ReplaceScene (new CCTransitionFade(1.5f,scene));

			scene.CheckIfBackPressed ();

		}
			
		public static void GoToGameScene()
		{
			var scene = new GamePlayScene (mainWindow);
			director.ReplaceScene (new CCTransitionFade(1.5f, scene));
		}

		public static void EndGame()
		{
			mainWindow.EndAllSceneDirectors ();
		}
	}
}