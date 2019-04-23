using UnityEngine;
using System.Collections;

// eqn (44) from http://lsc.amss.ac.cn/~ljh/04LCC.pdf
// Primary ref: http://sprott.physics.wisc.edu/pubs/paper212.pdf

public class SprottL : ChaosEqn  {

	public SprottL() {
		name = "SprottL";

		eqnStrings = new string[]{
				"xdot = y + 3.9z",
				"ydot = 0.9x^2 - y ",
				"zdot = 1 - x"
		};

		paramBundles = new ParamBundle[] {
			// sigma, b, rho
			new ParamBundle("default (scaled)", 0f, 0f, 0f, 
				new Vector3(.1f, .1f, .1f), new Vector3(0.3f,-10.5f,3.4f), 0.35f),
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
		x_out[0] = x_in[1] + 3.9f*x_in[2];
		x_out[1] = 0.9f*x_in[0]*x_in[0] - x_in[1];
		x_out[2] = 1 - x_in[0];
	}

}
