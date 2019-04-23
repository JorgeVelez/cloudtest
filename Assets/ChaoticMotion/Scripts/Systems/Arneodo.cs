using UnityEngine;
using System.Collections;

// see http://iats09.karabuk.edu.tr/press/bildiriler_pdf/iats09_02-01_468.pdf
// does not match CA picture
public class Arneodo : ChaosEqn  {

	private float a;	
	private float b;		
	private float c;		
	private float d;		

	public Arneodo() {
		name = "Arneodo";

		eqnStrings = new string[]{
				"xdot = y",
				"ydot = z",
				"zdot = -a x - b y - c z  + d x^3 "
		};

		paramBundles = new ParamBundle[] {
			new ParamBundle("default (scaled)", new float[]{-5.5f, 3.5f, 1f, -1f}, 
				new Vector3(0.5f, -1f, 0.5f), new Vector3(00.0f,00.1f,00.2f), 0.46f),
			new ParamBundle("default", new float[]{-5.5f, 3.5f, 1f, -1f}, 
					new Vector3(0.5f, -1f, 0.5f)),
		};

		paramNames = new string[] { "a", "b", "c", "d"};
		slideShowSpeed = 1f;
	}

	public override void SetParams(ParamBundle pb)
 	{
		a = pb.eqnParam[0];
		b = pb.eqnParam[1]; 
		c = pb.eqnParam[2];
		d = pb.eqnParam[3];
	}



	public override void Function(ref float[] x_in, ref float[] x_out) {

		x_out[0] = x_in[1];
		x_out[1] = x_in[2];
		x_out[2] = - a*x_in[0]- b*x_in[1] - c*x_in[2] + d*x_in[0]*x_in[0]*x_in[0];
	}

}
