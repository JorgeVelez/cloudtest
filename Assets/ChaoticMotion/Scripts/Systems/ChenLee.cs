using UnityEngine;
using System.Collections;

// http://chaoticatmospheres.deviantart.com/art/Strange-Attractors-The-Chen-Lee-Attractor-375986645
public class ChenLee : ChaosEqn  {

	private float a ;	
	private float b ;		
	private float c ;		

	public ChenLee() {
		name = "ChenLee";

		eqnStrings = new string[]{
				"xdot = ax - yz",
				"ydot = by + xz",
				"zdot = cz + xy/3"
		};

		paramBundles = new ParamBundle[] {
			new ParamBundle("default (scaled)", 5f, -10f, -0.38f, 
				new Vector3(1f,1f,1f), new Vector3(-04.1f,-04.4f,-15.5f), 0.15f),
			new ParamBundle("default", 5f, -10f, -0.38f, 
					new Vector3(1f,1f,1f)),
		};

		paramNames = new string[] { "a", "b", "c"};
		slideShowSpeed = 1f;
	}

	public override void SetParams(ParamBundle pb)
 	{
		a = pb.eqnParam[0];
		b = pb.eqnParam[1]; 
		c = pb.eqnParam[2];
	}

	public override void Function(ref float[] x_in, ref float[] x_out) {
		x_out[0] = a*x_in[0]-x_in[1]*x_in[2];
		x_out[1] = b*x_in[1]+x_in[0]*x_in[2];
		x_out[2] = c*x_in[2]+x_in[0]*x_in[1]/3f;
	}

}
