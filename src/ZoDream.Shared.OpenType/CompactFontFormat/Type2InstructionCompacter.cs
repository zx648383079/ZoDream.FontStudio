using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.Shared.OpenType.CompactFontFormat
{
    public class Type2InstructionCompacter
    {
        List<Type2Instruction> _step1List;
        List<Type2Instruction> _step2List;

        void CompactStep1OnlyLoadInt(List<Type2Instruction> insts)
        {
            int j = insts.Count;
            CompactRange _latestCompactRange = CompactRange.None;
            int startCollectAt = -1;
            int collecting_count = 0;
            void FlushWaitingNumbers()
            {
                //Nested method
                //flush waiting integer
                if (_latestCompactRange == CompactRange.Short)
                {
                    switch (collecting_count)
                    {
                        default: throw new NotSupportedException();
                        case 0: break; //nothing
                        case 2:
                            _step1List.Add(new Type2Instruction(Operand.LoadShort2,
                                      (((ushort)insts[startCollectAt].Value) << 16) |
                                      (((ushort)insts[startCollectAt + 1].Value))
                                      ));
                            startCollectAt += 2;
                            collecting_count -= 2;
                            break;
                        case 1:
                            _step1List.Add(insts[startCollectAt]);
                            startCollectAt += 1;
                            collecting_count -= 1;
                            break;

                    }
                }
                else
                {
                    switch (collecting_count)
                    {
                        default: throw new NotSupportedException();
                        case 0: break;//nothing
                        case 4:
                            {
                                _step1List.Add(new Type2Instruction(Operand.LoadSbyte4,
                                   (((byte)insts[startCollectAt].Value) << 24) |
                                   (((byte)insts[startCollectAt + 1].Value) << 16) |
                                   (((byte)insts[startCollectAt + 2].Value) << 8) |
                                   (((byte)insts[startCollectAt + 3].Value) << 0)
                                   ));
                                startCollectAt += 4;
                                collecting_count -= 4;
                            }
                            break;
                        case 3:
                            _step1List.Add(new Type2Instruction(Operand.LoadSbyte3,
                                (((byte)insts[startCollectAt].Value) << 24) |
                                (((byte)insts[startCollectAt + 1].Value) << 16) |
                                (((byte)insts[startCollectAt + 2].Value) << 8)
                                ));
                            startCollectAt += 3;
                            collecting_count -= 3;
                            break;
                        case 2:
                            _step1List.Add(new Type2Instruction(Operand.LoadShort2,
                              (((ushort)insts[startCollectAt].Value) << 16) |
                              ((ushort)insts[startCollectAt + 1].Value)
                              ));
                            startCollectAt += 2;
                            collecting_count -= 2;
                            break;
                        case 1:
                            _step1List.Add(insts[startCollectAt]);
                            startCollectAt += 1;
                            collecting_count -= 1;
                            break;

                    }
                }

                startCollectAt = -1;
                collecting_count = 0;
            }

            for (int i = 0; i < j; ++i)
            {
                Type2Instruction inst = insts[i];
                if (inst.IsLoadInt)
                {
                    //check waiting data in queue
                    //get compact range
                    CompactRange c1 = GetCompactRange(inst.Value);
                    switch (c1)
                    {
                        default: throw new NotSupportedException();
                        case CompactRange.None:
                            {
                                if (collecting_count > 0)
                                {
                                    FlushWaitingNumbers();
                                }
                                _step1List.Add(inst);
                                _latestCompactRange = CompactRange.None;
                            }
                            break;
                        case CompactRange.SByte:
                            {
                                if (_latestCompactRange == CompactRange.Short)
                                {
                                    FlushWaitingNumbers();
                                    _latestCompactRange = CompactRange.SByte;
                                }

                                switch (collecting_count)
                                {
                                    default: throw new NotSupportedException();
                                    case 0:
                                        startCollectAt = i;
                                        _latestCompactRange = CompactRange.SByte;
                                        break;
                                    case 1:
                                        break;
                                    case 2:
                                        break;
                                    case 3:
                                        //we already have 3 bytes
                                        //so this is 4th byte
                                        collecting_count++;
                                        FlushWaitingNumbers();
                                        continue;
                                }
                                collecting_count++;
                            }
                            break;
                        case CompactRange.Short:
                            {
                                if (_latestCompactRange == CompactRange.SByte)
                                {
                                    FlushWaitingNumbers();
                                    _latestCompactRange = CompactRange.Short;
                                }

                                switch (collecting_count)
                                {
                                    default: throw new NotSupportedException();
                                    case 0:
                                        startCollectAt = i;
                                        _latestCompactRange = CompactRange.Short;
                                        break;
                                    case 1:
                                        //we already have 1 so this is 2nd 
                                        collecting_count++;
                                        FlushWaitingNumbers();
                                        continue;
                                }

                                collecting_count++;
                            }
                            break;
                    }
                }
                else
                {
                    //other cmds
                    //flush waiting cmd
                    if (collecting_count > 0)
                    {
                        FlushWaitingNumbers();
                    }

                    _step1List.Add(inst);
                    _latestCompactRange = CompactRange.None;
                }
            }
        }
        static byte IsLoadIntOrMergeableLoadIntExtension(Operand opName)
        {
            return opName switch
            {
                //case Operand.LoadSbyte3://except LoadSbyte3 ***                
                //merge-able
                Operand.LoadInt => 1,
                //merge-able
                Operand.LoadShort2 => 2,
                //merge-able
                Operand.LoadSbyte4 => 3,
                _ => 0,
            };
        }
        void CompactStep2MergeLoadIntWithNextCommand()
        {
            //a second pass
            //check if we can merge some load int( LoadInt, LoadSByte4, LoadShort2) except LoadSByte3 
            //to next instruction command or not
            int j = _step1List.Count;
            for (int i = 0; i < j; ++i)
            {
                Type2Instruction i0 = _step1List[i];

                if (i + 1 < j)
                {
                    //has next cmd           
                    byte merge_flags = IsLoadIntOrMergeableLoadIntExtension((Operand)i0.Op);
                    if (merge_flags > 0)
                    {
                        Type2Instruction i1 = _step1List[i + 1];
                        //check i1 has empty space for i0 or not
                        bool canbe_merged = false;
                        switch ((Operand)i1.Op)
                        {
                            case Operand.LoadInt:
                            case Operand.LoadShort2:
                            case Operand.LoadSbyte4:
                            case Operand.LoadSbyte3:
                            case Operand.LoadFloat:

                            case Operand.hintmask1:
                            case Operand.hintmask2:
                            case Operand.hintmask3:
                            case Operand.hintmask4:
                            case Operand.hintmask_bits:
                            case Operand.cntrmask1:
                            case Operand.cntrmask2:
                            case Operand.cntrmask3:
                            case Operand.cntrmask4:
                            case Operand.cntrmask_bits:
                                break;
                            default:
                                canbe_merged = true;
                                break;
                        }
                        if (canbe_merged)
                        {

#if DEBUG
                            if (merge_flags > 3) { throw new NotSupportedException(); }
#endif

                            _step2List.Add(new Type2Instruction((byte)((merge_flags << 6) | i1.Op), i0.Value));
                            i += 1;
                        }
                        else
                        {
                            _step2List.Add(i0);
                        }
                    }
                    else
                    {
                        //this is the last one
                        _step2List.Add(i0);
                    }

                }
                else
                {
                    //this is the last one
                    _step2List.Add(i0);
                }
            }
        }
        public Type2Instruction[] Compact(List<Type2Instruction> insts)
        {
            //for simpicity
            //we have 2 passes
            //1. compact consecutive numbers
            //2. compact other cmd

            //reset
            if (_step1List == null)
            {
                _step1List = new List<Type2Instruction>();
            }
            if (_step2List == null)
            {
                _step2List = new List<Type2Instruction>();
            }
            _step1List.Clear();
            _step2List.Clear();
            //
            CompactStep1OnlyLoadInt(insts);
            CompactStep2MergeLoadIntWithNextCommand();
            return _step2List.ToArray();
        }

        static CompactRange GetCompactRange(int value)
        {
            if (value > sbyte.MinValue && value < sbyte.MaxValue)
            {
                return CompactRange.SByte;
            }
            else if (value > short.MinValue && value < short.MaxValue)
            {
                return CompactRange.Short;
            }
            else
            {
                return CompactRange.None;
            }
        }
    }
}
