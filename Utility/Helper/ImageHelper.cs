using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Utility
{
	/// <summary>
	/// 图片公共操作类
	/// </summary>
	public sealed class ImageHelper
	{

		#region 构造实例

		/// <summary>
		/// 私有默认构造函数，禁止用户使用new()创建对象
		/// </summary>
		private ImageHelper()
		{
		}

		/// <summary>
		/// 构建类的实例对象
		/// </summary>
		public static ImageHelper Instance
		{
			get
			{
				return LazyClass.instance;
			}
		}

		/// <summary>
		/// 内部加载类
		/// </summary>
		private class LazyClass
		{
			internal static readonly ImageHelper instance = new ImageHelper();
		}

		#endregion 构造实例

		#region 公共枚举

		#region ThumbnailMode enum

		/// <summary>
		/// 缩略图的模式
		/// </summary>
		public enum ThumbnailMode
		{
			/// <summary>
			/// 指定高宽缩放(可能变形)
			/// </summary>
			WH,

			/// <summary>
			/// 指定宽，高按比例
			/// </summary>
			W,

			/// <summary>
			/// 指定高，宽按比例
			/// </summary>
			H,

			/// <summary>
			/// 指定高宽裁减(不变形)
			/// </summary>
			Cut,

			/// <summary>
			/// 等比缩放(不变形，如果高大按高，宽大按宽缩放)
			/// </summary>
			DB
		}

		#endregion ThumbnailMode enum

		#region ThumbnailType enum

		/// <summary>
		/// 缩略图保存格式
		/// </summary>
		public enum ThumbnailType
		{
			/// <summary>
			/// JPG格式
			/// </summary>
			JPG,

			/// <summary>
			/// BMP格式
			/// </summary>
			BMP,

			/// <summary>
			/// GIF格式
			/// </summary>
			GIF,

			/// <summary>
			/// PNG格式
			/// </summary>
			PNG
		}

		#endregion ThumbnailType enum

		#region WatermarkPosition enum

		/// <summary>
		/// 水印位置
		/// </summary>
		public enum WatermarkPosition
		{
			/// <summary>
			/// 水印放置左上角
			/// </summary>
			LeftTop = 1,

			/// <summary>
			/// 水平居中垂直顶部
			/// </summary>
			CenterTop = 2,

			/// <summary>
			/// 右上角
			/// </summary>
			RightTop = 3,

			/// <summary>
			/// 垂直居中水平靠左
			/// </summary>
			LeftMiddle = 4,

			/// <summary>
			/// 居中
			/// </summary>
			CenterMiddle = 5,

			/// <summary>
			/// 垂直居中水平靠右
			/// </summary>
			RightMiddle = 6,

			/// <summary>
			/// 左下角
			/// </summary>
			LeftBottom = 7,

			/// <summary>
			/// 水平居中垂直底部
			/// </summary>
			CenterBottom = 8,

			/// <summary>
			/// 右下角
			/// </summary>
			RightBottom = 9,
		}

		#endregion WatermarkPosition enum

		#region CompressType enum
		/// <summary>
		/// 指定缩放类型
		/// </summary>
		public enum CompressType
		{
			/// <summary>
			/// 无
			/// </summary>
			None = 0,

			/// <summary>
			/// 指定高宽缩放（可能变形）
			/// </summary>
			WidthAndHeight = 1,

			/// <summary>
			/// 指定宽，高按比例
			/// </summary>
			Width = 2,

			/// <summary>
			/// 指定高，宽按比例
			/// </summary>
			Height = 3,

			/// <summary>
			/// 指定高宽裁减（不变形）
			/// </summary>
			Cut = 4,

			/// <summary>
			/// 按照宽度成比例缩放后，按照指定的高度进行裁剪
			/// </summary>
			WidthAndHeightCut = 5,

			/// <summary>
			/// 自动缩放（不变形）
			/// </summary>
			Zoom = 6,
		}
		#endregion

		#endregion 公共枚举

		#region 公共方法

		/// <summary>
		/// 字体号数数组
		/// </summary>
		private readonly int[] sizes = new[] { 48,32,16,8,6,4 };

		#region 无损压缩图片
		/// <summary>
		/// 无损压缩图片
		/// </summary>
		/// <param name="sFile">原图片</param>
		/// <param name="dFile">压缩后保存位置</param>
		/// <param name="quality">压缩质量 1-100</param>
		/// <returns></returns>

		public static bool Thumb(string sFile,string dFile,int flag)
		{
			System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);
			int dHeight = iSource.Height;
			int dWidth = iSource.Width;
			ImageFormat tFormat = iSource.RawFormat;
			int sW = 0, sH = 0;
			//按比例缩放
			Size tem_size = new Size(iSource.Width,iSource.Height);
			if(tem_size.Width > dHeight || tem_size.Width > dWidth) //将**改成c#中的或者操作符号
			{
				if((tem_size.Width * dHeight) > (tem_size.Height * dWidth))
				{
					sW = dWidth;
					sH = (dWidth * tem_size.Height) / tem_size.Width;
				}
				else
				{
					sH = dHeight;
					sW = (tem_size.Width * dHeight) / tem_size.Height;
				}
			}
			else
			{
				sW = tem_size.Width;
				sH = tem_size.Height;
			}
			Bitmap ob = new Bitmap(dWidth,dHeight);
			Graphics g = Graphics.FromImage(ob);
			g.Clear(Color.Transparent);
			g.CompositingQuality = CompositingQuality.HighQuality;
			g.SmoothingMode = SmoothingMode.HighQuality;
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			g.DrawImage(iSource,new Rectangle((dWidth - sW) / 2,(dHeight - sH) / 2,sW,sH),0,0,iSource.Width,iSource.Height,GraphicsUnit.Pixel);
			g.Dispose();
			//以下代码为保存图片时，设置压缩质量
			EncoderParameters ep = new EncoderParameters();
			long[] qy = new long[1];
			qy[0] = flag;//设置压缩的比例1-100
			EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality,qy);
			ep.Param[0] = eParam;
			try
			{
				string extension = Path.GetExtension(sFile);
				ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
				ImageCodecInfo jpegICIinfo = null;
				for(int x = 0;x < arrayICI.Length;x++)
				{
					if(arrayICI[x].FormatDescription.Equals(extension.Substring(1).ToUpper()))
					{
						jpegICIinfo = arrayICI[x];
						break;
					}
				}
				if(jpegICIinfo != null)
				{
					ob.Save(dFile,jpegICIinfo,ep);//dFile是压缩后的新路径
				}
				else
				{
					ob.Save(dFile,tFormat);
				}
				return true;
			}
			catch
			{
				return false;
			}
			finally
			{
				iSource.Dispose();
				ob.Dispose();
			}
		}
		/// <summary>
		/// 无损压缩图片
		/// </summary>
		/// <param name="fromFilePath">原图片</param>
		/// <param name="toFilePath"></param>
		/// <param name="width">宽度</param>
		/// <param name="height">高度</param>
		/// <param name="type">压缩缩放类型</param>
		/// <param name="quality">图片质量</param>
		/// <returns></returns>
		public static Bitmap Thumb(string fromFilePath,string toFilePath,int width,int height,CompressType type,int quality = 100)
		{
			using(Image fromFile = Image.FromFile(fromFilePath))
			{
				//缩放后的宽度和高度
				float toWidth = width;
				float toHeight = height;

				float x = 0;
				float y = 0;
				float oWidth = fromFile.Width;
				float oHeight = fromFile.Height;

				switch(type)
				{
					case CompressType.WidthAndHeight: //指定高宽缩放（可能变形）            
						{
							break;
						}
					case CompressType.Width: //指定宽，高按比例      
						{
							toHeight = oHeight * width / oWidth;
							break;
						}
					case CompressType.Height: //指定高，宽按比例
						{
							toWidth = oWidth * height / oHeight;
							break;
						}
					case CompressType.Cut: //指定高宽裁减（不变形）      
						{
							if(oWidth / oHeight > toWidth / toHeight)
							{
								oWidth = oHeight * toWidth / toHeight;
								y = 0;
								x = (oWidth - oWidth) / 2;
							}
							else
							{
								oHeight = oWidth * height / toWidth;
								x = 0;
								y = (oHeight - oHeight) / 2;
							}
							break;
						}
					case CompressType.WidthAndHeightCut: //按照宽度成比例缩放后，按照指定的高度进行裁剪
						{
							toHeight = oHeight * width / oWidth;
							if(height < toHeight)
							{
								oHeight = oHeight * height / toHeight;
								toHeight = toHeight * height / toHeight;
							}
							break;
						}
					case CompressType.Zoom: //自动缩放（不变形）            
						{
							if(oWidth / oHeight > width / height)
							{
								toHeight = width / (oWidth / oHeight);
							}
							else
							{
								toWidth = oWidth / oHeight * height;
							}
							break;
						}
				}

				var ob = new Bitmap((int)toWidth,(int)toHeight);
				Graphics g = Graphics.FromImage(ob);
				g.Clear(Color.Transparent);
				g.CompositingQuality = CompositingQuality.HighQuality;
				g.SmoothingMode = SmoothingMode.HighQuality;
				g.InterpolationMode = InterpolationMode.HighQualityBicubic;
				g.DrawImage(fromFile
							,new RectangleF(x,y,toWidth,toHeight)
							,new RectangleF(0,0,oWidth,oHeight)
							,GraphicsUnit.Pixel);
				g.Dispose();
				//bitmap.Save(toFilePath);
				//以下代码为保存图片时，设置压缩质量
				ImageFormat tFormat = fromFile.RawFormat;
				EncoderParameters ep = new EncoderParameters();
				long[] qy = new long[1];
				qy[0] = quality;//设置压缩的比例1-100
				EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality,qy);
				ep.Param[0] = eParam;
				string extension = Path.GetExtension(fromFilePath);
				ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
				ImageCodecInfo jpegICIinfo = null;
				for(int j = 0;j < arrayICI.Length;j++)
				{
					if(arrayICI[j].FormatDescription.Equals(extension.Substring(1).ToUpper()) ||
						(arrayICI[j].FormatDescription.Equals("JPEG") && extension.Substring(1).ToUpper() == "JPG"))
					{
						jpegICIinfo = arrayICI[j];
						break;
					}
				}
				if(jpegICIinfo != null)
				{
					ob.Save(toFilePath,jpegICIinfo,ep);//dFile是压缩后的新路径
				}
				else
				{
					ob.Save(toFilePath,tFormat);
				}
				return ob;
			}
		}
		#endregion

		#region 生成缩略图
		/// <summary>
		/// 生成缩略图
		/// </summary>
		/// <param name="originalImagePath">源图路径(物理路径)</param>
		/// <param name="thumbnailPath">缩略图保存路径(物理路径)</param>
		/// <param name="width">缩略图宽度</param>
		/// <param name="height">缩略图高度</param>
		/// <param name="mode">生成缩略图的模式</param>
		public void MakeThumbnail(string originalImagePath,string thumbnailPath,int width,int height,ThumbnailMode mode)
		{
			Image originalImage = Image.FromFile(originalImagePath);

			int towidth = width;
			int toheight = height;

			int x = 0;
			int y = 0;
			int ow = originalImage.Width;
			int oh = originalImage.Height;

			#region 根据缩略图的模式计算宽与高

			switch(mode)
			{
				case ThumbnailMode.WH:
					break;
				case ThumbnailMode.W:
					toheight = originalImage.Height * width / originalImage.Width;
					break;
				case ThumbnailMode.H:
					towidth = originalImage.Width * height / originalImage.Height;
					break;
				case ThumbnailMode.Cut:
					if(originalImage.Width / (double)originalImage.Height > towidth / (double)toheight)
					{
						oh = originalImage.Height;
						ow = originalImage.Height * towidth / toheight;
						y = 0;
						x = (originalImage.Width - ow) / 2;
					}
					else
					{
						ow = originalImage.Width;
						oh = originalImage.Width * height / towidth;
						x = 0;
						y = (originalImage.Height - oh) / 2;
					}
					break;
				case ThumbnailMode.DB:
					if(originalImage.Width / (double)towidth < originalImage.Height / (double)toheight)
					{
						toheight = height;
						towidth = originalImage.Width * height / originalImage.Height;
					}
					else
					{
						towidth = width;
						toheight = originalImage.Height * width / originalImage.Width;
					}
					break;
			}

			#endregion 根据缩略图的模式计算宽与高

			//新建一个bmp图片
			Image bitmap = new Bitmap(towidth,toheight);

			//新建一个画板
			Graphics g = Graphics.FromImage(bitmap);

			//设置高质量插值法
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;

			//设置高质量,低速度呈现平滑程度
			g.SmoothingMode = SmoothingMode.HighQuality;

			//清空画布并以透明背景色填充
			g.Clear(Color.Transparent);

			//在指定位置并且按指定大小绘制原图片的指定部分
			g.DrawImage(originalImage,new Rectangle(0,0,towidth,toheight),
						new Rectangle(x,y,ow,oh),
						GraphicsUnit.Pixel);

			try
			{
				//关键质量控制
				//获取系统编码类型数组,包含了jpeg,bmp,png,gif,tiff
				ImageCodecInfo[] icis = ImageCodecInfo.GetImageEncoders();
				ImageCodecInfo ici = null;
				foreach(ImageCodecInfo i in icis.Where(i => i.MimeType == "image/jpeg" || i.MimeType == "image/bmp" || i.MimeType == "image/png" ||
															i.MimeType == "image/gif"))
				{
					ici = i;
				}
				var ep = new EncoderParameters(1);
				ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality,100);
				//保存缩略图
				bitmap.Save(thumbnailPath,ici,ep);
			}
			finally
			{
				originalImage.Dispose();
				bitmap.Dispose();
				g.Dispose();
			}
		}
		#endregion 缩略图

		#region 生成文字水印
		/// <summary>
		/// 生成文字水印
		/// </summary>
		/// <param name="originalImagePath">源图路径(物理路径)</param>
		/// <param name="savePath">保存的图片路径(物理路径)</param>
		/// <param name="text">水印文字</param>
		/// <param name="fontFamily">水印文字的字体</param>
		/// <param name="fontSize">水印文字的字体号数</param>
		/// <param name="position">水印位置</param>
		/// <param name="isBold">是否加粗</param>
		/// <param name="textColor">水印文字的字体颜色</param>
		public void MakeWordWatermark(string originalImagePath,string savePath,string text,FontFamily fontFamily,
									  float? fontSize,WatermarkPosition position,bool isBold,Color textColor)
		{
			Image originalImage = Image.FromFile(originalImagePath);

			//根据源图片生成新的Bitmap对象作为作图区，为了给gif图片添加水印，才有此周折
			var bitmap = new Bitmap(originalImage.Width,originalImage.Height,PixelFormat.Format32bppArgb);
			//设置新建位图得分辨率
			bitmap.SetResolution(originalImage.HorizontalResolution,originalImage.VerticalResolution);
			//创建Graphics对象，以对该位图进行操作
			Graphics g = Graphics.FromImage(bitmap);
			//消除锯齿
			g.SmoothingMode = SmoothingMode.AntiAlias;
			//将原图拷贝到作图区
			g.DrawImage(originalImage,new Rectangle(0,0,originalImage.Width,originalImage.Height),0,0,
						originalImage.Width,originalImage.Height,GraphicsUnit.Pixel);

			//声明字体对象
			Font cFont = null;
			//用来测试水印文本长度得尺子
			var size = new SizeF();
			if(!fontSize.HasValue)
			{
				//探测出一个适合图片大小得字体大小，以适应水印文字大小得自适应
				for(int i = 0;i < 6;i++)
				{
					//创建一个字体对象
					//是否加粗
					if(!isBold)
					{
						cFont = new Font(fontFamily,sizes[i],FontStyle.Regular);
					}
					else
					{
						cFont = new Font(fontFamily,sizes[i],FontStyle.Bold);
					}
					//测量文本大小
					size = g.MeasureString(text,cFont);
					//匹配第一个符合要求得字体大小
					if((ushort)size.Width < (ushort)originalImage.Width)
					{
						break;
					}
				}
			}
			else
			{
				if(!isBold)
				{
					cFont = new Font(fontFamily,fontSize.Value,FontStyle.Regular);
				}
				else
				{
					cFont = new Font(fontFamily,fontSize.Value,FontStyle.Bold);
				}
			}
			//设置水印坐标位置.
			int markx = 5;
			int marky = 5;

			SetMarkPosition(originalImage,(int)size.Width,(int)size.Height,position,ref markx,ref marky);
			//创建刷子对象，准备给图片写上文字
			Brush brush = new SolidBrush(textColor);
			//在指定得位置写上文字
			g.DrawString(text,cFont,brush,markx,marky);
			try
			{
				//默认60%的图片质量, 图片较模糊, 论坛12楼 http://topic.csdn.net/u/20090819/14/c302cba0-a70a-43f7-a265-6e91c55a2f6f.html
				bitmap.Save(savePath,ImageFormat.Jpeg);
				//以全质量重新保存图片
				EncoderParameter p = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality,100L);
				EncoderParameters ps = new EncoderParameters();
				ps.Param[0] = p;

				var cInfo = ImageCodecInfo.GetImageEncoders()
											.Where(x => x.MimeType == "image/jpeg")
											.FirstOrDefault();
				if(cInfo != null)
				{
					File.Delete(savePath);
					bitmap.Save(savePath,cInfo,ps);
				}
			}
			finally
			{
				originalImage.Dispose();
				bitmap.Dispose();
				//释放Graphics对象
				g.Dispose();
			}
		}
		#endregion

		#region 生成图片水印
		/// <summary>
		/// 生成图片水印.
		/// </summary>
		/// <param name="originalImagePath">源图路径（物理路径）</param>
		/// <param name="savePath">保存的图片路径(物理路径)</param>
		/// <param name="waterImagePath">水印源图路径（物理路径）</param>
		/// <param name="position">水印位置枚举</param>
		/// <param name="isTransparence">是否半透明</param>
		public bool MakeImageWatermark(string originalImagePath,string savePath,string waterImagePath,
									   WatermarkPosition position,bool isTransparence)
		{
			Image originalImage = Image.FromFile(originalImagePath);

			//获得水印图像
			Image markImg = Image.FromFile(waterImagePath);

			if(originalImage.Width < markImg.Width || originalImage.Height < markImg.Height)
				return false;

			//创建颜色矩阵
			float transparence = isTransparence ? 0.5f : 1.0f;
			float[][] ptsArray = {
									 new float[] {1, 0, 0, 0, 0},
									 new float[] {0, 1, 0, 0, 0},
									 new float[] {0, 0, 1, 0, 0},
									 new float[] {0, 0, 0, transparence, 0}, //注意：此处为0.0f为完全透明，1.0f为完全不透明
                                     new float[] {0, 0, 0, 0, 1}
								 };
			var colorMatrix = new ColorMatrix(ptsArray);
			//新建一个Image属性
			var imageAttributes = new ImageAttributes();
			//将颜色矩阵添加到属性
			imageAttributes.SetColorMatrix(colorMatrix,ColorMatrixFlag.Default,
										   ColorAdjustType.Default);
			//生成位图作图区
			var bitmap = new Bitmap(originalImage.Width,originalImage.Height,PixelFormat.Format32bppArgb);
			//设置分辨率
			bitmap.SetResolution(originalImage.HorizontalResolution,originalImage.VerticalResolution);
			//创建Graphics
			Graphics g = Graphics.FromImage(bitmap);
			//消除锯齿
			g.SmoothingMode = SmoothingMode.AntiAlias;
			//拷贝原图到作图区
			g.DrawImage(originalImage,new Rectangle(0,0,originalImage.Width,originalImage.Height),0,0,
						originalImage.Width,originalImage.Height,GraphicsUnit.Pixel);

			int markx = 5;
			int marky = 5;
			SetMarkPosition(originalImage,markImg.Width,markImg.Height,position,ref markx,ref marky);

			//添加水印
			g.DrawImage(markImg,new Rectangle(markx,marky,markImg.Width,markImg.Height),0,0,markImg.Width,
						markImg.Height,GraphicsUnit.Pixel,imageAttributes);
			try
			{
				//默认60%的图片质量, 图片较模糊, 论坛12楼 http://topic.csdn.net/u/20090819/14/c302cba0-a70a-43f7-a265-6e91c55a2f6f.html
				bitmap.Save(savePath,ImageFormat.Jpeg);
				//以全质量重新保存图片
				EncoderParameter p = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality,100L);
				EncoderParameters ps = new EncoderParameters();
				ps.Param[0] = p;

				var cInfo = ImageCodecInfo.GetImageEncoders()
											.Where(x => x.MimeType == "image/jpeg")
											.FirstOrDefault();
				if(cInfo != null)
				{
					File.Delete(savePath);
					bitmap.Save(savePath,cInfo,ps);
				}
			}
			finally
			{
				originalImage.Dispose();
				bitmap.Dispose();
				g.Dispose();
			}

			return true;
		}
		#endregion

		#region 添加文字水印并生成缩略图
		/// <summary>
		/// 添加文字水印并生成缩略图
		/// </summary>
		/// <param name="originalImagePath">源图路径(物理路径)</param>
		/// <param name="savePath">保存的图片路径(物理路径)</param>
		/// <param name="text">水印的文字</param>
		/// <param name="fontFamily">水印文字的字体</param>
		/// <param name="fontSize">水印文字的号数</param>
		/// <param name="position">水印位置</param>
		/// <param name="isBold">是否加粗</param>
		/// <param name="textColor">水印文字的颜色</param>
		/// <param name="width">缩略图宽度</param>
		/// <param name="height">缩略图高度</param>
		/// <param name="mode">生成缩略图的模式</param>
		public void MakeWordWatermarkAndThumbnail(string originalImagePath,string savePath,string text,
												  FontFamily fontFamily,float? fontSize,WatermarkPosition position,
												  bool isBold,Color textColor,int width,int height,
												  ThumbnailMode mode)
		{
			//添加水印
			MakeWordWatermark(originalImagePath,savePath,text,fontFamily,fontSize,position,isBold,textColor);

			//获取图片名称
			string fileName = Path.GetFileNameWithoutExtension(savePath);
			string thImagePath = savePath.Replace(fileName,"th_" + fileName);

			//生成缩略图
			//以添加水印的图片为准
			MakeThumbnail(savePath,thImagePath,width,height,mode);
		}
		#endregion

		#region 添加图片水印并生成缩略图
		/// <summary>
		/// 添加图片水印并生成缩略图
		/// </summary>
		/// <param name="originalImagePath">源图路径(物理路径)</param>
		/// <param name="savePath">保存的图片路径(物理路径)</param>
		/// <param name="waterImagePath">水印源图片路径(物理路径)</param>
		/// <param name="position">水印位置</param>
		/// <param name="isTransparence">是否半透明</param>
		/// <param name="width">缩略图宽度</param>
		/// <param name="height">缩略图高度</param>
		/// <param name="mode">生成缩略图的模式</param>
		public void MakeImageWatermarkAndThumbnail(string originalImagePath,string savePath,string waterImagePath,
												   WatermarkPosition position,bool isTransparence,int width,
												   int height,ThumbnailMode mode)
		{
			//水印
			MakeImageWatermark(originalImagePath,savePath,waterImagePath,position,isTransparence);

			//获取图片名称
			string fileName = Path.GetFileNameWithoutExtension(savePath);
			string thImagePath = savePath.Replace(fileName,"th_" + fileName);

			//缩略图
			MakeThumbnail(savePath,thImagePath,width,height,mode);
		}
		#endregion

		#region 计算水印坐标位置
		/// <summary>
		/// 计算水印坐标位置.
		/// </summary>
		/// <param name="image"></param>
		/// <param name="watermarkWidth"></param>
		/// <param name="watermarkHeight"></param>
		/// <param name="position"></param>
		/// <param name="markx"></param>
		/// <param name="marky"></param>
		private void SetMarkPosition(Image image,int watermarkWidth,int watermarkHeight,WatermarkPosition position,
									 ref int markx,ref int marky)
		{
			int width = image.Width / 3;
			int height = image.Height / 3;
			switch(position)
			{
				case WatermarkPosition.LeftTop:
					markx = width / 2;
					marky = height / 2;
					break;
				case WatermarkPosition.CenterTop:
					markx = (image.Width - watermarkWidth) / 2;
					marky = height / 2;
					break;
				case WatermarkPosition.RightTop:
					markx = width * 2 + width / 2 - watermarkWidth / 2;
					marky = height / 2;
					break;
				case WatermarkPosition.LeftMiddle:
					markx = width / 2;
					marky = (image.Height - watermarkHeight) / 2;
					break;
				case WatermarkPosition.CenterMiddle:
					markx = (image.Width - watermarkWidth) / 2;
					marky = (image.Height - watermarkHeight) / 2;
					break;
				case WatermarkPosition.RightMiddle:
					markx = width * 2 + width / 2 - watermarkWidth / 2;
					marky = (image.Height - watermarkHeight) / 2;
					break;
				case WatermarkPosition.LeftBottom:
					markx = width / 2;
					marky = height * 2 + height / 2 - watermarkHeight / 2;
					break;
				case WatermarkPosition.CenterBottom:
					markx = (image.Width - watermarkWidth) / 2;
					marky = height * 2 + height / 2 - watermarkHeight / 2;
					break;
				case WatermarkPosition.RightBottom:
					markx = width * 2 + width / 2 - watermarkWidth / 2;
					marky = height * 2 + height / 2 - watermarkHeight / 2;
					break;
			}
		}
		#endregion

		#region 获得图片信息
		/// <summary>
		/// 获得图片信息
		/// </summary>
		/// <param name="strFilePath">图片物理路径</param>
		/// <returns></returns>
		public static string GetImageInfo(string strFilePath,string strFileSaveVirtualPath)
		{
			Image image = Image.FromFile(strFilePath);
			string strJson = string.Format("{{\"width\":{0},\"height\":{1},\"verticalResolution\":{2},\"url\":\"{3}\"}}",image.Width,image.Height,image.VerticalResolution,strFileSaveVirtualPath);
			image.Dispose();
			return strJson;
		}
		#endregion
	}
	#endregion 公共方法

	/*

	///// <summary>
	///// 会产生graphics异常的PixelFormat
	///// </summary>
	//private static PixelFormat[] indexedPixelFormats = { PixelFormat.Undefined, PixelFormat.DontCare, PixelFormat.Format16bppArgb1555, PixelFormat.Format1bppIndexed, PixelFormat.Format4bppIndexed, PixelFormat.Format8bppIndexed };

	///// <summary>
	///// 判断图片的PixelFormat 是否在 引发异常的 PixelFormat 之中
	///// 无法从带有索引像素格式的图像创建graphics对象
	///// </summary>
	///// <param name="imgPixelFormat">原图片的PixelFormat</param>
	///// <returns></returns>
	//private static bool IsPixelFormatIndexed(PixelFormat imgPixelFormat)
	//{
	//    foreach (PixelFormat pf in indexedPixelFormats)
	//    {
	//        if (pf.Equals(imgPixelFormat)) return true;
	//    }

	//    return false;
	//}

	#region 按比例生成文字水印图
	/// <summary>
	/// 按比例生成文字水印图
	/// </summary>
	/// <param name="originalImagePath">图片路径</param>
	/// <param name="savePath">保存路径</param>
	/// <param name="text">水印文字</param>
	/// <param name="fontFamily">水印字体</param>
	/// <param name="fontColor">文字颜色</param>
	/// <param name="position">水印位置</param>
	/// <param name="proportion">比例大小, 小于整数1</param>
	/// <param name="transparency">水印透明度（0-255）</param>
	public void MakeTextMark(string originalImagePath,
		string savePath,
		string text,
		FontFamily fontFamily,
		Color fontColor,
		WatermarkPosition position,
		float? proportion, int transparency)
	{
		Image img = Image.FromFile(originalImagePath);

		if (IsPixelFormatIndexed(img.PixelFormat))
		{
			Bitmap bmp = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppArgb);
			using (Graphics g = Graphics.FromImage(bmp))
			{
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
				g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
				g.DrawImage(img, 0, 0);


				int originalImageWidth = img.Width;

				int drawWidth = Convert.ToInt32(originalImageWidth * proportion);

				int fontSize = drawWidth / text.Length;//原定的字体大小
				Font drawFont = new Font(fontFamily, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
				SizeF crSize;
				crSize = g.MeasureString(text, drawFont);

				float xpos = 0;
				float ypos = 0;

				switch (position)
				{
					case WatermarkPosition.LeftTop:
						xpos = (float)img.Width * (float).01;
						ypos = (float)img.Height * (float).01;
						break;
					case WatermarkPosition.LeftMiddle:
						xpos = ((float)img.Width * (float).50) - (crSize.Width / 2);
						ypos = (float)img.Height * (float).01;
						break;
					case WatermarkPosition.LeftBottom:
						xpos = ((float)img.Width * (float).99) - crSize.Width;
						ypos = (float)img.Height * (float).01;
						break;
					case WatermarkPosition.CenterTop:
						xpos = (float)img.Width * (float).01;
						ypos = ((float)img.Height * (float).50) - (crSize.Height / 2);
						break;
					case WatermarkPosition.CenterMiddle:
						xpos = ((float)img.Width * (float).50) - (crSize.Width / 2);
						ypos = ((float)img.Height * (float).50) - (crSize.Height / 2);
						break;
					case WatermarkPosition.CenterBottom:
						xpos = ((float)img.Width * (float).99) - crSize.Width;
						ypos = ((float)img.Height * (float).50) - (crSize.Height / 2);
						break;
					case WatermarkPosition.RightTop:
						xpos = (float)img.Width * (float).01;
						ypos = ((float)img.Height * (float).99) - crSize.Height;
						break;
					case WatermarkPosition.RightMiddle:
						xpos = ((float)img.Width * (float).50) - (crSize.Width / 2);
						ypos = ((float)img.Height * (float).99) - crSize.Height;
						break;
					case WatermarkPosition.RightBottom:
						xpos = ((float)img.Width * (float).99) - crSize.Width;
						ypos = ((float)img.Height * (float).99) - crSize.Height;
						break;
				}
				Brush brush = new SolidBrush(Color.FromArgb(transparency, fontColor.R, fontColor.G, fontColor.B));
				//g_2.DrawString(watermarkText, drawFont, new SolidBrush(Color.White), xpos + 1, ypos + 1);文字阴影 www.keleyi.com
				g.DrawString(text, drawFont, brush, xpos, ypos);

				ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
				ImageCodecInfo ici = null;
				foreach (ImageCodecInfo codec in codecs)
				{
					if (codec.MimeType.IndexOf("jpeg") > -1)
						ici = codec;
				}
				EncoderParameters encoderParams = new EncoderParameters();
				long[] qualityParam = new long[1];
				int quality = 99;//加水印后的质量0~100,数字越大质量越高
				if (quality < 0 || quality > 100)
					quality = 80;

				qualityParam[0] = quality;

				EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityParam);
				encoderParams.Param[0] = encoderParam;

				if (ici != null)
					img.Save(savePath, ici, encoderParams);
				else
					img.Save(savePath);

				g.Dispose();
				img.Dispose();
			}
		}
		else
		{
			Graphics g = Graphics.FromImage(img);
			int originalImageWidth = img.Width;

			int drawWidth = Convert.ToInt32(originalImageWidth * proportion);

			int fontSize = drawWidth / text.Length;//原定的字体大小
			Font drawFont = new Font(fontFamily, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
			SizeF crSize;
			crSize = g.MeasureString(text, drawFont);

			float xpos = 0;
			float ypos = 0;

			switch (position)
			{
				case WatermarkPosition.LeftTop:
					xpos = (float)img.Width * (float).01;
					ypos = (float)img.Height * (float).01;
					break;
				case WatermarkPosition.LeftMiddle:
					xpos = ((float)img.Width * (float).50) - (crSize.Width / 2);
					ypos = (float)img.Height * (float).01;
					break;
				case WatermarkPosition.LeftBottom:
					xpos = ((float)img.Width * (float).99) - crSize.Width;
					ypos = (float)img.Height * (float).01;
					break;
				case WatermarkPosition.CenterTop:
					xpos = (float)img.Width * (float).01;
					ypos = ((float)img.Height * (float).50) - (crSize.Height / 2);
					break;
				case WatermarkPosition.CenterMiddle:
					xpos = ((float)img.Width * (float).50) - (crSize.Width / 2);
					ypos = ((float)img.Height * (float).50) - (crSize.Height / 2);
					break;
				case WatermarkPosition.CenterBottom:
					xpos = ((float)img.Width * (float).99) - crSize.Width;
					ypos = ((float)img.Height * (float).50) - (crSize.Height / 2);
					break;
				case WatermarkPosition.RightTop:
					xpos = (float)img.Width * (float).01;
					ypos = ((float)img.Height * (float).99) - crSize.Height;
					break;
				case WatermarkPosition.RightMiddle:
					xpos = ((float)img.Width * (float).50) - (crSize.Width / 2);
					ypos = ((float)img.Height * (float).99) - crSize.Height;
					break;
				case WatermarkPosition.RightBottom:
					xpos = ((float)img.Width * (float).99) - crSize.Width;
					ypos = ((float)img.Height * (float).99) - crSize.Height;
					break;
			}
			Brush brush = new SolidBrush(Color.FromArgb(transparency, fontColor.R, fontColor.G, fontColor.B));
			//g_2.DrawString(watermarkText, drawFont, new SolidBrush(Color.White), xpos + 1, ypos + 1);文字阴影 www.keleyi.com
			g.DrawString(text, drawFont, brush, xpos, ypos);

			ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
			ImageCodecInfo ici = null;
			foreach (ImageCodecInfo codec in codecs)
			{
				if (codec.MimeType.IndexOf("jpeg") > -1)
					ici = codec;
			}
			EncoderParameters encoderParams = new EncoderParameters();
			long[] qualityParam = new long[1];
			int quality = 99;//加水印后的质量0~100,数字越大质量越高
			if (quality < 0 || quality > 100)
				quality = 80;

			qualityParam[0] = quality;

			EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityParam);
			encoderParams.Param[0] = encoderParam;

			if (ici != null)
				img.Save(savePath, ici, encoderParams);
			else
				img.Save(savePath);

			g.Dispose();
			img.Dispose();
		}
	}
	#endregion

	#region 按比例生成图片水印图
	/// <summary>
	/// 按比例生成图片水印图
	/// </summary>
	/// <param name="originalImagePath">图片路径</param>
	/// <param name="savePath">保存路径</param>
	/// <param name="markImagePath">水印底图的存放路径</param>
	/// <param name="tempMarkImagePath">按比例生成临时水印底图的存放路径</param>
	/// <param name="position">水印位置</param>
	/// <param name="proportion">比例大小, 小于整数1</param>
	public void MakeImageMark(string originalImagePath,
		string savePath,
		string markImagePath,
		string tempMarkImagePath,
		WatermarkPosition position,
		float? proportion)
	{
		Image originalImage = Image.FromFile(originalImagePath);

		Image markImage = Image.FromFile(markImagePath);

		int finalMarkHeight = 0;//最终水印的高度
		int finalMarkWidth = 0; //最终水印的宽度

		if (originalImage.Height > originalImage.Width)
		{
			//以宽度为准计算比例
			finalMarkWidth = Convert.ToInt32(originalImage.Width * proportion);
			finalMarkHeight = markImage.Height * finalMarkWidth / markImage.Width;
		}
		else
		{
			//以高度为准计算比例
			finalMarkHeight = Convert.ToInt32(originalImage.Height * proportion);
			finalMarkWidth = markImage.Width * finalMarkHeight / markImage.Height;
		}

		Bitmap bitmap = new Bitmap(finalMarkWidth, finalMarkHeight);
		Graphics g = Graphics.FromImage(bitmap);
		g.InterpolationMode = InterpolationMode.HighQualityBicubic;
		g.SmoothingMode = SmoothingMode.HighQuality;
		g.Clear(Color.Transparent);
		g.DrawImage(markImage,
			new Rectangle(0, 0, finalMarkWidth, finalMarkHeight),
			new Rectangle(0, 0, markImage.Width, markImage.Height),
			GraphicsUnit.Pixel
		);

		try
		{
			//将水印图计算比例后, 暂时存入待存放路径
			ImageCodecInfo[] arrCodecInfo = ImageCodecInfo.GetImageEncoders();
			ImageCodecInfo codecInfo = null;
			foreach (ImageCodecInfo i in arrCodecInfo.Where(i => i.MimeType == "image/jpeg" ||
				i.MimeType == "image/bmp" ||
				i.MimeType == "image/png" ||
				i.MimeType == "image/gif"))
			{
				codecInfo = i;
			}

			var ep = new EncoderParameters(1);
			ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100);
			bitmap.Save(tempMarkImagePath, codecInfo, ep);

			//重新取出水印图, 待印到原图
			markImage = Image.FromFile(tempMarkImagePath);

			#region 计算绘制的位置

			int positionX = 0;
			int positionY = 0;
			int padding = 3;

			//垂直居顶 - Y
			int valignTop = padding;
			//垂直居中 - Y
			int valignMiddle = (originalImage.Height - (padding * 2) - markImage.Height) / 2;
			//垂直居底 - Y
			int valignBottom = originalImage.Height - (padding * 2) - markImage.Height;
			//水平居左 - X
			int alignLeft = padding;
			//水平居中 - X  
			int alignCenter = (originalImage.Width - (padding * 2) - markImage.Width) / 2;
			//水平居右 - X
			int alignRight = originalImage.Width - (padding * 2) - markImage.Width;

			switch (position)
			{
				case WatermarkPosition.LeftTop:
					positionX = alignLeft;
					positionY = valignTop;
					break;
				case WatermarkPosition.LeftMiddle:
					positionX = alignLeft;
					positionY = valignMiddle;
					break;
				case WatermarkPosition.LeftBottom:
					positionX = alignLeft;
					positionY = valignBottom;
					break;
				case WatermarkPosition.CenterTop:
					positionX = alignCenter;
					positionY = valignTop;
					break;
				case WatermarkPosition.CenterMiddle:
					positionX = alignCenter;
					positionY = valignMiddle;
					break;
				case WatermarkPosition.CenterBottom:
					positionX = alignCenter;
					positionY = valignBottom;
					break;
				case WatermarkPosition.RightTop:
					positionX = alignRight;
					positionY = valignTop;
					break;
				case WatermarkPosition.RightMiddle:
					positionX = alignRight;
					positionY = valignMiddle;
					break;
				case WatermarkPosition.RightBottom:
					positionX = alignRight;
					positionY = valignBottom;
					break;
			}

			#endregion 计算绘制的位置

			bitmap = new Bitmap(originalImage.Width, originalImage.Height, PixelFormat.Format32bppArgb);
			bitmap.SetResolution(originalImage.HorizontalResolution, originalImage.VerticalResolution);
			g = Graphics.FromImage(bitmap);
			g.SmoothingMode = SmoothingMode.HighQuality;
			g.DrawImage(originalImage,
				new Rectangle(0, 0, originalImage.Width, originalImage.Height),
				0, 0,
				originalImage.Width,
				originalImage.Height,
				GraphicsUnit.Pixel);
			g.DrawImage(markImage,
				new Rectangle(positionX, positionY, markImage.Width, markImage.Height),
				0, 0,
				markImage.Width,
				markImage.Height,
				GraphicsUnit.Pixel
				);

			//bitmap.Save(savePath, ImageFormat.Jpeg);

			//默认60%的图片质量, 图片较模糊, 论坛12楼 http://topic.csdn.net/u/20090819/14/c302cba0-a70a-43f7-a265-6e91c55a2f6f.html
			bitmap.Save(savePath, ImageFormat.Jpeg);
			//以全质量重新保存图片
			EncoderParameter p = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
			EncoderParameters ps = new EncoderParameters();
			ps.Param[0] = p;

			var cInfo = ImageCodecInfo.GetImageEncoders()
										.Where(x => x.MimeType == "image/jpeg")
										.FirstOrDefault();
			if (cInfo != null)
			{
				File.Delete(savePath);
				bitmap.Save(savePath, cInfo, ps);
			}
		}
		catch (Exception ex)
		{
		}
		finally
		{
			originalImage.Dispose();
			markImage.Dispose();
			bitmap.Dispose();
			g.Dispose();
		}

	}
	#endregion

	#region 按比例生成文字和图片水印图(即文字和图片水印都有)
	/// <summary>
	/// 按比例生成文字和图片水印图(即文字和图片水印都有)
	/// </summary>
	/// <param name="originalImagePath">图片路径</param>
	/// <param name="savePath">保存路径(第一次)</param>
	/// <param name="savePathSecond">保存路径(第二次)</param>
	/// <param name="text">水印文字</param>
	/// <param name="markImagePath">水印底图的存放路径</param>
	/// <param name="tempMarkImagePath">按比例生成临时水印底图的存放路径</param>
	/// <param name="fontFamily">水印字体</param>
	/// <param name="fontColor">文字颜色</param>
	/// <param name="position">水印位置</param>
	/// <param name="proportion">比例大小, 小于整数1</param>
	/// <param name="transparency">水印透明度（0-255）</param>
	public void MakeTextImageMark(string originalImagePath,
		string savePath,
		string savePathSecond,
		string text,
		string markImagePath,
		string tempMarkImagePath,
		FontFamily fontFamily,
		Color fontColor,
		WatermarkPosition position,
		float? proportion, int transparency)
	{
		Image img = Image.FromFile(originalImagePath);
		SizeF crSize;

		if (IsPixelFormatIndexed(img.PixelFormat))
		{
			Bitmap bmp = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppArgb);
			using (Graphics g = Graphics.FromImage(bmp))
			{
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
				g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
				g.DrawImage(img, 0, 0);


				int originalImageWidth = img.Width;

				int drawWidth = Convert.ToInt32(originalImageWidth * proportion);

				int fontSize = drawWidth / text.Length;//原定的字体大小
				Font drawFont = new Font(fontFamily, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
				//SizeF crSize;
				crSize = g.MeasureString(text, drawFont);

				float xpos = 0;
				float ypos = 0;

				switch (position)
				{
					case WatermarkPosition.LeftTop:
						xpos = (float)img.Width * (float).01;
						ypos = (float)img.Height * (float).01;
						break;
					case WatermarkPosition.LeftMiddle:
						xpos = ((float)img.Width * (float).50) - (crSize.Width / 2);
						ypos = (float)img.Height * (float).01;
						break;
					case WatermarkPosition.LeftBottom:
						xpos = ((float)img.Width * (float).99) - crSize.Width;
						ypos = (float)img.Height * (float).01;
						break;
					case WatermarkPosition.CenterTop:
						xpos = (float)img.Width * (float).01;
						ypos = ((float)img.Height * (float).50) - (crSize.Height / 2);
						break;
					case WatermarkPosition.CenterMiddle:
						xpos = ((float)img.Width * (float).50) - (crSize.Width / 2);
						ypos = ((float)img.Height * (float).50) - (crSize.Height / 2);
						break;
					case WatermarkPosition.CenterBottom:
						xpos = ((float)img.Width * (float).99) - crSize.Width;
						ypos = ((float)img.Height * (float).50) - (crSize.Height / 2);
						break;
					case WatermarkPosition.RightTop:
						xpos = (float)img.Width * (float).01;
						ypos = ((float)img.Height * (float).99) - crSize.Height;
						break;
					case WatermarkPosition.RightMiddle:
						xpos = ((float)img.Width * (float).50) - (crSize.Width / 2);
						ypos = ((float)img.Height * (float).99) - crSize.Height;
						break;
					case WatermarkPosition.RightBottom:
						xpos = ((float)img.Width * (float).99) - crSize.Width;
						ypos = ((float)img.Height * (float).99) - crSize.Height;
						break;
				}
				Brush brush = new SolidBrush(Color.FromArgb(transparency, fontColor.R, fontColor.G, fontColor.B));
				//g_2.DrawString(watermarkText, drawFont, new SolidBrush(Color.White), xpos + 1, ypos + 1);文字阴影 www.keleyi.com
				g.DrawString(text, drawFont, brush, xpos, ypos);

				ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
				ImageCodecInfo ici = null;
				foreach (ImageCodecInfo codec in codecs)
				{
					if (codec.MimeType.IndexOf("jpeg") > -1)
						ici = codec;
				}
				EncoderParameters encoderParams = new EncoderParameters();
				long[] qualityParam = new long[1];
				int quality = 99;//加水印后的质量0~100,数字越大质量越高
				if (quality < 0 || quality > 100)
					quality = 80;

				qualityParam[0] = quality;

				EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityParam);
				encoderParams.Param[0] = encoderParam;

				if (ici != null)
					img.Save(savePath, ici, encoderParams);
				else
					img.Save(savePath);

				g.Dispose();
				img.Dispose();
			}
		}
		else
		{
			Graphics g = Graphics.FromImage(img);
			int originalImageWidth = img.Width;

			int drawWidth = Convert.ToInt32(originalImageWidth * proportion);

			int fontSize = drawWidth / text.Length;//原定的字体大小
			Font drawFont = new Font(fontFamily, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
			//SizeF crSize;
			crSize = g.MeasureString(text, drawFont);

			float xpos = 0;
			float ypos = 0;

			switch (position)
			{
				case WatermarkPosition.LeftTop:
					xpos = (float)img.Width * (float).01;
					ypos = (float)img.Height * (float).01;
					break;
				case WatermarkPosition.LeftMiddle:
					xpos = ((float)img.Width * (float).50) - (crSize.Width / 2);
					ypos = (float)img.Height * (float).01;
					break;
				case WatermarkPosition.LeftBottom:
					xpos = ((float)img.Width * (float).99) - crSize.Width;
					ypos = (float)img.Height * (float).01;
					break;
				case WatermarkPosition.CenterTop:
					xpos = (float)img.Width * (float).01;
					ypos = ((float)img.Height * (float).50) - (crSize.Height / 2);
					break;
				case WatermarkPosition.CenterMiddle:
					xpos = ((float)img.Width * (float).50) - (crSize.Width / 2);
					ypos = ((float)img.Height * (float).50) - (crSize.Height / 2);
					break;
				case WatermarkPosition.CenterBottom:
					xpos = ((float)img.Width * (float).99) - crSize.Width;
					ypos = ((float)img.Height * (float).50) - (crSize.Height / 2);
					break;
				case WatermarkPosition.RightTop:
					xpos = (float)img.Width * (float).01;
					ypos = ((float)img.Height * (float).99) - crSize.Height;
					break;
				case WatermarkPosition.RightMiddle:
					xpos = ((float)img.Width * (float).50) - (crSize.Width / 2);
					ypos = ((float)img.Height * (float).99) - crSize.Height;
					break;
				case WatermarkPosition.RightBottom:
					xpos = ((float)img.Width * (float).99) - crSize.Width;
					ypos = ((float)img.Height * (float).99) - crSize.Height;
					break;
			}
			Brush brush = new SolidBrush(Color.FromArgb(transparency, fontColor.R, fontColor.G, fontColor.B));
			//g_2.DrawString(watermarkText, drawFont, new SolidBrush(Color.White), xpos + 1, ypos + 1);文字阴影 www.keleyi.com
			g.DrawString(text, drawFont, brush, xpos, ypos);

			ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
			ImageCodecInfo ici = null;
			foreach (ImageCodecInfo codec in codecs)
			{
				if (codec.MimeType.IndexOf("jpeg") > -1)
					ici = codec;
			}
			EncoderParameters encoderParams = new EncoderParameters();
			long[] qualityParam = new long[1];
			int quality = 99;//加水印后的质量0~100,数字越大质量越高
			if (quality < 0 || quality > 100)
				quality = 80;

			qualityParam[0] = quality;

			EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityParam);
			encoderParams.Param[0] = encoderParam;

			if (ici != null)
				img.Save(savePath, ici, encoderParams);
			else
				img.Save(savePath);

			g.Dispose();
			img.Dispose();
		}

		//添加图片水印


		Image originalImage = Image.FromFile(savePath);

		Image markImage = Image.FromFile(markImagePath);

		int finalMarkHeight = 0;//最终水印的高度
		int finalMarkWidth = 0; //最终水印的宽度

		//if (originalImage.Height > originalImage.Width)
		//{
		//    //以宽度为准计算比例
		//    finalMarkWidth = Convert.ToInt32(originalImage.Width * proportion);
		//    finalMarkHeight = markImage.Height * finalMarkWidth / markImage.Width;
		//}
		//else
		//{
		//    //以高度为准计算比例
		//    finalMarkHeight = Convert.ToInt32(originalImage.Height * proportion);
		//    finalMarkWidth = markImage.Width * finalMarkHeight / markImage.Height;
		//}

		finalMarkHeight = Convert.ToInt16(crSize.Height);
		finalMarkWidth = Convert.ToInt16(markImage.Width / (markImage.Height / crSize.Height));

		Bitmap bitmap = new Bitmap(finalMarkWidth, finalMarkHeight);
		Graphics g_2 = Graphics.FromImage(bitmap);
		g_2.InterpolationMode = InterpolationMode.HighQualityBicubic;
		g_2.SmoothingMode = SmoothingMode.HighQuality;
		g_2.Clear(Color.Transparent);
		g_2.DrawImage(markImage,
			new Rectangle(0, 0, finalMarkWidth, finalMarkHeight),
			new Rectangle(0, 0, markImage.Width, markImage.Height),
			GraphicsUnit.Pixel
		);

		try
		{
			//将水印图计算比例后, 暂时存入待存放路径
			ImageCodecInfo[] arrCodecInfo = ImageCodecInfo.GetImageEncoders();
			ImageCodecInfo codecInfo = null;
			foreach (ImageCodecInfo i in arrCodecInfo.Where(i => i.MimeType == "image/jpeg" ||
				i.MimeType == "image/bmp" ||
				i.MimeType == "image/png" ||
				i.MimeType == "image/gif"))
			{
				codecInfo = i;
			}

			var ep = new EncoderParameters(1);
			ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100);
			bitmap.Save(tempMarkImagePath, codecInfo, ep);

			//重新取出水印图, 待印到原图
			markImage = Image.FromFile(tempMarkImagePath);


			#region 计算绘制的位置

			int positionX = 0;
			int positionY = 0;
			int padding = 3;

			//垂直居顶 - Y
			int valignTop = padding;
			//垂直居中 - Y
			int valignMiddle = (originalImage.Height - (padding * 2) - markImage.Height) / 2;
			//垂直居底 - Y
			int valignBottom = originalImage.Height - (padding * 2) - markImage.Height;
			//水平居左 - X
			int alignLeft = padding;
			//水平居中 - X  
			int alignCenter = (originalImage.Width - (padding * 2) - markImage.Width) / 2;
			//水平居右 - X
			int alignRight = originalImage.Width - (padding * 2) - markImage.Width;

			switch (position)
			{
				case WatermarkPosition.LeftTop:
					positionX = alignLeft;
					positionY = valignTop;
					break;
				case WatermarkPosition.LeftMiddle:
					positionX = alignLeft;
					positionY = valignMiddle;
					break;
				case WatermarkPosition.LeftBottom:
					positionX = alignLeft;
					positionY = valignBottom;
					break;
				case WatermarkPosition.CenterTop:
					positionX = alignCenter;
					positionY = valignTop;
					break;
				case WatermarkPosition.CenterMiddle:
					positionX = alignCenter;
					positionY = valignMiddle;
					break;
				case WatermarkPosition.CenterBottom:
					positionX = alignCenter;
					positionY = valignBottom;
					break;
				case WatermarkPosition.RightTop:
					positionX = alignRight;
					positionY = valignTop;
					break;
				case WatermarkPosition.RightMiddle:
					positionX = alignRight;
					positionY = valignMiddle;
					break;
				case WatermarkPosition.RightBottom:
					positionX = alignRight;
					positionY = valignBottom;
					break;
			}

			#endregion 计算绘制的位置

			positionX = positionX - (int)crSize.Width;

			bitmap = new Bitmap(originalImage.Width, originalImage.Height, PixelFormat.Format32bppArgb);
			bitmap.SetResolution(originalImage.HorizontalResolution, originalImage.VerticalResolution);
			g_2 = Graphics.FromImage(bitmap);
			g_2.SmoothingMode = SmoothingMode.HighQuality;
			g_2.DrawImage(originalImage,
				new Rectangle(0, 0, originalImage.Width, originalImage.Height),
				0, 0,
				originalImage.Width,
				originalImage.Height,
				GraphicsUnit.Pixel);
			g_2.DrawImage(markImage,
				new Rectangle(positionX, positionY, markImage.Width, markImage.Height),
				0, 0,
				markImage.Width,
				markImage.Height,
				GraphicsUnit.Pixel
				);

			//bitmap.Save(savePath, ImageFormat.Jpeg);

			//默认60%的图片质量, 图片较模糊, 论坛12楼 http://topic.csdn.net/u/20090819/14/c302cba0-a70a-43f7-a265-6e91c55a2f6f.html
			bitmap.Save(savePathSecond, ImageFormat.Jpeg);
			//以全质量重新保存图片
			EncoderParameter p = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
			EncoderParameters ps = new EncoderParameters();
			ps.Param[0] = p;

			var cInfo = ImageCodecInfo.GetImageEncoders()
										.Where(x => x.MimeType == "image/jpeg")
										.FirstOrDefault();
			if (cInfo != null)
			{
				File.Delete(savePathSecond);
				bitmap.Save(savePathSecond, cInfo, ps);
			}
		}
		catch (Exception ex)
		{
		}
		finally
		{
			originalImage.Dispose();
			markImage.Dispose();
			bitmap.Dispose();
			g_2.Dispose();
		}

		//删除只添加了文字水印的那张图
		if (File.Exists(savePath))
			File.Delete(savePath);
		//删除临时存放水印的那张图
		if (File.Exists(tempMarkImagePath))
			File.Delete(tempMarkImagePath);

	}
	#endregion

	#region 裁剪图片

	/// <summary>
	/// 裁剪图片。
	/// </summary>
	/// <param name="sourcePath">源文件地址。</param>
	/// <param name="destPath">目标文件地址。</param>
	/// <param name="ratio">原图片缩放比例。</param>
	/// <param name="x">裁剪开始横坐标。</param>
	/// <param name="y">裁剪开始纵坐标。</param>
	/// <param name="width">裁剪宽度。</param>
	/// <param name="height">裁剪高度。</param>
	public static bool Tailor(string sourcePath, string destPath, float ratio, float x, float y, int width, int height)
	{
		bool tailorResult = false;
		try
		{
			var extension = System.IO.Path.GetExtension(sourcePath);
			Bitmap source = new Bitmap(System.Drawing.Bitmap.FromFile(sourcePath));
			if (ratio != 1)
			{// 需要缩放
				source = GeometricZoom(source, ratio);
			}
			System.Drawing.Image destImage = new System.Drawing.Bitmap(width, height);
			System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(destImage);

			graphics.CompositingQuality = CompositingQuality.HighQuality;
			graphics.SmoothingMode = SmoothingMode.HighQuality;
			graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

			graphics.DrawImage(source,
				  new RectangleF(0, 0, width, height),
				  new RectangleF(x, y, width, height),
				  GraphicsUnit.Pixel
				  );
			graphics.Save();

			string directory = System.IO.Path.GetDirectoryName(destPath);
			if (!System.IO.Directory.Exists(directory))
			{
				System.IO.Directory.CreateDirectory(directory);
			}
			if (extension == ".png")
			{
				destImage.Save(destPath, System.Drawing.Imaging.ImageFormat.Png);
			}
			else
			{
				destImage.Save(destPath, System.Drawing.Imaging.ImageFormat.Jpeg);
			}
			graphics.Dispose();

			destImage.Dispose();
			source.Dispose();
			tailorResult = true;
		}
		catch (System.Exception e)
		{
			tailorResult = false;
		}
		return tailorResult;
	}

	/// <summary>
	/// 获取等比缩放后的Bitmap。
	/// </summary>
	/// <param name="bmp">原始图片对象。</param>
	/// <param name="ratio">缩放比例。</param>
	/// <returns></returns>
	static Bitmap GeometricZoom(Bitmap bmp, float ratio)
	{
		try
		{
			int w = (int)(bmp.Width * ratio), h = (int)(bmp.Height * ratio);
			Bitmap b = new Bitmap(w, h);
			Graphics g = Graphics.FromImage(b);
			// 插值算法的质量
			g.CompositingQuality = CompositingQuality.HighQuality;
			g.SmoothingMode = SmoothingMode.HighQuality;
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			g.DrawImage(bmp, new Rectangle(0, 0, w, h), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
			g.Dispose(); return b;
		}
		catch
		{
			return null;
		}
	}

	#endregion

  */

}