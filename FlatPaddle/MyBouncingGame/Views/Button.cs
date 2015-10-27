using System;
using System.Linq;
using CocosSharp;
using System.Collections.Generic;

namespace MyBouncingGame.Views
{
	public enum ButtonStyle
	{
		levelSelectBtn,
		confirmBtn1,
		confirmBtn2
	}

	public class Button : CCNode
	{
		ButtonStyle buttonStyle;

		CCSprite sprite;
		CCLabel label;
		CCLayer ownerLayer;

		string btntext;

		public event EventHandler Clicked;
		CCEventListenerTouchAllAtOnce touchListener;

		public ButtonStyle ButtonStyle
		{
			get
			{
				return buttonStyle;
			}
			set
			{
				buttonStyle = value;
				switch (buttonStyle)
				{
				case ButtonStyle.confirmBtn1:
					sprite.Texture = new CCTexture2D ("ViewsImage/btnBase.png");
					sprite.IsAntialiased = false;
					sprite.FlipX = false;
					break;

				case ButtonStyle.confirmBtn2:
					sprite.Texture = new CCTexture2D ("ViewsImage/btnBase2.png");
					sprite.IsAntialiased = false;
					sprite.FlipX = false;
					break;

				case ButtonStyle.levelSelectBtn:
					sprite.Texture = new CCTexture2D ("ViewsImage/levelBtnBase.png");
					sprite.IsAntialiased = false;
					sprite.FlipX = false;
					break;
				}

				sprite.TextureRectInPixels = new CCRect (0, 0, sprite.Texture.PixelsWide, sprite.Texture.PixelsHigh);
			}
		}

		public string btnText
		{
			get
			{
				return btntext;
			}
			set
			{
				btntext = value;
				label.Text = btntext.ToString ();
			}
		}

		public Button(CCLayer layer)
		{
			ownerLayer = layer;

			// Give it a default texture, may get changed in ButtonStyle
			sprite = new CCSprite ("ViewsImage/btnBase.png");
			sprite.IsAntialiased = false;
			this.AddChild (sprite);

			label = new CCLabel("", "fonts/go3v2.ttf", 40, CCLabelFormat.SystemFont);
			label.IsAntialiased = false;
			this.AddChild (label);

			//touch event
			touchListener = new CCEventListenerTouchAllAtOnce ();
			touchListener.OnTouchesBegan = HandleTouchesBegan;

			ownerLayer.AddEventListener (touchListener);
		}

		private void HandleTouchesBegan(List<CCTouch> touches, CCEvent touchEvent)
		{
			if (this.Visible)
			{
				// did the user actually click within the CCSprite bounds?
				//get the first touch
				var firstTouch = touches.FirstOrDefault ();

				if (firstTouch != null)
				{
					//if the touch is inside the button
					bool isTouchInside = sprite.BoundingBoxTransformedToWorld.ContainsPoint (firstTouch.Location);

					if (isTouchInside && Clicked != null)
					{
						//do Clicked
						//Clicked对应在调用button的类里面定义的方法
						//this 是把这个按钮的类传递过去, e 是传递一些参数
						Clicked (this, null);
					}
				}
			}
		}

		public void RemoveBtnTouchListener()
		{
			ownerLayer.RemoveEventListener (touchListener);
		}
	}
}

