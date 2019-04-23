using UnityEngine;
using System.Collections;

// from http://chaoticatmospheres.com/mathrules-strange-attractors
public class Anishchenko_Astakhov : ChaosEqn  {

	private float mu ;	
	private float eta ;		

	public Anishchenko_Astakhov() {
		name = "Anishchenko_Astakhov";

		eqnStrings = new string[]{
				"xdot = mu x + y - x z",
				"ydot = -x",
				"zdot = -eta z + eta I(x) x^2"
		};

		paramBundles = new ParamBundle[] {
			new ParamBundle("default (scaled)", 1.2f, 0.5f, 0f, 
				new Vector3(0.1f, 0f, 0f), new Vector3(-00.6f,-02.3f,-01.9f), 1.54f),
			new ParamBundle("default", 1.2f, 0.5f, 0f, 
					new Vector3(0.1f, 0f, 0f)),
		};

		paramNames = new string[] { "mu", "eta"};
		slideShowSpeed = 2.5f;
	}

	public override void SetParams(ParamBundle pb)
 	{
		mu = pb.eqnParam[0];
		eta = pb.eqnParam[1]; 
	}

	public override void Function(ref float[] x_in, ref float[] x_out) {
		x_out[0] = mu*x_in[0] + x_in[1] - x_in[0]*x_in[2];
		x_out[1] = -x_in[0];
		float I = (x_in[0] > 0) ? 1f: 0f;
		x_out[2] = -eta* x_in[2] + eta * I * x_in[0]*x_in[0];
	}

}
