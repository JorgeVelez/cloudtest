using UnityEngine;
using System.Collections;

// eqn (54) from http://lsc.amss.ac.cn/~ljh/04LCC.pdf

public class LorenzNew : ChaosEqn  {

	private float a, b; 
	private float F = 2.5f;	// control parameter
	private float G = 1.4f; 	// control parameter

	public LorenzNew() {
		name = "LorenzNew";

		eqnStrings = new string[]{
				"x = -ax - y^2 - z^2 + aF",
				"y = -y + xy - bxz + G",
				"z = -z + bxy + xz"
		};

		paramBundles = new ParamBundle[] {
			// sigma, b, rho
			new ParamBundle("default (scaled)", new float[]{0.25f, 4f, 2.5f, 1.4f}, 
					new Vector3(0.1f, 0.1f, 0.1f), new Vector3(0f, 0f, -25.2f), 0.22f),
			new ParamBundle("default", new float[]{0.25f, 4f, 1.77f, 1.8f}, 
					new Vector3(1f, 1f, 1f)),
		};

		paramNames = new string[] { "a", "b", "F", "G"};

	}

	public override void SetParams(ParamBundle pb)
 	{
		a = pb.eqnParam[0];
		b = pb.eqnParam[1]; 
		F = pb.eqnParam[2]; 
		G = pb.eqnParam[3]; 
	}

	public override void Function(ref float[] x_in, ref float[] x_out) {
		x_out[0] = -a*x_in[0] - x_in[1]*x_in[1] - x_in[2]*x_in[2] + a*F;
		x_out[1] = -x_in[1] + x_in[0]*x_in[1] - b*x_in[0]*x_in[2] + G;
		x_out[2] = -x_in[2] + b*x_in[0]*x_in[1] + x_in[0]*x_in[2];
	}

}
