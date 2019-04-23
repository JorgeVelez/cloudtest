using UnityEngine;
using System.Collections;

// http://chaoticatmospheres.deviantart.com/art/Strange-Attractors-The-Dequan-Li-Attractor-376066584
// http://3d-meier.de/tut19/Seite9.html
public class DequanLee : ChaosEqn  {

	private float alpha ;	
	private float beta ;		
	private float delta ;
	private float epsilon;
	private float rho;
	private float zeta;		

	public DequanLee() {
		name = "DequanLee";

		eqnStrings = new string[]{
				"xdot = alpha(y-x) - delta xz",
				"ydot = rho x + zeta y - xz",
				"zdot = beta z + xy - epsilon x^2"
		};

		paramBundles = new ParamBundle[] {
//			new ParamBundle("default (scaled)", 5f, -10f, -0.38f, 
//				new Vector3(1f,1f,1f), new Vector3(-04.1f,-04.4f,-15.5f), 0.15f),
			new ParamBundle("default", new float[] {40f,1.833f,0.16f,0.65f,55f,20f}, 
					new Vector3(0.349f,0f,-0.16f)),
		};

		paramNames = new string[] { "alpha", "beta", "delta", "eplsilon", "rho", "zeta"};
		slideShowSpeed = 1f;
	}

	public override void SetParams(ParamBundle pb)
 	{
		alpha 	= pb.eqnParam[0];
		beta 	= pb.eqnParam[1]; 
		delta 	= pb.eqnParam[2];
		epsilon = pb.eqnParam[3];
		rho	 	= pb.eqnParam[4]; 
		zeta 	= pb.eqnParam[5];
	}

	public override void Function(ref float[] x_in, ref float[] x_out) {
		x_out[0] = alpha*(x_in[1]-x_in[0]) - delta*x_in[0]*x_in[2];
		x_out[1] = rho*x_in[0] + zeta*x_in[1] - x_in[0]*x_in[2];
		x_out[2] = beta*x_in[2] + x_in[0]*x_in[1] - epsilon*x_in[0]*x_in[0];
	}

}
