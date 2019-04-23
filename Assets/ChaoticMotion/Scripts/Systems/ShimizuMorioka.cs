using UnityEngine;
using System.Collections;

// eqn (53) from http://lsc.amss.ac.cn/~ljh/04LCC.pdf

public class ShimizuMorioka : ChaosEqn  {

	private float a, b;

	public ShimizuMorioka() {
		name = "ShimizuMorioka";

		eqnStrings = new string[]{
				"xdot = y",
				"ydot = x - ay - xz",
				"zdot = -bz + x^2"
		};

		paramBundles = new ParamBundle[] {
			// sigma, b, rho
			new ParamBundle("default (scaled)", 0.85f, 0.5f, 0f, 
				new Vector3(.1f, .1f, .1f), new Vector3(-000.1f,000.2f,-001.1f), 04.23f),
			new ParamBundle("default", 0.85f, 0.5f, 0f, 
					new Vector3(.1f,.1f,.1f)),
		};

		paramNames = new string[] { "a", "b", ChaoticSystem.NO_PARAM};

	}

	public override void SetParams(ParamBundle pb)
 	{
		a = pb.eqnParam[0];
		b = pb.eqnParam[1]; 
	}

	public override void Function(ref float[] x_in, ref float[] x_out) {
		x_out[0] = x_in[1];
		x_out[1] = x_in[0] - a*x_in[1] - x_in[0]*x_in[2];
		x_out[2] = -b*x_in[2] + x_in[0]*x_in[0];
	}

}
