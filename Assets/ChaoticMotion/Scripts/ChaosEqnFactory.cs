using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosEqnFactory  {

	// Add any new chaotic equations to this list
	private static System.Type[] eqnClasses = 
		{ 
		  typeof(Aizawa),
		  typeof(Anishchenko_Astakhov),
		  typeof(Arneodo),
		  // typeof(Bouali2), need to find stable params
		  typeof(Chen), 
		  typeof(ChenLee),
		  // typeof(DequanLee), // needs tuning
		  typeof(GenesioTesi),
		  typeof(Hadley),
		  typeof(Halvorsen),
		  // typeof(LiuChen), // needs more research
		  typeof(Lorenz), 
		  // typeof(LorenzNew), // Needs param tuning
		  typeof(LuChenCheng),
		  typeof(LuChen_Transition),
		  typeof(NoseHoover),
		  typeof(Rossler),
		  typeof(Rucklidge),
		  typeof(ShimizuMorioka),
		  typeof(SprottB),
		  typeof(SprottC),
		  typeof(SprottD),
		  typeof(SprottE),
		  typeof(SprottF),
		  typeof(SprottG),
		  typeof(SprottH),
		  typeof(SprottI),
		  typeof(SprottJ),
		  typeof(SprottK),
		  typeof(SprottL),
		  typeof(SprottM),
		  typeof(SprottN),
		  typeof(SprottO),
		  typeof(SprottP),
		  typeof(SprottQ),
		  typeof(SprottR),
		  typeof(SprottS),
		  typeof(Thomas),
		};

	private static List<ChaosEqn> eqnList;

	/// <summary>
	/// Returns a List of Equation objects. A singleton reference list is provided for the 
	/// inspector script to make reference to all the available equations. 
	/// </summary>
	/// <returns>The equations.</returns>
	public static List<ChaosEqn> GetEquations() {

		if (eqnList == null) {
			eqnList = new List<ChaosEqn>();
			for (int i=0; i < eqnClasses.Length; i++) {
				eqnList.Add( Create(i, 0, null) );
			}
		}
		return eqnList;
	}

	/// <summary>
	/// Create the ChaosEqn implementation for the specified index in the eqnList, with the
	/// selected param bundle in place
	/// </summary>
	/// <param name="index">Index.</param>
	public static ChaosEqn Create(int index, int paramIndex, ParamBundle customParams) {

		ChaosEqn chaosEqn = System.Activator.CreateInstance(eqnClasses[index]) as ChaosEqn;
		ParamBundle[] paramBundles = chaosEqn.GetParamBundles(); 
		if (paramIndex < paramBundles.Length) {
			chaosEqn.paramBundle = paramBundles[paramIndex];
		} else {
			chaosEqn.paramBundle = customParams;
		}
		chaosEqn.SetParams(chaosEqn.paramBundle);
		return chaosEqn;
	}
}
