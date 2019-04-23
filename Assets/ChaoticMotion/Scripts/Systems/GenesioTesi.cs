using UnityEngine;
using System.Collections;

// eqn (52) from http://lsc.amss.ac.cn/~ljh/04LCC.pdf

public class GenesioTesi : ChaosEqn  {

	private float a, b, c;

	public GenesioTesi() {
		name = "GenesioTesi";

		eqnStrings = new string[]{
				"xdot = y",
				"ydot = z",
				"zdot = -c x - by - az + x^2"
		};

		paramBundles = new ParamBundle[] {
			// sigma, b, rho
			new ParamBundle("default (scaled)", 0.44f, 1.1f, 1f, 
				new Vector3(.1f, .1f, .1f), new Vector3(-000.3f,-000.1f,000.0f), 06.66f),
			new ParamBundle("default", 0.44f, 1.1f, 1f, 
					new Vector3(.1f,.1f,.1f)),
		};

		paramNames = new string[] { "a", "b", "c"};
		slideShowSpeed = 2f;
	}

	public override void SetParams(ParamBundle pb)
 	{
		a = pb.eqnParam[0];
		b = pb.eqnParam[1]; 
		c = pb.eqnParam[2];
	}

	public override void Function(ref float[] x_in, ref float[] x_out) {
		x_out[0] = x_in[1];
		x_out[1] = x_in[2];
		x_out[2] = -c*x_in[0] - b*x_in[1] - a*x_in[2] + x_in[0]*x_in[0];
	}

}
