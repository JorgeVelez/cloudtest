﻿using UnityEngine;
using System.Collections;

// eqn (39) from http://lsc.amss.ac.cn/~ljh/04LCC.pdf
// Primary ref: http://sprott.physics.wisc.edu/pubs/paper212.pdf

public class SprottG : ChaosEqn  {

	public SprottG() {
		name = "SprottG";

		eqnStrings = new string[]{
				"xdot = 0.4x + z",
				"ydot = xz - y",
				"zdot = -x + y"
		};

		paramBundles = new ParamBundle[] {
			// sigma, b, rho
			new ParamBundle("default (scaled)", 0f, 0f, 0f, 
					new Vector3(.1f, .1f, .1f), new Vector3(0.7f, 1.2f, -0.2f), 2.59f),
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
		x_out[0] = 0.4f*x_in[0] + x_in[2];
		x_out[1] = x_in[0]*x_in[2] - x_in[1];
		x_out[2] = -x_in[0] + x_in[1];
	}

}
