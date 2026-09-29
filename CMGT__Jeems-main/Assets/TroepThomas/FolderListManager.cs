using System.Collections.Generic;

using UnityEngine;

using UnityEngine.UI;

using UnityEngine.EventSystems; // Nodig voor rechtsklik detectie

using TMPro;



public class FolderListManager : MonoBehaviour

{

public enum ItemType { Folder, Bestand }

public enum VirusType { DraaiAchtergrond, GlitchTekst, ScrollManipulator }

public QuestManager questManager;

private bool isBezigMetPlakken = false;



[System.Serializable]

public class FolderData

{

//This sets up everything in the editor. The tooltips are for the person making the assignments. It explains the function of each button they can toggle.

[Tooltip("De naam van het bestand of folder.")]

public string itemName;

[Tooltip("Kies hier uit of het een bestand of folder moet zijn. Folders kan je openen, en bestanden kun je bewerken of verwijderen.")]

public ItemType type;



[Tooltip("Klik dit aan als je wil dat het bestand verwjiderbaar is.")]

public bool kanVerwijderen = true;

[Tooltip("Klik dit aan als je wil dat de bestandsnaam gewijzigd kan worden.")]

public bool kanHernoemen = true;

[Tooltip("Klik dit aan als je wil dat de folder pas verschijnt na een aantal secondes. Dit kan bijvoorbeeld gebruikt worden voor virussen.")]

public float verschijnVertraging = 0f;



[Header("PDF Instellingen")]

[Tooltip("Het pad of URL naar de PDF die geopend moet worden wanneer je dit bestand aanklikt.")]

public string pdfPath;



[Header("Categorie Instellingen")]

[Tooltip("Voer hier de naam in van de categorie van de folder. Dit wordt 'metadata' genoemd in het spel.")]

public string categorieNaam = "Ongecategoriseerd";

[Tooltip("Voer hier de kleur in van de categorie van de folder. Dit wordt 'metadata' genoemd in het spel. Ga naar CategorieIndex aan de linkerkant om standaard kleuren in te stellen die dezen overschrijden, zodat je niet steeds dezelfde kleur hoeft te kiezen voor dezelfde categorie.")]

public Color categorieKleur = Color.gray;



[Header("Alleen invullen bij Folder")]

public FolderContainer targetContainer;



[Header("Virus Opties")]

[Tooltip("Selecteer of dit bestand een virus is of niet.")]

public bool isVirus = false;

[Tooltip("Selecteer het soort virus. Draai Achtergrond draait de achtergrond, Glitch Text laat de tekst bovenaan het scherm veranderen, en Scroll Manipulator neemt de controle van het scrollwiel weg. Iets hier kiezen terwijl je Is Virus leeg laat zorgt ervoor dat er geen effecten plaatsnemen. Je kunt in Virus Manager aan de linkerkant deze effecten aanpassen.")]

public VirusType typeVirus;



[HideInInspector]

public bool isZichtbaar = false;

}



[System.Serializable]

public class FolderContainer

{

public string containerName;

public List<FolderData> items;

}



//this is where the lists and files are created. For further explanation, watch the demonstration video.

[Header("Prefabs")]

public GameObject folderPrefab;

public GameObject bestandPrefab;

public ContextMenuManager contextMenu;



[Header("UI Referenties")]

public Transform contentParent;

public Button terugKnop;

public TextMeshProUGUI titelTekst;



[Header("Instellingen")]

public FolderContainer mainList;

private Stack<FolderContainer> geschiedenis = new Stack<FolderContainer>();

private FolderContainer huidigeContainer;



[Header("Overig")]

public PasteMenuManager pasteMenu;

public CategoryColorManager colorManager;

public MetadataMenuManager metadataMenu;

public MeldBijITManager meldBijITMenu;

private FolderData gekniptItemData;

private FolderContainer bronContainer;



void Start() //Start Method, sets things up

{

if (terugKnop != null) terugKnop.onClick.AddListener(GaTerug);

if (contextMenu != null) contextMenu.Hide();

OpenNieuweLijst(mainList);

InitieerTijdgestuurdeBestanden(mainList);

}



public void OpenNieuweLijst(FolderContainer nieuweContainer) //Open New List, happens when you click on a folder

{

if (nieuweContainer == null) return;

if (huidigeContainer != null) geschiedenis.Push(huidigeContainer);

DisplayList(nieuweContainer);

}



public void GaTerug() //Go back button, for when you press the GA TERUG (GO BACK) red button

{

if (geschiedenis.Count > 0)

{

FolderContainer vorigeContainer = geschiedenis.Pop();

DisplayList(vorigeContainer);

}

}



private Transform ZoekChildRecursief(Transform parent, string naam) //Looks for the correct UI element by going through the childs.

{

foreach (Transform child in parent)

{

if (child.name == naam) return child;

Transform gevonden = ZoekChildRecursief(child, naam);

if (gevonden != null) return gevonden;

}

return null;

}



private void DisplayList(FolderContainer container)

{

huidigeContainer = container;

if (titelTekst != null) titelTekst.text = container.containerName;

if (terugKnop != null) terugKnop.gameObject.SetActive(geschiedenis.Count > 0);



foreach (Transform child in contentParent) Destroy(child.gameObject);

foreach (FolderData data in container.items)

{

if (data.verschijnVertraging > 0f && !data.isZichtbaar)

{

continue;

}



GameObject prefabToSpawn = (data.type == ItemType.Folder) ? folderPrefab : bestandPrefab;

GameObject newItem = Instantiate(prefabToSpawn, contentParent);

newItem.GetComponentInChildren<TextMeshProUGUI>().text = data.itemName;



//Looks for component

Transform tekstTransform = ZoekChildRecursief(newItem.transform, "CategorieTekst");

Transform balkTransform = ZoekChildRecursief(newItem.transform, "CategorieBalk");



if (tekstTransform != null)

{

var tComp = tekstTransform.GetComponent<TextMeshProUGUI>();

if (tComp != null) tComp.text = data.categorieNaam;

}



if (balkTransform != null)

{

var bComp = balkTransform.GetComponent<RawImage>();

if (bComp != null)

{

Color definitieveKleur = data.categorieKleur;

if (colorManager != null)

{

definitieveKleur = colorManager.GetColorForCategory(data.categorieNaam, data.categorieKleur);

}

bComp.color = definitieveKleur;

}

}



EventTrigger trigger = newItem.GetComponent<EventTrigger>();

if (trigger == null) trigger = newItem.AddComponent<EventTrigger>();

EventTrigger.Entry entry = new EventTrigger.Entry();

entry.eventID = EventTriggerType.PointerClick;

entry.callback.AddListener((eventData) => {

OnPointerClick((PointerEventData)eventData, data, newItem, container);

});

trigger.triggers.Add(entry);

}

}



private void OnPointerClick(PointerEventData eventData, FolderData data, GameObject itemVisual, FolderContainer container)

{

if (eventData.button == PointerEventData.InputButton.Right)

{

if (data.type == ItemType.Bestand)

{

if (data.isVirus)

{

if (meldBijITMenu != null)

{

if (contextMenu != null) contextMenu.Hide();

meldBijITMenu.Show(data, container, this);

return;

}

}



if (contextMenu != null)

{

if (meldBijITMenu != null) meldBijITMenu.Hide();

contextMenu.Show(itemVisual, data, container);

}

}

else if (data.type == ItemType.Folder && metadataMenu != null)

{

metadataMenu.Show(data, this);

}

}

else if (eventData.button == PointerEventData.InputButton.Left)

{

if (data.type == ItemType.Folder)

{

OpenNieuweLijst(data.targetContainer);

}

else if (data.type == ItemType.Bestand)

{

// Open real PDF file when clicking a 'Bestand'

if (!string.IsNullOrEmpty(data.pdfPath))

{

Application.OpenURL(data.pdfPath);

}

else

{

Debug.LogWarning("Geen PDF pad ingesteld voor bestand: " + data.itemName);

}

}

}

}



public void OpenPlakMenu()

{

if (contextMenu != null) contextMenu.Hide();

if (pasteMenu != null) pasteMenu.Show();

}



public void KnipItem(FolderData data, FolderContainer vanContainer, GameObject visual)

{

gekniptItemData = data;

bronContainer = vanContainer;

Destroy(visual);

Debug.Log("Item geknipt: " + data.itemName);

}



public void PlakItem()

{

if (isBezigMetPlakken) return;



if (gekniptItemData == null)

{

Debug.Log("Je probeert te plakken zonder iets te plakken hebben. Schaam je.");

return;

}



isBezigMetPlakken = true;



string naamVanBestand = gekniptItemData.itemName;

string naamVanMap = huidigeContainer.containerName;



if (bronContainer != null) bronContainer.items.Remove(gekniptItemData);

huidigeContainer.items.Add(gekniptItemData);



if (questManager != null)

{

questManager.CheckPlakOpdracht(naamVanBestand, naamVanMap);

}



DisplayList(huidigeContainer);

gekniptItemData = null;

bronContainer = null;

Debug.Log("Succesvol geplakt: " + naamVanBestand);

Invoke("ResetPlakSlot", 0.2f);

}



private void ResetPlakSlot()

{

isBezigMetPlakken = false;

}



public void DisplayCurrentList()

{

DisplayList(huidigeContainer);

}



private void InitieerTijdgestuurdeBestanden(FolderContainer container)

{

if (container == null || container.items == null) return;

foreach (FolderData data in container.items)

{

if (data.verschijnVertraging > 0f)

{

data.isZichtbaar = false;

StartCoroutine(SpawnBestandNaTijd(data, data.verschijnVertraging));

}

else

{

data.isZichtbaar = true;

}



if (data.type == ItemType.Folder && data.targetContainer != null)

{

InitieerTijdgestuurdeBestanden(data.targetContainer);

}

}

}



private System.Collections.IEnumerator SpawnBestandNaTijd(FolderData data, float tijd)

{

yield return new WaitForSeconds(tijd);

data.isZichtbaar = true;

DisplayCurrentList();

}



public bool IsVirusTypeActief(VirusType type)

{

if (huidigeContainer == null || huidigeContainer.items == null) return false;

foreach (FolderData data in huidigeContainer.items)

{

if (data.isZichtbaar && data.isVirus && data.typeVirus == type)

{

return true;

}

}

return false;

}



public bool IsErEenVirusActief()

{

if (huidigeContainer == null || huidigeContainer.items == null) return false;

foreach (FolderData data in huidigeContainer.items)

{

if (data.isZichtbaar && data.isVirus) return true;

}

return false;

}

} 

