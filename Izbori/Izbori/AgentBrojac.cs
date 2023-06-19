using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActressMas;

namespace Izbori
{
    public class AgentBrojac : Agent
    {
        private int brojKoraka;
        private List<string> glasovi;
        private List<Kandidat> listaKandidata;

        public AgentBrojac()
        {
            glasovi = new List<string>();
            listaKandidata = new List<Kandidat>();
        }

        public override void Act(Message message)
        {
            message.Parse(out string action, out string parameters);
            glasovi.Add(parameters);
        }

        public override void ActDefault()
        {
            if (--brojKoraka <= 0)
            {
                for (int i = 0; i < glasovi.Count; i++)
                {
                    Console.WriteLine($"Glasac_{i + 1}: {glasovi[i]}");
                }

                string pobjednik = PronadjiPobjednikaCondorcetom(glasovi);

                listaKandidata = Environment.Memory["listaKandidata"];

                foreach (var kandidat in listaKandidata)
                {
                    if (pobjednik == kandidat.Oznaka.ToString())
                    {
                        Console.WriteLine("Pobjednik izbora Condorcetovom metodom je: " + kandidat.Ime.ToString() + " " + kandidat.Prezime.ToString() + " ( " + pobjednik + " )");
                        break;
                    }
                }

                Stop();
            }
        }

        public override void Setup()
        {
            brojKoraka = 1;
            Broadcast("start");
        }

        public string PronadjiPobjednikaCondorcetom(List<string> glasovi)
        {
            string pobjednik = null;

            HashSet<string> kandidati = new HashSet<string>();

            foreach (var glas in glasovi)
            {
                string[] rangiraniKandidati = glas.Split(' ');
                foreach (var kandidat in rangiraniKandidati)
                {
                    kandidati.Add(kandidat.Trim());
                }
            }

            List<string[]> paroviKandidata = new List<string[]>();
            string[] poljeKandidata = kandidati.ToArray();

            for (int i = 0; i < poljeKandidata.Length - 1; i++)
            {
                for (int j = i + 1; j < poljeKandidata.Length; j++)
                {
                    string[] par = { poljeKandidata[i], poljeKandidata[j] };
                    paroviKandidata.Add(par);
                }
            }

            Dictionary<string, int> kandidatPobjede = new Dictionary<string, int>();
            
            foreach (var kandidat in kandidati)
            {
                kandidatPobjede[kandidat] = 0;
            }

            foreach (string[] par in paroviKandidata)
            {
                string kandidat1 = par[0];
                string kandidat2 = par[1];

                foreach (string glas in glasovi)
                {
                    string[] rangiraniKandidati = glas.Split(' ');
                    int index1 = Array.IndexOf(rangiraniKandidati, kandidat1);
                    int index2 = Array.IndexOf(rangiraniKandidati, kandidat2);

                    if (index1 < index2)
                    {
                        kandidatPobjede[kandidat1]++;
                    }
                    else if (index2 < index1)
                    {
                        kandidatPobjede[kandidat2]++;
                    }
                }
            }

            int maxPobjeda = 0;
            
            foreach (KeyValuePair<string, int> par in kandidatPobjede)
            {
                if (par.Value > maxPobjeda)
                {
                    maxPobjeda = par.Value;
                    pobjednik = par.Key;
                }
            }

            return pobjednik;            
        }
    }
}
