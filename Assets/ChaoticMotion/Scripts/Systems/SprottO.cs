using UnityEngine;
using System.Collections;

// eqn (47) from http://lsc.amss.ac.cn/~ljh/04LCC.pdf
// Primary ref: http://sprott.physics.wisc.edu/pubs/paper212.pdf

public class SprottO : ChaosEqn  {

	public SprottO() {
		name = "SprottO";

		eqnStrings = new string[]{
				"xdot = y",
				"ydot = x - z",
				"zdot = x + xz + 2.7y"
		};

		paramBundles = new ParamBundle[] {
			// sigma, b, rho
			new ParamBundle("default (scaled)", 0f, 0f, 0f, 
				new Vector3(.1f, .1f, 0f), new Vector3(000.4f,000.2f,000.3f), 03.35f),
			new ParamBundle("default", 0f, 0f, 0f, 
					new Vector3(.1f, .1f, 0f)),
		};

		paramNames = new string[] { ChaoticSystem.NO_PARAM, ChaoticSystem.NO_PARAM,ChaoticSystem.NO_PARAM};
		slideShowSpeed = 2f;
	}

	public override void SetParams(ParamBundle pb)
 	{
	}

	public override void Function(ref float[] x_in, ref float[] x_out) {
		x_out[0] = x_in[1];
		x_out[1] = x_in[0] - x_in[2];
		x_out[2] = x_in[0] + x_in[0]*x_in[2] + 2.7f*x_in[1];
	}

}
