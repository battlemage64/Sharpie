using System;
using XRL;
using XRL.Language;
using XRL.Rules;
using XRL.World;
using XRL.World.Parts;

namespace XRL.World.Parts {
	[Serializable]
	public class GraffitiedCreatureOrItem : IPart {

		public string graffitiText = "";

		public override bool SameAs(IPart p) {
			return false;
		}

		public override bool WantEvent(int ID, int cascade) {
			return base.WantEvent(ID, cascade) || ID == GetDisplayNameEvent.ID || ID == GetShortDescriptionEvent.ID || ID == GetShortDisplayNameEvent.ID || ID == GetUnknownShortDescriptionEvent.ID;
		}

		public override bool HandleEvent(IDisplayNameEvent E) {
			if (!ParentObject.Understood() || !ParentObject.HasProperName) {
				E.AddAdjective("{{graffitied|graffitied}}");
			}
			return true;
		}

		public override bool HandleEvent(IShortDescriptionEvent E) {
			if (!string.IsNullOrEmpty(graffitiText)) {
				E.Base.Append("\n\n").Append("Someone has scrawled a message on this. It reads: \n\n\"").Append("{{")
					.Append(ParentObject.pRender.GetForegroundColor())
					.Append("|")
					.Append(graffitiText)
					.Append("}}")
					.Append("\"\n");
			}
			return true;
		}
	}
}