using System;
using System.Collections.Generic;
using CocosSharp;
using MyBouncingGame.TiledMapClass;
using MyBouncingGame.Entity;
using System.Linq;

namespace MyBouncingGame.Util
{
	public class LevelCollision
	{

		//collision 是游戏地图中所有的固体部分 是player不可以进入的部分

		int tileDimensionWidth;
		int tileDimensionHeight;

		List<RectWithDirection> collisions;

		CCTileMapLayer mBrickLayer;

		public void PopulateFrom(CCTileMap tileMap)
		{
			//得到瓦片地图里面的瓦片信息，比如说哪些瓦片可以与entity接触，具体是瓦片的哪个方向的面可以接触

			collisions=new List<RectWithDirection> ();

			//每个小瓦片的边长加上0.5，这样碰撞时就不会陷进去才弹出
			tileDimensionWidth = (int)(tileMap.TileTexelSize.Width + .5f);
			tileDimensionHeight = (int)(tileMap.TileTexelSize.Height + .5f);
			mBrickLayer = tileMap.LayerNamed ("BrickLayer");

			TileMapPropertyFinder finder = new TileMapPropertyFinder (tileMap);


			foreach (var propertyLocation in finder.GetPropertyLocations())
			{
				//如果在这个位置的瓦片是一个固体
				if (propertyLocation.Properties.ContainsKey ("SolidCollision"))
				{
					//worldX worldY 是每个瓦片的中心的坐标
					float centerX = propertyLocation.WorldX;
					float centerY = propertyLocation.WorldY;

					//得到每个小瓦片的左边界和下边界
					float left = centerX - tileDimensionWidth/2.0f;
					float bottom = centerY - tileDimensionHeight/2.0f;

					//在那个点构造一个小瓦片
					RectWithDirection rectangle = new RectWithDirection {
						Left = left,
						Bottom = bottom,
						Width = tileDimensionWidth, 
						Height = tileDimensionHeight,
						tiledMapCoordinate=propertyLocation.TileCoordinates
							
					};

					//得到地图上所有的固体小块
					collisions.Add (rectangle);
				}
			}

			// Sort by XAxis to speed future searches:
			//collisions是把整个瓦片地图按每个瓦片的左边的坐标进行排序的list
			//在每一个左边坐标有一列的瓦片
			//debug看一下
			collisions = collisions.OrderBy(item=>item.Left).ToList();

			// now let's adjust the directions that these point
			//调整每个小块的角度

			for (int i = 0; i < collisions.Count; i++)
			{
				var rect = collisions [i];

				// By default rectangles can reposition objects in all directions:
				int valueToAssign = (int)Directions.All; //15

				float centerX = rect.CenterX;
				float centerY = rect.CenterY;

				// If there are collisions on the sides, then this 
				// rectangle can no longer repositon objects in that direction.
				// 一开始小瓦片的方向valueToAssign可能是所有的方向，每次减去一种不可能的方向值，最后得到的就是正确的方向的值的和
				//direction是这个实体瓦片暴露在外，可以与entity接触的方向
				//比如说瓦片地图里的地面，方向就为up
				if (HasCollisionAt (centerX - tileDimensionWidth, centerY))
				{
					valueToAssign -= (int)Directions.Left;
				}
				if (HasCollisionAt (centerX + tileDimensionWidth, centerY))
				{
					valueToAssign -= (int)Directions.Right;
				}
				if (HasCollisionAt (centerX, centerY + tileDimensionHeight))
				{
					valueToAssign -= (int)Directions.Up;
				}
				if (HasCollisionAt (centerX, centerY - tileDimensionHeight))
				{
					valueToAssign -= (int)Directions.Down;
				}

				rect.Directions = (Directions)valueToAssign;
				//更新瓦片列表中的瓦片的方向
				collisions [i] = rect;
			}

			for (int i = collisions.Count - 1; i > -1; i--)
			{
				//经过筛选后，遍历删除那些没有方向的瓦片
				if (collisions [i].Directions == Directions.None)
				{
					collisions.RemoveAt (i);
				}
			}
		}

		int GetFirstAfter(float value)
		{
			//最后返回以这个value为左边边长的瓦片在list中的index

			int lowBoundIndex = 0;
			int highBoundIndex = collisions.Count;

			//总瓦片数为0，出错
			if (lowBoundIndex == highBoundIndex)
			{
				return lowBoundIndex;
			}

			// We want it inclusive
			//list从0开始，所以要减1
			highBoundIndex -= 1;
			int current = 0;  

			while (true)
			{
				// >>1 相当于除以2  二分法得到collisions列表的中点
				current = (lowBoundIndex + highBoundIndex) >> 1;
				// .Left是左边的坐标
				//二分法不断逼近，直到相差小于2
				if (highBoundIndex - lowBoundIndex < 2)
				{
					//二分法的边界条件
					if (collisions[highBoundIndex].Left <= value)
					{
						return highBoundIndex + 1;
					}
					else if (collisions[lowBoundIndex].Left <= value)
					{
						return lowBoundIndex + 1;
					}
					else if (collisions[lowBoundIndex].Left > value)
					{
						return lowBoundIndex;
					}
				}

				//二分法不断更新边界
				if (collisions[current].Left >= value)
				{
					highBoundIndex = current;
				}
				else if (collisions[current].Left < value)
				{
					lowBoundIndex = current;
				}
			}
		}

		bool HasCollisionAt(float worldX, float worldY)
		{
			int leftIndex;
			int rightIndex;

			//如果是判断是不是向左时，GetIndicesBetween的前两个参数就是这个瓦片的中心X坐标和左数两个的瓦片的中心X坐标，其他方向类似
			//leftIndex是左数两个的瓦片在collisions中的index
			//rightIndex是这个瓦片在collisions中的index
			//这样生成了一个三个小瓦片组成的瓦片系
			GetIndicesBetween (worldX - tileDimensionWidth, worldX + tileDimensionWidth, out leftIndex, out rightIndex);

			//遍历这个瓦片系
			for (int i = leftIndex; i < rightIndex; i++)
			{
				//如果瓦片系中的一个瓦片包含中间瓦片的中心点，则瓦片不是所要判断的方向
				if (collisions [i].ContainsPoint (worldX, worldY))
				{
					return true;
				}
			}
			return false;
		}

		void GetIndicesBetween(float leftX, float rightX, out int leftIndex, out int rightIndex)
		{
			//leftAdjusted是左数第二个的瓦片的左边的X坐标
			float leftAdjusted = tileDimensionWidth * (((int)leftX) / tileDimensionWidth) - tileDimensionWidth/2; 
			//rightAdjusted是这个瓦片的右边的X坐标
			float rightAdjusted = tileDimensionWidth * (((int)rightX) / tileDimensionWidth) + tileDimensionWidth/2; 

			leftIndex = GetFirstAfter (leftAdjusted);
			rightIndex = GetFirstAfter (rightAdjusted);
		}

		public void PerformCollisionAgainst(PhysicsEntity entity, out CCTileMapCoordinates tileAtXy, out bool didCollisionOccur)
		{
			didCollisionOccur = false;
			tileAtXy = CCTileMapCoordinates.Zero;


			int leftIndex;
			int rightIndex;

			int directionCount = 0;
			//entity是player/enemy
			//boundiongBox是包裹entity的最小的矩形
			//lowerLeft是左下角，UpperRight是右上角
			//得到的leftIndex和rightIndex应该是和entity有接触的瓦片地图的实体瓦片的list
			//例如，如果entity在地面上，则这个list里面应该只有和entity接触的地面瓦片
			//然后进一步判断这个地面瓦片对于entity的作用（地面就是支持entity）
			GetIndicesBetween (entity.LeftX, entity.RightX, out leftIndex, out rightIndex);

			var boundingBoxWorld = entity.BoundingBoxTransformedToWorld;

			//遍历所有和entity有接触的瓦片，来判断这些瓦片对于entity的物理作用
			for (int i = leftIndex; i < rightIndex; i++)
			{
				//计算得到这个瓦片和entity接触后对entity的作用力产生的运动vector
				//把ball从砖块里弹出来
				var separatingVector = GetSeparatingVector (boundingBoxWorld, collisions [i]);

				//如果player和瓦片地图中的不可进入的瓦片相碰
				for (directionCount = 0; directionCount < 4; directionCount++) 
				{
					if (separatingVector[directionCount] != CCVector2.Zero)
					{
						//更新entity的位置
						entity.PositionX += separatingVector[directionCount].X;
						entity.PositionY += separatingVector[directionCount].Y;
						// refresh boundingBoxWorld:
						boundingBoxWorld = entity.BoundingBoxTransformedToWorld;

						didCollisionOccur = true;

						tileAtXy = collisions [i].tiledMapCoordinate;
					}
				}
					
			}

		}


		List<CCVector2> GetSeparatingVector(CCRect first, RectWithDirection second)
		{
			//返回一个向量，是player在碰撞之后反方向移动的向量

			List<CCVector2> separation = new List<CCVector2> {
				CCVector2.Zero,
				CCVector2.Zero,
				CCVector2.Zero,
				CCVector2.Zero
			};

			// Only calculate separation if the rectangles intersect
			if (Intersects(first, second))
			{
				// The intersectionRect returns the rectangle produced
				// by overlapping the two rectangles.
				// This is protected by partitioning and deep collision, so it
				// won't happen too often - it's okay to do a ToRect here
				//得到两个rect重叠部分的rect
				var intersectionRect = first.Intersection (second.ToRect());

				float minDistance = float.PositiveInfinity;

				float firstCenterX = first.Center.X;
				float firstCenterY = first.Center.Y;

				float secondCenterX = second.Left + second.Width / 2.0f;
				float secondCenterY = second.Bottom + second.Height / 2.0f;

				//second的方向和想要判断的方向（左）取交集，如果和想要判断的方向（左）一致，且第一个的中心点在第二个的中心点的（左边）
				//则当碰撞时，player可以向想要的方向移动
				bool canMoveLeft = (second.Directions & Directions.Left) == Directions.Left && firstCenterX < secondCenterX;
				bool canMoveRight = (second.Directions & Directions.Right) == Directions.Right && firstCenterX > secondCenterX;
				bool canMoveDown = (second.Directions & Directions.Down) == Directions.Down && firstCenterY < secondCenterY;
				bool canMoveUp = (second.Directions & Directions.Up) == Directions.Up && firstCenterY > secondCenterY;

				if (canMoveLeft)
				{
					//左重叠
					//得到重叠的rect的X方向边长
					float candidate = first.UpperRight.X - second.Left;

					if (candidate > 0)
					{
						minDistance = candidate;
						//x方向移动回到地图实体瓦片的外面，y方向不动
						separation [0] = new CCVector2 (-minDistance, 0);
					}

				}

				if (canMoveRight)
				{
					//右重叠
					float candidate = (second.Left + second.Width) - first.LowerLeft.X;

					if (candidate > 0)
					{
						minDistance = candidate;
						//向右移动
						separation [1] = new CCVector2 (minDistance, 0);
					}

				}

				//其他方向同理
				if (canMoveUp)
				{
					float candidate = (second.Bottom + second.Height) - first.Origin.Y;

					if (candidate > 0)
					{
						minDistance = candidate;
						separation [2] = new CCVector2 (0, minDistance);
					}

				}

				if (canMoveDown)
				{
					float candidate = first.UpperRight.Y - second.Bottom;

					if (candidate > 0)
					{
						minDistance = candidate;
						separation [3] = new CCVector2 (0, -minDistance);
					}

				}
	

				if ((intersectionRect.UpperRight.X > (second.Left + first.Size.Width))
				   && (intersectionRect.LowerLeft.X < (second.Left + second.Width - first.Size.Width))) {
					separation [0] = new CCVector2 (0, 0);
					separation [1] = new CCVector2 (0, 0);
				}


			}

			//左后返回player移动的vector
			return separation;
		}

		bool Intersects(CCRect first, RectWithDirection second)
		{
			//判断两个rect是否有重叠
			return first.UpperRight.X > second.Left &&
				first.LowerLeft.X < second.Left + second.Width &&
				first.UpperRight.Y > second.Bottom &&
				first.LowerLeft.Y < second.Bottom + second.Height;

		}
	}
}
