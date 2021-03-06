﻿using System;
using CocosSharp;

namespace MyBouncingGame.Data
{
	public class ConstantData
	{
		CCLayer mLayer;

		const float gravity=150;
		
		public ConstantData (CCLayer layer)
		{
			this.mLayer = layer;
		}

		public ConstantData ()
		{
			
		}

		public float screenLeftX
		{
			get
			{
				return mLayer.VisibleBoundsWorldspace.MinX;
			}
		}

		public float screenRightX
		{
			get
			{
				return mLayer.VisibleBoundsWorldspace.MaxX;
			}
		}

		public float screenTopY
		{
			get
			{
				return mLayer.VisibleBoundsWorldspace.MaxY;
			}
		}

		public float screenBottomY
		{
			get
			{
				return mLayer.VisibleBoundsWorldspace.MinY;
			}
		}

		public float getGravity(int level)
		{
			return (gravity + level * 30.0f);
		}

		public float getSpeedCoeff(int level)
		{
			return (1.0f + level * 0.5f);
		}

		public float paddleCornerDefine
		{
			get
			{
				return 30.0f;
			}
		}

	}
}

