using UnityEngine;
using System.Collections;

// see http://chaoticatmospheres.deviantart.com/art/Strange-Attractors-The-2nd-Bouali-Attractor-375986029
public class Bouali2 : ChaosEqn  {

	private float a;	
	private float b;		

	public Bouali2() {
		name = "Bouali2";

		eqnStrings = new string[]{
				"xdot = x(4-y)+az",
				"ydot = -y(1-x^2)",
				"zdot = -x(1.5 - bz) - 0.05z "
		};

		paramBundles = new ParamBundle[] {
			new ParamBundle("default", new float[]{0.3f, 1f}, 
					new Vector3(0f, 0, 0.1f)),
//			new ParamBundle("default", new float[]{0.3f, 1f}, 
//					new Vector3(0.5f, -1f, 0.5f)),
		};

		paramNames = new string[] { "a", "b",};
		slideShowSpeed = 1f;
	}

	public override void SetParams(ParamBundle pb)
 	{
		a = pb.eqnParam[0];
		b = pb.eqnParam[1]; 
	}



	public override void Function(ref float[] x_in, ref float[] x_out) {

		x_out[0] = x_in[0]*(4f-x_in[1]) + a*x_in[2];
		x_out[1] = -x_in[1]*(1f-x_in[0]*x_in[0]);
		x_out[2] = -x_in[0]*(1.5f - b * x_in[2]) - 0.05f*x_in[2];
	}

}
