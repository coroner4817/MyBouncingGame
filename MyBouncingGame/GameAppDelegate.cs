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

		int i=0;
		
		public static SplashScene sceneSplash;
		public static GameStartScene sceneGameStart;
		public static GamePlayScene sceneGamePlay;

		int i = 0;

		#if __ANDROID__
		public static Android.App.Activity activity { get; set;}
		#endif

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


			director = new CCDirector ();
			mainWindow.AddSceneDirector (director);

			var scene = new SplashScene (mainWindow);
			director.RunWithScene (scene);

			scene.PerformSplash ();

			CCSimpleAudioEngine.SharedEngine.PlayBackgroundMusic ("SplashBackMusic.wav",false);

			CCSimpleAudioEngine.SharedEngine.PreloadEffect ("BallCollideHigh.wav");
			CCSimpleAudioEngine.SharedEngine.PreloadEffect ("BallCollideLow.wav");

		}
			
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
			CCSimpleAudioEngine.SharedEngine.StopBackgroundMusic (true);
			var scene = new GameStartScene (mainWindow);
			director.ReplaceScene (new CCTransitionFade(1.5f,scene));

			CCSimpleAudioEngine.SharedEngine.PlayBackgroundMusic ("backgroundMusic.wav", true);

			scene.PerformActivity ();
		}
			
		public static void GoToGameScene()
		{
			CCSimpleAudioEngine.SharedEngine.StopBackgroundMusic (true);
			var scene = new GamePlayScene (mainWindow);
			director.ReplaceScene (new CCTransitionFade(1.5f, scene));
			CCSimpleAudioEngine.SharedEngine.PlayBackgroundMusic ("GameBackgroundMusic.wav", true);
		}

		public static void EndGame()
		{
			if (activity != null) {
				activity.MoveTaskToBack (true);
			}
		}
	}
}