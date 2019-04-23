using UnityEngine;
using System.Collections;

// eqn (28) from http://lsc.amss.ac.cn/~ljh/04LCC.pdf
public class Rucklidge : ChaosEqn  {

	private float a ;	
	private float l ;		

	public Rucklidge() {
		name = "Rucklidge";

		eqnStrings = new string[]{
				"xdot = - a x + l y - y z",
				"ydot = x",
				"zdot = -z + y^2"
		};

		paramBundles = new ParamBundle[] {
			// sigma, b, rho
			new ParamBundle("default (scaled)", 2f, 6.7f, 0f,  
					new Vector3(0f, 0.8f, 0f), new Vector3(-0.2f, 0f, -7.8f), 0.57f),
			new ParamBundle("default", 2f, 6.7f, 0f,
					new Vector3(0f, 0.8f, 0f)),
		};

		paramNames = new string[] { "a", "l", ""};
		slideShowSpeed = 2.5f;
	}

	public override void SetParams(ParamBundle pb)
 	{
		a = pb.eqnParam[0];
		l = pb.eqnParam[1]; 
	}

	public override void Function(ref float[] x_in, ref float[] x_out) {
		x_out[0] = -a * x_in[0] + l * x_in[1] - x_in[1]*x_in[2];
		x_out[1] = x_in[0];
		x_out[2] = -x_in[2] + x_in[1]*x_in[1];
	}

}
