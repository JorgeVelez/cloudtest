using UnityEngine;
using System.Collections;

// eqn (30) from http://lsc.amss.ac.cn/~ljh/04LCC.pdf
public class LuChen_Transition : ChaosEqn  {

	private float a ;	
	private float b ;		
	private float c ;		

	public LuChen_Transition() {
		name = "Lu-Chen Transition";

		eqnStrings = new string[]{
				"xdot = a(y-x)",
				"ydot = - xz + cy",
				"zdot = xy - bz"
		};

		paramBundles = new ParamBundle[] {
			// sigma, b, rho
			new ParamBundle("default (scaled)", 36f, 3f, 20f,  
					new Vector3(-0.1f, 0.5f, -0.6f), new Vector3(2.7f, 3.4f,-21.9f), 0.24f),
			new ParamBundle("default", 36f, 3f, 20f, 
					new Vector3(-0.1f, 0.5f, -0.6f)),
		};

		paramNames = new string[] { "a", "b", "c"};
		slideShowSpeed = 0.8f;

	}

	public override void SetParams(ParamBundle pb)
 	{
		a = pb.eqnParam[0];
		b = pb.eqnParam[1]; 
		c = pb.eqnParam[2];
	}

	public override void Function(ref float[] x_in, ref float[] x_out) {
		x_out[0] = a*(x_in[1]-x_in[0]);
		x_out[1] = - x_in[0]*x_in[2] + c*x_in[1];
		x_out[2] = x_in[0]*x_in[1] - b * x_in[2];
	}

}
