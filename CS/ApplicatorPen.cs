using System;
using XRL.UI;
using XRL.World.Parts;

namespace XRL.World.Parts {
	[Serializable]
	public class ApplicatorPen : IPart {
		public override bool SameAs(IPart p) { //same as Applicator, but you can't have this inherit from Applicator or it isn't a part
			return true;
		}

		public override bool WantEvent(int ID, int cascade) { //same as Applicator
			return base.WantEvent(ID, cascade) || ID == GetInventoryActionsAlwaysEvent.ID;
		}
		public override bool HandleEvent(GetInventoryActionsAlwaysEvent E) {
			string name = "Change color";
			string display = "change color";
			string command = "Change color";
			char key = 'C';
			E.AddAction(name, display, command, Key: key, FireOnActor: false, Default: -100);
			name = "Write";
			display = "write";
			command = "Write";
			key = 'w';
			E.AddAction(name, display, command, Key: key, FireOnActor: false, Default:100);
			return true;
		}
	}
}