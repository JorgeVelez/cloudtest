using UnityEngine;
using System.Collections;

// http://chaoticatmospheres.deviantart.com/art/Strange-Attractors-The-Halvorsen-Attractor-376173330
public class Halvorsen : ChaosEqn  {

	private float alpha ;	

	public Halvorsen() {
		name = "Halvorsen";

		eqnStrings = new string[]{
				"xdot = -alpha x - 4y -4z -y^2",
				"ydot = -alpha y - 4z - 4x - z^2",
				"zdot = -alpha z - 4x -4y -x^2"
		};

		paramBundles = new ParamBundle[] {
			new ParamBundle("default (scaled)", new float[] {1.4f}, 
					new Vector3(0.1f, 0, 0), new Vector3(02.9f,03.5f,03.5f), 0.51f),
			new ParamBundle("default", new float[] {1.4f}, 
					new Vector3(0.1f, 0, 0)),
		};

		paramNames = new string[] { "alpha"};
		slideShowSpeed = 1f;
	}

	public override void SetParams(ParamBundle pb)
 	{
		alpha 	= pb.eqnParam[0];
	}

	public override void Function(ref float[] x_in, ref float[] x_out) {
		x_out[0] = -alpha*x_in[0] - 4f*x_in[1] - 4f*x_in[2] - x_in[1]*x_in[1];
		x_out[1] = -alpha*x_in[1] - 4f*x_in[2] - 4f*x_in[0] - x_in[2]*x_in[2];
		x_out[2] = -alpha*x_in[2] - 4f*x_in[0] - 4f*x_in[1] - x_in[0]*x_in[0];
	}

}
