using UnityEngine;
using System.Collections;

// http://chaoticatmospheres.deviantart.com/art/Strange-Attractors-The-Thomas-Attractor-376574123
// (has a typo - see https://en.wikipedia.org/wiki/Thomas%27_cyclically_symmetric_attractor)
public class Thomas : ChaosEqn  {

	private float beta ;	

	public Thomas() {
		name = "Thomas";

		eqnStrings = new string[]{
				"xdot = -beta x + sin(y)",
				"ydot = -beta y + sin(z)",
				"zdot = -beta z + sin(x)"
		};

		paramBundles = new ParamBundle[] {
			new ParamBundle("default (scaled)", new float[] {0.19f}, 
				new Vector3(0.1f, 0, 0), new  Vector3(-00.5f,-00.3f,00.3f), 1.32f),
			new ParamBundle("default", new float[] {0.19f}, 
					new Vector3(0.1f, 0, 0)),
		};

		paramNames = new string[] { "beta"};
		slideShowSpeed = 8f;
	}

	public override void SetParams(ParamBundle pb)
 	{
		beta 	= pb.eqnParam[0];
	}

	public override void Function(ref float[] x_in, ref float[] x_out) {
		x_out[0] = -beta*x_in[0] + Mathf.Sin(x_in[1]);
		x_out[1] = -beta*x_in[1] + Mathf.Sin(x_in[2]);
		x_out[2] = -beta*x_in[2] + Mathf.Sin(x_in[0]);
	}

}
