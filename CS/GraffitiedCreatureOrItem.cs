using System;
using XRL;
using XRL.Language;
using XRL.Rules;
using XRL.World;
using XRL.World.Parts;

namespace XRL.World.Parts {
	[Serializable]
	public class GraffitiedThing : IPart {

		public string graffitiText = "";

		public string colorPrefix;

		public override bool SameAs(IPart p) {
			return false;
		}

		public override bool WantEvent(int ID, int cascade) {
			if (!base.WantEvent(ID, cascade) && ID != GetDisplayNameEvent.ID && ID != GetShortDescriptionEvent.ID && ID != GetUnknownShortDescriptionEvent.ID)
			{
				return ID == ObjectCreatedEvent.ID;
			}
			return true;
		}

		public override bool HandleEvent(GetDisplayNameEvent E) {
			if (!ParentObject.Understood() || !ParentObject.HasProperName) {
				E.AddAdjective("{{graffitied|graffitied}}");
			}
			return true;
		}

		public override bool HandleEvent(GetShortDescriptionEvent E) {
			if (!string.IsNullOrEmpty(graffitiText)) {
				E.Base.Append("\n\n").Append("Someone has scrawled a message on this. It reads: \n\n\"").Append("{{")
					.Append(colorPrefix)
					.Append("|")
					.Append(graffitiText)
					.Append("}}")
					.Append("\"\n");
			}
			return true;
		}
	}
}