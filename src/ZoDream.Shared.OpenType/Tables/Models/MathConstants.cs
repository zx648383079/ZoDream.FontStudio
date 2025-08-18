namespace ZoDream.Shared.OpenType.Tables
{
    public class MathConstants
    {
        /// <summary>
        /// Percentage of scaling down for script level 1. 
        /// Suggested value: 80%.
        /// </summary>
        public short ScriptPercentScaleDown { get; internal set; }
        /// <summary>
        /// Percentage of scaling down for script level 2 (ScriptScript).
        /// Suggested value: 60%.
        /// </summary>
        public short ScriptScriptPercentScaleDown { get; internal set; }
        /// <summary>
        /// Minimum height required for a delimited expression to be treated as a sub-formula.
        /// Suggested value: normal line height ×1.5.
        /// </summary>
        public ushort DelimitedSubFormulaMinHeight { get; internal set; }
        /// <summary>
        ///  	Minimum height of n-ary operators (such as integral and summation) for formulas in display mode.
        /// </summary>
        public ushort DisplayOperatorMinHeight { get; internal set; }


        /// <summary>
        /// White space to be left between math formulas to ensure proper line spacing. 
        /// For example, for applications that treat line gap as a part of line ascender,
        /// formulas with ink going above (os2.sTypoAscender + os2.sTypoLineGap - MathLeading) 
        /// or with ink going below os2.sTypoDescender will result in increasing line height.
        /// </summary>
        public MathValueRecord MathLeading { get; internal set; }
        /// <summary>
        /// Axis height of the font.
        /// </summary>
        public MathValueRecord AxisHeight { get; internal set; }
        /// <summary>
        /// Maximum (ink) height of accent base that does not require raising the accents.
        /// Suggested: x‑height of the font (os2.sxHeight) plus any possible overshots.
        /// </summary>
        public MathValueRecord AccentBaseHeight { get; internal set; }
        /// <summary>
        ///Maximum (ink) height of accent base that does not require flattening the accents. 
        ///Suggested: cap height of the font (os2.sCapHeight).
        /// </summary>
        public MathValueRecord FlattenedAccentBaseHeight { get; internal set; }

        //---------------------------------------------------------
        /// <summary>
        /// The standard shift down applied to subscript elements.
        /// Positive for moving in the downward direction. 
        /// Suggested: os2.ySubscriptYOffset.
        /// </summary>
        public MathValueRecord SubscriptShiftDown { get; internal set; }
        /// <summary>
        /// Maximum allowed height of the (ink) top of subscripts that does not require moving subscripts further down.
        /// Suggested: 4/5 x- height.
        /// </summary>
        public MathValueRecord SubscriptTopMax { get; internal set; }
        /// <summary>
        /// Minimum allowed drop of the baseline of subscripts relative to the (ink) bottom of the base.
        /// Checked for bases that are treated as a box or extended shape. 
        /// Positive for subscript baseline dropped below the base bottom.
        /// </summary>
        public MathValueRecord SubscriptBaselineDropMin { get; internal set; }
        /// <summary>
        /// Standard shift up applied to superscript elements. 
        /// Suggested: os2.ySuperscriptYOffset.
        /// </summary>
        public MathValueRecord SuperscriptShiftUp { get; internal set; }
        /// <summary>
        /// Standard shift of superscripts relative to the base, in cramped style.
        /// </summary>
        public MathValueRecord SuperscriptShiftUpCramped { get; internal set; }
        /// <summary>
        /// Minimum allowed height of the (ink) bottom of superscripts that does not require moving subscripts further up. 
        /// Suggested: ¼ x-height.
        /// </summary>
        public MathValueRecord SuperscriptBottomMin { get; internal set; }
        /// <summary>
        ///  Maximum allowed drop of the baseline of superscripts relative to the (ink) top of the base. Checked for bases that are treated as a box or extended shape. 
        ///  Positive for superscript baseline below the base top.
        /// </summary>
        public MathValueRecord SuperscriptBaselineDropMax { get; internal set; }
        /// <summary>
        /// Minimum gap between the superscript and subscript ink. 
        /// Suggested: 4×default rule thickness.
        /// </summary>
        public MathValueRecord SubSuperscriptGapMin { get; internal set; }
        /// <summary>
        /// The maximum level to which the (ink) bottom of superscript can be pushed to increase the gap between 
        /// superscript and subscript, before subscript starts being moved down. 
        /// Suggested: 4/5 x-height.
        /// </summary>
        public MathValueRecord SuperscriptBottomMaxWithSubscript { get; internal set; }
        /// <summary>
        /// Extra white space to be added after each subscript and superscript. Suggested: 0.5pt for a 12 pt font.
        /// </summary>
        public MathValueRecord SpaceAfterScript { get; internal set; }

        //---------------------------------------------------------
        /// <summary>
        /// Minimum gap between the (ink) bottom of the upper limit, and the (ink) top of the base operator.
        /// </summary>
        public MathValueRecord UpperLimitGapMin { get; internal set; }
        /// <summary>
        /// Minimum distance between baseline of upper limit and (ink) top of the base operator.
        /// </summary>
        public MathValueRecord UpperLimitBaselineRiseMin { get; internal set; }
        /// <summary>
        /// Minimum gap between (ink) top of the lower limit, and (ink) bottom of the base operator.
        /// </summary>
        public MathValueRecord LowerLimitGapMin { get; internal set; }
        /// <summary>
        /// Minimum distance between baseline of the lower limit and (ink) bottom of the base operator.
        /// </summary>
        public MathValueRecord LowerLimitBaselineDropMin { get; internal set; }

        //---------------------------------------------------------
        /// <summary>
        /// Standard shift up applied to the top element of a stack.
        /// </summary>
        public MathValueRecord StackTopShiftUp { get; internal set; }
        /// <summary>
        /// Standard shift up applied to the top element of a stack in display style.
        /// </summary>
        public MathValueRecord StackTopDisplayStyleShiftUp { get; internal set; }
        /// <summary>
        /// Standard shift down applied to the bottom element of a stack. 
        /// Positive for moving in the downward direction.
        /// </summary>
        public MathValueRecord StackBottomShiftDown { get; internal set; }
        /// <summary>
        /// Standard shift down applied to the bottom element of a stack in display style.
        /// Positive for moving in the downward direction.
        /// </summary>
        public MathValueRecord StackBottomDisplayStyleShiftDown { get; internal set; }
        /// <summary>
        /// Minimum gap between (ink) bottom of the top element of a stack, and the (ink) top of the bottom element.
        /// Suggested: 3×default rule thickness.
        /// </summary>
        public MathValueRecord StackGapMin { get; internal set; }
        /// <summary>
        /// Minimum gap between (ink) bottom of the top element of a stack, and the (ink) top of the bottom element in display style.
        /// Suggested: 7×default rule thickness.
        /// </summary>
        public MathValueRecord StackDisplayStyleGapMin { get; internal set; }

        /// <summary>
        /// Standard shift up applied to the top element of the stretch stack.
        /// </summary>
        public MathValueRecord StretchStackTopShiftUp { get; internal set; }
        /// <summary>
        /// Standard shift down applied to the bottom element of the stretch stack.
        /// Positive for moving in the downward direction.
        /// </summary>         
        public MathValueRecord StretchStackBottomShiftDown { get; internal set; }
        /// <summary>
        /// Minimum gap between the ink of the stretched element, and the (ink) bottom of the element above. 
        /// Suggested: UpperLimitGapMin.
        /// </summary>
        public MathValueRecord StretchStackGapAboveMin { get; internal set; }
        /// <summary>
        /// Minimum gap between the ink of the stretched element, and the (ink) top of the element below. 
        /// Suggested: LowerLimitGapMin.
        /// </summary>
        public MathValueRecord StretchStackGapBelowMin { get; internal set; }



        //---------------------------------------------------------
        /// <summary>
        /// Standard shift up applied to the numerator.
        /// </summary>
        public MathValueRecord FractionNumeratorShiftUp { get; internal set; }
        /// <summary>
        /// Standard shift up applied to the numerator in display style. Suggested: StackTopDisplayStyleShiftUp.
        /// </summary>
        public MathValueRecord FractionNumeratorDisplayStyleShiftUp { get; internal set; }
        /// <summary>
        /// Standard shift down applied to the denominator. Positive for moving in the downward direction.
        /// </summary>
        public MathValueRecord FractionDenominatorShiftDown { get; internal set; }
        /// <summary>
        /// Standard shift down applied to the denominator in display style. Positive for moving in the downward direction. 
        /// Suggested: StackBottomDisplayStyleShiftDown
        /// </summary>
        public MathValueRecord FractionDenominatorDisplayStyleShiftDown { get; internal set; }
        /// <summary>
        ///  Minimum tolerated gap between the (ink) bottom of the numerator and the ink of the fraction bar. 
        ///  Suggested: default rule thickness.
        /// </summary>
        public MathValueRecord FractionNumeratorGapMin { get; internal set; }
        /// <summary>
        /// Minimum tolerated gap between the (ink) bottom of the numerator and the ink of the fraction bar in display style. 
        /// Suggested: 3×default rule thickness
        /// </summary>
        public MathValueRecord FractionNumDisplayStyleGapMin { get; internal set; }
        /// <summary>
        /// Thickness of the fraction bar. 
        /// Suggested: default rule thickness.
        /// </summary>
        public MathValueRecord FractionRuleThickness { get; internal set; }
        /// <summary>
        ///  Minimum tolerated gap between the (ink) top of the denominator and the ink of the fraction bar.
        ///  Suggested: default rule thickness.
        /// </summary>
        public MathValueRecord FractionDenominatorGapMin { get; internal set; }
        /// <summary>
        /// Minimum tolerated gap between the (ink) top of the denominator and the ink of the fraction bar in display style. 
        /// Suggested: 3×default rule thickness
        /// </summary>
        public MathValueRecord FractionDenomDisplayStyleGapMin { get; internal set; }



        //---------------------------------------------------------
        /// <summary>
        /// Horizontal distance between the top and bottom elements of a skewed fraction.
        /// </summary>
        public MathValueRecord SkewedFractionHorizontalGap { get; internal set; }
        /// <summary>
        /// Vertical distance between the ink of the top and bottom elements of a skewed fraction
        /// </summary>
        public MathValueRecord SkewedFractionVerticalGap { get; internal set; }



        //---------------------------------------------------------
        /// <summary>
        /// Distance between the overbar and the (ink) top of he base.
        /// Suggested: 3×default rule thickness.
        /// </summary>
        public MathValueRecord OverbarVerticalGap { get; internal set; }
        /// <summary>
        /// Thickness of overbar. 
        /// Suggested: default rule thickness.
        /// </summary>
        public MathValueRecord OverbarRuleThickness { get; internal set; }
        /// <summary>
        /// Extra white space reserved above the overbar. 
        /// Suggested: default rule thickness.
        /// </summary>
        public MathValueRecord OverbarExtraAscender { get; internal set; }



        //---------------------------------------------------------
        /// <summary>
        /// Distance between underbar and (ink) bottom of the base. 
        /// Suggested: 3×default rule thickness.
        /// </summary>
        public MathValueRecord UnderbarVerticalGap { get; internal set; }
        /// <summary>
        /// Thickness of underbar. 
        /// Suggested: default rule thickness.
        /// </summary>
        public MathValueRecord UnderbarRuleThickness { get; internal set; }
        /// <summary>
        /// Extra white space reserved below the underbar. Always positive. 
        /// Suggested: default rule thickness.
        /// </summary>
        public MathValueRecord UnderbarExtraDescender { get; internal set; }



        //---------------------------------------------------------
        /// <summary>
        /// Space between the (ink) top of the expression and the bar over it. 
        /// Suggested: 1¼ default rule thickness.
        /// </summary>
        public MathValueRecord RadicalVerticalGap { get; internal set; }
        /// <summary>
        ///  Space between the (ink) top of the expression and the bar over it. 
        ///  Suggested: default rule thickness + ¼ x-height.
        /// </summary>
        public MathValueRecord RadicalDisplayStyleVerticalGap { get; internal set; }
        /// <summary>
        ///  Thickness of the radical rule. This is the thickness of the rule in designed or constructed radical signs. 
        ///  Suggested: default rule thickness.
        /// </summary>
        public MathValueRecord RadicalRuleThickness { get; internal set; }
        /// <summary>
        /// Extra white space reserved above the radical.
        /// Suggested: RadicalRuleThickness.
        /// </summary>
        public MathValueRecord RadicalExtraAscender { get; internal set; }
        /// <summary>
        /// Extra horizontal kern before the degree of a radical, if such is present.
        /// </summary>
        public MathValueRecord RadicalKernBeforeDegree { get; internal set; }
        /// <summary>
        /// Negative kern after the degree of a radical, if such is present. 
        /// Suggested: −10/18 of em
        /// </summary>
        public MathValueRecord RadicalKernAfterDegree { get; internal set; }
        /// <summary>
        ///  Height of the bottom of the radical degree, 
        ///  if such is present, in proportion to the ascender of the radical sign. 
        ///  Suggested: 60%.
        /// </summary>
        public short RadicalDegreeBottomRaisePercent { get; internal set; }


        //---------------------------------------------------------
        //ONLY this value come from  "MathVariants" *** 
        //I expose that value on this class
        public ushort MinConnectorOverlap { get; internal set; }
    }
}
