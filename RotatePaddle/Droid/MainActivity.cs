using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Media;
using Microsoft.Xna.Framework;

using CocosSharp;

namespace MyBouncingGame
{
	[Activity (
		Label = "BouncingGame",
		AlwaysRetainTaskState = true,
		Icon = "@drawable/icon",
		Theme = "@android:style/Theme.NoTitleBar",
		ScreenOrientation = ScreenOrientation.Landscape | ScreenOrientation.ReverseLandscape,
		LaunchMode = LaunchMode.SingleInstance,
		MainLauncher = true,
		ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)
	]
	public class MainActivity : AndroidGameActivity
	{
		CCApplication application;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			MyBouncingGame.GameAppDelegate.activity = this;

			this.VolumeControlStream = Android.Media.Stream.Music;

			application = new CCApplication ();
			application.ApplicationDelegate = new GameAppDelegate ();
			SetContentView (application.AndroidContentView);
			application.StartGame ();
		}
	}
}