using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActressMas;

namespace Izbori
{
    public class Program
    {
        public static void Main(string[] args)
        {
            EnvironmentMas okruzenje = new EnvironmentMas(parallel: false, randomOrder: false);

            int brojKandidata = 5;
            int brojGlasaca = 100;

            okruzenje.Memory["brojKandidata"] = brojKandidata;
            okruzenje.Memory["brojGlasaca"] = brojGlasaca;

            for (int i = 0; i < brojGlasaca; i++)
            {
                AgentGlasac agentGlasac = new AgentGlasac();
                okruzenje.Add(agentGlasac, $"agentGlasac_{i}");
            }

            AgentBrojac agentBrojac = new AgentBrojac();
            okruzenje.Add(agentBrojac, "agentBrojac");

            okruzenje.Start();

            Console.ReadLine();
        }
    }
}
