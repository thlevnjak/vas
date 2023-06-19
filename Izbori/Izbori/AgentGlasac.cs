using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActressMas;

namespace Izbori
{
    public class AgentGlasac : Agent
    {
        private static Random random;
        private static Type typeOsobine;
        private static Type typeImena;
        private static Type typePrezimena;
        private Array osobine;
        private Array imena;
        private Array prezimena;
        private List<OsobineEnum> listaOsobina;
        private List<ImenaEnum> listaImena;
        private List<PrezimenaEnum> listaPrezimena;
        private List<Kandidat> listaKandidata;
        private static int brojKandidata;
        private static char[] sviKandidati;
        private string preferencija;

        public AgentGlasac()
        {
            random = new Random();
            typeOsobine = typeof(OsobineEnum);
            typeImena = typeof(ImenaEnum);
            typePrezimena = typeof(PrezimenaEnum);
            listaOsobina = new List<OsobineEnum>();
            listaImena = new List<ImenaEnum>();
            listaPrezimena = new List<PrezimenaEnum>();
        }

        public override void Act(Message message)
        {
            message.Parse(out string action, out string parameters);
            Send("agentBrojac", $"{preferencija}");

            Stop();
        }

        public override void Setup()
        {
            listaKandidata = GenerirajListuKandidata();

            string[] kandidati = new string[brojKandidata];
            for (int i = 0; i < brojKandidata; i++)
            {
                kandidati[i] = listaKandidata[i].Oznaka.ToString();
            }

            string[] izbor = RandomSortirajKandidate(kandidati);
            for (int i = 0; i < izbor.Length; i++)
            {
                preferencija = preferencija + " " + izbor[i];
            }

            Environment.Memory["listaKandidata"] = listaKandidata;
        }

        public List<Kandidat> GenerirajListuKandidata()
        {
            brojKandidata = Environment.Memory["brojKandidata"];
            sviKandidati = new char[brojKandidata];
            char prvaOznaka = 'A';

            osobine = typeOsobine.GetEnumValues();
            imena = typeImena.GetEnumValues();
            prezimena = typePrezimena.GetEnumValues();

            listaKandidata = new List<Kandidat>();

            for (int i = 0; i < brojKandidata; i++)
            {
                int index = random.Next(osobine.Length);
                OsobineEnum osobina = (OsobineEnum)osobine.GetValue(index);
                listaOsobina.Add(osobina);
                index = random.Next(imena.Length);
                ImenaEnum ime = (ImenaEnum)imena.GetValue(index);
                listaImena.Add(ime);
                index = random.Next(prezimena.Length);
                PrezimenaEnum prezime = (PrezimenaEnum)prezimena.GetValue(index);
                listaPrezimena.Add(prezime);

                Kandidat kandidat = new Kandidat(ime, prezime, prvaOznaka, osobina);
                listaKandidata.Add(kandidat);

                sviKandidati[i] = prvaOznaka;
                prvaOznaka++;
            }

            return listaKandidata;
        }

        public string[] RandomSortirajKandidate(string[] kandidati)
        {
            var osobina = listaOsobina.OrderBy(x => Guid.NewGuid()).FirstOrDefault();

            Glasac glasac = new Glasac(random.Next(), osobina);

            for (int i = 0; i < brojKandidata; i++)
            {
                if (glasac.Osobina == listaKandidata[i].Osobina)
                {
                    kandidati[0] = listaKandidata[i].Oznaka.ToString();
                    break;
                }
            }

            List<string> slobodniKandidati = new List<string>(sviKandidati.Select(c => c.ToString()));
            slobodniKandidati.Remove(kandidati[0]);

            int n = slobodniKandidati.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                string temp = slobodniKandidati[k];
                slobodniKandidati[k] = slobodniKandidati[n];
                slobodniKandidati[n] = temp;
            }

            for (int i = 1; i < kandidati.Length; i++)
            {
                kandidati[i] = slobodniKandidati[i - 1];
            }

            return kandidati;
        }
    }
}