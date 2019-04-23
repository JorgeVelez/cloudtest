using UnityEngine;
using System.Collections;

// see http://www.algosome.com/articles/aizawa-attractor-chaos.html
// BUT code does not match eqns and z eqn does match http://chaoticatmospheres.com/mathrules-strange-attractors
// (which has a typo in the first equation, sheesh)
public class Aizawa : ChaosEqn  {

	private float a;	
	private float b;		
	private float c;		
	private float d;		
	private float e;		
	private float f;		

	public Aizawa() {
		name = "Aizawa";

		eqnStrings = new string[]{
				"xdot = (z-b)x - d y",
				"ydot = d x + (z-b)y",
				"zdot = c + a z - z^3/3 - (x^2+y^2)(1+ez) + f z x^3 "
		};

		paramBundles = new ParamBundle[] {
			new ParamBundle("default (scaled)", new float[]{0.95f, 0.7f, 0.6f, 3.5f, 0.25f, 0.1f}, 
					new Vector3(0.1f, 0f, 0f), new Vector3(00.0f,00.0f,-00.8f), 3.37f),
			new ParamBundle("default", new float[]{0.95f, 0.7f, 0.6f, 3.5f, 0.25f, 0.1f}, 
					new Vector3(0.1f, 0f, 0f)),
		};

		paramNames = new string[] { "a", "b", "c", "d", "e", "f"};
		slideShowSpeed = 1f;
	}

	public override void SetParams(ParamBundle pb)
 	{
		a = pb.eqnParam[0];
		b = pb.eqnParam[1]; 
		c = pb.eqnParam[2];
		d = pb.eqnParam[3];
		e = pb.eqnParam[4];
		f = pb.eqnParam[5];
	}


	public override void Function(ref float[] x_in, ref float[] x_out) {

		x_out[0] = (x_in[2] - b) * x_in[0] - d*x_in[1];
		x_out[1] = d * x_in[0] + (x_in[2] - b) * x_in[1];
		x_out[2] = c + a*x_in[2]-x_in[2]*x_in[2]*x_in[2]/3f 
					- (x_in[0]*x_in[0] + x_in[1]*x_in[1])*(1+e*x_in[2])
					+ f * x_in[2] * x_in[0]*x_in[0]*x_in[0];
	}

}
