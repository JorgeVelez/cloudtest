using UnityEngine;
using System.Collections;

// eqn (45) from http://lsc.amss.ac.cn/~ljh/04LCC.pdf
// Primary ref: http://sprott.physics.wisc.edu/pubs/paper212.pdf

public class SprottM : ChaosEqn  {

	public SprottM() {
		name = "SprottM";

		eqnStrings = new string[]{
				"xdot = - z",
				"ydot = -x^2 - y ",
				"zdot = 1.7 + 1.7x + y"
		};

		paramBundles = new ParamBundle[] {
			// sigma, b, rho
			new ParamBundle("default (scaled)", 0f, 0f, 0f, 
					new Vector3(.1f, .1f, .1f), new Vector3(-0.3f,2.5f,0.5f), 1.79f),
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
		x_out[1] = -x_in[0]*x_in[0] - x_in[1];
		x_out[2] = 1.7f + 1.7f*x_in[0] + x_in[1];
	}

}
