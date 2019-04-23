using UnityEngine;
using System.Collections;

// http://chaoticatmospheres.deviantart.com/art/Strange-Attractors-The-Hadley-Attractor-376133780
public class Hadley : ChaosEqn  {

	private float alpha ;	
	private float beta ;		
	private float zeta;		
	private float delta ;

	public Hadley() {
		name = "Hadley";

		eqnStrings = new string[]{
				"xdot = -y^2 -z^2 - alpha x + alpha zeta",
				"ydot = xy - beta xz - y + delta",
				"zdot = beta x y + xz -z"
		};

		paramBundles = new ParamBundle[] {
			new ParamBundle("default (scaled)", new float[] {0.2f, 4f, 8f, 1f}, 
					new Vector3(0.1f, 0, 0), new Vector3(-01.1f,00.2f,-00.4f), 2.97f),
			new ParamBundle("default", new float[] {0.2f, 4f, 8f, 1f}, 
					new Vector3(0.1f, 0, 0)),
		};

		paramNames = new string[] { "alpha", "beta", "zeta", "delta"};
		slideShowSpeed = 1f;
	}

	public override void SetParams(ParamBundle pb)
 	{
		alpha 	= pb.eqnParam[0];
		beta 	= pb.eqnParam[1]; 
		zeta 	= pb.eqnParam[2];
		delta 	= pb.eqnParam[3];
	}

	public override void Function(ref float[] x_in, ref float[] x_out) {
		x_out[0] = -x_in[1]*x_in[1] - x_in[2]*x_in[2] - alpha*x_in[0] + alpha*zeta;
		x_out[1] = x_in[0]*x_in[1] - beta*x_in[0]*x_in[2] - x_in[1] + delta;
		x_out[2] = beta*x_in[0]*x_in[1]+x_in[0]*x_in[2] - x_in[2];
	}

}
