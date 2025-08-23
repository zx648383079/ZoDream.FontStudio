using SkiaSharp;

namespace ZoDream.Shared.ImageEditor
{
    public interface IImageCanvas
    {
        public void DrawBitmap(SKBitmap source);
        public void DrawBitmap(SKBitmap source, SKPoint point);
        public void DrawBitmap(SKBitmap source, SKRect rect);
        public void DrawSurface(SKSurface surface);
        public void DrawSurface(SKSurface surface, SKPoint point);

        public void DrawSurface(SKSurface surface, SKRect rect);
        public void DrawText(string text, SKPoint point, SKTextAlign textAlign, SKFont font, SKPaint paint);

        public void DrawPath(SKPath path, SKPaint paint);
        /// <summary>
        /// 绘制纹理
        /// </summary>
        /// <param name="source">纹理图片</param>
        /// <param name="sourceVertices">纹理上的顶点</param>
        /// <param name="vertices">顶点对于的位置</param>
        public void DrawTexture(SKBitmap source, SKPoint[] sourceVertices, SKPoint[] vertices);
        /// <summary>
        /// 画矩形
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="paint"></param>
        public void DrawRect(SKRect rect, SKPaint paint);
        /// <summary>
        /// 画圆角矩形
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="paint"></param>
        public void DrawRect(SKRoundRect rect, SKPaint paint);
        /// <summary>
        /// 画圆
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="paint"></param>
        public void DrawCircle(SKPoint center, float radius, SKPaint paint);
        /// <summary>
        /// 画椭圆
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="paint"></param>
        public void DrawOval(SKPoint center, SKSize radius, SKPaint paint);
    }
}
