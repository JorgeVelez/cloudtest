using UnityEngine;
using System.Collections;

// eqn (41) from http://lsc.amss.ac.cn/~ljh/04LCC.pdf
// Primary ref: http://sprott.physics.wisc.edu/pubs/paper212.pdf

public class SprottI : ChaosEqn  {

	public SprottI() {
		name = "SprottI";

		eqnStrings = new string[]{
				"xdot = -0.2 y",
				"ydot = x + z",
				"zdot = x + y^2 - z"
		};

		paramBundles = new ParamBundle[] {
			// sigma, b, rho
			new ParamBundle("default (scaled)", 0f, 0f, 0f, 
					new Vector3(.1f, .1f, .1f), new Vector3(0.2f,0f,0f), 7.06f),
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
		x_out[0] = -0.2f*x_in[1];
		x_out[1] = x_in[0] + x_in[2];
		x_out[2] = x_in[0] + x_in[1]*x_in[1] - x_in[2];
	}

}
