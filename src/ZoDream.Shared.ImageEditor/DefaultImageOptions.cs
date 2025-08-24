using SkiaSharp;

namespace ZoDream.Shared.ImageEditor
{
    public class DefaultImageOptions : IImageOptions
    {

        private readonly SKColor _background = SKColors.White;
        private readonly SKColor _foreground = SKColors.Black;
        private readonly SKColor _hovered = SKColors.Blue.WithAlpha(50);
        private readonly SKColor _activated = SKColors.Blue;

        private readonly SKPaint _jointStrokePaint = new()
        {
            Color = SKColors.Blue,
            StrokeWidth = 2,
            IsStroke = true,
            IsAntialias = true,
        };
        private readonly SKPaint _jointPaint = new()
        {
            IsStroke = false,
            ColorF = SKColors.White,
            IsAntialias = true,
        };
        private readonly SKPaint _jointHoveredPaint = new()
        {
            IsStroke = false,
            ColorF = SKColors.Blue,
            IsAntialias = true,
        };

        private readonly SKPaint _backgroundPaint = new()
        {
            IsStroke = false,
            ColorF = SKColors.White,
            IsAntialias = true,
        };
        private readonly SKPaint _foregroundPaint = new()
        {
            IsStroke = false,
            ColorF = SKColors.Black,
            IsAntialias = true,
        };
        public SKPaint JointStrokePaint => _jointStrokePaint;

        public SKPaint JointPaint => _jointPaint;

        public SKPaint JointHoveredPaint => _jointHoveredPaint;

        public float JointSize => 16;

        public SKColor Hovered => _hovered;
        public SKColor Activated => _activated;

        public SKColor Background => _background;

        public SKPaint BackgroundPaint => _backgroundPaint;

        public SKColor Foreground => _foreground;

        public SKPaint ForegroundPaint => _foregroundPaint;

        public void Dispose()
        {
            _jointPaint.Dispose();
            _jointHoveredPaint.Dispose();
            _jointStrokePaint.Dispose();
            _backgroundPaint.Dispose();
            _foregroundPaint.Dispose();
        }
    }
}
