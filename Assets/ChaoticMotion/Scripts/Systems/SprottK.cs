﻿using UnityEngine;
using System.Collections;

// eqn (43) from http://lsc.amss.ac.cn/~ljh/04LCC.pdf
// Primary ref: http://sprott.physics.wisc.edu/pubs/paper212.pdf

public class SprottK : ChaosEqn  {

	public SprottK() {
		name = "SprottK";

		eqnStrings = new string[]{
				"xdot = xy - z",
				"ydot = x - y ",
				"zdot = x + 0.3z"
		};

		paramBundles = new ParamBundle[] {
			// sigma, b, rho
			new ParamBundle("default (scaled)", 0f, 0f, 0f, 
					new Vector3(.1f, .1f, .1f), new Vector3(0.7f,0.3f,-1.3f), 2.41f),
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
		x_out[0] = x_in[0]*x_in[1] - x_in[2];
		x_out[1] = x_in[0] - x_in[1];
		x_out[2] = x_in[0] + 0.3f*x_in[2];
	}

}