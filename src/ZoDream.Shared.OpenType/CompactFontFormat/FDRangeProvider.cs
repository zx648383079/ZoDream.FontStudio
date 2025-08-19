using System;

namespace ZoDream.Shared.OpenType.CompactFontFormat
{
    internal struct FDRangeProvider
    {
        FDRange3[] _ranges;
        ushort _currentGlyphIndex;
        ushort _endGlyphIndexLim;
        byte _selectedFdArray;
        FDRange3 _currentRange;
        int _currentSelectedRangeIndex;

        public FDRangeProvider(FDRange3[] ranges)
        {
            _ranges = ranges;
            _currentGlyphIndex = 0;
            _currentSelectedRangeIndex = 0;

            if (ranges != null)
            {
                _currentRange = ranges[0];
                _endGlyphIndexLim = ranges[1].first;
            }
            else
            {
                //empty
                _currentRange = new FDRange3();
                _endGlyphIndexLim = 0;
            }
            _selectedFdArray = 0;
        }

        public int SelectedFDArray { get; internal set; }

        public void SetCurrentGlyphIndex(ushort index)
        {
            //find proper range for selected index
            if (index >= _currentRange.first && index < _endGlyphIndexLim)
            {
                //ok, in current range
                _selectedFdArray = _currentRange.fd;
            }
            else
            {
                //move to next range
                _currentSelectedRangeIndex++;
                _currentRange = _ranges[_currentSelectedRangeIndex];

                _endGlyphIndexLim = _ranges[_currentSelectedRangeIndex + 1].first;
                if (index >= _currentRange.first && index < _endGlyphIndexLim)
                {
                    _selectedFdArray = _currentRange.fd;

                }
                else
                {
                    throw new NotSupportedException();
                }

            }
            _currentGlyphIndex = index;
        }
    }
}
