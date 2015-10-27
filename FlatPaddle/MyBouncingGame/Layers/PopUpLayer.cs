using System;
using CocosSharp;
using System.Collections.Generic;
using MyBouncingGame.Views;

namespace MyBouncingGame.Layers
{
	public class PopUpLayer:CCLayer
	{
		CCLabel mTitle;
		CCSprite mBackgroundImage;
		Button mConfirmBtn;

		public PopUpLayer (CCSprite background, CCLabel title)
		{
			mBackgroundImage = background;
			mTitle = title;

			CreateLayer ();

		}
			
		private void CreateLayer()
		{
			mBackgroundImage.PositionX = ContentSize.Center.X;
			mBackgroundImage.PositionY = ContentSize.Center.Y;
			mBackgroundImage.IsAntialiased = false;
			this.AddChild (mBackgroundImage);

			mTitle.PositionX = mBackgroundImage.VisibleBoundsWorldspace.Center.X;
			mTitle.PositionY = mBackgroundImage.VisibleBoundsWorldspace.Center.Y + 250;
			mTitle.IsAntialiased = false;
			this.AddChild (mTitle);

			mConfirmBtn = new Button (this);
			mConfirmBtn.ButtonStyle = ButtonStyle.confirmBtn1;
			mConfirmBtn.btnText = "OK";
			mConfirmBtn.PositionX = mBackgroundImage.VisibleBoundsWorldspace.Center.X;
			mConfirmBtn.PositionY = mBackgroundImage.VisibleBoundsWorldspace.Center.Y - 250;
			mConfirmBtn.Clicked += BtnCallBack;
			this.AddChild (mConfirmBtn);

		}

		private void BtnCallBack(object sender, EventArgs args)
		{
			mConfirmBtn.RemoveBtnTouchListener ();
			this.RemoveFromParent();
		}
			
	}
}

