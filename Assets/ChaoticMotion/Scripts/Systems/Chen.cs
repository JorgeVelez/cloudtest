using UnityEngine;
using System.Collections;

// eqn (28) from http://lsc.amss.ac.cn/~ljh/04LCC.pdf
public class Chen : ChaosEqn  {

	private float a ;	
	private float b ;		
	private float c ;		

	public Chen() {
		name = "Chen";

		eqnStrings = new string[]{
				"xdot = a(y-x)",
				"ydot = (c-a)x - xz + cy",
				"zdot = xy - bz"
		};

		paramBundles = new ParamBundle[] {
			// sigma, b, rho
			new ParamBundle("default (scaled)", 40f, 3f, 28f, 
					new Vector3(-0.1f, 0.5f, -0.6f), new Vector3(-2.5f, -3f, -18.9f), 0.25f),
			new ParamBundle("default", 40f, 3f, 28f, 
					new Vector3(-0.1f, 0.5f, -0.6f)),
		};

		paramNames = new string[] { "a", "b", "c"};
		slideShowSpeed = 0.9f;
	}

	public override void SetParams(ParamBundle pb)
 	{
		a = pb.eqnParam[0];
		b = pb.eqnParam[1]; 
		c = pb.eqnParam[2];
	}

	public override void Function(ref float[] x_in, ref float[] x_out) {
		x_out[0] = a*(x_in[1]-x_in[0]);
		x_out[1] = (c-a)*x_in[0] - x_in[0]*x_in[2] + c*x_in[1];
		x_out[2] = x_in[0]*x_in[1] - b * x_in[2];
	}

}
