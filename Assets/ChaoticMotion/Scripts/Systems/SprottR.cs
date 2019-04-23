using UnityEngine;
using System.Collections;

// eqn (50) from http://lsc.amss.ac.cn/~ljh/04LCC.pdf
// Primary ref: http://sprott.physics.wisc.edu/pubs/paper212.pdf

public class SprottR : ChaosEqn  {

	public SprottR() {
		name = "SprottR";

		eqnStrings = new string[]{
				"xdot = 0.9 - y",
				"ydot = 0.4 + z",
				"zdot = xy - z"
		};

		paramBundles = new ParamBundle[] {
			// sigma, b, rho
			new ParamBundle("default (scaled)", 0f, 0f, 0f, 
				new Vector3(.1f, .1f, .1f), new Vector3(1.5f,-002.1f,004.6f), 0.49f),
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
		x_out[0] = 0.9f - x_in[1];
		x_out[1] = 0.4f + x_in[2];
		x_out[2] = x_in[0]*x_in[1] - x_in[2];
	}

}
