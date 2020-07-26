using System;
using System.Collections.Generic;
using XRL.UI;
using XRL.World.Parts;

namespace XRL.World.Parts {
	[Serializable]
    public class Sharpie : IPart {
		private string color = ""; 

		public override bool WantEvent(int ID, int cascade) {
			return base.WantEvent(ID, cascade) || ID == InventoryActionEvent.ID;
		}

		public override bool HandleEvent(InventoryActionEvent E) {
			if (E.Command == "Write") {
					if (E.Actor.IsPlayer() && !ParentObject.Understood()) {
						ParentObject.MakeUnderstood();
						Popup.Show(ParentObject.Itis + " " + ParentObject.a + ParentObject.ShortDisplayName + "!");
					}
					Cell cell = null;
					if (E.Actor != null && E.Actor.IsPlayer()) {
						cell = E.Actor.pPhysics.PickDirection();
					}
				if (cell != null) {
					bool somethingWritten = false;
					foreach (GameObject gameObject in cell.Objects) {
						if (gameObject.HasTag("Wall")) {
							string text = Popup.AskString(String.Format("What do you want to write on the {0}?",gameObject.DisplayName), "", 999);
							if (color == "") {
								gameObject.pRender.SetForegroundColor(Crayons.GetRandomColorAll());
							}
							else {
								gameObject.pRender.SetForegroundColor(color);
							}
							Graffitied graffiti;
							if (!gameObject.HasPart(typeof(Graffitied))) {
								graffiti = gameObject.AddPart<Graffitied>();
							} else {
								graffiti = gameObject.GetPart<Graffitied>();
                            }
							graffiti.graffitiText = text;
							gameObject.SetIntProperty("HasGraffiti", 1);
							somethingWritten = true;
						} else if (gameObject.HasTag("Creature") || gameObject.HasTag("Item") || gameObject.HasTag("Furniture")) {
							string text = Popup.AskString(String.Format("What do you want to write on the {0}?", gameObject.DisplayName), "", 999);
							if (color == "") {
								gameObject.pRender.SetForegroundColor(Crayons.GetRandomColorAll());
							}
							else {
								gameObject.pRender.SetForegroundColor(color);
							}
							GraffitiedCreatureOrItem graffiti;
							if (!gameObject.HasPart(typeof(GraffitiedCreatureOrItem))) {
								graffiti = gameObject.AddPart<GraffitiedCreatureOrItem>();
							}
							else {
								graffiti = gameObject.GetPart<GraffitiedCreatureOrItem>();
							}
							graffiti.graffitiText = text;
							somethingWritten = true;
						}
                    }
					if (!somethingWritten) {
						Popup.Show("There's nothing there suitable to write on.");
					} else {
						E.Actor.UseEnergy(1000);
					}
					
					E.RequestInterfaceExit();
				}
			} else if (E.Command == "Change color") {
				color = Popup.AskString("Set a color character (or leave blank for random)", "", 50);

			}
			return true;
		}
	}
}
