using UnityEngine;
using System.Collections;

// eqn (27) from http://lsc.amss.ac.cn/~ljh/04LCC.pdf

public class Rossler : ChaosEqn  {

	private float a = 0.2f;	
	private float b = 0.2f;		
	private float c = 5.7f; 		

	public Rossler() {
		name = "Rossler";
		eqnStrings = new string[]{
			"x = -y - z",
			"y = x + a y",
			"z = b z (x - c)"
		};

		paramBundles = new ParamBundle[] {
			// sigma, b, rho
			// a, b, c
			new ParamBundle("default (scaled)", 0.2f, 0.2f, 5.7f, 
					new Vector3(-2f,5f,-10f), new Vector3(-1.2f, 1.5f, -11f), 0.45f),
			new ParamBundle("default", 0.2f, 0.2f, 5.7f, 
					new Vector3(0f, 0f, 0f))
		};

		paramNames = new string[] { "sigma", "b", "rho"};
		slideShowSpeed = 2.5f;
	}

	public override void SetParams(ParamBundle pb)
 	{
		a = pb.eqnParam[0];
		b = pb.eqnParam[1]; 
		c = pb.eqnParam[2];
	}

	public override void Function(ref float[] x_in, ref float[] x_out) {
		x_out[0] = -x_in[1] - x_in[2];
		x_out[1] = x_in[0] + a* x_in[1];
		x_out[2] = b + x_in[2] * (x_in[0] - c);
	}

}
