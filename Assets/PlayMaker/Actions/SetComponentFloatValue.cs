using System;
using System.Reflection;
using UnityEngine;

// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
    
	[ActionCategory(ActionCategory.Math)]
	[Tooltip("Sets the value of a Float Variable on an attached component.")]
	public class SetComponentFloatValue : FsmStateAction
	{
	    [RequiredField]
		public FsmOwnerDefault gameObject;
	
		[RequiredField]
		[UIHint(UIHint.Behaviour)]
		public FsmString behaviour;
		
		[RequiredField]
		public FsmString floatMember;
		
		[RequiredField]
		public FsmFloat floatValue;
		public bool everyFrame;

		public override void Reset()
		{
		    gameObject = null;
		    behaviour = null;
		    floatMember = null;
		    
			floatValue = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoUpdate();
			
			if (!everyFrame)
				Finish();		
		}

		public override void OnUpdate()
		{
		    if (everyFrame)
    			DoUpdate();
		}
		
		delegate void valsetter(float val);
		private valsetter setter = null;
		
		void DoUpdate() {
		    if (setter == null) {		
                var go = Fsm.GetOwnerDefaultTarget(gameObject);
                if (go != null) {
                    var b = go.GetComponent(behaviour.Value);
                    if (b != null) {
                        FieldInfo fi = b.GetType().GetField(floatMember.Value);
                        PropertyInfo pi = b.GetType().GetProperty(floatMember.Value);
                        if (fi != null) {
                            if (fi.FieldType != typeof(float))
                                LogError("Component member is not a float: " + floatMember.Value);
                            setter = (float a) => { fi.SetValue(b,a); };
                        } else if (pi != null){
                            if (pi.PropertyType != typeof(float))
                                LogError("Component member is not a float: " + floatMember.Value);
                            setter = (float a) => { pi.SetValue(b,a,null); };                            
                        } else {
                            LogError("Component does not contain float member: " + floatMember.Value);
                        }
                    }
                }
			}
			if (setter != null) {
			    setter(floatValue.Value);
			}
		}
	}
}