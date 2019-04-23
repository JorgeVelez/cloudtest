using UnityEngine;
using System.Collections;


// eqn (28) from http://lsc.amss.ac.cn/~ljh/04LCC.pdf
public class LuChenCheng : ChaosEqn  {

	private float a ;	
	private float b ;		

	public LuChenCheng() {
		name = "LuChenCheng";

		eqnStrings = new string[]{
				"xdot = -(ab)/(a+b)x - yz",
				"ydot = ay + xy",
				"zdot = bz + xy"
		};

		paramBundles = new ParamBundle[] {
			// sigma, b, rho
			new ParamBundle("default (scaled)", -6f, -15f, 0f,  
					new Vector3(1f, 0f, 1f), new Vector3(-3.4f, -20.1f, -4f ), 0.12f),
			new ParamBundle("default", -6f, -15f, 0f,  
					new Vector3(1f, 0f, 1f)),
		};

		paramNames = new string[] { "a", "b", ChaoticSystem.NO_PARAM};

	}

	public override void SetParams(ParamBundle pb)
 	{
		a = pb.eqnParam[0];
		b = pb.eqnParam[1]; 
	}

	public override void Function(ref float[] x_in, ref float[] x_out) {
		x_out[0] = -(a*b)*x_in[0]/(a+b) - x_in[1]*x_in[2];
		x_out[1] = a*x_in[1] + x_in[0]*x_in[2];
		x_out[2] = b*x_in[2] + x_in[0]*x_in[1];
	}

}
