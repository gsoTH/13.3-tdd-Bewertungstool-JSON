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
            List<Note> noten = BeispielObjekteErzeugen();
            

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain(noten));

            ErzeugeJSON(noten);
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