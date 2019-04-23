using UnityEngine;
using System.Collections;

// eqn (49) from http://lsc.amss.ac.cn/~ljh/04LCC.pdf
// Primary ref: http://sprott.physics.wisc.edu/pubs/paper212.pdf

public class SprottQ : ChaosEqn  {

	public SprottQ() {
		name = "SprottQ";

		eqnStrings = new string[]{
				"xdot = - z",
				"ydot = x - y",
				"zdot = 3.1x + y^2 +0.5z"
		};

		paramBundles = new ParamBundle[] {
			// sigma, b, rho
			new ParamBundle("default (scaled)", 0f, 0f, 0f, 
				new Vector3(.1f, .1f, .1f)), // ok unscaled
			new ParamBundle("default", 0f, 0f, 0f, 
					new Vector3(.1f, .1f, .1f)),
		};

		paramNames = new string[] { ChaoticSystem.NO_PARAM, ChaoticSystem.NO_PARAM,ChaoticSystem.NO_PARAM};
		slideShowSpeed = 2f;
	}

	public override void SetParams(ParamBundle pb)
 	{
	}

	public override void Function(ref float[] x_in, ref float[] x_out) {
		x_out[0] = - x_in[2];
		x_out[1] = x_in[0] - x_in[1];
		x_out[2] = 3.1f*x_in[0] + x_in[1]*x_in[1] + 0.5f*x_in[2];
	}

}
