using UnityEngine;
using System.Collections;

// eqn (34) from http://lsc.amss.ac.cn/~ljh/04LCC.pdf
// Primary ref: http://sprott.physics.wisc.edu/pubs/paper212.pdf

public class SprottB : ChaosEqn  {

	public SprottB() {
		name = "SprottB";

		eqnStrings = new string[]{
				"xdot = yz",
				"ydot = x-y",
				"zdot = 1 - xy"
		};

		paramBundles = new ParamBundle[] {
			// sigma, b, rho
			new ParamBundle("default (scaled)", 0f, 0f, 0f, 
					new Vector3(-0.1f, 0.5f, -0.6f), new Vector3(-0.8f,-0.7f,0.3f), 1.51f),
			new ParamBundle("default", 0f, 0f, 0f, 
					new Vector3(-0.1f, 0.5f, -0.6f)),
		};

		paramNames = new string[] { ChaoticSystem.NO_PARAM, ChaoticSystem.NO_PARAM,ChaoticSystem.NO_PARAM};
		slideShowSpeed = 2f;
	}

	public override void SetParams(ParamBundle pb)
 	{
	}

	public override void Function(ref float[] x_in, ref float[] x_out) {
		x_out[0] = x_in[1] * x_in[2];
		x_out[1] = x_in[0] - x_in[1];
		x_out[2] = 1 - x_in[0]*x_in[1];
	}

}
