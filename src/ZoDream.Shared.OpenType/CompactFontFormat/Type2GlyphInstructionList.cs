using System;
using System.Collections.Generic;

namespace ZoDream.Shared.OpenType.CompactFontFormat
{
    public class Type2GlyphInstructionList
    {
        List<Type2Instruction> _insts;

        public Type2GlyphInstructionList()
        {
            _insts = new List<Type2Instruction>();
        }

        public Type2Instruction RemoveLast()
        {
            int last = _insts.Count - 1;
            Type2Instruction _lastInst = _insts[last];
            _insts.RemoveAt(last);
            return _lastInst;
        }
        //
        public void AddInt(int intValue)
        {
            _insts.Add(new Type2Instruction(Operand.LoadInt, intValue));
        }
        public void AddFloat(int float1616Fmt)
        {
            _insts.Add(new Type2Instruction(Operand.LoadFloat, float1616Fmt));
        }
        public void AddOp(Operand opName)
        {
            _insts.Add(new Type2Instruction(opName));
        }

        public void AddOp(Operand opName, int value)
        {
            _insts.Add(new Type2Instruction(opName, value));
        }
        public int Count => _insts.Count;
        internal void ChangeFirstInstToGlyphWidthValue()
        {
            //check the first element must be loadint
            if (_insts.Count == 0) return;

            Type2Instruction firstInst = _insts[0];
            if (!firstInst.IsLoadInt) { throw new NotSupportedException(); }
            //the replace
            _insts[0] = new Type2Instruction(Operand.GlyphWidth, firstInst.Value);
        }




        internal List<Type2Instruction> InnerInsts => _insts;
    }
}
