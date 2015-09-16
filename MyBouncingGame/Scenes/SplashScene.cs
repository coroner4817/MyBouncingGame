using System;
using CocosSharp;
using System.Threading;

namespace MyBouncingGame.Scenes
{
	public class SplashScene : CCScene
	{
		CCSprite splashImage;
		CCLayer splashLayer;

		int timeCount=0;

		public SplashScene (CCWindow mainWindow) : base(mainWindow)
		{
			splashLayer = new CCLayer ();
			this.AddChild (splashLayer);

			splashImage = new CCSprite ("Splash1.png");
			splashImage.Position = ContentSize.Center;
			splashImage.IsAntialiased = false;
			splashImage.Opacity = 0;
		}

		public void PerformSplash()
		{
			timeCount = 0;
			splashLayer.AddChild (splashImage);	

			Schedule (Splash);
		}

		private void Splash(float seconds)
		{
			timeCount++;

			splashImage.Opacity = getOpacity (timeCount);

			if (timeCount == 256) {
				splashLayer.RemoveChild (splashImage);
				splashImage = new CCSprite ("Splash2.png");
				splashImage.Position = ContentSize.Center;
				splashImage.IsAntialiased = false;
				splashImage.Opacity = 0;
				splashLayer.AddChild (splashImage);
			}
			else if (timeCount == 512) {
				
				splashLayer.RemoveChild (splashImage);

				GameAppDelegate.GoToGameScene ();
			}
		}

		private byte getOpacity(int count)
		{
			if (count < 128) {
				return (byte)((1.98) * count);
			} else if ((count >= 128) && (count < 256)) {
				return (byte)((-1.98) * count + 507);
			} else if ((count >= 256) && (count < 384)) {
				return (byte)(1.98 * count - 506);
			} else if ((count >= 384) && (count < 512)) {
				return (byte)((-1.98) * count + 1014);
			} else {
				return (byte)0;
			}
				
		}
	}
}

