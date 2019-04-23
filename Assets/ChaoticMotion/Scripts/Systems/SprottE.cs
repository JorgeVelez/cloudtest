﻿using UnityEngine;
using System.Collections;

// eqn (37) from http://lsc.amss.ac.cn/~ljh/04LCC.pdf
// Primary ref: http://sprott.physics.wisc.edu/pubs/paper212.pdf

public class SprottE : ChaosEqn  {

	public SprottE() {
		name = "SprottE";

		eqnStrings = new string[]{
				"xdot = yz",
				"ydot = x^2 - y",
				"zdot = 1 - 4x"
		};

		paramBundles = new ParamBundle[] {
			// sigma, b, rho
			new ParamBundle("default (scaled)", 0f, 0f, 0f, 
					new Vector3(-2.9f, 4.8f, .4f)),
			new ParamBundle("default", 0f, 0f, 0f, 
					new Vector3(-2.9f, 4.8f, .4f)),
		};

		paramNames = new string[] { ChaoticSystem.NO_PARAM, ChaoticSystem.NO_PARAM,ChaoticSystem.NO_PARAM};
		slideShowSpeed = 2f;
	}

	public override void SetParams(ParamBundle pb)
 	{
	}

	public override void Function(ref float[] x_in, ref float[] x_out) {
		x_out[0] = x_in[1]*x_in[2];
		x_out[1] = x_in[0]*x_in[0] - x_in[1];
		x_out[2] = 1f - 4f*x_in[0];
	}

}