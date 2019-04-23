using UnityEngine;
using System.Collections;

// eqn (26) from http://lsc.amss.ac.cn/~ljh/04LCC.pdf

public class Lorenz : ChaosEqn  {

	private float sigma = 10f;		// value Lorenz used
	private float b = 8f/3f;		// value Lorenz used
	private float rho = 28f; 		// guess 

	public Lorenz() {
		name = "Lorenz";

		eqnStrings = new string[]{
				"x = sigma (y - x)",
				"y = - x z + rho x - y",
				"z = x y - b z"
		};

		paramBundles = new ParamBundle[] {
			// sigma, b, rho
			new ParamBundle("default (scaled)", 10f, 8f/3f, 28f, 
				new Vector3(2f, 0.5f, 23f), new Vector3(0f, 0f, -25.2f), 0.22f),
			new ParamBundle("default", 10f, 8f/3f, 28f, 
					new Vector3(0.1f, 0.1f, 0.1f)),
		};

		paramNames = new string[] { "sigma", "b", "rho"};

	}

	public override void SetParams(ParamBundle pb)
 	{
		sigma = pb.eqnParam[0];
		b = pb.eqnParam[1]; 
		rho = pb.eqnParam[2];
	}

	public override void Function(ref float[] x_in, ref float[] x_out) {
		x_out[0] = sigma * (x_in[1] - x_in[0]);
		x_out[1] = -1f * x_in[0]*x_in[2] + rho * x_in[0] - x_in[1];
		x_out[2] = x_in[0]*x_in[1] - b * x_in[2];
	}

}
