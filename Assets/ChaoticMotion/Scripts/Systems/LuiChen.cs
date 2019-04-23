using UnityEngine;
using System.Collections;

// http://chaoticatmospheres.deviantart.com/art/Strange-Attractors-The-Liu-Chen-Attractor-376173482
public class LiuChen : ChaosEqn  {

	private float alpha ;	
	private float beta ;		
	private float eta ;		
	private float delta ;
	private float epsilon;
	private float rho;
	private float zeta;		

	public LiuChen() {
		name = "LiuChen";

		eqnStrings = new string[]{
				"xdot = alpha y + beta x + eta yz",
				"ydot = delta y - z + epsilon x z",
				"zdot = zeta z + rho xy"
		};

		paramBundles = new ParamBundle[] {
//			new ParamBundle("default (scaled)", 5f, -10f, -0.38f, 
//				new Vector3(1f,1f,1f), new Vector3(-04.1f,-04.4f,-15.5f), 0.15f),
			new ParamBundle("default", new float[] {2.4f, -3.78f, 14f, -11f, 4f, 5.58f, 1f}, 
					new Vector3(0.1f, 0, 0)),
		};

		paramNames = new string[] { "alpha", "beta", "eta", "delta", "eplsilon", "rho", "zeta"};
		slideShowSpeed = 1f;
	}

	public override void SetParams(ParamBundle pb)
 	{
		alpha 	= pb.eqnParam[0];
		beta 	= pb.eqnParam[1]; 
		eta 	= pb.eqnParam[2];
		delta 	= pb.eqnParam[3];
		epsilon = pb.eqnParam[4];
		rho	 	= pb.eqnParam[5]; 
		zeta 	= pb.eqnParam[6];
	}

	public override void Function(ref float[] x_in, ref float[] x_out) {
		x_out[0] = alpha*x_in[1] + beta*x_in[0] + eta*x_in[1]*x_in[2];
		x_out[1] = delta*x_in[1] - x_in[2] + epsilon*x_in[0]*x_in[2];
		x_out[2] = zeta*x_in[2] + rho*x_in[0]*x_in[1];
	}

}
