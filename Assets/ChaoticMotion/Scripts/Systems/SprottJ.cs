using UnityEngine;
using System.Collections;

// eqn (42) from http://lsc.amss.ac.cn/~ljh/04LCC.pdf
// Primary ref: http://sprott.physics.wisc.edu/pubs/paper212.pdf

public class SprottJ : ChaosEqn  {

	public SprottJ() {
		name = "SprottJ";

		eqnStrings = new string[]{
				"xdot = 2 z",
				"ydot = -2y + z",
				"zdot = -x + y + y^2"
		};

		paramBundles = new ParamBundle[] {
			// sigma, b, rho
			new ParamBundle("default (scaled)", 0f, 0f, 0f, 
					new Vector3(.1f, -1.5f, -3f), new Vector3(-7.9f,-0.1f,0.6f), 0.35f),
			new ParamBundle("default", 0f, 0f, 0f,  
				new Vector3(.1f, -1.5f, -3f)),
		};

		paramNames = new string[] { ChaoticSystem.NO_PARAM, ChaoticSystem.NO_PARAM,ChaoticSystem.NO_PARAM};
		slideShowSpeed = 2f;
	}

	public override void SetParams(ParamBundle pb)
 	{
	}

	public override void Function(ref float[] x_in, ref float[] x_out) {
		x_out[0] = 2f*x_in[2];
		x_out[1] = -2f*x_in[1] + x_in[2];
		x_out[2] = -x_in[0] + x_in[1] + x_in[1]*x_in[1];
	}

}
