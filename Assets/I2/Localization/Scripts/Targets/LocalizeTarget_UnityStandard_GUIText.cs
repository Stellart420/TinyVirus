﻿using UnityEngine;
#pragma warning disable 618

namespace I2.Loc
{
    #if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad] 
    #endif
    public class LocalizeTarget_UnityStandard_GUIText : LocalizeTarget<UnityEngine.UI.Text>
    {
        static LocalizeTarget_UnityStandard_GUIText() { AutoRegister(); }
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] static void AutoRegister() { LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<UnityEngine.UI.Text, LocalizeTarget_UnityStandard_GUIText>() { Name = "GUIText", Priority = 100 }); }

        TextAlignment mAlignment_RTL = TextAlignment.Right;
        TextAlignment mAlignment_LTR = TextAlignment.Left;
        bool mAlignmentWasRTL;
        bool mInitializeAlignment = true;
        
        public override eTermType GetPrimaryTermType(Localize cmp) { return eTermType.Text; }
        public override eTermType GetSecondaryTermType(Localize cmp) { return eTermType.Font; }
        public override bool CanUseSecondaryTerm() { return true; }
        public override bool AllowMainTermToBeRTL() { return true; }
        public override bool AllowSecondTermToBeRTL() { return false; }

        public override void GetFinalTerms ( Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
        {
            primaryTerm = mTarget ? mTarget.text : null;
            secondaryTerm = (string.IsNullOrEmpty(Secondary) && mTarget.font != null) ? mTarget.font.name : null;
        }

        public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
        {
            //--[ Localize Font Object ]----------
            Font newFont = cmp.GetSecondaryTranslatedObj<Font>(ref mainTranslation, ref secondaryTranslation);
            if (newFont != null && mTarget.font != newFont)
                mTarget.font = newFont;

            //--[ Localize Text ]----------
            if (mInitializeAlignment)
            {
                mInitializeAlignment = false;

                mAlignment_LTR = mAlignment_RTL = (TextAlignment)mTarget.alignment;

                if (LocalizationManager.IsRight2Left && mAlignment_RTL == TextAlignment.Right)
                    mAlignment_LTR = TextAlignment.Left;
                if (!LocalizationManager.IsRight2Left && mAlignment_LTR == TextAlignment.Left)
                    mAlignment_RTL = TextAlignment.Right;

            }
            if (mainTranslation != null && mTarget.text != mainTranslation)
            {
                if (cmp.CorrectAlignmentForRTL && (TextAlignment)mTarget.alignment != TextAlignment.Center)
                    mTarget.alignment = (TextAnchor)(LocalizationManager.IsRight2Left ? mAlignment_RTL : mAlignment_LTR);

                mTarget.text = mainTranslation;
            }
        }
    }
}
