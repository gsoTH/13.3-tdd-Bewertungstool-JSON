using System.Diagnostics;
using System.Text;
using System.Text.Json;


namespace Bewertungstool
{
    class Program
    {
        private const string nameJsonDatei = "noten.json";
        public static void Main(string[] args)
        {
            List<Note> noten = ObjekteErzeugen();
            

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain(noten));

            ErzeugeJSON(noten);
        }

        private static List<Note> ObjekteErzeugen()
        {
            // Liste vorbereiten, in die alle eingelesenen Noten gespeichert werden
            List<Note> notenListe = new List<Note>();

            // Prüfen, ob die Datei überhaupt existiert
            if (!File.Exists(nameJsonDatei))
            {
                // Falls nicht, Liste mit Beispielobjekten zurückgeben
                return BeispielObjekteErzeugen();
            }

            // Gesamten Inhalt der JSON-Datei als Text einlesen
            string jsonInhalt = File.ReadAllText(nameJsonDatei, Encoding.UTF8);

            // JSON-Dokument parsen (die key:value-Paare erkennen)
            JsonDocument dokument = JsonDocument.Parse(jsonInhalt);

            // Wurzelelement des JSON holen (das äußere Objekt {})
            JsonElement wurzel = dokument.RootElement;

            // Das Array "noten" aus dem JSON holen
            JsonElement notenArray = wurzel.GetProperty("noten");

            // Durch alle Einträge im Array iterieren
            foreach (JsonElement eintrag in notenArray.EnumerateArray())
            {
                // Einzelne Werte aus dem JSON-Objekt auslesen
                int moeglichePunkte = eintrag.GetProperty("MoeglichePunkte").GetInt32();
                int erreichtePunkte = eintrag.GetProperty("ErreichtePunkte").GetInt32();

                // Datum als String einlesen und in DateOnly umwandeln
                string datumString = eintrag.GetProperty("ErzeugtAm").GetString();
                DateOnly erzeugtAm = DateOnly.Parse(datumString);

                // Neues Note-Objekt erzeugen
                Note note = new Note(moeglichePunkte, erreichtePunkte, erzeugtAm);

                // Objekt zur Liste hinzufügen
                notenListe.Add(note);
            }

            // Fertige Liste zurückgeben
            return notenListe;
        }
        private static List<Note> BeispielObjekteErzeugen()
        {
            // TODO: Noten aus Datei einlesen, statt hier zu erzeugen

            List<Note> beispielNoten = new List<Note>();

            beispielNoten.Add(new Note(100, 80));
            beispielNoten.Add(new Note(100, 42));
            beispielNoten.Add(new Note(100, 67));
            beispielNoten.Add(new Note(100, 93));

            return beispielNoten;
        }

        public static void ErzeugeJSON(List<Note> noten)
        {
            using (StreamWriter sw = new StreamWriter(nameJsonDatei, false, Encoding.UTF8))
            {
                // Objekt eröffnen
                sw.WriteLine("{");

                // Anzahl der Objekte in Datei eintragen
                    // \" verhindert, dass der String beendet wird. (Escape)
                sw.WriteLine($"\"anzahl\": {noten.Count},"); 
                    
                // Alle Objekte in Datei aufliste
                sw.WriteLine("\"noten\": [");
                for (int i = 0; i < noten.Count; i++)
                {
                    Note note = noten[i];
                    sw.WriteLine("{");
                    sw.WriteLine($"\"ErzeugtAm\": \"{note.ErzeugtAm:yyyy-MM-dd}\",");
                    sw.WriteLine($"\"MoeglichePunkte\": {note.MoeglichePunkte},");
                    sw.WriteLine($"\"ErreichtePunkte\": {note.ErreichtePunkte}");
                    sw.Write("}");

                    // Komma nur setzen, wenn nicht letztes Objekt
                    if (i < noten.Count - 1)
                    {
                        sw.Write(",");
                    }
                    sw.WriteLine();
                }
                // Ende Array und Objekt
                sw.WriteLine("]");
                sw.WriteLine("}");
            }

        }
    }
}