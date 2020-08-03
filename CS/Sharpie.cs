using System;
using System.Collections.Generic;
using XRL.UI;
using XRL.World.Parts;
using System.Text.RegularExpressions;
using System.Globalization;

namespace XRL.World.Parts {
	[Serializable]
    public class Sharpie : IPart {
		private string color = "";

		private readonly List<string> validColors = new List<string>() {"R", "W", "G", "B", "M", "C", "Y", "r", "w", "g", "b", "m", "c", "y"};

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
						if (gameObject.HasTag("Creature") || gameObject.HasTag("Item") || gameObject.HasTag("Wall") || gameObject.HasTag("Furniture")) {
							somethingWritten = true;
							if (color.Equals("e") || color.Equals("E")) {
								if (gameObject.HasPart(typeof(GraffitiedThing))) {
									gameObject.RemovePart<GraffitiedThing>();
									if (!gameObject.HasPart(typeof(Graffitied))) gameObject.SetIntProperty("HasGraffiti", 0);
									Popup.Show("You erase the graffiti on the " + gameObject.DisplayName + ".");
									E.Actor.UseEnergy(1000);
								}
								else if (gameObject.HasPart(typeof(Graffitied))) {
									gameObject.RemovePart<Graffitied>();
									if (!gameObject.HasPart(typeof(GraffitiedThing))) gameObject.SetIntProperty("HasGraffiti", 0);
									gameObject.SetIntProperty("HasGraffiti", 0);
									Popup.Show("You erase the graffiti on the " + gameObject.DisplayName + ".");
									E.Actor.UseEnergy(1000);
								}
								else {
									Popup.Show("There's nothing to erase on the " + gameObject.DisplayName + ".");
								}
								continue;
							}

							string text = Popup.AskString(String.Format("What do you want to write on the {0}?", gameObject.DisplayName), "", 999);
							if (text.Equals("")) continue;

							Regex reg = new Regex("\\\\u[0-9a-fA-F]{4}");
							
							text = reg.Replace(text, delegate(Match m) {
								int value = int.Parse(m.Value.Substring(2, 4), NumberStyles.HexNumber);
								char str = (char)value;
								return str.ToString();
							}
							);
							Popup.Show("&yYou write " + "{{" + color + "|" + text + "}}&y on the " + gameObject.DisplayName + ".");
							if (color == "" || !validColors.Contains(color)) {
								gameObject.pRender.SetForegroundColor(Crayons.GetRandomColorAll());
							}
							else {
								gameObject.pRender.SetForegroundColor(color);
							}
							GraffitiedThing graffiti;
							if (!gameObject.HasPart(typeof(GraffitiedThing))) {
								graffiti = gameObject.AddPart<GraffitiedThing>();
							}
							else {
								graffiti = gameObject.GetPart<GraffitiedThing>();
							}
							graffiti.graffitiText = text;
							if (color.Length > 1) {
								graffiti.colorPrefix = color;
                            } else {
								graffiti.colorPrefix = gameObject.pRender.GetForegroundColor();
                            }
							gameObject.SetIntProperty("HasGraffiti", 1);
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
				color = Popup.AskString("Set a color character or pattern (or leave blank for random, or enter E to erase)", "", 50);
			}
			return true;
		}
	}
}